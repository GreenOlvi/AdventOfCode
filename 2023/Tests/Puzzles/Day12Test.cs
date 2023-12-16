namespace Tests.Puzzles;

public class Day12Tests
{
    private static readonly string _testInput1 = """
???.### 1,1,3
.??..??...?##. 1,1,3
?#?#?#?#?#?#?#? 1,3,1,6
????.#...#... 4,1,1
????.######..#####. 1,6,5
?###???????? 3,2,1
""";

    [Test]
    public async ValueTask Solve1Test()
    {
        var day = new Day12(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        (await day.Solve_1()).Should().Be("21");
    }

    [Test]
    [Ignore("Not finished")]
    public async ValueTask Solve2Test()
    {
        var day = new Day12(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        (await day.Solve_2()).Should().Be("525152");
    }
}