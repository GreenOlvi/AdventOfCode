namespace Tests.Puzzles;

public class Day06Tests
{
    private static readonly string _testInput1 = """
Time:      7  15   30
Distance:  9  40  200
""";

    [Test]
    public async ValueTask Solve1Test()
    {
        var day = new Day06(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        (await day.Solve_1()).Should().Be("288");
    }

    [Test]
    public async ValueTask Solve2Test()
    {
        var day = new Day06(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        (await day.Solve_2()).Should().Be("71503");
    }
}