namespace AOC2025Tests.Puzzles;

public class Day09Test
{
    private static readonly string _testInput1 = """
7,1
11,1
11,7
9,7
9,5
2,5
2,3
7,3
""";

    [Test]
    public void Solve1Test()
    {
        var day = new Day09(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        _ = day.Solve1().Should().Be(50);
    }

    [Test]
    public void Solve2Test()
    {
        var day = new Day09(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        _ = day.Solve2().Should().Be(24);
    }
}
