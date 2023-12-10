namespace AOC2023.Common;

public readonly record struct Box(Point2 TopLeft, Point2 BottomRight)
{
    public long Width => BottomRight.X - TopLeft.X + 1;
    public long Height => BottomRight.Y - TopLeft.Y + 1;

    public bool IsInside(Point2 point) =>
        TopLeft.X <= point.X &&
        TopLeft.Y <= point.Y &&
        BottomRight.X >= point.X &&
        BottomRight.Y >= point.Y;

    public IEnumerable<Point2> GetPoints()
    {
        for (var x = TopLeft.X; x <= BottomRight.X; x++)
        {
            for (var y = TopLeft.Y; y <= BottomRight.Y; y++)
            {
                yield return new Point2(x, y);
            }
        }
    }
}
