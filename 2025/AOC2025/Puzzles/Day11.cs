
using System.Linq;

namespace AOC2025.Puzzles;

public class Day11 : CustomBaseProblem<long>
{
    private readonly Dictionary<string, string[]> _input;

    public Day11()
    {
        _input = ParseInput(ReadLinesFromFile());
    }

    public Day11(IEnumerable<string> lines)
    {
        _input = ParseInput(lines);
    }

    private static Dictionary<string, string[]> ParseInput(IEnumerable<string> lines)
    {
        var input = new Dictionary<string, string[]>();
        foreach (var line in lines)
        {
            var colon = line.IndexOf(':');
            var node = line[0..colon];

            var outputs = line[(colon + 1)..]
                .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            input[node] = outputs;
        }
        return input;
    }

    public override long Solve1() => FindPossiblePaths(_input, "you", "out").Count();

    private static IEnumerable<string[]> FindPossiblePaths(Dictionary<string, string[]> map, string start, string end) =>
        FindPossiblePaths(map, start, end, []);

    private static IEnumerable<string[]> FindPossiblePaths(Dictionary<string, string[]> map, string start, string end, string[] path)
    {
        var currentPath = path.Append(start).ToArray();

        if (start == end)
        {
            yield return currentPath;
            yield break;
        }

        var ways = map[start];
        foreach (var w in ways)
        {
            var paths = FindPossiblePaths(map, w, end, currentPath);
            foreach (var p in paths)
            {
                yield return p;
            }
        }
    }

    public override long Solve2()
    {
        return default;
    }
}
