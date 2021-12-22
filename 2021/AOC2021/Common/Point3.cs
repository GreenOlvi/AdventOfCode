namespace AOC2021.Common
{
    public record struct Point3
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

        public override string ToString() => $"({X},{Y},{Z})";

        public static readonly Point3 Zero = new(0, 0, 0);
    }
}
