using System.Collections;

namespace AOC2025.Puzzles;

public class Day10 : CustomBaseProblem<long>
{
    private readonly Machine[] _input;

    public Day10()
    {
        _input = [.. ReadLinesFromFile().Select(ParseLine)];
    }

    public Day10(IEnumerable<string> lines)
    {
        _input = [.. lines.Select(ParseLine)];
    }

    private Machine ParseLine(string line)
    {
        var parts = line.Split(" ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var (expected, length) = ParseLights(parts[0]);
        var buttons = parts[1..^1].Select(ParseButton).ToArray();
        var joltage = Vector.From(parts.Last()[1..^1].SplitAndParse<long>());
        return new Machine(expected, length, buttons, joltage);
    }

    public static (uint expected, int length) ParseLights(string lights)
    {
        var result = 0u;
        var bit = 1u;
        var i = 1;
        while (lights[i] != ']')
        {
            result |= lights[i] switch
            {
                '.' => 0,
                '#' => bit,
                _ => throw new InvalidDataException(lights),
            };
            bit <<= 1;
            i++;
        }
        return (result, i - 1);
    }

    public static uint ParseButton(string button) =>
        (uint)button[1..^1].SplitAndParse<int>()
            .Select(i => 1 << i)
            .Sum();

    public override long Solve1() => _input.Select(FindLowestButtonPressCount).Sum();

    private long FindLowestButtonPressCount(Machine machine)
    {
        // Console.WriteLine(machine);
        var l = machine.Buttons.Length;
        var last = 1 << machine.Buttons.Length;

        var minPresses = l;
        for (var i = 0u; i < last; i++)
        {
            var (lights, presses) = PressButtonsFromPermutation(machine.Buttons, i);
            if (lights == machine.Expected && presses < minPresses)
            {
                minPresses = presses;
            }
        }

        return minPresses;
    }

    private static (uint Lights, int Presses) PressButtonsFromPermutation(uint[] buttons, uint permutation)
    {
        var lights = 0u;
        uint bit = 1;
        var presses = 0;
        for (var b = 0; b < buttons.Length; b++)
        {
            if ((permutation & bit) > 0)
            {
                lights ^= buttons[b];
                presses++;
            }

            bit <<= 1;
        }

        return (lights, presses);
    }

    public override long Solve2() => _input.Select(FindLowestButtonPressCountForJoltage).Sum();

    private long FindLowestButtonPressCountForJoltage(Machine machine)
    {
        var lookup = BuildLookup(machine);
        var buttonPressCache = new Dictionary<string, int?>();
        var min = FindLowestButtonPressCountForJoltage(machine, lookup, machine.Joltage, buttonPressCache);
        return min!.Value;
    }

    private static int? FindLowestButtonPressCountForJoltage(Machine machine, Dictionary<uint, LookupEntry[]> lookup, Vector joltage,
            Dictionary<string, int?> buttonPressCache, int depth = 0)
    {
        if (joltage.IsZero())
        {
            return 0;
        }

        if (joltage.IsLowerThanZero())
        {
            return null;
        }

        var oddLights = BuildLightsFromOdd(joltage);
        if (!lookup.TryGetValue(oddLights, out var lookupEntries))
        {
            return null;
        }

        int? minPresses = null;
        var solutions = lookupEntries.OrderBy(e => e.Presses).ToArray();
        foreach (var s in solutions)
        {
            var joltageDiff = ButtonPermutationToJoltageDiff(machine, s);
            var div2 = joltage.Subtract(joltageDiff).Div2();

            if (!buttonPressCache.ContainsKey(div2.ToString()))
            {
                var p = FindLowestButtonPressCountForJoltage(machine, lookup, div2, buttonPressCache, depth + 1);
                buttonPressCache[div2.ToString()] = p;
            }

            var presses = buttonPressCache[div2.ToString()];
            if (presses.HasValue)
            {
                var allPresses = 2 * presses.Value + s.Presses;
                if (!minPresses.HasValue)
                {
                    minPresses = allPresses;
                }
                else
                {
                    if (minPresses.Value > allPresses)
                    {
                        minPresses = allPresses;
                    }
                }
            }
        }

        return minPresses;
    }

    private static Vector ButtonPermutationToJoltageDiff(Machine machine, LookupEntry entry)
    {
        var connectionsFromButtons = BitsToIndexes(entry.Permutation, machine.Buttons.Length)
            .SelectMany(b => BitsToIndexes(machine.Buttons[b], machine.Joltage.Length));

        var v = new long[machine.Joltage.Length];
        foreach (var i in connectionsFromButtons)
        {
            v[i]++;
        }
        return new Vector(v);
    }

    private static IEnumerable<uint> BitsToIndexes(uint bits, int count)
    {
        var bit = 1u;
        for (var i = 0u; i < count; i++)
        {
            if ((bit & bits) > 0)
            {
                yield return i;
            }
            bit <<= 1;
        }
    }

    private static uint BuildLightsFromOdd(Vector joltage)
    {
        var oddLights = 0u;
        var bit = 1u;
        for (var i = 0; i < joltage.Length; i++)
        {
            if (joltage[i] % 2 != 0)
            {
                oddLights |= bit;
            }
            bit <<= 1;
        }

        return oddLights;
    }

    private static Dictionary<uint, LookupEntry[]> BuildLookup(Machine machine)
    {
        var lookup = new Dictionary<uint, LookupEntry[]>();
        var last = 1 << machine.Buttons.Length;
        for (var i = 0u; i < last; i++)
        {
            var (lights, presses) = PressButtonsFromPermutation(machine.Buttons, i);
            var entry = new LookupEntry(lights, presses, i);
            if (lookup.TryGetValue(lights, out var list))
            {
                lookup[lights] = [.. list, entry];
            }
            else
            {
                lookup[lights] = [entry];
            }
        }
        return lookup;
    }

    private static string PrintLights(uint lights, int length)
    {
        var sb = new StringBuilder(length + 2);
        sb.Append('[');
        var bit = 1u;
        for (var i = 0; i < length; i++)
        {
            if ((lights & bit) > 0)
            {
                sb.Append('#');
            }
            else
            {
                sb.Append('.');
            }
            bit <<= 1;
        }
        sb.Append(']');
        return sb.ToString();
    }

    private readonly record struct LookupEntry(uint Lights, int Presses, uint Permutation);

    private readonly record struct Machine(uint Expected, int LightCount, uint[] Buttons, Vector Joltage)
    {
        public override string ToString() => $"{PrintLights(Expected, LightCount)} ({string.Join(", ", Buttons)}) {{{string.Join(", ", Joltage)}}}";
    }

    private readonly record struct Vector(long[] Values) : IEnumerable<long>
    {
        public readonly int Length = Values.Length;

        private readonly string ToStringValue = $"Vector<{Values.Length}>({string.Join(",", Values)})";

        public Vector Add(Vector other)
        {
            if (Length != other.Length)
            {
                throw new InvalidOperationException("Vector lengths must match");
            }

            var result = new long[Length];
            for (var i = 0; i < Length; i++)
            {
                result[i] = Values[i] + other.Values[i];
            }

            return new Vector(result);
        }

        public Vector Subtract(Vector other)
        {
            if (Length != other.Length)
            {
                throw new InvalidOperationException("Vector lengths must match");
            }

            var result = new long[Length];
            for (var i = 0; i < Length; i++)
            {
                result[i] = Values[i] - other.Values[i];
            }

            return new Vector(result);
        }

        public Vector Div2()
        {
            var result = new long[Length];
            for (var i = 0; i < Length; i++)
            {
                result[i] = Values[i] / 2;
            }

            return new Vector(result);
        }

        public bool IsZero() => Values.All(v => v == 0);

        public bool IsLowerThanZero() => Values.Any(v => v < 0);

        public long this[int index]
        {
            get => Values[index];
        }

        public override string ToString() => ToStringValue;

        public static Vector From(IEnumerable<long> collection) => new([.. collection]);
        public static Vector From(IEnumerable<uint> collection) => new([.. collection]);

        public IEnumerator<long> GetEnumerator() => Values.AsEnumerable().GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
