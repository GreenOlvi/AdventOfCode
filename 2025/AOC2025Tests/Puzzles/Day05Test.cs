namespace AOC2025Tests.Puzzles;

public class Day05Test
{
    private static readonly string _testInput1 = """
3-5
10-14
16-20
12-18

1
5
8
11
17
32
""";

    [Test]
    public void Solve1Test()
    {
        var day = new Day05(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        _ = day.Solve1().Should().Be(3);
    }

    [Test]
    public void Solve2Test()
    {
        var day = new Day05(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        _ = day.Solve2().Should().Be(14);
    }
}
