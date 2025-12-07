using System.Collections;
using System.Collections.Frozen;

namespace AOC2025.Common;

public interface IHashGrid<TPoint, TTile> : IEnumerable<(TPoint Position, TTile Tile)> where TPoint : struct where TTile : struct
{
    TTile this[TPoint p] { get; set; }
    TTile DefaultTile { get; }
    IEnumerable<Point2> FindTiles(Func<TTile, bool> predicate);
}

public interface IHashGrid2<TTile> : IHashGrid<Point2, TTile> where TTile : struct
{
    TTile this[(long x, long y) p] { get; set; }
    TTile this[(int x, int y) p] { get; set; }

    long MaxY { get; }
    long MinY { get; }
    long MinX { get; }
    long MaxX { get; }
    Point2 TopLeft { get; }
    Point2 BottomRight { get; }
    Box Box { get; }

    Box GetSurroundingBox();
}

public sealed class HashGrid<TTile>(TTile defaultValue = default) : IHashGrid2<TTile> where TTile : struct
{
    private readonly Dictionary<Point2, TTile> _tiles = [];

    private HashGrid(IEnumerable<KeyValuePair<Point2, TTile>> tiles, TTile defaultValue = default)
        : this(defaultValue)
    {
        _tiles = tiles.Where(t => !t.Value.Equals(defaultValue))
            .ToDictionary();
    }

    public HashGrid(IEnumerable<(Point2 Position, TTile Tile)> tiles, TTile defaultValue = default)
        : this(defaultValue)
    {
        _tiles = tiles.Where(t => !t.Tile.Equals(defaultValue))
            .ToDictionary();
    }

    public TTile this[(long x, long y) p]
    {
        get => _tiles.TryGetValue(new Point2(p), out var tile) ? tile : DefaultTile;
        set => SetValue(new Point2(p), value);
    }

    public TTile this[(int x, int y) p]
    {
        get => _tiles.TryGetValue(new Point2(p), out var tile) ? tile : DefaultTile;
        set => SetValue(new Point2(p), value);
    }

    public TTile this[Point2 p]
    {
        get => _tiles.TryGetValue(p, out var tile) ? tile : DefaultTile;
        set => SetValue(p, value);
    }

    private void SetValue(Point2 p, TTile v)
    {
        if (!v.Equals(DefaultTile))
        {
            _tiles[p] = v;
        }
        else if (_tiles.ContainsKey(p))
        {
            _ = _tiles.Remove(p);
        }
    }

    public long MaxY => _tiles.Keys.Max(static p => p.Y);
    public long MinY => _tiles.Keys.Min(static p => p.Y);
    public long MinX => _tiles.Keys.Min(static p => p.X);
    public long MaxX => _tiles.Keys.Max(static p => p.X);

    public Point2 TopLeft => new(MinX, MinY);
    public Point2 BottomRight => new(MaxX, MaxY);

    public Box Box => new(TopLeft, BottomRight);

    public TTile DefaultTile { get; } = defaultValue;

    public IEnumerable<(Point2 Position, TTile Tile)> Where(Func<(Point2 Position, TTile Tile), bool> predicate) =>
        _tiles.Select(static kv => (Position: kv.Key, Tile: kv.Value))
            .Where(predicate);

    public IEnumerable<Point2> FindTiles(Func<TTile, bool> predicate) =>
        Where(p => predicate(p.Tile))
            .Select(p => p.Position);

    public Box GetSurroundingBox() => new(new Point2(MinX, MinY), new Point2(MaxX, MaxY));

    public HashGrid<TTile> Clone() => new(_tiles, DefaultTile);
    public FrozenHashGrid<TTile> ToFrozen() => new(_tiles, DefaultTile);

    // TODO: Make enumerator work for default values

