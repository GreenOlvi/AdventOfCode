namespace Tests.Puzzles;

public class Day03Tests
{
    private static readonly string _testInput1 = """
467..114..
...*......
..35..633.
......#...
617*......
.....+.58.
..592.....
......755.
...$.*....
.664.598..
""";

    [Test]
    public async ValueTask Solve1Test()
    {
        var day = new Day03(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        (await day.Solve_1()).Should().Be("4361");
    }

    [Test]
    public async ValueTask Solve2Test()
    {
        var day = new Day03(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        (await day.Solve_2()).Should().Be("467835");
    }
}