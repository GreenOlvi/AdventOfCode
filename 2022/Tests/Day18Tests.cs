namespace Tests;

[TestFixture]
public class Day18Tests
{
    private readonly IEnumerable<string> _testInput = new string[]
    {
        "2,2,2",
        "1,2,2",
        "3,2,2",
        "2,1,2",
        "2,3,2",
        "2,2,1",
        "2,2,3",
        "2,2,4",
        "2,2,6",
        "1,2,5",
        "3,2,5",
        "2,1,5",
        "2,3,5",
    };

    [Test]
    public async ValueTask Solve1Test()
    {
        var puzzle = new Day18(_testInput);
        (await puzzle.Solve_1()).Should().Be("64");
    }

    [Test]
    public async ValueTask Solve2Test()
    {
        var puzzle = new Day18(_testInput);
        (await puzzle.Solve_2()).Should().Be("58");
    }
}