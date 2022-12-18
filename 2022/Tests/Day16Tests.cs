namespace Tests;

[TestFixture]
public class Day16Tests
{
    private readonly IEnumerable<string> _testInput = new string[]
    {
        "Valve AA has flow rate=0; tunnels lead to valves DD, II, BB",
        "Valve BB has flow rate=13; tunnels lead to valves CC, AA",
        "Valve CC has flow rate=2; tunnels lead to valves DD, BB",
        "Valve DD has flow rate=20; tunnels lead to valves CC, AA, EE",
        "Valve EE has flow rate=3; tunnels lead to valves FF, DD",
        "Valve FF has flow rate=0; tunnels lead to valves EE, GG",
        "Valve GG has flow rate=0; tunnels lead to valves FF, HH",
        "Valve HH has flow rate=22; tunnel leads to valve GG",
        "Valve II has flow rate=0; tunnels lead to valves AA, JJ",
        "Valve JJ has flow rate=21; tunnel leads to valve II",
    };

    [Test]
    public async ValueTask Solve1Test()
    {
        var puzzle = new Day16(_testInput);
        (await puzzle.Solve_1()).Should().Be("1651");
    }

    [Test]
    public async ValueTask Solve2Test()
    {
        var puzzle = new Day16(_testInput);
        (await puzzle.Solve_2()).Should().Be("result2");
    }
}