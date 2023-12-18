namespace AOC2023.Common;
public sealed class HashGrid<Tile>(Tile defaultValue = default) where Tile : struct
{
    private readonly Dictionary<Point2, Tile> _tiles = [];
    private readonly Tile _defaultValue = defaultValue;

    private HashGrid(IEnumerable<KeyValuePair<Point2, Tile>> tiles, Tile defaultValue = default)
        : this(defaultValue)
    {
        _tiles = tiles.ToDictionary();
    }

    public Tile this[(long x, long y) p]
    {
        get => _tiles.TryGetValue(new Point2(p), out var tile) ? tile : _defaultValue;
        set => _tiles[new Point2(p)] = value;
    }

    public Tile this[(int x, int y) p]
    {
        get => _tiles.TryGetValue(new Point2(p), out var tile) ? tile : _defaultValue;
        set => _tiles[new Point2(p)] = value;
    }

    public Tile this[Point2 p]
    {
        get => _tiles.TryGetValue(p, out var tile) ? tile : _defaultValue;
        set => _tiles[p] = value;
    }

    public long MaxY => _tiles.Keys.Max(p => p.Y);
    public long MinY => _tiles.Keys.Min(p => p.Y);
    public long MinX => _tiles.Keys.Min(p => p.X);
    public long MaxX => _tiles.Keys.Max(p => p.X);

    public Point2 TopLeft => new(MinX, MinY);
    public Point2 BottomRight => new(MaxX, MaxY);

    public Box Box => new(TopLeft, BottomRight);

    public IEnumerable<(Point2, Tile)> Where(Func<(Point2, Tile), bool> predicate) =>
        _tiles.Select(kv => (Position: kv.Key, Tile: kv.Value))
            .Where(predicate);

    public Box GetSurroundingBox() => new(new Point2(MinX, MinY), new Point2(MaxX, MaxY));

    public HashGrid<Tile> Clone() => new(_tiles, _defaultValue);
}

public static class HashGridExtensions
{
    public static string Draw<T>(this HashGrid<T> grid, Func<Point2, T, char> tileToChar, Point2 topLeft, Point2 bottomRight) where T : struct
    {
        var sb = new StringBuilder();
        for (var y = topLeft.Y; y <= bottomRight.Y; y++)
        {
            for (var x = topLeft.X; x <= bottomRight.X; x++)
            {
                var p = new Point2(x, y);
                sb.Append(tileToChar(p, grid[p]));
            }
            sb.AppendLine();
        }
        return sb.ToString();
    }

    public static string Draw<T>(this HashGrid<T> grid, Func<Point2, T, string> tileToStr, Point2 topLeft, Point2 bottomRight) where T : struct
    {
        var sb = new StringBuilder();
        for (var y = topLeft.Y; y <= bottomRight.Y; y++)
        {
            for (var x = topLeft.X; x <= bottomRight.X; x++)
            {
                var p = new Point2(x, y);
                sb.Append(tileToStr(p, grid[p]));
            }
            sb.AppendLine();
        }
        return sb.ToString();
    }

    public static void Print<T>(this HashGrid<T> grid, Func<Point2, T, (ConsoleColor?, string)> tileToStr, Point2 topLeft, Point2 bottomRight) where T : struct
    {
        var defaultColor = Console.ForegroundColor;

        Console.WriteLine();
        for (var y = topLeft.Y; y <= bottomRight.Y; y++)
        {
            for (var x = topLeft.X; x <= bottomRight.X; x++)
            {
                var p = new Point2(x, y);
                var colStr = tileToStr(p, grid[p]);
                Console.ForegroundColor = colStr.Item1 ?? defaultColor;
                Console.Write(colStr.Item2);
            }
            Console.WriteLine();
        }

        Console.ForegroundColor = defaultColor;
    }


    public static string Draw<T>(this HashGrid<T> grid, Func<T, char> tileToChar, Point2 topLeft, Point2 bottomRight) where T : struct =>
        Draw(grid, (_, t) => tileToChar(t), topLeft, bottomRight);

    public static string Draw<T>(this HashGrid<T> grid, Func<T, string> tileToStr, Point2 topLeft, Point2 bottomRight) where T : struct =>
        Draw(grid, (_, t) => tileToStr(t), topLeft, bottomRight);

    public static string Draw<T>(this HashGrid<T> grid, Func<T, char> tileToChar) where T : struct =>
        grid.Draw(tileToChar, grid.TopLeft - Point2.One, grid.BottomRight + Point2.One);

    public static string Draw<T>(this HashGrid<T> grid, IDictionary<T, char> tileToChar, Point2 topLeft, Point2 bottomRight) where T : struct =>
        Draw(grid, t => tileToChar[t], topLeft, bottomRight);

    public static string Draw<T>(this HashGrid<T> grid, IDictionary<T, char> tileChars) where T : struct =>
        Draw(grid, t => tileChars[t]);

    private static readonly Dictionary<bool, char> _boolTileChars = new()
    {
        [false] = '.',
        [true] = '#',
    };

    public static string Draw(this HashGrid<bool> grid) => Draw(grid, _boolTileChars);

    public static string Draw(this HashGrid<char> grid) => Draw(grid, c => c == '\0' ? ' ' : c);
    public static string Draw(this HashGrid<char> grid, Point2 topLeft, Point2 bottomRight) =>
        Draw(grid, c => c == '\0' ? ' ' : c, topLeft, bottomRight);
}
