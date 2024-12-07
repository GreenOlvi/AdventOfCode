namespace AOC2024Tests.Puzzles;

public class Day07Test
{
    private static readonly string _testInput1 = """
190: 10 19
3267: 81 40 27
83: 17 5
156: 15 6
7290: 6 8 6 15
161011: 16 10 13
192: 17 8 14
21037: 9 7 18 13
292: 11 6 16 20
""";

    [Test]
    public void Solve1Test()
    {
        var day = new Day07(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        _ = day.Solve1().Should().Be(3749);
    }

    [Test]
    public void Solve2Test()
    {
        var day = new Day07(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        _ = day.Solve2().Should().Be(11387);
    }
}
