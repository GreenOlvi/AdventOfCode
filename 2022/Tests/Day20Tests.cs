namespace Tests;

[TestFixture]
public class Day20Tests
{
    private readonly IEnumerable<string> _testInput = new string[]
    {
        "1",
        "2",
        "-3",
        "3",
        "-2",
        "0",
        "4",
    };

    [Test]
    public async ValueTask Solve1Test()
    {
        var puzzle = new Day20(_testInput);
        (await puzzle.Solve_1()).Should().Be("3");
    }

    [Test]
    public async ValueTask Solve2Test()
    {
        var puzzle = new Day20(_testInput);
        (await puzzle.Solve_2()).Should().Be("1623178306");
    }
}