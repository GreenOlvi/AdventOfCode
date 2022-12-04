namespace AOC2022.Puzzles;

public partial class Day04 : CustomBaseDay
{
    private readonly string[] _lines;

    public Day04()
    {
        _lines = ReadLinesFromFile().ToArray();
    }

    public Day04(IEnumerable<string> lines)
    {
        _lines = lines.ToArray();
    }

    [GeneratedRegex("^(?<s1>\\d+)-(?<e1>\\d+),(?<s2>\\d+)-(?<e2>\\d+)$", RegexOptions.Compiled)]
    private static partial Regex MakeLineRegex();

    private static readonly Regex LineRegex = MakeLineRegex();

    private static (Range, Range) ParseLine(string line)
    {
        var (s1, e1, s2, e2) = LineRegex.Parse(line,
            ("s1", int.Parse),
            ("e1", int.Parse),
            ("s2", int.Parse),
            ("e2", int.Parse));

        return (new Range(s1, e1), new Range(s2, e2));
    }

    private static bool IsContained(Range r1, Range r2) =>
        r1.Start.Value <= r2.Start.Value && r1.End.Value >= r2.End.Value;

    private static bool Ovelap(Range r1, Range r2) =>
        r1.End.Value >= r2.Start.Value && r1.Start.Value <= r2.Start.Value
        || r2.End.Value >= r1.Start.Value && r2.Start.Value <= r1.Start.Value;

    public override ValueTask<string> Solve_1() =>
        _lines.Select(ParseLine)
            .Count(r => IsContained(r.Item1, r.Item2) || IsContained(r.Item2, r.Item1))
            .ToResult();

    public override ValueTask<string> Solve_2() =>
        _lines.Select(ParseLine)
        .Count(r => Ovelap(r.Item1, r.Item2))
        .ToResult();
}
