using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace AOC2024.Puzzles;

public partial class Day14 : CustomBaseProblem<long>
{
    private readonly Robot[] _robots;

    public int Width { get; init; } = 101;
    public int Height { get; init; } = 103;

    public Day14()
    {
        _robots = ParseInput(ReadLinesFromFile()).ToArray();
    }

    public Day14(IEnumerable<string> lines)
    {
        _robots = ParseInput(lines).ToArray();
    }

    [GeneratedRegex(@"^p=(?<px>-?\d+),(?<py>-?\d+) v=(?<vx>-?\d+),(?<vy>-?\d+)$")]
    private static partial Regex LinePatternGenerator();

    private readonly Regex LinePattern = LinePatternGenerator();

    private IEnumerable<Robot> ParseInput(IEnumerable<string> lines)
    {
        foreach (var line in lines)
        {
            var m = LinePattern.Match(line);
            if (!m.Success)
            {
                throw new InvalidDataException("Line does not match pattern");
            }

            var px = m.Groups["px"].Value.ToLong();
            var py = m.Groups["py"].Value.ToLong();
            var vx = m.Groups["vx"].Value.ToLong();
            var vy = m.Groups["vy"].Value.ToLong();
            yield return new Robot(new Point2(px, py), new Point2(vx, vy));
        }
    }

    private static Point2 Contain(Point2 p, Point2 box) => new(p.X.Modulo(box.X), p.Y.Modulo(box.Y));

    private static Robot Simulate(Robot r, long steps, Point2 box) =>
        new(Contain(r.Position + (r.Velocity * steps), box), r.Velocity);

    private static IEnumerable<Robot> Simulate(IEnumerable<Robot> robots, long steps, Point2 box) =>
        robots.Select(r => Simulate(r, steps, box));

    public override long Solve1()
    {
        var box = new Point2(Width, Height);
        var middle = new Point2(Width / 2, Height / 2);
        var newRobots = Simulate(_robots, 100, box).ToArray();

        var q = new long[4];
        foreach (var r in newRobots)
        {
            if (r.Position.X < middle.X && r.Position.Y < middle.Y)
            {
                q[0]++;
            }
            else if (r.Position.X > middle.X && r.Position.Y < middle.Y)
            {
                q[1]++;
            }
            else if (r.Position.X < middle.X && r.Position.Y > middle.Y)
            {
                q[2]++;
            }
            else if (r.Position.X > middle.X && r.Position.Y > middle.Y)
            {
                q[3]++;
            }
        }

        return q.Product();
    }

    private static void PrintRobots(IEnumerable<Robot> robots)
    {
        var grid = new HashGrid<bool>(robots.DistinctBy(static r => r.Position).Select(static r => (r.Position, true)));
        Console.WriteLine(grid.Draw());
    }

    private static long AutoFind(IEnumerable<Robot> robots, Point2 box)
    {
        var end = box.X * box.Y;
        var varCollection = new ConcurrentBag<(double, double, long)>();

        _ = Parallel.For(0, end, i =>
        {
            var r = Simulate(robots, i, box).ToArray();
            var avgX = r.Sum(static r => r.Position.X) / (r.Length + .0);
            var avgY = r.Sum(static r => r.Position.Y) / (r.Length + .0);
            var varX = r.Sum(r => Math.Abs(r.Position.X - avgX));
            var varY = r.Sum(r => Math.Abs(r.Position.Y - avgY));

            varCollection.Add((varX, varY, i));
        });

        return varCollection.MinBy(t => t.Item1 + t.Item2).Item3;
    }

    private static long InteractiveSearch(IEnumerable<Robot> robots, Point2 box, long start = 0)
    {
        var i = start;
        while (true)
        {
            var r = Simulate(robots, i, box).ToArray();
            PrintRobots(r);
            Console.WriteLine(i);
            var k = Console.ReadKey();
            if (k.Key == ConsoleKey.Enter)
            {
                return i;
            }
            i += k.Key switch
            {
                ConsoleKey.RightArrow => box.X,
                ConsoleKey.LeftArrow => -box.X,
                ConsoleKey.DownArrow => 1,
                ConsoleKey.UpArrow => -1,
                _ => 0,
            };
        }
    }

    public override long Solve2()
    {
        var box = new Point2(Width, Height);
        // var i = InteractiveSearch(_robots, box, 8087);
        var i = AutoFind(_robots, box);
        return i;
    }

    private readonly record struct Robot(Point2 Position, Point2 Velocity);
}
