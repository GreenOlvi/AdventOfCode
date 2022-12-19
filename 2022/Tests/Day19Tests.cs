namespace Tests;

[TestFixture]
public class Day19Tests
{
    private readonly IEnumerable<string> _testInput = new string[]
    {
        "Blueprint 1: Each ore robot costs 4 ore. Each clay robot costs 2 ore. Each obsidian robot costs 3 ore and 14 clay. Each geode robot costs 2 ore and 7 obsidian.",
        "Blueprint 2: Each ore robot costs 2 ore. Each clay robot costs 3 ore. Each obsidian robot costs 3 ore and 8 clay. Each geode robot costs 3 ore and 12 obsidian.",
    };

    [Test]
    public async ValueTask Solve1Test()
    {
        var puzzle = new Day19(_testInput);
        (await puzzle.Solve_1()).Should().Be("33");
    }

    [Test]
    public async ValueTask Solve2Test()
    {
        var puzzle = new Day19(_testInput);
        (await puzzle.Solve_2()).Should().Be("3472");
    }
}