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

    public override long Solve2() => _input.Sum(FindLargestJoltage12);

    private static long FindLargestJoltage12(byte[] bank)
    {
        if (bank.Length < 12)
        {
            throw new InvalidDataException();
        }

        ReadOnlySpan<byte> span = bank.AsSpan();
        var digits = new byte[12];

        var lastId = -1;

        for (var i = 0; i < 12; i++)
        {
            var left = lastId + 1;
            var right = 11 - i;
            ReadOnlySpan<byte> range = span[left..^right];

            var maxI = 0;
            byte d = range[maxI];
            for (var j = 1; j < range.Length; j++)
            {
                if (range[j] > d)
                {
                    d = range[j];
                    maxI = j;
                }
            }

            digits[i] = d;
            lastId = maxI + left;
        }

        return digits.Aggregate(0L, (a, b) => 10 * a + b);
    }
}
