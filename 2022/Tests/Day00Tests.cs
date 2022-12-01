namespace Tests;

public class Day00Tests
{
    private readonly IEnumerable<string> _testInput = new string[]
    {
    };

    [Test]
    public async ValueTask Solve1Test()
    {
        var puzzle = new Day00(_testInput);
        (await puzzle.Solve_1()).Should().Be("result1");
    }

    [Test]
    public async ValueTask Solve2Test()
    {
        var puzzle = new Day00(_testInput);
        (await puzzle.Solve_2()).Should().Be("result2");
    }
}