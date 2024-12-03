namespace AOC2024Tests.Puzzles;

public class Day03Test
{
    private static readonly string _testInput1 = """
xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))
""";

    private static readonly string _testInput2 = """
xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))
""";

    [Test]
    public void Solve1Test()
    {
        var day = new Day03(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        _ = day.Solve1().Should().Be(161);
    }

    [Test]
    public void Solve2Test()
    {
        var day = new Day03(_testInput2.Split("\n", StringSplitOptions.TrimEntries));
        _ = day.Solve2().Should().Be(48);
    }
}
