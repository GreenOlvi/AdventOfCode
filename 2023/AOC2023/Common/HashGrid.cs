namespace AOC2023.Common;
public sealed class HashGrid<Tile>() where Tile : struct
{
    private readonly Dictionary<Point2, Tile> _tiles = [];

    public Tile this[(long x, long y) p]
    {
        get => _tiles.TryGetValue(new Point2(p), out var tile) ? tile : default;
        set => _tiles[new Point2(p)] = value;
    }

    public Tile this[(int x, int y) p]
    {
        get => _tiles.TryGetValue(new Point2(p), out var tile) ? tile : default;
        set => _tiles[new Point2(p)] = value;
    }

    public Tile this[Point2 p]
    {
        get => _tiles.TryGetValue(p, out var tile) ? tile : default;
        set => _tiles[p] = value;
    }

    public long MaxY => _tiles.Keys.Max(p => p.Y);
    public long MinY => _tiles.Keys.Min(p => p.Y);
    public long MinX => _tiles.Keys.Min(p => p.X);
    public long MaxX => _tiles.Keys.Max(p => p.X);
}

public static class HashGridExtensions
{
    public static string Draw<T>(this HashGrid<T> grid, IDictionary<T, char> tileChars) where T : struct
    {
        var sb = new StringBuilder();

        var minX = grid.MinX - 1;
        var minY = grid.MinY - 1;
        var maxX = grid.MaxX + 1;
        var maxY = grid.MaxY + 1;

        for (var y = minY; y <= maxY; y++)
        {
            for (var x = minX; x <= maxX; x++)
            {
                var ch = tileChars[grid[(x, y)]];
                sb.Append(ch);
            }
            sb.AppendLine();
        }
        return sb.ToString();
    }

    private static readonly Dictionary<bool, char> _boolTileChars = new()
    {
        [false] = '.',
        [true] = '#',
    };

    public static string Draw(this HashGrid<bool> grid) => Draw(grid, _boolTileChars);
}
