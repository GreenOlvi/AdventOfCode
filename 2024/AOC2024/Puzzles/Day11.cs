namespace AOC2024.Puzzles;

public class Day11 : CustomBaseProblem<long>
{
    private readonly Dictionary<long, long> _init;

    public Day11()
    {
        _init = ParseInput(ReadLinesFromFile());
    }

    public Day11(IEnumerable<string> lines)
    {
        _init = ParseInput(lines);
    }

    private static Dictionary<long, long> ParseInput(IEnumerable<string> lines) =>
        lines.First()
            .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Select(static s => s.ToLong())
            .GroupBy(static s => s)
            .ToDictionary(static g => g.Key, static g => g.Count() + 0L);

    private static int DigitCount(long number) => number switch
    {
        > 0 and < 10 => 1,
        < 100L => 2,
        < 1000L => 3,
        < 10000L => 4,
        < 100000L => 5,
        < 1000000L => 6,
        < 10000000L => 7,
        < 100000000L => 8,
        < 1000000000L => 9,
        < 10000000000L => 10,
        < 100000000000L => 11,
        < 1000000000000L => 12,
        < 10000000000000L => 13,
        _ => throw new InvalidOperationException("Number too large"),
    };

    private static long[] MutateStone(long stone)
    {
        if (stone == 0)
        {
            return [1];
        }

        var digitCount = DigitCount(stone);
        if (digitCount % 2 == 0)
        {
            var split = (long)Math.Pow(10, digitCount / 2);
            return [stone / split, stone % split];
        }

        return [stone * 2024];
    }

    private static Dictionary<long, long> MutateStones(Dictionary<long, long> stones)
    {
        var newStones = new Dictionary<long, long>();

        foreach (var kv in stones)
        {
            foreach (var s in MutateStone(kv.Key))
            {
                if (!newStones.TryGetValue(s, out var ns))
                {
                    ns = 0L;
                }
                newStones[s] = ns + kv.Value;
            }
        }

        return newStones;
    }

    public override long Solve1() =>
        Enumerable.Range(0, 25)
            .Aggregate(_init, static (a, b) => MutateStones(a))
            .Values
            .Sum();

    public override long Solve2() =>
        Enumerable.Range(0, 75)
            .Aggregate(_init, static (a, b) => MutateStones(a))
            .Values
            .Sum();
}
