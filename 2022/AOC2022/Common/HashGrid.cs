using System.Text;

namespace AOC2022.Common;

public class HashGrid<Tile> where Tile : struct
{
    protected readonly Dictionary<Point2, Tile> _tiles = new();
    private readonly IDictionary<Tile, char> _tileChars;
    private readonly bool _yInverted;

    public HashGrid(IDictionary<Tile, char> chars, bool yInverted = false)
    {
        _tileChars = chars;
        _yInverted = yInverted;
    }

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

    public string Draw()
    {
        var sb = new StringBuilder();

        var set = _tiles.Keys;
        var minX = set.Min(p => p.X) - 1;
        var minY = set.Min(p => p.Y) - 1;
        var maxX = set.Max(p => p.X) + 1;
        var maxY = set.Max(p => p.Y) + 1;

        if (_yInverted)
        {
            DrawYInverted(sb, minX, minY, maxX, maxY);
        }
        else
        {
            DrawNormal(sb, minX, minY, maxX, maxY);
        }

        return sb.ToString();
    }

    private void DrawNormal(StringBuilder sb, long minX, long minY, long maxX, long maxY)
    {
        for (var y = minY; y <= maxY; y++)
        {
            for (var x = minX; x <= maxX; x++)
            {
                var p = new Point2(x, y);
                var ch = _tiles.TryGetValue(p, out var v) ? _tileChars[v] : _tileChars[default];
                sb.Append(ch);
            }
            sb.AppendLine();
        }
    }

    private void DrawYInverted(StringBuilder sb, long minX, long minY, long maxX, long maxY)
    {
        for (var y = maxY; y >= minY; y--)
        {
            for (var x = minX; x <= maxX; x++)
            {
                var p = new Point2(x, y);
                var ch = _tiles.TryGetValue(p, out var v) ? _tileChars[v] : _tileChars[default];
                sb.Append(ch);
            }
            sb.AppendLine();
        }
    }
}
