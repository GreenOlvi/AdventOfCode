namespace AOC2025Tests.Puzzles;

public class Day10Test
{
    private static readonly string _testInput1 = """
[.##.] (3) (1,3) (2) (2,3) (0,2) (0,1) {3,5,4,7}
[...#.] (0,2,3,4) (2,3) (0,4) (0,1,2) (1,2,3,4) {7,5,12,7,2}
[.###.#] (0,1,2,3,4) (0,3,4) (0,1,2,4,5) (1,2) {10,11,11,5,10,5}
""";

    [Test]
    public void Solve1Test()
    {
        var day = new Day10(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        _ = day.Solve1().Should().Be(7);
    }

    [Test]
    public void Solve2Test()
    {
        var day = new Day10(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        _ = day.Solve2().Should().Be(33);
    }

    [TestCase("[####]", 0x0f)]
    [TestCase("[.#.#]", 0x0a)]
    public void ParseLightsTests(string input, int expected)
    {
        Day10.ParseLights(input).Should().Be((uint)expected);
    }

    [TestCase("(0,2,3,4)", 0x1d)]
    [TestCase("(0)", 0x01)]
    [TestCase("(1)", 0x02)]
    public void ParseButtonTests(string input, int expected)
    {
        Day10.ParseButton(input).Should().Be((uint)expected);
    }
}
