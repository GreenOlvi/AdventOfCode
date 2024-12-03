using System.Text.RegularExpressions;

namespace AOC2024.Puzzles;

public partial class Day03 : CustomBaseProblem<long>
{
    private readonly string[] _lines;

    public Day03()
    {
        _lines = ReadLinesFromFile().ToArray();
    }

    public Day03(IEnumerable<string> lines)
    {
        _lines = lines.ToArray();
    }

    [GeneratedRegex(@"mul\((?<numbers>\d+,\d+)\)", RegexOptions.Compiled, "en-US")]
    private static partial Regex MulMatch();

    public override long Solve1()
    {
        var sum = 0L;

        foreach (var line in _lines)
        {
            foreach (Match match in MulMatch().Matches(line))
            {
                var n = match.Groups["numbers"].Value
                    .Split(',', StringSplitOptions.TrimEntries)
                    .Select(static i => i.ToLong())
                    .ToArray();
                sum += n[0] * n[1];
            }
        }
        return sum;
    }

    [GeneratedRegex(@"((?<instruction>mul)\((?<numbers>\d+,\d+)\))|(?<instruction>do)\(\)|(?<instruction>don't)\(\)", RegexOptions.Compiled, "en-US")]
    private static partial Regex MulDoMatch();

    public override long Solve2()
    {
        var sum = 0L;
        var active = true;

        foreach (var line in _lines)
        {
            foreach (Match match in MulDoMatch().Matches(line))
            {
                var i = match.Groups["instruction"].Value;
                if (i == "mul")
                {
                    if (active)
                    {
                        var n = match.Groups["numbers"].Value
                            .Split(',', StringSplitOptions.TrimEntries)
                            .Select(static i => i.ToLong())
                            .ToArray();
                        sum += n[0] * n[1];
                    }
                }
                else
                {
                    active = i == "do";
                }
            }
        }
        return sum;
    }
}
