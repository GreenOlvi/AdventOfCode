namespace AOC2021.Common
{
    public record Point
    {
        public long X;
        public long Y;

        public Point(long x, long y) => (X, Y) = (x, y);

        public Point((long X, long Y) p) : this(p.X, p.Y)
        {
        }

        public void Deconstruct(out long x, out long y) => (x, y) = (X, Y);

        public Point Add(Point p) => new(X + p.X, Y + p.Y);
        public Point Subtract(Point p) => new(X - p.X, Y - p.Y);

        public static Point operator +(Point a, Point b) => a.Add(b);
        public static Point operator -(Point a, Point b) => a.Subtract(b);
        public static Point operator *(Point a, int b) => new(a.X * b, a.Y * b);

        public override string ToString() => $"({X},{Y})";

        public static readonly Point Zero = new(0, 0);
    }
}
