namespace AOC2025.Common;

public readonly record struct Box(Point2 TopLeft, Point2 BottomRight)
{
    public long Width => BottomRight.X - TopLeft.X + 1;
    public long Height => BottomRight.Y - TopLeft.Y + 1;

    public Box(long x1, long y1, long x2, long y2) : this(new Point2(x1, y1), new Point2(x2, y2))
    {
    }

    public bool IsInside(Point2 point) =>
        TopLeft.X <= point.X &&
        TopLeft.Y <= point.Y &&
        BottomRight.X >= point.X &&
        BottomRight.Y >= point.Y;

    public IEnumerable<Point2> GetPoints()
    {
        for (var y = TopLeft.Y; y <= BottomRight.Y; y++)
        {
            for (var x = TopLeft.X; x <= BottomRight.X; x++)
            {
                yield return new Point2(x, y);
            }
        }
    }

    public IEnumerable<Point2> GetBorderPoints()
    {
        for (var x = TopLeft.X; x < BottomRight.X; x++)
        {
            yield return new Point2(x, TopLeft.Y);
            yield return new Point2(x + 1, BottomRight.Y);
        }

        for (var y = TopLeft.Y; y < BottomRight.Y; y++)
        {
            yield return new Point2(BottomRight.X, y);
            yield return new Point2(TopLeft.X, y + 1);
        }
    }
}
