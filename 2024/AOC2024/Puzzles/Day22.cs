namespace AOC2024.Puzzles;

public class Day22 : CustomBaseProblem<long>
{
    private readonly long[] _init;

    public Day22()
    {
        _init = ReadLinesFromFile().Select(static l => l.ToLong()).ToArray();
    }

    public Day22(IEnumerable<string> lines)
    {
        _init = lines.Select(static l => l.ToLong()).ToArray();
    }

    private static long Next(long a)
    {
        var b = (a ^ (a << 6)) & 0xffffff;
        var c = (b ^ (b >> 5)) & 0xffffff;
        var d = (c ^ (c << 11)) & 0xffffff;
        return d;
    }

    private static long Next(long a, int count)
    {
        var n = a;
        for (var i = 0; i < count; i++)
        {
            n = Next(n);
        }

        return n;
    }

    private static IEnumerable<int> GetPrices(long seed)
    {
        var n = seed;
        while (true)
        {
            yield return (int)(n % 10);
            n = Next(n);
        }
    }

    private static string StrLabel(IEnumerable<int> numbers) =>
        string.Join(',', numbers);

    private static int IntLabel(IEnumerable<int> numbers)
    {
        var num = 0;
        foreach (var n in numbers)
        {
            num = (num * 19) + n + 9;
        }
        return num;
    }

    private static IEnumerable<(T, int)> LabelGains<T>(long seed, Func<IEnumerable<int>, T> labelMaker, int count = 2000)
    {
        var prices = GetPrices(seed).Take(count).ToArray();
        var fourDiffs = prices.Pairwise()
            .Select(static p => p.Item2 - p.Item1)
            .SlidingWindow(4)
            .Select(d => labelMaker(d));

        var pairs = fourDiffs.Zip(prices.Skip(4));

        var seen = new HashSet<T>();
        foreach (var p in pairs)
        {
            if (!seen.Contains(p.First))
            {
                yield return p;
                _ = seen.Add(p.First);
            }
        }
    }

    public override long Solve1() =>
        _init.Sum(static n => Next(n, 2000));

    public override long Solve2()
    {
        var bestGain = (Label: -1, Value: -1);
        var all = new int[19 * 19 * 19 * 19];
        foreach (var p in _init.SelectMany(static s => LabelGains(s, IntLabel, 2000)))
        {
            all[p.Item1] += p.Item2;
            if (all[p.Item1] > bestGain.Value)
            {
                bestGain = (p.Item1, all[p.Item1]);
            }
        }

        return bestGain.Value;
    }
}