    public IEnumerator<(Point2 Position, TTile Tile)> GetEnumerator() =>
        _tiles.Select(static kv => (kv.Key, kv.Value)).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public sealed class FrozenHashGrid<TTile> : IHashGrid2<TTile> where TTile : struct
{
    private readonly FrozenDictionary<Point2, TTile> _tiles;

    public FrozenHashGrid(IEnumerable<KeyValuePair<Point2, TTile>> tiles, TTile defaultValue = default)
    {
        _tiles = tiles.Where(t => !t.Value.Equals(defaultValue))
            .ToFrozenDictionary();

        DefaultTile = defaultValue;
        MaxY = _tiles.Keys.Max(static p => p.Y);
        MinY = _tiles.Keys.Min(static p => p.Y);
        MaxX = _tiles.Keys.Min(static p => p.X);
        MinX = _tiles.Keys.Max(static p => p.X);
        TopLeft = new(MinX, MinY);
        BottomRight = new(MaxX, MaxY);
        Box = new(TopLeft, BottomRight);
    }

    public TTile this[(long x, long y) p]
    {
        get => _tiles.TryGetValue(new Point2(p), out var tile) ? tile : DefaultTile;
        set => throw new InvalidOperationException();
    }

    public TTile this[(int x, int y) p]
    {
        get => _tiles.TryGetValue(new Point2(p), out var tile) ? tile : DefaultTile;
        set => throw new InvalidOperationException();
    }

    public TTile this[Point2 p]
    {
        get => _tiles.TryGetValue(p, out var tile) ? tile : DefaultTile;
        set => throw new InvalidOperationException();
    }

    public long MaxY { get; }
    public long MinY { get; }
    public long MinX { get; }
    public long MaxX { get; }

    public Point2 TopLeft { get; }
    public Point2 BottomRight { get; }

    public Box Box { get; }

    public TTile DefaultTile { get; }

    public IEnumerable<(Point2 Position, TTile Tile)> Where(Func<(Point2 Position, TTile Tile), bool> predicate) =>
        _tiles.Select(static kv => (Position: kv.Key, Tile: kv.Value))
            .Where(predicate);

    public IEnumerable<Point2> FindTiles(Func<TTile, bool> predicate) =>
        Where(p => predicate(p.Tile))
            .Select(p => p.Position);

    public Box GetSurroundingBox() => new(new Point2(MinX, MinY), new Point2(MaxX, MaxY));

    public IEnumerator<(Point2 Position, TTile Tile)> GetEnumerator() =>
        _tiles.Select(static kv => (kv.Key, kv.Value)).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public static class HashGridExtensions
{
    public static string Draw<T>(this IHashGrid2<T> grid, Func<Point2, T, char> tileToChar, Point2 topLeft, Point2 bottomRight) where T : struct
    {
        var sb = new StringBuilder();
        for (var y = topLeft.Y; y <= bottomRight.Y; y++)
        {
            for (var x = topLeft.X; x <= bottomRight.X; x++)
            {
                var p = new Point2(x, y);
                _ = sb.Append(tileToChar(p, grid[p]));
            }
            _ = sb.AppendLine();
        }
        return sb.ToString();
    }

    public static string Draw<T>(this IHashGrid2<T> grid, Func<Point2, T, string> tileToStr, Point2 topLeft, Point2 bottomRight) where T : struct
    {
        var sb = new StringBuilder();
        for (var y = topLeft.Y; y <= bottomRight.Y; y++)
        {
            for (var x = topLeft.X; x <= bottomRight.X; x++)
            {
                var p = new Point2(x, y);
                _ = sb.Append(tileToStr(p, grid[p]));
            }
            _ = sb.AppendLine();
        }
        return sb.ToString();
    }

    public static void Print<T>(this IHashGrid2<T> grid, Func<Point2, T, (ConsoleColor?, string)> tileToStr, Point2 topLeft, Point2 bottomRight) where T : struct
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
    public static void Print<T>(this IHashGrid2<T> grid, Func<Point2, T, (ConsoleColor?, string)> tileToStr) where T : struct =>
        grid.Print(tileToStr, grid.TopLeft - Point2.One, grid.BottomRight + Point2.One);


    public static string Draw<T>(this IHashGrid2<T> grid, Func<T, char> tileToChar, Point2 topLeft, Point2 bottomRight) where T : struct =>
        Draw(grid, (_, t) => tileToChar(t), topLeft, bottomRight);

    public static string Draw<T>(this IHashGrid2<T> grid, Func<T, string> tileToStr, Point2 topLeft, Point2 bottomRight) where T : struct =>
        Draw(grid, (_, t) => tileToStr(t), topLeft, bottomRight);

    public static string Draw<T>(this IHashGrid2<T> grid, Func<T, char> tileToChar) where T : struct =>
        grid.Draw(tileToChar, grid.TopLeft - Point2.One, grid.BottomRight + Point2.One);

    public static string Draw<T>(this IHashGrid2<T> grid, IDictionary<T, char> tileToChar, Point2 topLeft, Point2 bottomRight) where T : struct =>
        Draw(grid, t => tileToChar[t], topLeft, bottomRight);

    public static string Draw<T>(this IHashGrid2<T> grid, IDictionary<T, char> tileChars) where T : struct =>
        Draw(grid, t => tileChars[t]);

    private static readonly Dictionary<bool, char> _boolTileChars = new()
    {
        [false] = '.',
        [true] = '#',
    };

    public static string Draw(this IHashGrid2<bool> grid) => Draw(grid, _boolTileChars);

    public static string Draw(this IHashGrid2<char> grid) => Draw(grid, static c => c == '\0' ? ' ' : c);
    public static string Draw(this IHashGrid2<char> grid, Point2 topLeft, Point2 bottomRight) =>
        Draw(grid, static c => c == '\0' ? ' ' : c, topLeft, bottomRight);
}
