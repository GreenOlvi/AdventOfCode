namespace Tests;

[TestFixture]
public class Day24Tests
{
    private readonly IEnumerable<string> _testInput = new string[]
    {
        "#.######",
        "#>>.<^<#",
        "#.<..<<#",
        "#>v.><>#",
        "#<^v^^>#",
        "######.#",
    };

    [Test]
    public async ValueTask Solve1Test()
    {
        var puzzle = new Day24(_testInput);
        (await puzzle.Solve_1()).Should().Be("18");
    }

    [Test]
    public async ValueTask Solve2Test()
    {
        var puzzle = new Day24(_testInput);
        (await puzzle.Solve_2()).Should().Be("54");
    }
}