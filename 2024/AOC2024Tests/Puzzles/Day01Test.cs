namespace AOC2024Tests.Puzzles;

public class Day01Test
{
    private static readonly string _testInput1 = """
3   4
4   3
2   5
1   3
3   9
3   3
""";

    [Test]
    public void Solve1Test()
    {
        var input = _testInput1.Trim().Split("\n", StringSplitOptions.TrimEntries);

        var day = new Day01(input);
        _ = day.Solve1().Should().Be(11);
    }

    [Test]
    public void Solve2Test()
    {
        var day = new Day01(_testInput1.Trim().Split("\n", StringSplitOptions.TrimEntries));
        _ = day.Solve2().Should().Be(31);
    }
}
