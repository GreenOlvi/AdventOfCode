namespace AOC2025.Common;

public readonly record struct Point3
{
    public long X { get; init; }
    public long Y { get; init; }
    public long Z { get; init; }

    public Point3(long x, long y, long z) => (X, Y, Z) = (x, y, z);

    public Point3((long X, long Y, long Z) p) : this(p.X, p.Y, p.Z)
    {
    }

    public void Deconstruct(out long x, out long y, out long z) => (x, y, z) = (X, Y, Z);

    public Point3 Add(Point3 p) => new(X + p.X, Y + p.Y, Z + p.Z);
    public Point3 Subtract(Point3 p) => new(X - p.X, Y - p.Y, Z - p.Z);

    public static Point3 operator +(Point3 a, Point3 b) => a.Add(b);
    public static Point3 operator -(Point3 a, Point3 b) => a.Subtract(b);
    public static Point3 operator *(Point3 a, long b) => new(a.X * b, a.Y * b, a.Z * b);

    public override string ToString() => $"({X},{Y},{Z})";
}

public static class Point3Utils
{
    private static long Square(long a) => a * a;

    public static double Distance(Point3 a, Point3 b) =>
        Math.Sqrt(Square(a.X - b.X) + Square(a.Y - b.Y) + Square(a.Z - b.Z));
    public static double DistanceSquared(Point3 a, Point3 b) =>
        Square(a.X - b.X) + Square(a.Y - b.Y) + Square(a.Z - b.Z);
    public static long ManhattanDistance(Point3 a, Point3 b) =>
        Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y) + Math.Abs(a.Z - b.Z);

    public static double DistanceTo(this Point3 a, Point3 b) => Distance(a, b);
    public static double DistanceSquaredTo(this Point3 a, Point3 b) => DistanceSquared(a, b);
    public static long ManhattanDistanceTo(this Point3 a, Point3 b) => ManhattanDistance(a, b);
}
