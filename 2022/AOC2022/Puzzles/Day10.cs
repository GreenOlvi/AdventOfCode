using System.Text;

namespace AOC2022.Puzzles;

public class Day10 : CustomBaseDay
{
    private readonly Instruction[] _input;

    public Day10()
    {
        _input = ParseInput(ReadLinesFromFile()).ToArray();

    }

    public Day10(IEnumerable<string> lines)
    {
        _input = ParseInput(lines).ToArray();
    }

    private static IEnumerable<Instruction> ParseInput(IEnumerable<string> lines) =>
        lines.Select<string, Instruction>(l =>
            {
                if (l.StartsWith("noop"))
                {
                    return NoopInstance;
                }
                if (l.StartsWith("addx"))
                {
                    var v = l.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)[1];
                    return new Addx(int.Parse(v));
                }
                throw new InvalidDataException();
            });

    private IEnumerable<int> GenerateSignal()
    {
        var x = 1;
        foreach (var instruction in _input)
        {
            if (instruction is Noop)
            {
                yield return x;
            }
            else if (instruction is Addx add)
            {
                yield return x;
                yield return x;
                x += add.Value;
            }
        }
    }

    public override ValueTask<string> Solve_1()
    {
        var s = GenerateSignal().ToArray();
        var result = new[] { 20, 60, 100, 140, 180, 220 }
            .Select(c => c * s[c - 1])
            .Sum();

        return result.ToResult();
    }

    public char FullBlock = '█';
    public char EmptyBlock = ' ';

    public override ValueTask<string> Solve_2()
    {
        var position = GenerateSignal();
        var sb = new StringBuilder();

        var cycle = 1;
        foreach (var x in position)
        {
            var drawing = ((cycle - 1) % 40);
            if (Math.Abs(drawing - x) <= 1)
            {
                sb.Append(FullBlock);
            }
            else
            {
                sb.Append(EmptyBlock);
            }
            if (cycle % 40 == 0)
            {
                sb.Append('\n');
            }
            cycle++;
        }

        return sb.ToString().ToResult();
    }

    private interface Instruction { }
    private readonly record struct Noop : Instruction;
    private static readonly Noop NoopInstance = new();
    private readonly record struct Addx(int Value) : Instruction;
}
