using System.Formats.Asn1;

namespace AOC2023.Common;

public static class Utils
{
    public static IEnumerable<T> ParseLines<T>(this IEnumerable<string> lines, IFormatProvider? formatProvider = null) where T : IParsable<T>
        => lines.Select(i => T.Parse(i, formatProvider));

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

    public static ValueTask<string> ToResult<T>(this T value) => new(value?.ToString() ?? "<null>");
}
