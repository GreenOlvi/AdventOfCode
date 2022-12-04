namespace Tests;

[TestFixture]
public class Day04Tests
{
    private readonly IEnumerable<string> _testInput = new string[]
    {
        "2-4,6-8",
        "2-3,4-5",
        "5-7,7-9",
        "2-8,3-7",
        "6-6,4-6",
        "2-6,4-8",
    };

    [Test]
    public async ValueTask Solve1Test()
    {
        var puzzle = new Day04(_testInput);
        (await puzzle.Solve_1()).Should().Be("2");
    }

    [Test]
    public async ValueTask Solve2Test()
    {
        var puzzle = new Day04(_testInput);
        (await puzzle.Solve_2()).Should().Be("4");
    }
}