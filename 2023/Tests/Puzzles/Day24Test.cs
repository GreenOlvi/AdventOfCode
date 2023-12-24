namespace Tests.Puzzles;

public class Day24Tests
{
    private static readonly string _testInput1 = """
19, 13, 30 @ -2,  1, -2
18, 19, 22 @ -1, -1, -2
20, 25, 34 @ -2, -2, -4
12, 31, 28 @ -1, -2, -1
20, 19, 15 @  1, -5, -3
""";

    [Test]
    public async ValueTask Solve1Test()
    {
        var day = new Day24(_testInput1.Split("\n", StringSplitOptions.TrimEntries))
        {
            IntersectionArea = new BoxReal(new Point2Real(7, 7), new Point2Real(27, 27)),
        };

        (await day.Solve_1()).Should().Be("2");
    }

    [Test]
    [Ignore("Not solved yet")]
    public async ValueTask Solve2Test()
    {
        var day = new Day24(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        (await day.Solve_2()).Should().Be("47");
    }
}