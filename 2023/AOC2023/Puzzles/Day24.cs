namespace AOC2023.Puzzles;
public class Day24 : CustomBaseDay
{
    private readonly HailDescription[] _hails;

    public BoxReal IntersectionArea { get; init; } = new BoxReal(
        new Point2Real(200000000000000, 200000000000000),
        new Point2Real(400000000000000, 400000000000000));

    public Day24()
    {
        _hails = ReadLinesFromFile().Select(ParseHail).ToArray();
    }

    public Day24(IEnumerable<string> lines)
    {
        _hails = lines.Select(ParseHail).ToArray();
    }

    private HailDescription ParseHail(string line)
    {
        var coords = line.Split(new[] { ',', '@' }, StringSplitOptions.TrimEntries);
        var pos = new Point3(coords[0].ToLong(), coords[1].ToLong(), coords[2].ToLong());
        var vel = new Point3(coords[3].ToLong(), coords[4].ToLong(), coords[5].ToLong());
        return new HailDescription(pos, vel);
    }

    private static bool IsFuture(HailDescription hail, Point2Real point) =>
        Math.Sign(point.X - hail.Position.X) == Math.Sign(hail.Velocity.X);

    private static (Point2Real? Position, bool IsFuture) Intersection(HailDescription h1, HailDescription h2)
    {
        var (a1, b1) = h1.ToCanonical2();
        var (a2, b2) = h2.ToCanonical2();

        if (a1 == a2)
        {
            return (null, false);
        }

        var x = (b2 - b1) / (a1 - a2);
        var y = x * a1 + b1;
        var p = new Point2Real(x, y);
        var isFuture = IsFuture(h1, p) && IsFuture(h2, p);
        return (p, isFuture);
    }

    public override ValueTask<string> Solve_1()
    {
        var intersections = _hails.EachPair()
            .Select(p => Intersection(p.Item1, p.Item2))
            .Where(p => p.Position != null && p.IsFuture == true && IntersectionArea.IsInside(p.Position.Value))
            .Count();

        return intersections.ToResult();
    }

    public override ValueTask<string> Solve_2()
    {
        return "result 2".ToResult();
    }

    private readonly record struct HailDescription(Point3 Position, Point3 Velocity)
    {
        public Point2Real ToCanonical2()
        {
            var a = (Velocity.Y + .0) / Velocity.X;
            var b = Position.Y - a * Position.X;
            return new(a, b);
        }
    }
}
