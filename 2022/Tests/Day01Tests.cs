namespace Tests;

[TestFixture]
public class Day01Tests
{
    private readonly IEnumerable<string> _testInput = new[]
    {
        "1000",
        "2000",
        "3000",
        "",
        "4000",
        "",
        "5000",
        "6000",
        "",
        "7000",
        "8000",
        "9000",
        "",
        "10000",
    };

    [Test]
    public async ValueTask Solve1Test()
    {
        var puzzle = new Day01(_testInput);
        (await puzzle.Solve_1()).Should().Be("24000");
    }

    [Test]
    public async ValueTask Solve2Test()
    {
        var puzzle = new Day01(_testInput);
        (await puzzle.Solve_2()).Should().Be("45000");
    }
}