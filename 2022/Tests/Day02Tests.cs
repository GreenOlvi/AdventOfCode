namespace Tests;

[TestFixture]
public class Day02Tests
{
    private readonly IEnumerable<string> _testInput = new string[]
    {
        "A Y",
        "B X",
        "C Z"
    };

    [Test]
    public async ValueTask Solve1Test()
    {
        var puzzle = new Day02(_testInput);
        (await puzzle.Solve_1()).Should().Be("15");
    }

    [Test]
    public async ValueTask Solve2Test()
    {
        var puzzle = new Day02(_testInput);
        (await puzzle.Solve_2()).Should().Be("12");
    }
}