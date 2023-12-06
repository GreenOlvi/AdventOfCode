
using Spectre.Console;

namespace AOC2023.Puzzles;
public class Day06 : CustomBaseDay
{
    private readonly Record[] _records1;
    private readonly Record _record2;

    public Day06()
    {
        (_records1, _record2) = ParseRecords(ReadLinesFromFile());
    }

    public Day06(IEnumerable<string> lines)
    {
        (_records1, _record2) = ParseRecords(lines);
    }

    private static (Record[] Part1, Record Part2) ParseRecords(IEnumerable<string> lines)
    {
        var l = lines.ToArray();
        var times = l[0].Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Skip(1)
            .ParseLines<int>();
        var distances = l[1].Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Skip(1)
            .ParseLines<int>();

        var part1 = times.Zip(distances).Select(p => new Record(p.First, p.Second)).ToArray();

        var time2 = string.Join(string.Empty, l[0].Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Skip(1)).ToLong();
        var dist2 = string.Join(string.Empty, l[1].Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
            .Skip(1)).ToLong();

        return (part1, new Record(time2, dist2));
    }

    private static long WaysToWin(Record record)
    {
        var sqrtD = Math.Sqrt(record.Time * record.Time - 4 * record.Distance);
        var firstWon = (long)Math.Floor((record.Time - sqrtD) / 2 + 1);
        var lastWon = (long)Math.Ceiling((record.Time + sqrtD) / 2 - 1);
        return lastWon - firstWon + 1;
    }

    public override ValueTask<string> Solve_1() => _records1.Select(WaysToWin).Product().ToResult();

    public override ValueTask<string> Solve_2() => WaysToWin(_record2).ToResult();

    private readonly record struct Record(long Time, long Distance);
}
