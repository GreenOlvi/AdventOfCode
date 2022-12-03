namespace AOC2022.Puzzles;

public class Day03 : CustomBaseDay
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

    public static ulong GetBit(char letter) => 1ul << (GetPriority(letter) - 1);

    private static int GetPriority(char letter) =>
        letter switch
        {
            >= 'a' and <= 'z' => letter - 'a' + 1,
            >= 'A' and <= 'Z' => letter - 'A' + 27,
            _ => throw new InvalidDataException(),
        };

    public static int GetPriority(ulong number)
    {
        var n = 1ul;
        var i = 1;
        while ((number & n) == 0)
        {
            n <<= 1;
            i++;
        }
        return i;
    }

    private static ulong GetHash(IEnumerable<char> items) =>
        items.Select(GetBit).Aggregate(0ul, (a, b) => a | b);

    public static (string, string) SplitString(string value) =>
        (value[..(value.Length / 2)], value[(value.Length / 2)..]);

    public override ValueTask<string> Solve_1()
    {
        var sum = 0;
        foreach (var line in _lines)
        {
            var (a, b) = SplitString(line);
            var (ha, hb) = (GetHash(a), GetHash(b));

            var dup = ha & hb;
            sum += GetPriority(dup);
        }

        return sum.ToResult();
    }

    public override ValueTask<string> Solve_2()
    {
        var sum = 0;
        foreach (var chunk in _lines.Chunk(3))
        {
            var (a, b, c) = (GetHash(chunk[0]), GetHash(chunk[1]), GetHash(chunk[2]));
            var common = a & b & c;
            sum += GetPriority(common);
        }
        return sum.ToResult();
    }
}
