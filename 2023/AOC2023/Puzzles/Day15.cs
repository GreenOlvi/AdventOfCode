using Dunet;

namespace AOC2023.Puzzles;
public partial class Day15 : CustomBaseDay
{
    private readonly string[] _lines;

    public Day15()
    {
        _lines = ReadLinesFromFile().First().Split(',', StringSplitOptions.TrimEntries);
    }

    public Day15(IEnumerable<string> lines)
    {
        _lines = lines.First().Split(',', StringSplitOptions.TrimEntries);
    }

    public static int Hash(string input)
    {
        byte cv = 0;
        foreach (var c in input)
        {
            cv += (byte)c;
            cv *= 17;
        }
        return cv;
    }

    private static readonly Regex SetCommandPattern = BuildSetCommandPattern();
    private static readonly Regex RemoveCommandPattern = BuildRemoveCommandPattern();
    private static Command ParseCommand(string line)
    {
        if (SetCommandPattern.TryMatch(line, out var setMatch))
        {
            return new Command.SetLens(setMatch.Groups["label"].Value, setMatch.Groups["length"].Value.ToInt());
        }

        if (RemoveCommandPattern.TryMatch(line, out var removeMatch))
        {
            return new Command.RemoveLens(removeMatch.Groups["label"].Value);
        }

        throw new InvalidDataException(line);
    }

    private long GetFocusingPower(LensBox[] boxes)
    {
        var sum = 0L;
        for (var boxIdx = 0; boxIdx < boxes.Length; boxIdx++)
        {
            var box = boxes[boxIdx];
            var lensPos = 1;
            foreach (var (_, f) in box.Lenses)
            {
                sum += (boxIdx + 1) * lensPos * f;
                lensPos++;
            }
        }

        return sum;
    }

    public override ValueTask<string> Solve_1() => _lines.Sum(Hash).ToResult();

    public override ValueTask<string> Solve_2()
    {
        var boxes = Enumerable.Range(0, 256).Select(i => new LensBox()).ToArray();

        foreach (var cmd in _lines.Select(ParseCommand))
        {
            cmd.Match(
                s => boxes[Hash(s.Label)].Set(s.Label, s.FocalLength),
                r => boxes[Hash(r.Label)].Remove(r.Label));
        }

        return GetFocusingPower(boxes).ToResult();
    }

    [Union]
    internal partial record Command
    {
        partial record SetLens(string Label, int FocalLength);
        partial record RemoveLens(string Label);
    }

    private class LensBox()
    {
        public IReadOnlyCollection<(string Lens, int FocalLength)> Lenses => _lenses.ToArray();

        private readonly List<(string Lens, int FocalLength)> _lenses = [];

        public void Set(string lens, int focalLength)
        {
            var i = _lenses.FindIndex(p => p.Lens == lens);
            if (i == -1)
            {
                _lenses.Add((lens, focalLength));
            }
            else
            {
                _lenses[i] = (lens, focalLength);
            }
        }

        public void Remove(string label)
        {
            var i = _lenses.FindIndex(p => p.Lens == label);
            if (i != -1)
            {
                _lenses.RemoveAt(i);
            }
        }
    }

    [GeneratedRegex(@"^(?<label>\w+)=(?<length>\d+)$")]
    private static partial Regex BuildSetCommandPattern();
    [GeneratedRegex(@"^(?<label>\w+)-$")]
    private static partial Regex BuildRemoveCommandPattern();
}
