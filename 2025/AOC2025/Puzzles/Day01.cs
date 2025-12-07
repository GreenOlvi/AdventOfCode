using System.Text.RegularExpressions;

namespace AOC2025.Puzzles;

public partial class Day01 : CustomBaseProblem<long>
{
    private readonly (Direction Direction, int Distance)[] _input;

    public Day01()
    {
        _input = [.. ReadLinesFromFile().Select(ParseLine)];
    }

    public Day01(IEnumerable<string> lines)
    {
        _input = [.. lines.Select(ParseLine)];
    }

    [GeneratedRegex(@"^(?<dir>L|R)(?<dist>\d+)$", RegexOptions.Compiled)]
    private static partial Regex LinePatternGenerator();
    private static readonly Regex LinePattern = LinePatternGenerator();

    private static (Direction Direction, int Distance) ParseLine(string line)
    {
        var match = LinePattern.Match(line);
        if (!match.Success)
        {
            throw new InvalidDataException($"Line [{line}] does not match the pattern");
        }

        var dir = match.Groups["dir"].Value == "L" ? Direction.Left : Direction.Right;
        var dist = int.Parse(match.Groups["dist"].Value);
        return (dir, dist);
    }

    const int DialNumbers = 100;
    const int StartNumber = 50;

    public override long Solve1()
    {
        var zeroCount = 0;

        var current = StartNumber;
        foreach (var (dir, dist) in _input)
        {
            var mod = dir == Direction.Left ? -1 : 1;
            current = (current + mod * dist).Modulo(DialNumbers);
            if (current == 0)
            {
                zeroCount++;
            }
        }

        return zeroCount;
    }

    public override long Solve2()
    {
        var zeroCount = 0L;

        var current = StartNumber;
        foreach (var (dir, dist) in _input)
        {
            zeroCount += dist / DialNumbers;
            var newDist = dist.Modulo(DialNumbers);

            if (dir == Direction.Left)
            {
                if (current > 0 && current - newDist <= 0)
                {
                    zeroCount += 1;
                }
                current -= newDist;
            }
            else
            {
                if (current + newDist >= DialNumbers)
                {
                    zeroCount += 1;
                }
                current += newDist;
            }

            current = current.Modulo(DialNumbers);
        }

        return zeroCount;
    }

}
