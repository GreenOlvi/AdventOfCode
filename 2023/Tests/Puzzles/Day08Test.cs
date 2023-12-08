namespace Tests.Puzzles;

public class Day08Tests
{
    private static readonly string _testInput1 = """
RL

AAA = (BBB, CCC)
BBB = (DDD, EEE)
CCC = (ZZZ, GGG)
DDD = (DDD, DDD)
EEE = (EEE, EEE)
GGG = (GGG, GGG)
ZZZ = (ZZZ, ZZZ)
""";

    private static readonly string _testInput2 = """
LLR

AAA = (BBB, BBB)
BBB = (AAA, ZZZ)
ZZZ = (ZZZ, ZZZ)
""";

    private static readonly string _testInput3 = """
LR

11A = (11B, XXX)
11B = (XXX, 11Z)
11Z = (11B, XXX)
22A = (22B, XXX)
22B = (22C, 22C)
22C = (22Z, 22Z)
22Z = (22B, 22B)
XXX = (XXX, XXX)
""";

    [Test]
    public async ValueTask Solve1ATest()
    {
        var day = new Day08(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        (await day.Solve_1()).Should().Be("2");
    }

    [Test]
    public async ValueTask Solve1BTest()
    {
        var day = new Day08(_testInput2.Split("\n", StringSplitOptions.TrimEntries));
        (await day.Solve_1()).Should().Be("6");
    }

    [Test]
    public async ValueTask Solve2Test()
    {
        var day = new Day08(_testInput3.Split("\n", StringSplitOptions.TrimEntries));
        (await day.Solve_2()).Should().Be("6");
    }
}