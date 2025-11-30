namespace AOC2025.Common;

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

    public static long LeastCommonMultiple(long a, long b) =>
        (a == 0) || (b == 0)
            ? 0
            : Math.Abs(a / GreatestCommonDivisor(a, b) * b);

    public static int Modulo(this int a, int n) => (int)Modulo((long)a, n);

    public static long Modulo(this long a, long n)
    {
        var r = a % n;
        return r < 0 ? r + n : r;
    }

    public static byte Modulo(this byte a, byte n) => (byte)(a % n);

    public static IEnumerable<T> Repeat<T>(this IEnumerable<T> items, int count)
    {
        var itemArray = items.ToArray();
        for (var i = 0; i < count; i++)
        {
            foreach (var item in itemArray)
            {
                yield return item;
            }
        }
    }

    public static long Product(this IEnumerable<int> numbers) => numbers.Aggregate(1, static (a, b) => a * b);
    public static long Product(this IEnumerable<long> numbers) => numbers.Aggregate(1L, static (a, b) => a * b);
    public static long Product<T>(this IEnumerable<T> elements, Func<T, long> selector) => elements.Select(selector).Product();

    public static IEnumerable<IEnumerable<T>> SlidingWindow<T>(this IEnumerable<T> input, int n)
    {
        var arr = input.ToArray();
        for (var i = 0; i <= arr.Length - n; i++)
        {
            yield return arr.Skip(i).Take(n);
        }
    }

    public static IEnumerable<(T, T)> Pairwise<T>(this IEnumerable<T> items) => items.Zip(items.Skip(1));

    public static IEnumerable<(T, T)> EachPair<T>(this IEnumerable<T> items)
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
