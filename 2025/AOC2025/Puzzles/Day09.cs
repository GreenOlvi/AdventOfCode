
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
        return default;
    }
}
