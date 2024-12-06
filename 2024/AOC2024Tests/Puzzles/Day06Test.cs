namespace AOC2024Tests.Puzzles;

public class Day06Test
{
    private static readonly string _testInput1 = """
....#.....
.........#
..........
..#.......
.......#..
..........
.#..^.....
........#.
#.........
......#...
""";

    [Test]
    public void Solve1Test()
    {
        var day = new Day06(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        _ = day.Solve1().Should().Be(41);
    }

    [Test]
    public void Solve2Test()
    {
        var day = new Day06(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        _ = day.Solve2().Should().Be(6);
    }
}
