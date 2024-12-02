namespace AOC2024.Puzzles;

public class Day02 : CustomBaseProblem<long>
{
    private readonly long[][] _input;

    public Day02()
    {
        _input = ParseInput(ReadLinesFromFile()).ToArray();
    }

    public Day02(IEnumerable<string> lines)
    {
        _input = ParseInput(lines).ToArray();
    }

    public IEnumerable<long[]> ParseInput(IEnumerable<string> lines)
    {
        foreach (var line in lines)
        {
            yield return line.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .Select(static s => s.ToLong())
                .ToArray();
        }
    }

    public static bool IsSafe(long[] report)
    {
        static bool IsIncreasing((long a, long b) p) => p.a < p.b;
        static bool IsDistanceOk((long a, long b) p) => Math.Abs(p.a - p.b) is >= 1 and <= 3;

        var allPairs = report.Pairwise().ToArray();
        var eachIncreasing = allPairs.Select(IsIncreasing).ToArray();
        var allSameDirection = eachIncreasing.All(static i => i == true) || eachIncreasing.All(static i => i == false);
        var allDistancesOk = allPairs.Select(static p => IsDistanceOk(p)).All(i => i == true);

        return allSameDirection && allDistancesOk;
    }

    public static bool IsSafeWithDampener(long[] report)
    {
        if (IsSafe(report))
        {
            return true;
        }

        for (var i = 0; i < report.Length; i++)
        {
            var newReport = report.Take(i).
                Concat(report.Skip(i + 1).Take(report.Length - i - 1))
                .ToArray();
            if (IsSafe(newReport))
            {
                return true;
            }
        }

        return false;
    }

    public override long Solve1() => _input.Count(static r => IsSafe(r));

    public override long Solve2() => _input.Count(static r => IsSafeWithDampener(r));
}
