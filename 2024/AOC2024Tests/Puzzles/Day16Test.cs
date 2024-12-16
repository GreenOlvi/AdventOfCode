namespace AOC2024Tests.Puzzles;

public class Day16Test
{
    private static readonly string _testInputA = """
###############
#.......#....E#
#.#.###.#.###.#
#.....#.#...#.#
#.###.#####.#.#
#.#.#.......#.#
#.#.#####.###.#
#...........#.#
###.#.#####.#.#
#...#.....#.#.#
#.#.#.###.#.#.#
#.....#...#.#.#
#.###.#.#.#.#.#
#S..#.....#...#
###############
""";

    private static readonly string _testInputB = """
#################
#...#...#...#..E#
#.#.#.#.#.#.#.#.#
#.#.#.#...#...#.#
#.#.#.#.###.#.#.#
#...#.#.#.....#.#
#.#.#.#.#.#####.#
#.#...#.#.#.....#
#.#.#####.#.###.#
#.#.#.......#...#
#.#.###.#####.###
#.#.#...#.....#.#
#.#.#.#####.###.#
#.#.#.........#.#
#.#.#.#########.#
#S#.............#
#################
""";

    [Test]
    public void Solve1ATest()
    {
        var day = new Day16(_testInputA.Split("\n", StringSplitOptions.TrimEntries));
        _ = day.Solve1().Should().Be(7036);
    }

    [Test]
    public void Solve1BTest()
    {
        var day = new Day16(_testInputB.Split("\n", StringSplitOptions.TrimEntries));
        _ = day.Solve1().Should().Be(11048);
    }

    [Test]
    public void Solve2ATest()
    {
        var day = new Day16(_testInputA.Split("\n", StringSplitOptions.TrimEntries));
        _ = day.Solve2().Should().Be(45);
    }

    [Test]
    public void Solve2BTest()
    {
        var day = new Day16(_testInputB.Split("\n", StringSplitOptions.TrimEntries));
        _ = day.Solve2().Should().Be(64);
    }
}
