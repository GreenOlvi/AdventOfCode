namespace AOC2024.Puzzles;

public class Day07 : CustomBaseProblem<long>
{
    private readonly Line[] _lines;

    public Day07()
    {
        _lines = ParseInput(ReadLinesFromFile()).ToArray();
    }

    public Day07(IEnumerable<string> lines)
    {
        _lines = ParseInput(lines).ToArray();
    }

    private static IEnumerable<Line> ParseInput(IEnumerable<string> lines)
    {
        foreach (var line in lines)
        {
            var rs = line.Split(':', StringSplitOptions.TrimEntries);
            var result = rs[0].ToLong();
            var numbers = rs[1].Split(' ', StringSplitOptions.TrimEntries)
                .Select(static n => n.ToLong())
                .ToArray();
            yield return new Line(result, numbers);
        }
    }

    private static bool CheckOperators(long result, ReadOnlySpan<long> numbers, long partialResult = 0)
    {
        if (partialResult > result)
        {
            return false;
        }

        if (numbers.Length == 0)
        {
            return partialResult == result;
        }

        var first = numbers[0];
        var rest = numbers[1..];

        return (partialResult != 0 && CheckOperators(result, rest, partialResult * first))
            || CheckOperators(result, rest, partialResult + first);
    }

    private static long NumPadding(long number)
    {
        var n = 10L;
        while (n <= number)
        {
            n *= 10;
        }
        return n;
    }

    private static bool CheckOperators2(long result, ReadOnlySpan<long> numbers, long partialResult = 0)
    {
        if (partialResult > result)
        {
            return false;
        }

        if (numbers.Length == 0)
        {
            return partialResult == result;
        }

        var first = numbers[0];
        var rest = numbers[1..];

        if (partialResult != 0)
        {
            if (CheckOperators2(result, rest, partialResult * first))
            {
                return true;
            }

            var padding = NumPadding(first);
            if (CheckOperators2(result, rest, (partialResult * padding) + first))
            {
                return true;
            }
        }

        return CheckOperators2(result, rest, partialResult + first);
    }

    public override long Solve1() =>
        _lines.Where(static l => CheckOperators(l.Result, l.Numbers))
            .Sum(static l => l.Result);

    public override long Solve2() =>
        _lines.Where(static l => CheckOperators(l.Result, l.Numbers) || CheckOperators2(l.Result, l.Numbers))
            .Sum(static l => l.Result);

    private readonly record struct Line(long Result, long[] Numbers);
}
