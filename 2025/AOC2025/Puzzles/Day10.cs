
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
        var expected = ParseLights(parts[0]);
        var buttons = parts[1..^1].Select(ParseButton).ToArray();
        var joltage = parts.Last()[1..^1].SplitAndParse<long>().ToArray();
        return new Machine(expected, buttons, joltage);
    }

    public static uint ParseLights(string lights)
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
        return result;
    }

    public static uint ParseButton(string button) =>
        (uint)button[1..^1].SplitAndParse<int>()
            .Select(i => 1 << i)
            .Sum();

    public override long Solve1() => _input.Select(FindLowestButtonPressCount).Sum();

    private long FindLowestButtonPressCount(Machine machine)
    {
        var l = machine.Buttons.Length;
        var last = 1 << machine.Buttons.Length;

        var minPresses = l;
        for (var i = 0; i < last; i++)
        {
            var (lights, presses) = PressButtonsFromPermutation(machine.Buttons, i);
            if (lights == machine.Expected && presses < minPresses)
            {
                minPresses = presses;
            }
        }

        return minPresses;
    }

    private static (uint Lights, int Presses) PressButtonsFromPermutation(uint[] buttons, int permutation)
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

    public override long Solve2()
    {
        return default;
    }

    private readonly record struct Machine(uint Expected, uint[] Buttons, long[] Joltage)
    {
        public override string ToString() => $"[{Expected}] ({string.Join(", ", Buttons)}) {{{string.Join(", ", Joltage)}}}";
    }
}
