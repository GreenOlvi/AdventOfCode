namespace AOC2023.Puzzles;
public class Day09 : CustomBaseDay
{
    private readonly long[][] _sequences;

    public Day09()
    {
        _sequences = ReadLinesFromFile()
            .Select(l => l.Split(' ', StringSplitOptions.TrimEntries).ParseLines<long>().ToArray())
            .ToArray();
    }

    public Day09(IEnumerable<string> lines)
    {
        _sequences = lines
            .Select(l => l.Split(' ', StringSplitOptions.TrimEntries).ParseLines<long>().ToArray())
            .ToArray();
    }

    private static IEnumerable<T> GetValuesUntilAllZeros<T>(long[] s, Func<long[], T> selector)
    {
        var curr = s;
        while (true)
        {
            yield return selector(curr);
            if (curr.All(n => n == 0))
            {
                yield break;
            }

            curr = curr.Zip(curr.Skip(1)).Select(p => p.Second - p.First).ToArray();
        }
    }

    private long FindNextValue(long[] s) =>
        GetValuesUntilAllZeros(s, x => x.Last())
            .Reverse()
            .Aggregate(0L, (a, b) => a + b);

    private long FindPrevValue(long[] s) =>
        GetValuesUntilAllZeros(s, x => x[0])
            .Reverse()
            .Aggregate(0L, (a, b) => b - a);

    public override ValueTask<string> Solve_1() =>
        _sequences.Select(FindNextValue).Sum().ToResult();

    public override ValueTask<string> Solve_2() =>
        _sequences.Select(FindPrevValue).Sum().ToResult();
}
