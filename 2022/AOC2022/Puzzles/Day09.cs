namespace AOC2022.Puzzles;

public partial class Day09 : CustomBaseDay
{
    private readonly (Direction, int)[] _moves;

    public Day09()
    {
        _moves = ParseInput(ReadLinesFromFile()).ToArray();
    }

    public Day09(IEnumerable<string> lines)
    {
        _moves = ParseInput(lines).ToArray();
    }


    [GeneratedRegex("^(?<dir>[UDLR]) (?<count>\\d+)$", RegexOptions.Compiled)]
    private static partial Regex MakeInputPattern();
    private static readonly Regex InputPattern = MakeInputPattern();

    private static IEnumerable<(Direction, int)> ParseInput(IEnumerable<string> lines) =>
        lines.Select(line => InputPattern.Parse(line, ("dir", ParseDirection), ("count", int.Parse)));

    private static Direction ParseDirection(string d) =>
        d switch
        {
            "U" => Direction.Up,
            "D" => Direction.Down,
            "L" => Direction.Left,
            "R" => Direction.Right,
            _ => throw new InvalidDataException(d),
        };

    private static Point2 UpdateTail(Point2 head, Point2 tail)
    {
        var diff = tail - head;
        var normDiff = diff.Normalize();
        if (diff != Point2.Zero && diff != normDiff)
        {
            tail -= normDiff;
        }
        return tail;
    }

    private static (Point2 H, Point2 T) Move((Point2 H, Point2 T) position, (Direction D, int C) cmd, HashSet<Point2> visited)
    {
        var (h, t) = position;
        for (var i = 0; i < cmd.C; i++)
        {
            h = h.Move(cmd.D);
            t = UpdateTail(h, t);
            visited.Add(t);
        }
        return (h, t);
    }

    private static Point2[] Move(Point2[] rope, (Direction D, int C) cmd, HashSet<Point2> visited)
    {
        var direction = cmd.D;
        for (var i = 0; i < cmd.C; i++)
        {
            rope[0] = rope[0].Move(direction);

            for (var r = 0; r < rope.Length - 1; r++)
            {
                var t = UpdateTail(rope[r], rope[r + 1]);
                if (t == rope[r + 1])
                {
                    break;
                }
                rope[r + 1] = t;
            }
        
            visited.Add(rope[9]);
        }

        //Console.WriteLine(Point2.PrintPoints(rope));
        return rope;
    }

    public override ValueTask<string> Solve_1()
    {
        var pos = (Point2.Zero, Point2.Zero);

        var visited = new HashSet<Point2>() { Point2.Zero };
        foreach (var d in _moves)
        {
            pos = Move(pos, d, visited);
        }

        return visited.Count.ToResult();
    }

    public override ValueTask<string> Solve_2()
    {
        var rope = Enumerable.Range(0, 10).Select(i => Point2.Zero).ToArray();

        var visited = new HashSet<Point2>() { Point2.Zero };
        foreach (var d in _moves)
        {
            rope = Move(rope, d, visited);
        }

        return visited.Count.ToResult();
    }
}
