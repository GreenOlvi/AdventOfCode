namespace Tests.Puzzles;

public class Day16Tests
{
    private static readonly string _testInput1 = """
.|...\....
|.-.\.....
.....|-...
........|.
..........
.........\
..../.\\..
.-.-/..|..
.|....-|.\
..//.|....
""";

    [Test]
    public async ValueTask Solve1Test()
    {
        var day = new Day16(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        (await day.Solve_1()).Should().Be("46");
    }

    [Test]
    public async ValueTask Solve2Test()
    {
        var day = new Day16(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        (await day.Solve_2()).Should().Be("51");
    }
}