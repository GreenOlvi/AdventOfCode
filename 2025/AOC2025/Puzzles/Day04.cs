
namespace AOC2025.Puzzles;

public class Day04 : CustomBaseProblem<long>
{
    private readonly HashGrid<bool> _input;

    public Day04()
    {
        _input = ParseInput(ReadLinesFromFile());
    }

    public Day04(IEnumerable<string> lines)
    {
        _input = ParseInput(lines);
    }

    private static HashGrid<bool> ParseInput(IEnumerable<string> lines)
    {
        var grid = new HashGrid<bool>();

        var y = 0;
        foreach (var line in lines)
        {
            var x = 0;
            foreach (var c in line)
            {
                if (c == '@')
                {
                    var p = new Point2(x, y);
                    grid[p] = true;
                }
                x++;
            }
            y++;
        }

        return grid;
    }

    public override long Solve1()
    {
        return _input.Where(t => GetAdjacent(t.Position).Where(a => _input[a]).Count() < 4).Count();
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

    public override long Solve2()
    {
        return default;
    }

    private enum Tiles
    {
        Empty = 0,
        Roll,
    }
}
