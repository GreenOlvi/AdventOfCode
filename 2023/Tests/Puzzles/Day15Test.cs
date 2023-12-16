namespace Tests.Puzzles;

public class Day15Tests
{
    private static readonly string _testInput1 = """
rn=1,cm-,qp=3,cm=2,qp-,pc=4,ot=9,ab=5,pc-,pc=6,ot=7
""";

    [Test]
    public async ValueTask Solve1Test()
    {
        var day = new Day15(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        (await day.Solve_1()).Should().Be("1320");
    }

    [Test]
    public async ValueTask Solve2Test()
    {
        var day = new Day15(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        (await day.Solve_2()).Should().Be("145");
    }

    [TestCase("HASH", 52)]
    public void HashTests(string input, int expected)
    {
        Day15.Hash(input).Should().Be(expected);
    }
}