namespace Tests;

[TestFixture]
public class Day14Tests
{
    private readonly IEnumerable<string> _testInput = new string[]
    {
        "498,4 -> 498,6 -> 496,6",
        "503,4 -> 502,4 -> 502,9 -> 494,9",
    };

    [Test]
    public async ValueTask Solve1Test()
    {
        var puzzle = new Day14(_testInput);
        (await puzzle.Solve_1()).Should().Be("24");
    }

    [Test]
    public async ValueTask Solve2Test()
    {
        var puzzle = new Day14(_testInput);
        (await puzzle.Solve_2()).Should().Be("result2");
    }
}