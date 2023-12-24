namespace AOC2023.Common;
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
    public static Point3 operator *(Point3 a, int b) => new(a.X * b, a.Y * b, a.Z * b);
    public static Point3 operator *(Point3 a, long b) => new(a.X * b, a.Y * b, a.Z * b);

    //public Point3 Move(Direction direction) =>
    //    direction switch
    //    {
    //        Direction.Up => Add(Up),
    //        Direction.Down => Add(Down),
    //        Direction.Left => Add(Left),
    //        Direction.Right => Add(Right),
    //        _ => throw new NotImplementedException(),
    //    };

    //public Point3 Move(Direction direction, long distance) =>
    //    direction switch
    //    {
    //        Direction.Up => Add(Up * distance),
    //        Direction.Down => Add(Down * distance),
    //        Direction.Left => Add(Left * distance),
    //        Direction.Right => Add(Right * distance),
    //        _ => throw new NotImplementedException(),
    //    };

    public override string ToString() => $"({X},{Y},{Z})";

    public static readonly Point3 Zero = new(0, 0, 0);
    public static readonly Point3 One = new(1, 1, 1);
    //public static readonly Point3 Left = new(-1, 0, 0);
    //public static readonly Point3 Right = new(1, 0, 0);
    //public static readonly Point3 Up = new(0, 0, 1);
    //public static readonly Point3 Down = new(0, 0, -1);

    public static double Distance(Point3 a, Point3 b) =>
        Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2) + Math.Pow(a.Z - b.Z, 2));
    public static double DistanceSquared(Point3 a, Point3 b) =>
        Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2) + Math.Pow(a.Z - b.Z, 2);
    public static long ManhattanDistance(Point3 a, Point3 b) =>
        Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y) + Math.Abs(a.Z - b.Z);
}
