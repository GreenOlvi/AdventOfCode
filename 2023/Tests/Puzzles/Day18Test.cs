namespace Tests.Puzzles;

public class Day18Tests
{
    private static readonly string _testInput1 = """
R 6 (#70c710)
D 5 (#0dc571)
L 2 (#5713f0)
D 2 (#d2c081)
R 2 (#59c680)
D 2 (#411b91)
L 5 (#8ceee2)
U 2 (#caa173)
L 1 (#1b58a2)
U 2 (#caa171)
R 2 (#7807d2)
U 3 (#a77fa3)
L 2 (#015232)
U 2 (#7a21e3)
""";

    [Test]
    public async ValueTask Solve1Test()
    {
        var day = new Day18(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        (await day.Solve_1()).Should().Be("62");
    }

    [Test]
    public async ValueTask Solve2Test()
    {
        var day = new Day18(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        (await day.Solve_2()).Should().Be("952408144115");
    }
}