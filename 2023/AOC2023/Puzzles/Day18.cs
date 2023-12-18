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

    private Instruction ParseInstruction(string line)
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

    private int DigTerrain(HashGrid<bool> grid, IEnumerable<Instruction> instructions)
    {
        var dug = 0;
        var p = new Point2(0, 0);
        grid[p] = true;
        foreach (var instruction in instructions)
        {
            for (var i = 0; i < instruction.Length; i++)
            {
                p = p.Move(instruction.Direction);
                grid[p] = true;
                dug++;
            }
        }
        return dug;
    }

    private int FloodFill(HashGrid<bool> grid, Point2 start)
    {
        var queue = new Queue<Point2>();
        queue.Enqueue(start);

        var painted = new HashSet<Point2>();

        while (queue.Count > 0)
        {
            var p = queue.Dequeue();
            if (painted.Contains(p))
            {
                continue;
            }

            painted.Add(p);

            var neighbours = DirectionExtensions.AllExceptNone
                .Select(p.Move)
                .Where(n => !grid[n]);

            foreach (var n in neighbours)
            {
                queue.Enqueue(n);
            }
        }

        return painted.Count;
    }

    public override ValueTask<string> Solve_1()
    {
        var grid = new HashGrid<bool>();

        var edge = DigTerrain(grid, _instructions);
        var inside = FloodFill(grid, new Point2(1, 1));

        return (edge + inside).ToResult();
    }

    public override ValueTask<string> Solve_2()
    {
        return "result 2".ToResult();
    }

    private readonly record struct Instruction(Direction Direction, int Length, Color Color);

    private readonly record struct Color(uint Value)
    {
        public static Color FromHex(string hex) => new(uint.Parse(hex, NumberStyles.HexNumber));

        public override string ToString() => $"#{Value:x6}";
    }

    [GeneratedRegex(@"\(#(?<value>[0-9a-f]{6})\)")]
    private static partial Regex BuildColorPattern();
}
