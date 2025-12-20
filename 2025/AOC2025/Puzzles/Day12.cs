using System.Text.RegularExpressions;

namespace AOC2025.Puzzles;

public partial class Day12 : CustomBaseProblem<long>
{
    private readonly Present[] _presents;
    private readonly Region[] _regions;

    public Day12()
    {
        (_presents, _regions) = ParseInput(ReadLinesFromFile());
    }

    public Day12(IEnumerable<string> lines)
    {
        (_presents, _regions) = ParseInput(lines);
    }

    private static (Present[], Region[]) ParseInput(IEnumerable<string> lines)
    {
        var input = lines.SplitGroups().ToArray();
        var presents = input[0..6].Select(ParsePresent).ToArray();
        var regions = input[6].Select(ParseRegion).ToArray();
        return (presents, regions);
    }

    private static Present ParsePresent(string[] lines)
    {
        var id = int.Parse(lines[0][0..lines[0].IndexOf(':')]);
        var shape = new bool[9];
        for (var j = 0; j < 3; j++)
        {
            for (var i = 0; i < 3; i++)
            {
                shape[j * 3 + i] = lines[j + 1][i] == '#';
            }
        }
        return new Present(id, shape);
    }

    [GeneratedRegex(@"^(?<width>\d+)x(?<length>\d+):\s+(?<quantities>[\d\s]+?)\s*$", RegexOptions.Compiled)]
    private static partial Regex regionPatternGenerator();

    private static Region ParseRegion(string line)
    {
        var regionPattern = regionPatternGenerator();

        var m = regionPattern.Match(line);
        if (!m.Success)
        {
            throw new InvalidDataException(line);
        }

        var width = long.Parse(m.Groups["width"].Value);
        var length = long.Parse(m.Groups["length"].Value);
        var quantities = m.Groups["quantities"].Value.SplitAndParse<long>(" ").ToArray();

        return new Region(width, length, [.. quantities]);
    }

    private static bool CanFit(Region region, Present[] presents)
    {
        if ((region.Width / 3) * (region.Length / 3) > region.AllPresents)
        {
            return true;
        }

        var area = region.Width * region.Length;
        var blocks = Enumerable.Range(0, 6).Sum(i => region.Quantity[i] * presents[i].Blocks);
        return area > blocks;
    }

    public override long Solve1() => _regions.Count(r => CanFit(r, _presents));

    public override long Solve2() => default;

    private readonly record struct Present(int Id, bool[] Shape)
    {
        public int Blocks { get; init; } = Shape.Count(b => b);

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"{Id}:");
            for (var j = 0; j < 3; j++)
            {
                for (var i = 0; i < 3; i++)
                {
                    sb.Append(Shape[j * 3 + i] ? '#' : '.');
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }

    private readonly record struct Region(long Width, long Length, long[] Quantity)
    {
        public long AllPresents { get; init; } = Quantity.Sum();
        public override string ToString() => $"{Width}x{Length}: {string.Join(' ', Quantity)}";
    }
}
