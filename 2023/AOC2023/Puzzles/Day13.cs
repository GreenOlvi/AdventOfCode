
namespace AOC2023.Puzzles;
public class Day13 : CustomBaseDay
{
    private readonly Pattern[] _patterns;

    public Day13()
    {
        _patterns = ParsePatterns(ReadLinesFromFile()).ToArray();
    }

    public Day13(IEnumerable<string> lines)
    {
        _patterns = ParsePatterns(lines).ToArray();
    }

    private static IEnumerable<Pattern> ParsePatterns(IEnumerable<string> lines)
    {
        static bool ToBool(char c) => c switch
        {
            '#' => true,
            '.' => false,
            _ => throw new InvalidDataException(c.ToString()),
        };
        foreach (var group in lines.SplitGroups())
        {
            yield return new Pattern(group.Select(y => y.Select(ToBool).ToList()).ToList());
        }
    }

    private static bool IsReflection(IReadOnlyList<ulong> lines, int i)
    {
        if (lines[i] != lines[i + 1])
        {
            return false;
        }

        if (i == 0 || i == lines.Count - 1)
        {
            return true;
        }

        var isReflection = true;
        var maxJ = Math.Min(i, lines.Count - 2 - i);
        for (var j = 1; j <= maxJ; j++)
        {
            if (lines[i - j] != lines[i + j + 1])
            {
                isReflection = false;
                break;
            }
        }

        if (isReflection)
        {
            return true;
        }
        return false;
    }

    private static readonly HashSet<ulong> IsOneBit = Enumerable.Range(0, 63).Select(i => 1UL << i).ToHashSet();

    private static bool AreEqualOrDifferByOneBit(ulong a, ulong b, out bool flaw)
    {
        if (a == b)
        {
            flaw = false;
            return true;
        }

        flaw = true;
        return IsOneBit.Contains(a ^ b);
    }

    private static bool IsReflectionWithFlaw(IReadOnlyList<ulong> lines, int i)
    {
        var hasFlaw = false;
        var maxJ = Math.Min(i, lines.Count - 2 - i);
        for (var j = 0; j <= maxJ; j++)
        {
            var a = lines[i - j];
            var b = lines[i + j + 1];

            if (!AreEqualOrDifferByOneBit(a, b, out var f))
            {
                return false;
            }

            if (hasFlaw && f)
            {
                return false;
            }

            if (f)
            {
                hasFlaw = true;
            }
        }

        return hasFlaw;
    }

    private static Reflections FindReflections(Pattern pattern, Func<IReadOnlyList<ulong>, int, bool> isReflection)
    {
        for (var i = 0; i < pattern.Rows.Count - 1; i++)
        {
            if (isReflection(pattern.Rows, i))
            {
                return new Reflections(i + 1, 0);
            }
        }

        for (var i = 0; i < pattern.Columns.Count - 1; i++)
        {
            if (isReflection(pattern.Columns, i))
            {
                return new Reflections(0, pattern.Columns.Count - i - 1);
            }
        }

        throw new InvalidOperationException("No reflections found");
    }

    public override ValueTask<string> Solve_1()
    {
        var reflections = _patterns.Select(p => FindReflections(p, IsReflection)).Aggregate((a, b) => a + b);
        return (reflections.Rows * 100 + reflections.Columns).ToResult();
    }

    public override ValueTask<string> Solve_2()
    {
        var reflections = _patterns.Select(p => FindReflections(p, IsReflectionWithFlaw)).Aggregate((a, b) => a + b);
        return (reflections.Rows * 100 + reflections.Columns).ToResult();
    }

    private readonly record struct Pattern(List<List<bool>> Patterns)
    {
        public IReadOnlyList<ulong> Rows { get; } = ExtractRows(Patterns).ToList();
        public IReadOnlyList<ulong> Columns { get; } = ExtractColumns(Patterns).ToList();

        private static IEnumerable<ulong> ExtractRows(List<List<bool>> patterns)
        {
            foreach (var line in patterns)
            {
                var sum = 0UL;
                for (var x = line.Count - 1; x >= 0; x--)
                {
                    if (line[x])
                    {
                        sum |= (1UL << x);
                    }
                }
                yield return sum;
            }
        }

        private static IEnumerable<ulong> ExtractColumns(List<List<bool>> patterns)
        {
            for (var x = patterns[0].Count - 1; x >= 0; x--)
            {
                var sum = 0UL;
                for (var y = 0; y < patterns.Count; y++)
                {
                    if (patterns[y][x])
                    {
                        sum |= (1UL << y);
                    }
                }
                yield return sum;
            }
        }
    }

    private record struct Reflections(int Rows, int Columns)
    {
        public static Reflections operator +(Reflections a, Reflections b) =>
            new(a.Rows + b.Rows, a.Columns + b.Columns);
    }
}