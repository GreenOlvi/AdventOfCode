namespace AOC2025Tests.Puzzles;

public class Day03Test
{
    private static readonly string _testInput1 = """
987654321111111
811111111111119
234234234234278
818181911112111
""";

    [Test]
    public void Solve1Test()
    {
        var day = new Day03(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        _ = day.Solve1().Should().Be(357);
    }

    [Test]
    public void Solve2Test()
    {
        var day = new Day03(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        _ = day.Solve2().Should().Be(3121910778619);
    }
}
