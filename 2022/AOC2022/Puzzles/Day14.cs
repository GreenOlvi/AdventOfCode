using System.Runtime.CompilerServices;
using System.Text;

namespace AOC2022.Puzzles;

public class Day14 : CustomBaseDay
{
    private readonly string[] _lines;

    public Day14()
    {
        _lines = ReadLinesFromFile().ToArray();
    }

    public Day14(IEnumerable<string> lines)
    {
        _lines = lines.ToArray();
    }

    private static IEnumerable<Point2[]> ParseInput(IEnumerable<string> lines)
    {
        return lines.Select(ParseLine);
    }

    private static Point2[] ParseLine(string line) =>
        line.Split("->", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(p =>
            {
                var c = p.Split(",");
                return new Point2(int.Parse(c[0]), int.Parse(c[1]));
            })
            .ToArray();

    private void FillRock(HashGrid grid, IEnumerable<Point2[]> lines)
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
            }
        }
    }

    private static IEnumerable<Point2> DrawLine(Point2 from, Point2 to)
    {
        if (from.X == to.X)
        {
            if (from.Y >= to.Y)
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
            if (from.X >= to.X)
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

    public override ValueTask<string> Solve_1()
    {
        var rocks = ParseInput(_lines).ToArray();
        var grid = new HashGrid();

        FillRock(grid, rocks);

        return "result1".ToResult();
    }

    public override ValueTask<string> Solve_2()
    {
        return "result2".ToResult();
    }

    public enum Tile
    {
        Air = 0,
        Rock = 1,
        Sand = 2,
    };

    public class HashGrid
    {
        private readonly Dictionary<Point2, Tile> _tiles = new();

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
        };

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
