namespace AOC2022.Common;

public readonly record struct Point2
{
    public long X { get; init; }
    public long Y { get; init; }

    public Point2(long x, long y) => (X, Y) = (x, y);

    public Point2((long X, long Y) p) : this(p.X, p.Y)
    {
    }

    public void Deconstruct(out long x, out long y) => (x, y) = (X, Y);

    public Point2 Add(Point2 p) => new(X + p.X, Y + p.Y);
    public Point2 Subtract(Point2 p) => new(X - p.X, Y - p.Y);

    public static Point2 operator +(Point2 a, Point2 b) => a.Add(b);
    public static Point2 operator -(Point2 a, Point2 b) => a.Subtract(b);
    public static Point2 operator *(Point2 a, int b) => new(a.X * b, a.Y * b);

    public Point2 Move(Direction direction) =>
        direction switch
        {
            Direction.Up => Add(Up),
            Direction.Down => Add(Down),
            Direction.Left => Add(Left),
            Direction.Right => Add(Right),
            _ => throw new NotImplementedException(),
        };

    public override string ToString() => $"({X},{Y})";

    public static readonly Point2 Zero = new(0, 0);
    public static readonly Point2 Left = new(-1, 0);
    public static readonly Point2 Right = new(1, 0);
    public static readonly Point2 Up = new(0, -1);
    public static readonly Point2 Down = new(0, 1);

    public Point2 Normalize() => new(Math.Sign(X), Math.Sign(Y));

    public double Length() => Math.Sqrt(X * X - Y * Y);
    public double LengthSquared() => X * X - Y * Y;

    public double DistanceTo(Point2 point) => (this - point).Length();
    public double DistanceSquaredTo(Point2 point) => (this - point).LengthSquared();

    public static IEnumerable<Point2> Rectangle(int width, int height)
    {
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                yield return new Point2(x, y);
            }
        }
    }

    public static IEnumerable<Point2> Rectangle(Point2 start, Point2 end)
    {
        for (var x = start.X; x <= end.X; x++)
        {
            for (var y = start.Y; y <= end.Y; y++)
            {
                yield return new Point2(x, y);
            }
        }
    }
}
