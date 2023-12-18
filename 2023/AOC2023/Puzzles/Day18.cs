using System.Globalization;

namespace AOC2023.Puzzles;
public partial class Day18 : CustomBaseDay
{
    private readonly Instruction[] _instructions;

    public Day18()
    {
        _instructions = ReadLinesFromFile().Select(ParseInstruction).ToArray();
    }

    public Day18(IEnumerable<string> lines)
    {
        _instructions = lines.Select(ParseInstruction).ToArray();
    }

    private static readonly Regex ColorPattern = BuildColorPattern();

    private static Instruction ParseInstruction(string line)
    {
        var parts = line.Split(' ', StringSplitOptions.TrimEntries);
        var dir = parts[0] switch
        {
            "U" => Direction.Up,
            "D" => Direction.Down,
            "L" => Direction.Left,
            "R" => Direction.Right,
            _ => throw new InvalidDataException($"Invalid direction '{parts[0]}'"),
        };

        var len = parts[1].ToInt();

        if (!ColorPattern.TryMatch(parts[2], out var match))
        {
            throw new InvalidCastException($"Invalid color '{parts[2]}'");
        }

        var color = Color.FromHex(match.Groups["value"].Value);

        return new Instruction(dir, len, color);
    }

    private static IEnumerable<Point2> GetPoints(IEnumerable<Instruction> instructions) 
    {
        var p = Point2.Zero;
        foreach (var instruction in instructions)
        {
            yield return p;
            p = p.Move(instruction.Direction, instruction.Length);
        }
    }

    private static IEnumerable<(T, T)> SlidingWindowWithWrap<T>(IEnumerable<T> elements)
    {
        var first = elements.First();
        var prev = first;
        foreach (var point in elements.Skip(1))
        {
            yield return (prev, point);
            prev = point;
        }
        yield return (prev, first);
    }

    private static long ShoelaceArea(IEnumerable<Point2> points)
    {
        var sum = 0L;
        foreach (var (a, b) in SlidingWindowWithWrap(points))
        {
            sum += a.X * b.Y - a.Y * b.X + Point2.ManhattanDistance(a, b);
        }
        return sum / 2 + 1;
    }

    public override ValueTask<string> Solve_1() =>
        ShoelaceArea(GetPoints(_instructions)).ToResult();

    public override ValueTask<string> Solve_2() =>
        ShoelaceArea(GetPoints(_instructions.Select(i => i.FromColor()))).ToResult();

    private readonly record struct Instruction(Direction Direction, int Length, Color Color)
    {
        public Instruction FromColor() => new(Color.Direction, Color.Length, Color);
    }

    private readonly record struct Color(uint Value, Direction Direction, int Length)
    {
        public static Color FromHex(string hex) => new(
            uint.Parse(hex, NumberStyles.HexNumber),
            DecodeDirection(hex[5]),
            int.Parse(hex[0..5], NumberStyles.HexNumber));

        private static Direction DecodeDirection(char c) => c switch
        {
            '0' => Direction.Right,
            '1' => Direction.Down,
            '2' => Direction.Left,
            '3' => Direction.Up,
            _ => throw new InvalidDataException($"Invalid direction '{c}'"),
        };

        public override string ToString() => $"#{Value:x6} = {Direction} {Length}";
    }

    [GeneratedRegex(@"\(#(?<value>[0-9a-f]{6})\)")]
    private static partial Regex BuildColorPattern();
}
