using System.Text.RegularExpressions;

namespace AOC2022.Common;

public static class RegexExtensions
{
    public static bool TryMatch(this Regex regex, string input, out Match match)
    {
        match = regex.Match(input);
        return match.Success;
    }

    public static bool TryMatch(this Regex regex, string input, string group, out string value)
    {
        if (!regex.TryMatch(input, out var match))
        {
            value = string.Empty;
            return false;
        }
        value = match.Groups[group].Value;
        return true;
    }

    public static bool TryMatch(this Regex regex, string input, string[] groups, out string[] values)
    {
        if (!regex.TryMatch(input, out var match))
        {
            values = [];
            return false;
        }
        values = groups.Select(g => match.Groups[g].Value).ToArray();
        return true;
    }

    public static bool TryMatchAll(this Regex regex, string input, out Dictionary<string, string> values)
    {
        if (!regex.TryMatch(input, out var match))
        {
            values = [];
            return false;
        }
        values = match.Groups.Keys.Select(k => (k, match.Groups[k].Value)).ToDictionary();
        return true;
    }

    public static bool TryMatchAll<T>(this Regex regex, string input, out Dictionary<string, T> values) where T : IParsable<T>
    {
        if (!regex.TryMatch(input, out var match))
        {
            values = [];
            return false;
        }
        values = match.Groups.Keys
            .Where(k => k != "0")
            .Select(k => (k, T.Parse(match.Groups[k].Value, null)))
            .ToDictionary();
        return true;
    }


    public static bool TryParse<T>(this Regex regex, string input, string group, Func<string, T> converter, out T value)
    {
        if (!regex.TryMatch(input, group, out var val))
        {
            value = default;
            return false;
        }
        value = converter(val);
        return true;
    }

    public static bool TryParseMany<T1, T2>(this Regex regex, string input,
        (string Name, Func<string, T1> F) val1,
        (string Name, Func<string, T2> F) val2,
        out (T1, T2) result)
    {
        if (!regex.TryMatchAll(input, out var dict))
        {
            result = default;
            return false;
        }

        result = (val1.F(dict[val1.Name]), val2.F(dict[val2.Name]));
        return true;
    }

    public static bool TryParseMany<T1, T2, T3>(this Regex regex, string input,
        (string Name, Func<string, T1> F) val1,
        (string Name, Func<string, T2> F) val2,
        (string Name, Func<string, T3> F) val3,
        out (T1, T2, T3) result)
    {
        if (!regex.TryMatchAll(input, out var dict))
        {
            result = default;
            return false;
        }

        result = (
            val1.F(dict[val1.Name]),
            val2.F(dict[val2.Name]),
            val3.F(dict[val3.Name]));
        return true;
    }

    public static bool TryParseMany<T1, T2, T3, T4>(this Regex regex, string input,
        (string Name, Func<string, T1> F) val1,
        (string Name, Func<string, T2> F) val2,
        (string Name, Func<string, T3> F) val3,
        (string Name, Func<string, T4> F) val4,
        out (T1, T2, T3, T4) result)
    {
        if (!regex.TryMatchAll(input, out var dict))
        {
            result = default;
            return false;
        }

        result = (
            val1.F(dict[val1.Name]),
            val2.F(dict[val2.Name]),
            val3.F(dict[val3.Name]),
            val4.F(dict[val4.Name]));
        return true;
    }

    public static T Parse<T>(this Regex regex, string input, string group, Func<string, T> converter) =>
        regex.TryParse(input, group, converter, out T value) ? value! : throw new InvalidOperationException();

    public static (T1, T2) Parse<T1, T2>(this Regex regex, string input,
        (string Name, Func<string, T1> F) val1,
        (string Name, Func<string, T2> F) val2) =>
        regex.TryParseMany(input, val1, val2, out (T1, T2) value) ? value : throw new InvalidOperationException();

    public static (T1, T2, T3) Parse<T1, T2, T3>(this Regex regex, string input,
        (string Name, Func<string, T1> F) val1,
        (string Name, Func<string, T2> F) val2,
        (string Name, Func<string, T3> F) val3) =>
        regex.TryParseMany(input, val1, val2, val3, out (T1, T2, T3) value) ? value : throw new InvalidOperationException();

    public static (T1, T2, T3, T4) Parse<T1, T2, T3, T4>(this Regex regex, string input,
        (string Name, Func<string, T1> F) val1,
        (string Name, Func<string, T2> F) val2,
        (string Name, Func<string, T3> F) val3,
        (string Name, Func<string, T4> F) val4) =>
        regex.TryParseMany(input, val1, val2, val3, val4, out (T1, T2, T3, T4) value) ? value : throw new InvalidOperationException();
}

