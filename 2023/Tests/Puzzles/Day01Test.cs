namespace Tests.Puzzles;

public class Day01Tests
{
    private static readonly string _testInput = """
1abc2
pqr3stu8vwx
a1b2c3d4e5f
treb7uchet
""";

    private static readonly string _testInput2 = """
two1nine
eightwothree
abcone2threexyz
xtwone3four
4nineeightseven2
zoneight234
7pqrstsixteen
""";

    [Test]
    public async ValueTask Solve1Test()
    {
        var day = new Day01(_testInput.Split("\n", StringSplitOptions.TrimEntries));
        (await day.Solve_1()).Should().Be("142");
    }

    [Test]
    public async ValueTask Solve2Test()
    {
        var day = new Day01(_testInput2.Split("\n", StringSplitOptions.TrimEntries));
        (await day.Solve_2()).Should().Be("281");
    }
}