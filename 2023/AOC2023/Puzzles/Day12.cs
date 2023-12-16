
namespace AOC2023.Puzzles;
public class Day12 : CustomBaseDay
{
    private readonly Row[] _rows;

    public Day12()
    {
        _rows = ReadLinesFromFile().Select(ParseRow).ToArray();
    }

    public Day12(IEnumerable<string> lines)
    {
        _rows = lines.Select(ParseRow).ToArray();
    }

    private Row ParseRow(string line)
    {
        var g = line.Split(" ", StringSplitOptions.TrimEntries);
        var states = g[0].Select(c => c switch
        {
            '?' => SpringState.Unknown,
            '.' => SpringState.Operational,
            '#' => SpringState.Damaged,
            _ => throw new InvalidDataException(c.ToString()),
        }).ToArray();

        var groups = g[1].Split(",", StringSplitOptions.TrimEntries)
            .ParseLines<int>();

        return new Row(new Springs(states), groups);
    }

    private static int SetBitCount(long i, int length)
    {
        var sum = 0;
        for (var j = 0; j < length; j++)
        {
            if ((i & (1L << j)) > 0)
            {
                sum++;
            }
        }
        return sum;
    }

    private readonly StringBuilder _sb = new(200);
    private string GetBitGroups(long r, int length)
    {
        _sb.Clear();
        var group = 0;

        for (var j = length - 1; j >= 0; j--)
        {
            if ((r & (1L << j)) == 0)
            {
                if (group > 0)
                {
                    _sb.Append(group);
                    _sb.Append(',');
                    group = 0;
                }
            }
            else
            {
                group++;
            }
        }
        if (group > 0)
        {
            _sb.Append(group);
            _sb.Append(',');
        }
        return _sb.ToString();
    }

    private long CountArrangements(Row row)
    {
        var length = row.Springs.States.Length;
        if (length > 63)
        {
            throw new InvalidOperationException("Too long");
        }

        var unknowns = row.Springs.Unknowns;

        var stateBits = row.Springs.States
            .Reverse()
            .Select((s, i) => (s, 1L << i))
            .ToArray();

        var knownBroken = stateBits
            .Where(p => p.s == SpringState.Damaged)
            .Aggregate(0L, (a, b) => a | b.Item2);

        var unknownBits = stateBits
            .Where(p => p.s == SpringState.Unknown)
            .Select(p => p.Item2)
            .ToArray();

        var bitCount = row.Groups.Sum();
        var missingBits = bitCount - stateBits.Where(p => p.s == SpringState.Damaged).Count();

        var groupTag = string.Join(",", row.Groups) + ',';

        var c = 0;

        var allCombinations = Math.Pow(2, unknowns);
        for (var i = 0L; i < allCombinations; i++)
        {
            if (SetBitCount(i, length) != missingBits)
            {
                continue;
            }

            var r = knownBroken;
            for (var j = 0; j < unknowns; j++)
            {
                if ((i & (1L << j)) > 0)
                {
                    r |= unknownBits[j];
                }
            }

            if (groupTag == GetBitGroups(r, length))
            {
                c++;
            }

        }

        return c;
    }

    private Row Unfold(Row row)
    {
        var springs = new Springs(row.Springs.States.Repeat(5).ToArray());
        var groups = row.Groups.Repeat(5);
        return new Row(springs, groups);
    }

    public override ValueTask<string> Solve_1() =>
        _rows.Sum(CountArrangements).ToResult();

    public override ValueTask<string> Solve_2()
    {
        throw new NotImplementedException();
        //return _rows.Select(Unfold).Sum(CountArrangements).ToResult();
    }

    private class Row(Springs Springs, IEnumerable<int> Groups)
    {
        public Springs Springs { get; } = Springs;
        public int[] Groups { get; } = Groups.ToArray();
    }

    private readonly record struct Springs(SpringState[] States)
    {
        public int Unknowns => States.Count(s => s == SpringState.Unknown);
    }

    private enum SpringState
    {
        Unknown,
        Operational,
        Damaged,
    }
}
