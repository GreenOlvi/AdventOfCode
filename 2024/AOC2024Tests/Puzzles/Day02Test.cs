namespace AOC2024Tests.Puzzles;

public class Day02Test
{
    private static readonly string _testInput1 = """
7 6 4 2 1
1 2 7 8 9
9 7 6 2 1
1 3 2 4 5
8 6 4 4 1
1 3 6 7 9
""";

    [Test]
    public void Solve1Test()
    {
        var day = new Day02(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        _ = day.Solve1().Should().Be(2);
    }

    [Test]
    public void Solve2Test()
    {
        var day = new Day02(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        day.Solve2().Should().Be(4);
    }
}
