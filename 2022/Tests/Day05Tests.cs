namespace Tests;

[TestFixture]
public class Day05Tests
{
    private readonly IEnumerable<string> _testInput = new string[]
    {
        "    [D]",
        "[N] [C]",
        "[Z] [M] [P]",
        " 1   2   3",
        "",
        "move 1 from 2 to 1",
        "move 3 from 1 to 3",
        "move 2 from 2 to 1",
        "move 1 from 1 to 2",
    };

    [Test]
    public async ValueTask Solve1Test()
    {
        var puzzle = new Day05(_testInput);
        (await puzzle.Solve_1()).Should().Be("CMZ");
    }

    [Test]
    public async ValueTask Solve2Test()
    {
        var puzzle = new Day05(_testInput);
        (await puzzle.Solve_2()).Should().Be("MCD");
    }
}