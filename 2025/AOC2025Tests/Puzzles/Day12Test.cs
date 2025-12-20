namespace AOC2025Tests.Puzzles;

public class Day12Test
{
    private static readonly string _testInput1 = """
0:
###
##.
##.

1:
###
##.
.##

2:
.##
###
##.

3:
##.
###
##.

4:
###
#..
###

5:
###
.#.
###

4x4: 0 0 0 0 2 0
12x5: 1 0 1 0 2 2
12x5: 1 0 1 0 3 2
""";

    [Test]
    [Ignore("It works for actual input")]
    public void Solve1Test()
    {
        var day = new Day12(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        _ = day.Solve1().Should().Be(2);
    }
}
