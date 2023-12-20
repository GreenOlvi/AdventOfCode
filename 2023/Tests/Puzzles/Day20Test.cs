namespace Tests.Puzzles;

public class Day20Tests
{
    private static readonly string _testInput1 = """
broadcaster -> a, b, c
%a -> b
%b -> c
%c -> inv
&inv -> a
""";

    private static readonly string _testInput2 = """
broadcaster -> a
%a -> inv, con
&inv -> b
%b -> con
&con -> output
""";

    [Test]
    public async ValueTask Solve1_1Test()
    {
        var day = new Day20(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        (await day.Solve_1()).Should().Be("32000000");
    }

    [Test]
    public async ValueTask Solve1_2Test()
    {
        var day = new Day20(_testInput2.Split("\n", StringSplitOptions.TrimEntries));
        (await day.Solve_1()).Should().Be("11687500");
    }

    [Test]
    [Ignore("Not solved yet")]
    public async ValueTask Solve2Test()
    {
        var input = _testInput2
            .Replace("output", "rx")
            .Split("\n", StringSplitOptions.TrimEntries);

        var day = new Day20(input);
        (await day.Solve_2()).Should().Be("1");
    }
}