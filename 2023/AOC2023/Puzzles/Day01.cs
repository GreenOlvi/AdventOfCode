using System.Text.RegularExpressions;

namespace AOC2023.Puzzles;
public partial class Day01 : CustomBaseDay
{
    private readonly string[] _lines;

    public Day01()
    {
        _lines = ReadLinesFromFile().ToArray();
    }

    public Day01(IEnumerable<string> lines)
    {
        _lines = lines.ToArray();
    }

    private static readonly Regex firstDigit = BuildFirstDigit();
    private static readonly Regex lastDigit = BuildLastDigit();

    private static int ParseLine(string line) =>
        int.Parse(firstDigit.Match(line).Groups[1].Value) * 10
            + int.Parse(lastDigit.Match(line).Groups[1].Value);

    public override ValueTask<string> Solve_1() =>
        _lines.Select(ParseLine).Sum().ToResult();

    private static readonly Regex firstDigit2 = BuildFirstDigit2();
    private static readonly Regex lastDigit2 = BuildLastDigit2();

    private static int ToNumber(string value) => value switch
    {
        "1" or "one" => 1,
        "2" or "two" => 2,
        "3" or "three" => 3,
        "4" or "four" => 4,
        "5" or "five" => 5,
        "6" or "six" => 6,
        "7" or "seven" => 7,
        "8" or "eight" => 8,
        "9" or "nine" => 9,
        _ => throw new ArgumentOutOfRangeException(nameof(value)),
    };

    private static int ParseLine2(string line)
    {
        var n1 = firstDigit2.Match(line).Groups[1].Value;
        var n2 = lastDigit2.Match(line).Groups[1].Value;
        return ToNumber(n1) * 10 + ToNumber(n2);
    }

    public override ValueTask<string> Solve_2() =>
        _lines.Select(ParseLine2).Sum().ToResult();

    [GeneratedRegex(@"(\d)", RegexOptions.Compiled)]
    private static partial Regex BuildFirstDigit();

    [GeneratedRegex(@"(\d)", RegexOptions.RightToLeft | RegexOptions.Compiled)]
    private static partial Regex BuildLastDigit();

    [GeneratedRegex(@"(\d|one|two|three|four|five|six|seven|eight|nine)", RegexOptions.Compiled)]
    private static partial Regex BuildFirstDigit2();

    [GeneratedRegex(@"(\d|one|two|three|four|five|six|seven|eight|nine)", RegexOptions.RightToLeft | RegexOptions.Compiled)]
    private static partial Regex BuildLastDigit2();
}
