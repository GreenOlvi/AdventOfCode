using NUnit.Framework;

namespace AOC2022.Puzzles;

public class Day24 : CustomBaseDay
{
    private readonly IEnumerable<Blizzard> _initialBlizzards;
    private readonly Box _box;
    private readonly Point2 _start;
    private readonly Point2 _end;

    public Day24()
    {
        (_initialBlizzards, _box, _start, _end) = ParseInput(ReadLinesFromFile());
    }

    public Day24(IEnumerable<string> lines)
    {
        (_initialBlizzards, _box, _start, _end) = ParseInput(lines);
    }

    private static (IEnumerable<Blizzard> Blizzards, Box Box, Point2 Start, Point2 End) ParseInput(IEnumerable<string> lines)
    {
        var board = lines as string[] ?? lines.ToArray();

        var start = new Point2(board[0].IndexOf('.'), 0); ;
        var end = new Point2(board.Last().IndexOf('.'), board.Length - 1);

        var box = new Box(new Point2(1,1), new Point2(board[0].Length - 2, board.Length - 2));

        var blizzards = new List<Blizzard>();
        for (var y = 1; y < board.Length - 1; y++)
        {
            var line = board[y];
            for (var x = 1; x < line.Length - 1; x++)
            {
                switch (line[x])
                {
                    case '.':
                        break;
                    case '^':
                        blizzards.Add(new Blizzard(new Point2(x, y), Direction.Up));
                        break;
                    case 'v':
                        blizzards.Add(new Blizzard(new Point2(x, y), Direction.Down));
                        break;
                    case '<':
                        blizzards.Add(new Blizzard(new Point2(x, y), Direction.Left));
                        break;
                    case '>':
                        blizzards.Add(new Blizzard(new Point2(x, y), Direction.Right));
                        break;
                    default:
                        throw new InvalidDataException(line[x].ToString());
                }
            }
        }

        return (blizzards, box, start, end);
    }

    private static HashSet<Point2> GetMap(IEnumerable<Blizzard> blizzards) =>
        new(blizzards.Select(b => b.Position));

    private static IEnumerable<Blizzard> StepBlizzards(IEnumerable<Blizzard> blizzards, Box box) =>
        blizzards.Select(b => b.Step(box));

    private static readonly Point2[] Directions = new[] { Point2.Down, Point2.Right, Point2.Up, Point2.Left, Point2.Zero };

    private static (int, IEnumerable<Blizzard>) FindWay(IEnumerable<Blizzard> initialBlizzards, Box box, Point2 start, Point2 end)
    {
        var t = 0;
        var blizzards = initialBlizzards as Blizzard[] ?? initialBlizzards.ToArray();

        var queue = new Queue<Point2>();
        queue.Enqueue(start);

        while (true)
        {
            t++;
            blizzards = StepBlizzards(blizzards, box).ToArray();
            var map = GetMap(blizzards);

            var newPoints = new HashSet<Point2>();
            while (queue.TryDequeue(out var e))
            {
                var possibleMoves = Directions.Select(d => e + d)
                    .Where(p => (box.IsInside(p) && !map.Contains(p)) || p == start || p == end)
                    .ToArray();

                foreach (var move in possibleMoves)
                {
                    if (move == end)
                    {
                        return (t, blizzards);
                    }

                    newPoints.Add(move);
                }
            }

            Assert.IsTrue(newPoints.Any(), "No more elements in queue");

            queue = new Queue<Point2>(newPoints);
        }
    }

    public override ValueTask<string> Solve_1()
    {
        var (t, _) = FindWay(_initialBlizzards, _box, _start, _end);
        return t.ToResult();
    }

    public override ValueTask<string> Solve_2()
    {
        var (t1, b1) = FindWay(_initialBlizzards, _box, _start, _end);
        var (t2, b2) = FindWay(b1, _box, _end, _start);
        var (t3, _) = FindWay(b2, _box, _start, _end);
        return (t1 + t2 + t3).ToResult();
    }

    private readonly record struct Blizzard(Point2 Position, Direction Direction)
    {
        public Blizzard Step(Box box)
        {
            var newPos = Position.Move(Direction);
            if (box.IsInside(newPos))
            {
                return this with { Position = newPos };
            }

            var wrapped = Direction switch
            {
                Direction.Up => new Point2(Position.X, box.BottomRight.Y),
                Direction.Down => new Point2(Position.X, box.TopLeft.Y),
                Direction.Left => new Point2(box.BottomRight.X, Position.Y),
                Direction.Right => new Point2(box.TopLeft.X, Position.Y),
                _ => throw new ArgumentOutOfRangeException(nameof(Direction)),
            };

            return this with { Position = wrapped };
        }
    }

    private readonly record struct Box(Point2 TopLeft, Point2 BottomRight)
    {
        public long Width => BottomRight.X - TopLeft.X + 1;
        public long Height => BottomRight.Y - TopLeft.Y + 1;

        public bool IsInside(Point2 point) =>
            TopLeft.X <= point.X &&
            TopLeft.Y <= point.Y &&
            BottomRight.X >= point.X &&
            BottomRight.Y >= point.Y;
    }
}
