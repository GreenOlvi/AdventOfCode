namespace Tests;

[TestFixture]
public class Day09Tests
{
    private readonly IEnumerable<string> _testInput = new string[]
    {
        "R 4",
        "U 4",
        "L 3",
        "D 1",
        "R 4",
        "D 1",
        "L 5",
        "R 2",
    };

    [Test]
    public async ValueTask Solve1Test()
    {
        var puzzle = new Day09(_testInput);
        (await puzzle.Solve_1()).Should().Be("13");
    }

    [Test]
    public async ValueTask Solve2Test()
    {
        var puzzle = new Day09(_testInput);
        (await puzzle.Solve_2()).Should().Be("1");
    }

    [Test]
    public async ValueTask Solve2LargerExampleTest()
    {
        var puzzle = new Day09(new[]
        {
            "R 5",
            "U 8",
            "L 8",
            "D 3",
            "R 17",
            "D 10",
            "L 25",
            "U 20",
        });
        (await puzzle.Solve_2()).Should().Be("36");
    }
}