namespace AOC2023.Common;
public readonly record struct BoxReal(Point2Real TopLeft, Point2Real BottomRight)
{
    public double Width => BottomRight.X - TopLeft.X;
    public double Height => BottomRight.Y - TopLeft.Y;

    public bool IsInside(Point2Real point) =>
        TopLeft.X <= point.X &&
        TopLeft.Y <= point.Y &&
        BottomRight.X >= point.X &&
        BottomRight.Y >= point.Y;

}
