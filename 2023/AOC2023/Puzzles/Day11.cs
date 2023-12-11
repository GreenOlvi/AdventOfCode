namespace AOC2023.Puzzles;
public class Day11 : CustomBaseDay
{
    private readonly GalaxyMap _map;

    public long ExpansionMultiplier { get; init; } = 1_000_000;

    public Day11()
    {
        _map = ParseGalaxies(ReadLinesFromFile());
    }

    public Day11(IEnumerable<string> lines)
    {
        _map = ParseGalaxies(lines);
    }

    private static GalaxyMap ParseGalaxies(IEnumerable<string> lines)
    {
        var galaxies = new List<Point2>();
        var y = 0;
        foreach (var line in lines)
        {
            var x = 0;
            foreach (var c in  line)
            {
                if (c == '#')
                {
                    galaxies.Add(new Point2(x, y));
                }
                x++;
            }
            y++;
        }

        var ys = galaxies.Select(g => g.Y).ToHashSet();
        var emptyRows = Enumerable.Range(0, (int)ys.Max()).Where(y => !ys.Contains(y)).ToList();

        var xs = galaxies.Select(g => g.X).ToHashSet();
        var emptyCols = Enumerable.Range(0, (int)xs.Max()).Where(x => !xs.Contains(x)).ToList();

        return new GalaxyMap(galaxies, emptyRows, emptyCols);
    }

    private static long Distance((Point2, Point2) pair) =>
        Math.Abs(pair.Item1.X - pair.Item2.X) + Math.Abs(pair.Item1.Y - pair.Item2.Y);

    public override ValueTask<string> Solve_1() =>
        Utils.EachPair(_map.Expand()).Sum(Distance).ToResult();

    public override ValueTask<string> Solve_2() =>
        Utils.EachPair(_map.Expand(ExpansionMultiplier)).Sum(Distance).ToResult();

    private class GalaxyMap(List<Point2> Galaxies, List<int> EmptyRows, List<int> EmptyColumns)
    {
        public List<Point2> Galaxies { get; } = Galaxies;
        public List<int> EmptyRows { get; } = EmptyRows;
        public List<int> EmptyColumns { get; } = EmptyColumns;

        public IEnumerable<Point2> Expand(long multiplier = 2)
        {
            foreach (var g in Galaxies)
            {
                var ex = EmptyColumns.Count(ec => ec < g.X);
                var ey = EmptyRows.Count(er => er < g.Y);
                yield return new Point2(g.X + ex * (multiplier - 1), g.Y + ey * (multiplier - 1));
            }
        }
    }
}
