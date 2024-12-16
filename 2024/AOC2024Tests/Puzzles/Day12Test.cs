namespace AOC2024Tests.Puzzles;

public class Day12Test
{
    private static readonly string _testInput1 = """
RRRRIICCFF
RRRRIICCCF
VVRRRCCFFF
VVRCCCJFFF
VVVVCJJCFE
VVIVCCJJEE
VVIIICJJEE
MIIIIIJJEE
MIIISIJEEE
MMMISSJEEE
""";

    [Test]
    public void Solve1Test()
    {
        var day = new Day12(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        _ = day.Solve1().Should().Be(1930);
    }

    [Test]
    public void Solve2Test()
    {
        var day = new Day12(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        _ = day.Solve2().Should().Be(0);
    }
}
