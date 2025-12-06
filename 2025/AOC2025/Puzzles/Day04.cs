
namespace AOC2025.Puzzles;

public class Day04 : CustomBaseProblem<long>
{
    private readonly HashGrid<(bool HasRoll, byte Neighbours)> _input;

    public Day04()
    {
        _input = ParseInput(ReadLinesFromFile());
    }

    public Day04(IEnumerable<string> lines)
    {
        _input = ParseInput(lines);
    }

    private static HashGrid<(bool, byte)> ParseInput(IEnumerable<string> lines)
    {
        var grid = new HashGrid<(bool HasRoll, byte Neighbours)>();

        var y = 0;
        foreach (var line in lines)
        {
            var x = 0;
            foreach (var c in line)
            {
                if (c == '@')
                {
                    var p = new Point2(x, y);
                    grid[p] = (true, 0);
                }
                x++;
            }
            y++;
        }

        foreach (var (pos, _) in grid.Where(r => r.Tile.HasRoll))
        {
            var n = (byte)GetAdjacent(pos).Count(p => grid[p].HasRoll);
            grid[pos] = (true, n);
        }

        return grid;
    }

    private static IEnumerable<Point2> GetAdjacent(Point2 p) => [
        p + new Point2(-1, -1),
        p + new Point2(0, -1),
        p + new Point2(1, -1),
        p + new Point2(1, 0),
        p + new Point2(1, 1),
        p + new Point2(0, 1),
        p + new Point2(-1, 1),
        p + new Point2(-1, 0),
    ];

    public override long Solve1() => _input.Count(t => t.Tile.Neighbours < 4);

    public override long Solve2()
    {
        var grid = _input.Clone();

        var toRemove = grid.Where(t => t.Tile.Neighbours < 4)
            .Select(t => t.Position)
            .ToList();

        var allRemoved = 0L;

        while (toRemove.Count > 0)
        {
            allRemoved += toRemove.Count;

            foreach (var pos in toRemove)
            {
                grid[pos] = (false, 0);
            }

            var toReduce = toRemove.SelectMany(GetAdjacent)
                .Where(p => grid[p].HasRoll)
                .GroupBy(p => p)
                .Select(g => (Position: g.Key, Count: g.Count()))
                .ToArray();

            toRemove.Clear();

            foreach (var (pos, count) in toReduce)
            {
                var (_, neighbours) = grid[pos];

                var newNeighbours = (byte)(neighbours - count > 0 ? neighbours - count : 0);
                grid[pos] = (true, newNeighbours);

                if (newNeighbours < 4)
                {
                    toRemove.Add(pos);
                }
            }
        }

        return allRemoved;
    }
}
