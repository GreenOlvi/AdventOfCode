namespace AOC2022.Common;

public readonly record struct Grid<T>
{
    private readonly T[][] _items;

    public readonly int Width;
    public readonly int Height;

    public Grid(T[][] items)
    {
        _items = items;
        Width = _items[0].Length;
        Height = _items.Length;
    }

    public T this[Point2 p] => _items[p.Y][p.X];

    public T Get(Point2 p) => _items[p.Y][p.X];

    public IEnumerable<Point2> EnumeratePoints() => Point2.Rectangle(Width, Height);

    public bool IsInside(Point2 p) => p.X >= 0 && p.Y >= 0 && p.X < Width && p.Y < Height;
}
