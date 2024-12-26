namespace AOC2024.Puzzles;

public class Day12 : CustomBaseProblem<long>
{
    private readonly IHashGrid2<char> _garden;

    public Day12()
    {
        _garden = ParseInput(ReadLinesFromFile());
    }

    public Day12(IEnumerable<string> lines)
    {
        _garden = ParseInput(lines);
    }

    private static IHashGrid2<char> ParseInput(IEnumerable<string> lines)
    {
        var grid = new HashGrid<char>();

        var y = 0;
        foreach (var line in lines)
        {
            var x = 0;
            foreach (var c in line.Trim())
            {
                grid[(x, y)] = c;
                x++;
            }
            y++;
        }

        return grid.ToFrozen();
    }

    private static readonly Point2[] Directions = [Point2.Up, Point2.Down, Point2.Left, Point2.Right];

    private static int NewEdges(int neighbours) => neighbours switch
    {
        0 => 4,
        1 => 2,
        2 => 0,
        3 => -2,
        4 => -4,
        _ => throw new InvalidOperationException("Invalid number of neighbours"),
    };

    private static long GetFencePrice(IEnumerable<Point2> plotPoints)
    {
        var perimiter = 0L;
        var plot = new HashSet<Point2>();
        foreach (var p in plotPoints)
        {
            var n = Directions.Where(d => plot.Contains(p + d)).Count();
            perimiter += NewEdges(n);
            _ = plot.Add(p);
        }
        return plot.Count * perimiter;
    }

    private static long GetFencePrice2(IEnumerable<Point2> plot)
    {
        var points = new HashSet<Point2>(plot);
        var sides = 0;
        foreach (var p in points)
        {
            var up = p + Point2.Up;
            var right = p + Point2.Right;
            var down = p + Point2.Down;
            var left = p + Point2.Left;

            // Outside corner
            if (!points.Contains(up) && !points.Contains(left))
            {
                sides++;
            }

            if (!points.Contains(up) && !points.Contains(right))
            {
                sides++;
            }

            if (!points.Contains(down) && !points.Contains(right))
            {
                sides++;
            }

            if (!points.Contains(down) && !points.Contains(left))
            {
                sides++;
            }

            // Inside corner
            if (points.Contains(up) && points.Contains(left) && !points.Contains(p + Point2.Up + Point2.Left))
            {
                sides++;
            }

            if (points.Contains(up) && points.Contains(right) && !points.Contains(p + Point2.Up + Point2.Right))
            {
                sides++;
            }

            if (points.Contains(down) && points.Contains(right) && !points.Contains(p + Point2.Down + Point2.Right))
            {
                sides++;
            }

            if (points.Contains(down) && points.Contains(left) && !points.Contains(p + Point2.Down + Point2.Left))
            {
                sides++;
            }
        }

        return points.Count * sides;
    }

    private static IEnumerable<IEnumerable<Point2>> SplitPlots(IHashGrid2<char> garden)
    {
        var all = new HashSet<Point2>(garden.Select(pair => pair.Position));
        while (all.Count > 0)
        {
            var p = all.First();
            var letter = garden[p];
            var inPlot = new HashSet<Point2>() { p };
            var queue = new Queue<Point2>();
            queue.Enqueue(p);
            _ = all.Remove(p);

            while (queue.TryDequeue(out var point))
            {

                var neighbours = Directions.Select(d => point + d)
                    .Where(np => garden[np] == letter && !inPlot.Contains(np));

                foreach (var n in neighbours)
                {
                    _ = all.Remove(n);
                    queue.Enqueue(n);
                    _ = inPlot.Add(n);
                }
            }

            yield return inPlot;
        }
    }

    public override long Solve1() =>
        SplitPlots(_garden).Select(GetFencePrice).Sum();

    public override long Solve2() =>
        SplitPlots(_garden).Select(GetFencePrice2).Sum();
}
