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

        public Point Move(Direction direction) =>
            direction switch
            {
                Direction.Up => Add(Up),
                Direction.Down => Add(Down),
                Direction.Left => Add(Left),
                Direction.Right => Add(Right),
                _ => throw new NotImplementedException(),
            };

        public override string ToString() => $"({X},{Y})";

        public static readonly Point Zero = new(0, 0);
        public static readonly Point Left = new(-1, 0);
        public static readonly Point Right = new(1, 0);
        public static readonly Point Up = new(0, -1);
        public static readonly Point Down = new(0, 1);
    }
}
