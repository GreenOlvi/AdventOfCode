namespace Tests;

[TestFixture]
public class Day17Tests
{
    private readonly IEnumerable<string> _testInput = new string[]
    {
        ">>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>"
    };

    [Test]
    public async ValueTask Solve1Test()
    {
        var puzzle = new Day17(_testInput);
        (await puzzle.Solve_1()).Should().Be("3068");
    }

    [Test]
    public async ValueTask Solve2Test()
    {
        var puzzle = new Day17(_testInput);
        (await puzzle.Solve_2()).Should().Be("1514285714288");
    }
}