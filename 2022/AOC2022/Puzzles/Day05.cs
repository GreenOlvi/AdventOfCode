namespace AOC2022.Puzzles;

public partial class Day05 : CustomBaseDay
{
    private readonly string[] _lines;

    public Day05()
    {
        _lines = ReadLinesFromFile().ToArray();
    }

    public Day05(IEnumerable<string> lines)
    {
        _lines = lines.ToArray();
    }

    public static (IEnumerable<Stack<char>>, Instruction[]) ParseInput(IEnumerable<string> lines)
    {
        var groups = lines.SplitGroups().ToArray();
        var stacks = ParseStacks(groups[0].Select(l => l.TrimEnd()).ToArray());
        var instructions = ParseInstructions(groups[1]).ToArray();
        return (stacks, instructions);
    }

    private static IEnumerable<Stack<char>> ParseStacks(string[] lines)
    {
        var stackCount = lines.Last().Length / 4 + 1;
        var stacks = Enumerable.Range(0, stackCount).Select(i => new Stack<char>()).ToArray();

        for (var i = lines.Length - 2; i >= 0; i--)
        {
            var j = 0;
            for (var c = 1; c < lines[i].Length; c += 4)
            {
                var letter = lines[i][c];
                if (letter >= 'A' && letter <= 'Z')
                {
                    stacks[j].Push(letter);
                }
                j++;
            }
        }

        return stacks;
    }


    [GeneratedRegex("^move (?<count>\\d+) from (?<from>\\d+) to (?<to>\\d+)$")]
    private static partial Regex MakeInstructionMatch();
    private readonly static Regex InstructionMatch = MakeInstructionMatch();

    private static IEnumerable<Instruction> ParseInstructions(string[] strings)
    {
        foreach (var line in strings)
        {
            var (c, f, t) = InstructionMatch.Parse(line, ("count", int.Parse), ("from", int.Parse), ("to", int.Parse));
            yield return new Instruction(c, f, t);
        }
    }

    public override ValueTask<string> Solve_1()
    {
        var (stacks, instructions) = ParseInput(_lines);

        var stacker = new Stacker(stacks);
        foreach (var i in instructions)
        {
            stacker.Run(i);
        }

        return stacker.GetTop().ToResult();
    }

    public override ValueTask<string> Solve_2()
    {
        var (stacks, instructions) = ParseInput(_lines);

        var stacker = new Stacker9001(stacks);
        foreach (var i in instructions)
        {
            stacker.Run(i);
        }

        return stacker.GetTop().ToResult();
    }

    public class Stacker
    {
        protected readonly Stack<char>[] _piles;

        public Stacker(IEnumerable<Stack<char>> stacks)
        {
            _piles = stacks.ToArray();
        }

        public virtual void Run(Instruction instruction)
        {
            var from = instruction.From - 1;
            var to = instruction.To - 1;
            for (var i = 0; i < instruction.Count; i++)
            {
                _piles[to].Push(_piles[from].Pop());
            }
        }

        public string GetTop() => new(_piles.Select(s => s.Peek()).ToArray());
    }

    public class Stacker9001 : Stacker
    {
        public Stacker9001(IEnumerable<Stack<char>> stacks) : base(stacks)
        {
        }

        public override void Run(Instruction instruction)
        {
            var from = instruction.From - 1;
            var to = instruction.To - 1;
            var buf = new Stack<char>();

            for (var i = 0; i < instruction.Count; i++)
            {
                buf.Push(_piles[from].Pop());
            }

            for (var i = 0; i < instruction.Count; i++)
            {
                _piles[to].Push(buf.Pop());
            }
        }
    }

    public readonly struct Instruction
    {
        public readonly int Count;
        public readonly int From;
        public readonly int To;

        public Instruction(int count, int from, int to)
        {
            Count = count;
            From = from;
            To = to;
        }

        public override string ToString() => $"move {Count} from {From} to {To}";
    }
}
