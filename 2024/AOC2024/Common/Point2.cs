namespace AOC2024.Common;

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
    public static Point2 operator *(Point2 a, long b) => new(a.X * b, a.Y * b);

    public Point2 Move(Direction direction) =>
        direction switch
        {
            Direction.Up => Add(Up),
            Direction.Down => Add(Down),
            Direction.Left => Add(Left),
            Direction.Right => Add(Right),
            _ => throw new NotImplementedException(),
        };

    public Point2 Move(Direction direction, long distance) =>
        direction switch
        {
            Direction.Up => Add(Up * distance),
            Direction.Down => Add(Down * distance),
            Direction.Left => Add(Left * distance),
            Direction.Right => Add(Right * distance),
            _ => throw new NotImplementedException(),
        };

    public override string ToString() => $"({X},{Y})";

    public static readonly Point2 Zero = new(0, 0);
    public static readonly Point2 One = new(1, 1);
    public static readonly Point2 Left = new(-1, 0);
    public static readonly Point2 Right = new(1, 0);
    public static readonly Point2 Up = new(0, -1);
    public static readonly Point2 Down = new(0, 1);
}

public static class Point2Utils
{
    public static double Distance(Point2 a, Point2 b) => Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
    public static double DistanceSquared(Point2 a, Point2 b) => Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2);
    public static long ManhattanDistance(Point2 a, Point2 b) => Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);

    public static double DistanceTo(this Point2 a, Point2 b) => Distance(a, b);
    public static double DistanceSquaredTo(this Point2 a, Point2 b) => DistanceSquared(a, b);
    public static long ManhattanDistanceTo(this Point2 a, Point2 b) => ManhattanDistance(a, b);
}
