namespace Tests.Puzzles;

public class Day13Tests
{
    private static readonly string _testInput1 = """
#.##..##.
..#.##.#.
##......#
##......#
..#.##.#.
..##..##.
#.#.##.#.

#...##..#
#....#..#
..##..###
#####.##.
#####.##.
..##..###
#....#..#
""";

    [Test]
    public async ValueTask Solve1Test()
    {
        var day = new Day13(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        (await day.Solve_1()).Should().Be("405");
    }

    [Test]
    public async ValueTask Solve2Test()
    {
        var day = new Day13(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        (await day.Solve_2()).Should().Be("400");
    }
}