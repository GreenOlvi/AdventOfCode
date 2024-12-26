namespace AOC2024Tests.Puzzles;

[TestFixture]
public class Day12Test
{
    private static readonly string _testInputA = """
AAAA
BBCD
BBCC
EEEC
""";

    private static readonly string _testInputB = """
OOOOO
OXOXO
OOOOO
OXOXO
OOOOO
""";

    private static readonly string _testInputC = """
RRRRIICCFF
RRRRIICCCF
VVRRRCCFFF
VVRCCCJFFF
VVVVCJJCFE
VVIVCCJJEE
VVIIICJJEE
MIIIIIJJEE
MIIISIJEEE
MMMISSJEEE
""";

    private static readonly string _testInputD = """
EEEEE
EXXXX
EEEEE
EXXXX
EEEEE
""";


    private static readonly string _testInputE = """
AAAAAA
AAABBA
AAABBA
ABBAAA
ABBAAA
AAAAAA
""";


    private static readonly TestCaseData[] Solve1TestCases = [
        new(_testInputA, 140),
        new(_testInputB, 772),
        new(_testInputC, 1930),
    ];

    [Test, TestCaseSource(nameof(Solve1TestCases))]
    public void Solve1Test(string input, long expected)
    {
        var day = new Day12(input.Split("\n", StringSplitOptions.TrimEntries));
        _ = day.Solve1().Should().Be(expected);
    }

    private static readonly TestCaseData[] Solve2TestCases = [
        new(_testInputA, 80),
        new(_testInputB, 436),
        new(_testInputC, 1206),
        new(_testInputD, 236),
        new(_testInputE, 368),
    ];

    [Test, TestCaseSource(nameof(Solve2TestCases))]
    public void Solve2Test(string input, long expected)
    {
        var day = new Day12(input.Split("\n", StringSplitOptions.TrimEntries));
        _ = day.Solve2().Should().Be(expected);
    }
}
