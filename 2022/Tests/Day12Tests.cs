namespace Tests;

[TestFixture]
public class Day12Tests
{
    private readonly IEnumerable<string> _testInput = new string[]
    {
        "Sabqponm",
        "abcryxxl",
        "accszExk",
        "acctuvwj",
        "abdefghi",
    };

    [Test]
    public async ValueTask Solve1Test()
    {
        var puzzle = new Day12(_testInput);
        (await puzzle.Solve_1()).Should().Be("31");
    }

    [Test]
    public async ValueTask Solve2Test()
    {
        var puzzle = new Day12(_testInput);
        (await puzzle.Solve_2()).Should().Be("29");
    }
}