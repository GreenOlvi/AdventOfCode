
namespace AOC2024.Puzzles;

public class Day08 : CustomBaseProblem<long>
{
    private readonly HashGrid<char> _grid;
    private readonly Box _bounds;

    public Day08()
    {
        (_grid, _bounds) = ParseInput(ReadLinesFromFile());
    }

    public Day08(IEnumerable<string> lines)
    {
        (_grid, _bounds) = ParseInput(lines);
    }

    private static bool IsFrequency(char c) => c is (>= 'a' and <= 'z') or (>= 'A' and <= 'Z') or (>= '0' and <= '9');

    private static (HashGrid<char>, Box) ParseInput(IEnumerable<string> lines)
    {
        var grid = new HashGrid<char>();
        var maxX = 0;
        var y = 0;
        foreach (var line in lines)
        {
            var x = 0;
            foreach (var c in line)
            {
                if (IsFrequency(c))
                {
                    grid[(x, y)] = c;
                }
                x++;
            }
            if (x > maxX)
            {
                maxX = x;
            }
            y++;
        }
        return (grid, new Box(0, 0, maxX - 1, y - 1));
    }

    private static IEnumerable<Point2> GetAntinodes(Point2 a, Point2 b)
    {
        yield return a + (a - b);
        yield return b + (b - a);
    }

    private static IEnumerable<Point2> GetMoreAntinodes(Point2 a, Point2 b, Box bounds)
    {
        var diff = a - b;

        var antinode = a;
        while (bounds.IsInside(antinode))
        {
            yield return antinode;
            antinode += diff;
        }

        antinode = b;
        while (bounds.IsInside(antinode))
        {
            yield return antinode;
            antinode -= diff;
        }
    }

    public override long Solve1() =>
        _grid.GroupBy(static t => t.Tile)
            .SelectMany(static freq =>
                freq.EachPair()
                    .SelectMany(static p => GetAntinodes(p.Item1.Position, p.Item2.Position)))
            .Distinct()
            .Count(_bounds.IsInside);

    public override long Solve2() =>
        _grid.GroupBy(static t => t.Tile)
            .SelectMany(freq =>
                freq.EachPair()
                    .SelectMany(p => GetMoreAntinodes(p.Item1.Position, p.Item2.Position, _bounds)))
            .Distinct()
            .Count();
}
