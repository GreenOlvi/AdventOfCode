namespace AOC2024.Puzzles;

public class Day01 : CustomBaseProblem<long>
{
    private readonly (long[] List1, long[] List2) _input;

    public Day01()
    {
        _input = ParseInput(ReadLinesFromFile());
    }

    public Day01(IEnumerable<string> lines)
    {
        _input = ParseInput(lines);
    }

    private static (long[] List1, long[] List2) ParseInput(IEnumerable<string> lines)
    {
        List<long> l1 = [];
        List<long> l2 = [];
        foreach (var line in lines)
        {
            var (a, b) = ParseLine(line);
            l1.Add(a);
            l2.Add(b);
        }
        return (l1.ToArray(), l2.ToArray());
    }

    private static (long, long) ParseLine(string line)
    {
        var s = line.Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        return (s[0].ToLong(), s[1].ToLong());
    }

    public override long Solve1() =>
        _input.List1.Order()
            .Zip(_input.List2.Order())
            .Aggregate(0L, static (s, p) => s + Math.Abs(p.First - p.Second));

    public override long Solve2()
    {
        Dictionary<long, long> score = [];
        var sum = 0L;
        foreach (var i in _input.List1)
        {
            if (!score.TryGetValue(i, out var s))
            {
                s = score[i] = _input.List2.Count(e => e == i) * i;
            }
            sum += s;
        }
        return sum;
    }
}
