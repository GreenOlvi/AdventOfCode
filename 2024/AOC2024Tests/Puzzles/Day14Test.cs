namespace AOC2024Tests.Puzzles;

public class Day14Test
{
    private static readonly string _testInput1 = """
p=0,4 v=3,-3
p=6,3 v=-1,-3
p=10,3 v=-1,2
p=2,0 v=2,-1
p=0,0 v=1,3
p=3,0 v=-2,-2
p=7,6 v=-1,-3
p=3,0 v=-1,-2
p=9,3 v=2,3
p=7,3 v=-1,2
p=2,4 v=2,-3
p=9,5 v=-3,-3
""";

    [Test]
    public void Solve1Test()
    {
        var day = new Day14(_testInput1.Split("\n", StringSplitOptions.TrimEntries))
        {
            Width = 11,
            Height = 7,
        };
        _ = day.Solve1().Should().Be(12);
    }
}
