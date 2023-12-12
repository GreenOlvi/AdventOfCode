namespace AOC2023.Common;

public static class Utils
{
    public static IEnumerable<T> ParseLines<T>(this IEnumerable<string> lines, IFormatProvider? formatProvider = null) where T : IParsable<T>
        => lines.Select(i => T.Parse(i, formatProvider));

    public static int ToInt(this string value) => int.Parse(value);
    public static long ToLong(this string value) => long.Parse(value);

    public static IEnumerable<string[]> SplitGroups(this IEnumerable<string> input)
    {
        var current = new List<string>();
        foreach (var line in input)
        {
            if (line == string.Empty && current.Count != 0)
            {
                yield return current.ToArray();
                current.Clear();
            }
            else
            {
                current.Add(line);
            }
        }

        if (current.Count != 0)
        {
            yield return current.ToArray();
        }
    }

    public static IEnumerable<T[]> SplitGroups<T>(this IEnumerable<string> input, Func<string, T> converter)
    {
        var current = new List<T>();
        foreach (var line in input)
        {
            if (line == string.Empty && current.Count != 0)
            {
                yield return current.ToArray();
                current.Clear();
            }
            else
            {
                current.Add(converter(line));
            }
        }

        if (current.Count != 0)
        {
            yield return current.ToArray();
        }
    }

    public static long GreatestCommonDivisor(long a, long b)
    {
        while (b != 0)
        {
            var r = a % b;
            (a, b) = (b, r);
        }

        return Math.Abs(a);
    }

    public static long LeastCommonMultiple(long a, long b)
    {
        if ((a == 0) || (b == 0))
        {
            return 0;
        }

        return Math.Abs((a / GreatestCommonDivisor(a, b)) * b);
    }

    public static IEnumerable<T> Repeat<T>(this IEnumerable<T> items, int count)
    {
        var itemArray = items.ToArray();
        for (var i = 0; i < count; i++)
        {
            foreach(var item in itemArray)
            {
                yield return item;
            }
        }
    }

    public static long Product(this IEnumerable<int> numbers) => numbers.Aggregate(1, (a, b) => a * b);
    public static long Product(this IEnumerable<long> numbers) => numbers.Aggregate(1L, (a, b) => a * b);
    public static long Product<T>(this IEnumerable<T> elements, Func<T, long> selector) => elements.Select(selector).Product();

    public static ValueTask<string> ToResult<T>(this T value) => new(value?.ToString() ?? "<null>");

    public static IEnumerable<(T, T)> EachPair<T>(IEnumerable<T> items)
    {
        var p = items.ToArray();
        for (var i = 0; i < p.Length - 1; i++)
        {
            for (var j = i + 1; j < p.Length; j++)
            {
                yield return (p[i], p[j]);
            }
        }
    }
}
