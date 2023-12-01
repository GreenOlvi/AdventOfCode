namespace Tests.Puzzles;

public class Day00Tests
{
    private static readonly string _testInput1 = """

""";

    private static readonly string _testInput2 = """

""";

    [Test]
    public async ValueTask Solve1Test()
    {
        var day = new Day00(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        (await day.Solve_1()).Should().Be("result 1");
    }

    [Test]
    public async ValueTask Solve2Test()
    {
        var day = new Day00(_testInput2.Split("\n", StringSplitOptions.TrimEntries));
        (await day.Solve_2()).Should().Be("result 2");
    }
}