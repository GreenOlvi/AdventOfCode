namespace Tests.Puzzles;

public class Day07Tests
{
    private static readonly string _testInput1 = """
32T3K 765
T55J5 684
KK677 28
KTJJT 220
QQQJA 483
""";

    [Test]
    public async ValueTask Solve1Test()
    {
        var day = new Day07(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        (await day.Solve_1()).Should().Be("6440");
    }

    [Test]
    public async ValueTask Solve2Test()
    {
        var day = new Day07(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        (await day.Solve_2()).Should().Be("5905");
    }
}