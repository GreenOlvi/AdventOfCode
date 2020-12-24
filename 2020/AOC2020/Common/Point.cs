namespace AOC2020.Common
{
    public record Point
    {
        public int X;
        public int Y;

        public Point(int x, int y) => (X, Y) = (x, y);

        public void Deconstruct(out int x, out int y) => (x, y) = (X, Y);

        public Point Add(Point p) => new Point(X + p.X, Y + p.Y);

        public static Point operator +(Point a, Point b) => a.Add(b);
        public static Point operator *(Point a, int b) => new Point(a.X * b, a.Y * b);

        public override string ToString() => $"({X},{Y})";

        public static readonly Point Zero = new Point(0, 0);
    }
}
