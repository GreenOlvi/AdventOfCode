

namespace AOC2025.Puzzles;

public class Day03 : CustomBaseProblem<long>
{
    private readonly byte[][] _input;

    public Day03()
    {
        _input = [.. ParseInput(ReadLinesFromFile())];
    }

    public Day03(IEnumerable<string> lines)
    {
        _input = [.. ParseInput(lines)];
    }

    private static IEnumerable<byte[]> ParseInput(IEnumerable<string> lines) =>
        lines.Select(l => l.ToCharArray().Select(c => (byte)(c - '0')).ToArray());


    public override long Solve1() => _input.Sum(FindLargestJoltage2);

    private static long FindLargestJoltage2(byte[] bank)
    {
        if (bank.Length < 2)
        {
            throw new InvalidDataException();
        }

        var largest = bank[0] * 10 + bank[1];
        var a = Math.Max(bank[0], bank[1]);

        for (var i = 2; i < bank.Length; i++)
        {
            var b = bank[i];
            var jolts = a * 10 + b;

            if (jolts > largest)
            {
                largest = jolts;
            }

            if (b > a)
            {
                a = b;
            }
        }

        return largest;
    }

    public override long Solve2() => default;
}
