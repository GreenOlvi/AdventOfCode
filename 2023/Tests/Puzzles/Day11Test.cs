namespace Tests.Puzzles;

public class Day11Tests
{
    private static readonly string _testInput1 = """
...#......
.......#..
#.........
..........
......#...
.#........
.........#
..........
.......#..
#...#.....
""";

    [Test]
    public async ValueTask Solve1Test()
    {
        var day = new Day11(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        (await day.Solve_1()).Should().Be("374");
    }

    [Test]
    public async ValueTask Solve2_1Test()
    {
        var day = new Day11(_testInput1.Split("\n", StringSplitOptions.TrimEntries))
        {
            ExpansionMultiplier = 10,
        };

        (await day.Solve_2()).Should().Be("1030");
    }

    [Test]
    public async ValueTask Solve2_2Test()
    {
        var day = new Day11(_testInput1.Split("\n", StringSplitOptions.TrimEntries))
        {
            ExpansionMultiplier = 100,
        };

        (await day.Solve_2()).Should().Be("8410");
    }
}