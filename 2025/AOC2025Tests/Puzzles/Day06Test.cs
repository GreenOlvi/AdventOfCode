namespace AOC2025Tests.Puzzles;

public class Day06Test
{
    private static readonly string _testInput1 = """
123 328  51 64 
 45 64  387 23 
  6 98  215 314
*   +   *   +  
""";

    [Test]
    public void Solve1Test()
    {
        var day = new Day06(_testInput1.Split("\n"));
        _ = day.Solve1().Should().Be(4277556);
    }

    [Test]
    public void Solve2Test()
    {
        var day = new Day06(_testInput1.Split("\n"));
        _ = day.Solve2().Should().Be(3263827);
    }
}
