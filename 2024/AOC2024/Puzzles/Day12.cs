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

    private static long GetFencePrice(IEnumerable<Point2> grid)
    {
        var perimiter = 0L;
        var plot = new HashSet<Point2>();
        foreach (var p in grid)
        {
            var n = Directions.Where(d => plot.Contains(p + d)).Count();
            perimiter += NewEdges(n);
            _ = plot.Add(p);
        }
        return plot.Count * perimiter;
    }

    public override long Solve1()
    {
        var plots = _garden.GroupBy(static g => g.Tile)
            .ToDictionary(g => g.Key, g => GetFencePrice(g.Select(p => p.Position)));
        // .Select(static g => GetFencePrice(g.Select(static p => p.Position)))
        // .Sum();
        //
        return plots.Values.Sum();
    }

    public override long Solve2()
    {
        return default;
    }
}
