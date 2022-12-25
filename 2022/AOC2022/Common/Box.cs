namespace AOC2022.Common;

public readonly record struct Box(Point2 TopLeft, Point2 BottomRight)
{
    public long Width => BottomRight.X - TopLeft.X + 1;
    public long Height => BottomRight.Y - TopLeft.Y + 1;

    public bool IsInside(Point2 point) =>
        TopLeft.X <= point.X &&
        TopLeft.Y <= point.Y &&
        BottomRight.X >= point.X &&
        BottomRight.Y >= point.Y;
}
