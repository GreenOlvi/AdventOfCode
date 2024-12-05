namespace AOC2024.Puzzles;

public class Day04 : CustomBaseProblem<long>
{
    private readonly HashGrid<char> _grid;

    public Day04()
    {
        _grid = ParseInput(ReadLinesFromFile());
    }

    public Day04(IEnumerable<string> lines)
    {
        _grid = ParseInput(lines);
    }

    private static HashGrid<char> ParseInput(IEnumerable<string> lines)
    {
        var grid = new HashGrid<char>();
        var y = 0;
        foreach (var line in lines)
        {
            var x = 0;
            foreach (var c in line)
            {
                grid[(x, y)] = c;
                x++;
            }
            y++;
        }
        return grid;
    }

    private static readonly Point2 UpLeft = Point2.Up + Point2.Left;
    private static readonly Point2 UpRight = Point2.Up + Point2.Right;
    private static readonly Point2 DownLeft = Point2.Down + Point2.Left;
    private static readonly Point2 DownRight = Point2.Down + Point2.Right;

    private static readonly Point2[] Directions = [
        Point2.Right,
        DownRight,
        Point2.Down,
        DownLeft,
        Point2.Left,
        UpLeft,
        Point2.Up,
        UpRight,
    ];

    private static bool CheckInDirection(HashGrid<char> grid, Point2 position, Point2 direction, string word)
    {
        var i = 0;
        var p = position;
        while (i < word.Length)
        {
            p += direction;
            var c = word[i];
            if (grid[p] != c)
            {
                return false;
            }
            i++;
        }

        return true;
    }

    private static int CheckForWord(HashGrid<char> grid, Point2 start, string word) =>
        Directions.Count(d => CheckInDirection(grid, start, d, word));

    private static bool CheckForMas(HashGrid<char> grid, Point2 position)
    {
        var tl = grid[position + UpLeft];
        var tr = grid[position + UpRight];
        var bl = grid[position + DownLeft];
        var br = grid[position + DownRight];
        return ((tl == 'M' && br == 'S') || (tl == 'S' && br == 'M'))
            && ((tr == 'M' && bl == 'S') || (tr == 'S' && bl == 'M'));
    }

    public override long Solve1() =>
        _grid.FindTiles(t => t == 'X')
            .Sum(x => CheckForWord(_grid, x, "MAS"));

    public override long Solve2() =>
        _grid.FindTiles(static t => t == 'A')
            .Count(p => CheckForMas(_grid, p));
}
