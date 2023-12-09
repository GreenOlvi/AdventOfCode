namespace Tests.Puzzles;

public class Day09Tests
{
    private static readonly string _testInput1 = """
0 3 6 9 12 15
1 3 6 10 15 21
10 13 16 21 30 45
""";

    [Test]
    public async ValueTask Solve1Test()
    {
        var day = new Day09(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        (await day.Solve_1()).Should().Be("114");
    }

    [Test]
    public async ValueTask Solve2Test()
    {
        var day = new Day09(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        (await day.Solve_2()).Should().Be("2");
    }
}