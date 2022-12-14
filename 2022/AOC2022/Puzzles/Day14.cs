using System.Text;

namespace AOC2022.Puzzles;

public class Day14 : CustomBaseDay
{
    private readonly Point2[][] _rocks;

    public Day14()
    {
        _rocks = ParseInput(ReadLinesFromFile()).ToArray();
    }

    public Day14(IEnumerable<string> lines)
    {
        _rocks = ParseInput(lines).ToArray();
    }

    private static IEnumerable<Point2[]> ParseInput(IEnumerable<string> lines) => lines.Select(ParseLine);

    private static Point2[] ParseLine(string line) =>
        line.Split("->", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(p =>
            {
                var c = p.Split(",");
                return new Point2(int.Parse(c[0]), int.Parse(c[1]));
            })
            .ToArray();

    private static void FillRock(HashGrid grid, IEnumerable<Point2[]> lines)
    {
        foreach (var line in lines)
        {
            var p = line[0];
            foreach (var point in line.Skip(1))
            {
                foreach (var linePoints in DrawLine(p, point))
                {
                    grid[linePoints] = Tile.Rock;
                }
                p = point;
            }
        }
    }

    private static IEnumerable<Point2> DrawLine(Point2 from, Point2 to)
    {
        if (from.X == to.X)
        {
            if (from.Y <= to.Y)
            {
                for (var i = from.Y; i <= to.Y; i++)
                {
                    yield return new Point2(from.X, i);
                }
            }
            else
            {
                for (var i = from.Y; i >= to.Y; i--)
                {
                    yield return new Point2(from.X, i);
                }
            }
            yield break;
        }

        if (from.Y == to.Y)
        {
            if (from.X <= to.X)
            {
                for (var i = from.X; i <= to.X; i++)
                {
                    yield return new Point2(i, from.Y);
                }
            }
            else
            {
                for (var i = from.X; i >= to.X; i--)
                {
                    yield return new Point2(i, from.Y);
                }
            }
            yield break;
        }

        throw new InvalidOperationException();
    }

    private static readonly Point2 DownLeft = Point2.Down + Point2.Left;
    private static readonly Point2 DownRight = Point2.Down + Point2.Right;

    private static bool Simulate(HashGrid grid, Point2 point2, long lowerLimit)
    {
        var sand = point2;
        while(true)
        {
            var down = sand.Move(Direction.Down);
            if (down.Y >= lowerLimit)
            {
                return true;
            }

            if (grid[down] == Tile.Air)
            {
                sand = down;
                continue;
            }

            var dl = sand + DownLeft;
            if (grid[dl] == Tile.Air)
            {
                sand = dl;
                continue;
            }

            var dr = sand + DownRight;
            if (grid[dr] == Tile.Air)
            {
                sand = dr;
                continue;
            }

            break;
        }

        grid[sand] = Tile.Sand;
        return false;
    }

    private static bool Simulate2(HashGrid grid, Point2 point2, long floor)
    {
        var sand = point2;
        while(true)
        {
            var down = sand.Move(Direction.Down);
            if (down.Y >= floor)
            {
                break;
            }

            if (grid[down] == Tile.Air)
            {
                sand = down;
                continue;
            }

            var dl = sand + DownLeft;
            if (grid[dl] == Tile.Air)
            {
                sand = dl;
                continue;
            }

            var dr = sand + DownRight;
            if (grid[dr] == Tile.Air)
            {
                sand = dr;
                continue;
            }

            break;
        }

        grid[sand] = Tile.Sand;
        return sand.X == 500 && sand.Y == 0;
    }

    public override ValueTask<string> Solve_1()
    {
        var grid = new HashGrid();
        FillRock(grid, _rocks);
        grid[(500, 0)] = Tile.Source;

        var limit = grid.LowerBorder;
        var finished = false;
        var i = 0;
        while (!finished)
        {
            finished = Simulate(grid, new Point2(500, 0), limit);
            i++;
        }

        return (i - 1).ToResult();
    }

    public override ValueTask<string> Solve_2()
    {
        var grid = new HashGrid();
        FillRock(grid, _rocks);
        grid[(500, 0)] = Tile.Source;

        var limit = grid.LowerBorder;
        var finished = false;
        var i = 0;
        while (!finished)
        {
            finished = Simulate2(grid, new Point2(500, 0), limit);
            i++;
        }

        return i.ToResult();
    }

    public enum Tile
    {
        Air,
        Rock,
        Sand,
        Source,
    };

    public class HashGrid
    {
        private readonly Dictionary<Point2, Tile> _tiles = new();

        public Tile this[(int x, int y) p]
        {
            get => _tiles.TryGetValue(new Point2(p), out var tile) ? tile : Tile.Air;
            set => _tiles[new Point2(p)] = value;
        }

        public Tile this[Point2 p]
        {
            get => _tiles.TryGetValue(p, out var tile) ? tile : Tile.Air;
            set => _tiles[p] = value;
        }

        private static readonly Dictionary<Tile, char> _tileChars = new()
        {
            [Tile.Air] = '.',
            [Tile.Rock] = '#',
            [Tile.Sand] = 'o',
            [Tile.Source] = '*',
        };

        public long LowerBorder => _tiles.Keys.Max(p => p.Y) + 2;

        public string Draw()
        {
            var sb = new StringBuilder();

            var set = _tiles.Keys;
            var minX = set.Min(p => p.X) - 1;
            var minY = set.Min(p => p.Y) - 1;
            var maxX = set.Max(p => p.X) + 1;
            var maxY = set.Max(p => p.Y) + 1;

            for (var y = minY; y <= maxY; y++)
            {
                for (var x = minX; x <= maxX; x++)
                {
                    var p = new Point2(x, y);
                    var ch = _tiles.TryGetValue(p, out var v) ? _tileChars[v] : _tileChars[Tile.Air];
                    sb.Append(ch);
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
