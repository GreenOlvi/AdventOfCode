namespace AOC2024Tests.Puzzles;

public class Day09Test
{
    private static readonly string _testInput1 = """
2333133121414131402
""";

    [Test]
    public void Solve1Test()
    {
        var day = new Day09(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        _ = day.Solve1().Should().Be(1928);
    }

    [Test]
    public void Solve2Test()
    {
        var day = new Day09(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        _ = day.Solve2().Should().Be(2858);
    }
}
