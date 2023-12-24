namespace AOC2023.Common;
public readonly record struct Point2Real
{
    public double X { get; init; }
    public double Y { get; init; }

    public Point2Real(double x, double y) => (X, Y) = (x, y);

    public Point2Real((double X, double Y) p) : this(p.X, p.Y)
    {
    }

    public void Deconstruct(out double x, out double y) => (x, y) = (X, Y);

    public Point2Real Add(Point2Real p) => new(X + p.X, Y + p.Y);
    public Point2Real Subtract(Point2Real p) => new(X - p.X, Y - p.Y);

    public static Point2Real operator +(Point2Real a, Point2Real b) => a.Add(b);
    public static Point2Real operator -(Point2Real a, Point2Real b) => a.Subtract(b);
    public static Point2Real operator *(Point2Real a, double b) => new(a.X * b, a.Y * b);

    public Point2Real Move(Direction direction) =>
        direction switch
        {
            Direction.Up => Add(Up),
            Direction.Down => Add(Down),
            Direction.Left => Add(Left),
            Direction.Right => Add(Right),
            _ => throw new NotImplementedException(),
        };

    public Point2Real Move(Direction direction, double distance) =>
        direction switch
        {
            Direction.Up => Add(Up * distance),
            Direction.Down => Add(Down * distance),
            Direction.Left => Add(Left * distance),
            Direction.Right => Add(Right * distance),
            _ => throw new NotImplementedException(),
        };

    public override string ToString() => $"({X},{Y})";

    public static readonly Point2Real Zero = new(0, 0);
    public static readonly Point2Real One = new(1, 1);
    public static readonly Point2Real Left = new(-1, 0);
    public static readonly Point2Real Right = new(1, 0);
    public static readonly Point2Real Up = new(0, -1);
    public static readonly Point2Real Down = new(0, 1);

    public static double Distance(Point2Real a, Point2Real b) =>
        Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
    public static double DistanceSquared(Point2Real a, Point2Real b) =>
        Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2);
    public static double ManhattanDistance(Point2Real a, Point2Real b) =>
        Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
}
