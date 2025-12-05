namespace AOC2025.Puzzles;

public class Day05 : CustomBaseProblem<long, ulong>
{
    private readonly Range[] _ranges;
    private readonly ulong[] _ids;

    public Day05()
    {
        (_ranges, _ids) = ParseInput(ReadLinesFromFile());
    }

    public Day05(IEnumerable<string> lines)
    {
        (_ranges, _ids) = ParseInput(lines);
    }

    private static (Range[] Ranges, ulong[] Ids) ParseInput(IEnumerable<string> lines)
    {
        var ranges = new List<Range>();
        var e = lines.GetEnumerator();
        e.MoveNext();

        while (true)
        {
            var s = e.Current.Split('-', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            ranges.Add(new Range(ulong.Parse(s[0]), ulong.Parse(s[1])));
            e.MoveNext();
            if (e.Current == string.Empty)
            {
                break;
            }
        }

        var ids = new List<ulong>();
        while (e.MoveNext())
        {
            ids.Add(ulong.Parse(e.Current));
        }
        return (ranges.ToArray(), ids.ToArray());
    }

    public override long Solve1() => _ids.Count(id => _ranges.Any(r => r.Contains(id)));

    public override ulong Solve2()
    {
        var merged = new List<Range>() { _ranges[0] };
        foreach (var r in _ranges.Skip(1))
        {
            var overlapping = merged.Where(r.Overlaps).ToArray();
            if (overlapping.Length == 0)
            {
                merged.Add(r);
                continue;
            }

            var newR = r;
            foreach (var o in overlapping)
            {
                merged.Remove(o);
                newR = newR.Merge(o);
            }
            merged.Add(newR);
        }
        return merged.Aggregate(0uL, (a, r) => a + r.Length);
    }

    private readonly record struct Range(ulong First, ulong Last)
    {
        public ulong Length => Last - First + 1;

        public bool Contains(ulong i) => i >= First && i <= Last;

        public bool Overlaps(Range r) =>
            r.Contains(First) || r.Contains(Last) || Contains(r.First) || Contains(r.Last);

        public Range Merge(Range r)
        {
            if (!Overlaps(r))
            {
                throw new InvalidOperationException("Ranges must overlap");
            }

            var left = Math.Min(First, r.First);
            var right = Math.Max(Last, r.Last);
            return new Range(left, right);
        }
    }
}
