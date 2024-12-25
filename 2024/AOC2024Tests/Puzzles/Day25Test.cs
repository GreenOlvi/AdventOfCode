namespace AOC2024Tests.Puzzles;

public class Day25Test
{
    private static readonly string _testInput1 = """
#####
.####
.####
.####
.#.#.
.#...
.....

#####
##.##
.#.##
...##
...#.
...#.
.....

.....
#....
#....
#...#
#.#.#
#.###
#####

.....
.....
#.#..
###..
###.#
###.#
#####

.....
.....
.....
#....
#.#..
#.#.#
#####
""";


    [Test]
    public void Solve1Test()
    {
        var day = new Day25(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        _ = day.Solve1().Should().Be(3);
    }
}
