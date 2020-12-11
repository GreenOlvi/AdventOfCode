namespace AOC2020.Common
{
    public record Point
    {
        public int X;
        public int Y;

        public Point(int x, int y) => (X, Y) = (x, y);

        public void Deconstruct(out int x, out int y) => (x, y) = (X, Y);

        public Point Add(Point p) => new Point(X + p.X, Y + p.Y);
    }
}
