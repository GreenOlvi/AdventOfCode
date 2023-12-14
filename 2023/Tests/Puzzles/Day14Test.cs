namespace Tests.Puzzles;

public class Day14Tests
{
    private static readonly string _testInput1 = """
O....#....
O.OO#....#
.....##...
OO.#O....O
.O.....O#.
O.#..O.#.#
..O..#O..O
.......O..
#....###..
#OO..#....
""";

    [Test]
    public async ValueTask Solve1Test()
    {
        var day = new Day14(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        (await day.Solve_1()).Should().Be("136");
    }

    [Test]
    public async ValueTask Solve2Test()
    {
        var day = new Day14(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        (await day.Solve_2()).Should().Be("64");
    }
}