namespace AOC2025.Puzzles;

public class Day09 : CustomBaseProblem<long>
{
    private readonly Point2[] _input;

    public Day09()
    {
        _input = [.. ParseInput(ReadLinesFromFile())];
    }

    public Day09(IEnumerable<string> lines)
    {
        _input = [.. ParseInput(lines)];
    }

    private static IEnumerable<Point2> ParseInput(IEnumerable<string> lines) =>
        lines.Select(l =>
        {
            var s = l.Split(",", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            return new Point2(long.Parse(s[0]), long.Parse(s[1]));
        });

    public override long Solve1() =>
        _input.EachPair()
            .Select(Area)
            .Max();

    private static long Area((Point2 a, Point2 b) pair) =>
        (Math.Abs(pair.a.X - pair.b.X) + 1) * (Math.Abs(pair.a.Y - pair.b.Y) + 1);

    public override long Solve2()
    {
        var lines = _input.Pairwise()
            .Select(pair => new Line(pair.Item1, pair.Item2))
            .Append(new Line(_input[^1], _input[0]))
            .ToArray();

        var horizontal = lines.Where(l => l.A.Y == l.B.Y)
            .Select(l => l.A.X < l.B.X ? l : new Line(l.B, l.A))
            .OrderBy(l => l.A.Y)
            .ToArray();
        var vertical = lines.Where(l => l.A.X == l.B.X)
            .Select(l => l.A.Y < l.B.Y ? l : new Line(l.B, l.A))
            .OrderBy(l => l.A.X)
            .ToArray();

        bool IsOk((Point2 A, Point2 B) pair)
        {
            var box = Box.FromTwoPoints(pair.A, pair.B);
            var hor = horizontal.Where(l => l.A.Y > box.TopLeft.Y
                    && l.A.Y < box.BottomRight.Y
                    && box.TopLeft.X < l.B.X
                    && box.BottomRight.X > l.A.X);

            if (hor.Any())
            {
                return false;
            }

            var ver = vertical.Where(l => l.A.X > box.TopLeft.X
                    && l.A.X < box.BottomRight.X
                    && box.TopLeft.Y < l.B.Y
                    && box.BottomRight.Y > l.A.Y);

            return !ver.Any();
        }

        var max = _input.EachPair()
            .Select(p => (Points: p, Area: Area(p)))
            .OrderByDescending(p => p.Area)
            .First(p => IsOk(p.Points));

        return max.Area;
    }

    private readonly record struct Line(Point2 A, Point2 B);
}
