namespace AOC2024.Puzzles;

public class Day25 : CustomBaseProblem<long>
{
    private readonly Schematic[] _schematics;

    public Day25()
    {
        _schematics = ParseInput(ReadLinesFromFile()).ToArray();
    }

    public Day25(IEnumerable<string> lines)
    {
        _schematics = ParseInput(lines).ToArray();
    }

    private static IEnumerable<Schematic> ParseInput(IEnumerable<string> lines)
    {
        var groups = Utils.SplitGroups(lines);
        foreach (var group in groups)
        {
            var type = group[0] == "#####"
                ? SchematicType.Lock
                : SchematicType.Key;

            var h = new int[5];
            for (var i = 0; i < 5; i++)
            {
                h[i] = Enumerable.Range(0, 5)
                    .Count(r => group[r + 1][i] == '#');
            }

            yield return new Schematic(type, new Heights(h[0], h[1], h[2], h[3], h[4]));
        }
    }

    private static bool Fits(Heights key, Heights @lock) =>
        key.H1 + @lock.H1 < 6
            && key.H2 + @lock.H2 < 6
            && key.H3 + @lock.H3 < 6
            && key.H4 + @lock.H4 < 6
            && key.H5 + @lock.H5 < 6;

    public override long Solve1()
    {
        var keys = _schematics.Where(static s => s.Type == SchematicType.Key)
            .Select(static s => s.Heights);
        var locks = _schematics.Where(static s => s.Type == SchematicType.Lock)
            .Select(static s => s.Heights)
            .ToArray();

        var fits = 0L;
        foreach (var key in keys)
        {
            foreach (var @lock in locks)
            {
                if (Fits(key, @lock))
                {
                    fits++;
                }
            }
        }

        return fits;
    }

    public override long Solve2() => default;

    private class Schematic(SchematicType Type, Heights Heights)
    {
        public SchematicType Type { get; } = Type;
        public Heights Heights { get; } = Heights;

        public override string ToString() =>
            $"{Type} [{Heights.H1},{Heights.H2},{Heights.H3},{Heights.H4},{Heights.H5}]";
    }

    private readonly record struct Heights(int H1, int H2, int H3, int H4, int H5);
    private enum SchematicType
    {
        Unknown = 0,
        Lock,
        Key,
    }
}
