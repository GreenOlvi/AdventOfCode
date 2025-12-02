using System.Collections;
using System.Text.RegularExpressions;

namespace AOC2025.Puzzles;

public partial class Day02 : CustomBaseProblem<long>
{
    private readonly Range[] _input;

    public Day02()
    {
        _input = [.. ParseInput(ReadLinesFromFile())];
    }

    public Day02(IEnumerable<string> lines)
    {
        _input = [.. ParseInput(lines)];
    }

    private IEnumerable<Range> ParseInput(IEnumerable<string> lines) =>
        lines.SelectMany(l => l.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(ParseRange));

    private Range ParseRange(string text)
    {
        var s = text.Split('-', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var a = long.Parse(s[0]);
        var b = long.Parse(s[1]);
        if (a > b)
        {
            throw new InvalidDataException(text);
        }
        return new Range(a, b);
    }

    public override long Solve1() => _input.SelectMany(GetInvalidPairsFromRange).Sum();

    private static IEnumerable<long> GetInvalidPairsFromRange(Range r)
    {
        var s = r.First.ToString();
        var l = r.Last.ToString();

        if (s.Length != l.Length)
        {
            if (l.Length - s.Length > 1)
            {
                throw new InvalidDataException("Diff too large");
            }
            var div = (long)Math.Pow(10, s.Length);
            return GetInvalidPairsFromRange(new Range(r.First, div - 1))
                .Concat(GetInvalidPairsFromRange(new Range(div, r.Last)));
        }

        if (s.Length % 2 != 0)
        {
            return [];
        }

        return EnumerateDuplicates(r);
    }

    private static IEnumerable<long> EnumerateDuplicates(Range r)
    {
        var s = r.First.ToString();

        var halfS = s[0..(s.Length / 2)];
        var shift = (long)Math.Pow(10, s.Length / 2);
        var left = long.Parse(halfS);

        while (true)
        {
            var invalid = left * shift + left;
            if (invalid > r.Last)
            {
                break;
            }

            if (r.Contains(invalid))
            {
                yield return left * shift + left;
            }

            left++;
        }
    }

    public override long Solve2() => _input.SelectMany(GetInvalidMultiplesFromRange).Sum();

    private static IEnumerable<long> GetInvalidMultiplesFromRange(Range r)
    {
        var s = r.First.ToString();
        var l = r.Last.ToString();

        if (s.Length != l.Length)
        {
            if (l.Length - s.Length > 1)
            {
                throw new InvalidDataException("Diff too large");
            }

            var div = (long)Math.Pow(10, s.Length);
            return GetInvalidMultiplesFromRange(new Range(r.First, div - 1))
                .Concat(GetInvalidMultiplesFromRange(new Range(div, r.Last)));
        }

        return Enumerable.Range(1, s.Length / 2)
            .Where(i => s.Length % i == 0)
            .SelectMany(i => EnumerateMultiples(r, i))
            .Distinct();
    }

    private static IEnumerable<long> EnumerateMultiples(Range r, int i)
    {
        var s = r.First.ToString();

        var gen = long.Parse(s[0..i]);
        var shift = (long)Math.Pow(10, i);
        var parts = s.Length / i;

        long repeat(long g)
        {
            var n = 0L;
            for (var j = 0; j < parts; j++)
            {
                n = n * shift + g;
            }
            return n;
        }

        while (true)
        {
            var invalid = repeat(gen);
            if (invalid > r.Last)
            {
                break;
            }

            if (r.Contains(invalid))
            {
                yield return invalid;
            }

            gen++;
        }
    }

    private readonly record struct Range(long First, long Last) : IEnumerable<long>
    {
        public long Length => Last - First + 1;

        public bool Contains(long i) => i >= First && i <= Last;

        public IEnumerator<long> GetEnumerator()
        {
            var i = First;
            while (i <= Last)
            {
                yield return i;
                i++;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

}
