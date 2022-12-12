namespace AOC2022.Puzzles;

public class Day12 : CustomBaseDay
{
    private readonly Grid<int> _grid;
    private readonly Point2 _start;
    private readonly Point2 _end;

    public Day12()
    {
        (_grid, _start, _end) = ParseInput(ReadLinesFromFile());
    }

    public Day12(IEnumerable<string> lines)
    {
        (_grid, _start, _end) = ParseInput(lines);
    }

    private static (Grid<int> Grid, Point2 Start, Point2 End)  ParseInput(IEnumerable<string> lines)
    {
        Point2? start = null;
        Point2? end = null;

        var rows = new List<int[]>();
        var y = 0;
        foreach (var line in lines)
        {
            rows.Add(line.Select((ch, x) =>
            {
                if (ch == 'S')
                {
                    start = new Point2(x, y);
                    ch = 'a';
                }
                if (ch == 'E')
                {
                    end = new Point2(x, y);
                    ch = 'z';
                }
                return ch - 'a';
            }).ToArray());
            y++;
        }

        if (start is null || end is null)
        {
            throw new InvalidDataException();
        }

        return (new Grid<int>(rows.ToArray()), start.Value, end.Value);
    }

    public override ValueTask<string> Solve_1()
    {
        var dist = FindDistances(_grid, _start, _end);
        return dist[_end].ToResult();
    }

    private static Dictionary<Point2, int> FindDistances(Grid<int> grid, Point2 s, Point2 e)
    {
        double Weight(Point2 point) => - point.DistanceSquaredTo(e);

        var dist = new Dictionary<Point2, int>
        {
            [s] = 0
        };

        var queue = new PriorityQueue<Point2, double>();
        queue.Enqueue(s, 0);

        while (queue.TryDequeue(out var p, out _))
        {
            var steps = dist[p];
            foreach (var n in GetNeighboursFrom(grid, p))
            {
                if (dist.TryGetValue(n, out var d))
                {
                    if (d > steps + 1)
                    {
                        dist[n] = steps + 1;
                        queue.Enqueue(n, Weight(n));
                    }
                }
                else
                {
                    dist.Add(n, steps + 1);
                    queue.Enqueue(n, Weight(n));
                }
            }
        }

        return dist;
    }

    private static readonly Point2[] Neighbours = new[] { Point2.Up, Point2.Down, Point2.Left, Point2.Right };
    private static IEnumerable<Point2> GetNeighboursFrom(Grid<int> grid, Point2 p) =>
        Neighbours.Select(n => p + n).Where(grid.IsInside).Where(n => grid[n] - 1 <= grid[p]);

    public override ValueTask<string> Solve_2()
    {
        var aPoints = _grid.EnumeratePoints().Where(p => _grid[p] == 0).ToArray();

        var lowestDist = int.MaxValue;
        foreach (var a in aPoints)
        {
            var dist = FindDistances(_grid, a, _end);
            if (dist.TryGetValue(_end, out var d) && d < lowestDist)
            {
                lowestDist = d;
            }
        }

        return lowestDist.ToResult();
    }
}
