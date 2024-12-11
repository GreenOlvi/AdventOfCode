namespace AOC2024Tests.Puzzles;

public class Day11Test
{
    private static readonly string _testInput1 = """
125 17
""";

    [Test]
    public void Solve1Test()
    {
        var day = new Day11(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        _ = day.Solve1().Should().Be(55312);
    }

    [Test]
    public void Solve2Test()
    {
        var day = new Day11(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        _ = day.Solve2().Should().Be(65601038650482);
    }
}
