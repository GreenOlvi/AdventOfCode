using Spectre.Console;

namespace AOC2022.Puzzles;

public class Day18 : CustomBaseDay
{
    private readonly Point3[] _points;

    public Day18()
    {
        _points = ParseInput(ReadLinesFromFile()).ToArray();
    }

    public Day18(IEnumerable<string> lines)
    {
        _points = ParseInput(lines).ToArray();
    }

    private static IEnumerable<Point3> ParseInput(IEnumerable<string> lines) =>
        lines.Select(l =>
        {
            var s = l.Split(',').Select(int.Parse).ToArray();
            return new Point3(s[0], s[1], s[2]);
        });

    private static readonly Point3[] _neighbours = new[]
    {
        new Point3(1, 0, 0), new Point3(-1, 0, 0),
        new Point3(0, 1, 0), new Point3(0, -1, 0),
        new Point3(0, 0, 1), new Point3(0, 0, -1),
    };

    private static IEnumerable<Point3> GetNeighbours(Point3 point) => _neighbours.Select(n => point + n);

    public override ValueTask<string> Solve_1()
    {
        var placed = new HashSet<Point3>();
        int CountNeighbours(Point3 point) => _neighbours.Select(n => point + n).Count(placed.Contains);

        var area = 0;
        foreach (var point in _points)
        {
            area = area + 6 - 2 * CountNeighbours(point);
            placed.Add(point);
        }

        return area.ToResult();
    }

    public override ValueTask<string> Solve_2()
    {
        var points = new HashSet<Point3>(_points);
        var max = new Point3(points.Max(p => p.X) + 1, points.Max(p => p.Y) + 1, points.Max(p => p.Z) + 1);
        var min = new Point3(points.Min(p => p.X) - 1, points.Min(p => p.Y) - 1, points.Min(p => p.Z) - 1);

        var box = new Box(min, max);

        var queue = new Queue<Point3>();
        queue.Enqueue(min);

        var visited = new HashSet<Point3>();
        var area = 0;

        while (queue.TryDequeue(out var point))
        {
            if (visited.Contains(point))
            {
                continue;
            }

            visited.Add(point);

            var n = GetNeighbours(point).Where(box.IsInside).ToArray();
            var touching = n.Count(points.Contains);
            area += touching;

            foreach (var p in n.Where(ne => !(points.Contains(ne) || visited.Contains(ne))))
            {
                queue.Enqueue(p);
            }
        }

        return area.ToResult();
    }

    private readonly record struct Box
    {
        public readonly Point3 Min;
        public readonly Point3 Max;

        public Box(Point3 min, Point3 max)
        {
            Min = min;
            Max = max;
        }

        public bool IsInside(Point3 point) =>
            point.X >= Min.X
            && point.X <= Max.X
            && point.Y >= Min.Y
            && point.Y <= Max.Y
            && point.Z >= Min.Z
            && point.Z <= Max.Z;
    }
}
