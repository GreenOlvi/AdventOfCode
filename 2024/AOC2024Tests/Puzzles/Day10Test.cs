namespace AOC2024Tests.Puzzles;

public class Day10Test
{
    private static readonly string _testInput1 = """
89010123
78121874
87430965
96549874
45678903
32019012
01329801
10456732
""";

    [Test]
    public void Solve1Test()
    {
        var day = new Day10(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        _ = day.Solve1().Should().Be(36);
    }

    [Test]
    public void Solve2Test()
    {
        var day = new Day10(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        _ = day.Solve2().Should().Be(81);
    }
}
