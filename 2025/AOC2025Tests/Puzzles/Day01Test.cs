namespace AOC2025Tests.Puzzles;

public class Day01Test
{
    private static readonly string _testInput1 = """
L68
L30
R48
L5
R60
L55
L1
L99
R14
L82
""";

    [Test]
    public void Solve1Test()
    {
        var day = new Day01(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        _ = day.Solve1().Should().Be(3);
    }

    [Test]
    public void Solve2Test()
    {
        var day = new Day01(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        _ = day.Solve2().Should().Be(6);
    }
}
