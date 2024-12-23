namespace AOC2024Tests.Puzzles;

public class Day22Test
{
    private static readonly string _testInput1 = """
1
10
100
2024
""";

    private static readonly string _testInput2 = """
1
2
3
2024
""";

    [Test]
    public void Solve1Test()
    {
        var day = new Day22(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        _ = day.Solve1().Should().Be(37327623);
    }

    [Test]
    public void Solve2Test()
    {
        var day = new Day22(_testInput2.Split("\n", StringSplitOptions.TrimEntries));
        _ = day.Solve2().Should().Be(23);
    }
}
