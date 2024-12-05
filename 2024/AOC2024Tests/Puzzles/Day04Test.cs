namespace AOC2024Tests.Puzzles;

public class Day04Test
{
    private static readonly string _testInput1 = """
MMMSXXMASM
MSAMXMSMSA
AMXSXMAAMM
MSAMASMSMX
XMASAMXAMM
XXAMMXXAMA
SMSMSASXSS
SAXAMASAAA
MAMMMXMMMM
MXMXAXMASX
""";

    [Test]
    public void Solve1Test()
    {
        var day = new Day04(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        _ = day.Solve1().Should().Be(18);
    }

    [Test]
    public void Solve2Test()
    {
        var day = new Day04(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        _ = day.Solve2().Should().Be(9);
    }
}
