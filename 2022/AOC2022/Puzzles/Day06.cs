using System.Security.Cryptography;

namespace AOC2022.Puzzles;

public class Day06 : CustomBaseDay
{
    private readonly string[] _lines;

    public Day06()
    {
        _lines = ReadLinesFromFile().ToArray();
    }

    public Day06(IEnumerable<string> lines)
    {
        _lines = lines.ToArray();
    }

    public static int FirstDifferent(string line, int window) =>
        line.SlidingWindow(window)
            .Select((c, i) => (i + window, c.ToArray()))
            .Where(p => AllAreDifferent(p.Item2))
            .Select(p => p.Item1)
            .First();

    private static bool AllAreDifferent(char[] chars) =>
        chars.Length == chars.Distinct().Count();

    public override ValueTask<string> Solve_1() =>
        FirstDifferent(_lines[0], 4).ToResult();

    public override ValueTask<string> Solve_2() => FirstDifferent(_lines[0], 14).ToResult();
}
