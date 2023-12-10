namespace Tests.Puzzles;

public class Day10Tests
{
    private static readonly string _testInput1 = """
-L|F7
7S-7|
L|7||
-L-J|
L|-JF
""";

    private static readonly string _testInput2 = """
..F7.
.FJ|.
SJ.L7
|F--J
LJ...
""";

    private static readonly string _testInput3 = """
...........
.S-------7.
.|F-----7|.
.||.....||.
.||.....||.
.|L-7.F-J|.
.|..|.|..|.
.L--J.L--J.
...........
""";

    private static readonly string _testInput4 = """
.F----7F7F7F7F-7....
.|F--7||||||||FJ....
.||.FJ||||||||L7....
FJL7L7LJLJ||LJ.L-7..
L--J.L7...LJS7F-7L7.
....F-J..F7FJ|L7L7L7
....L7.F7||L7|.L7L7|
.....|FJLJ|FJ|F7|.LJ
....FJL-7.||.||||...
....L---J.LJ.LJLJ....
""";

    private static readonly string _testInput5 = """
FF7FSF7F7F7F7F7F---7
L|LJ||||||||||||F--J
FL-7LJLJ||||||LJL-77
F--JF--7||LJLJ7F7FJ-
L---JF-JLJ.||-FJLJJ7
|F|F-JF---7F7-L7L|7|
|FFJF7L7F-JF7|JL---7
7-L-JL7||F7|L7F-7F7|
L.L7LFJ|||||FJL7||LJ
L7JLJL-JLJLJL--JLJ.L
""";

    [Test]
    public async ValueTask Solve1ATest()
    {
        var day = new Day10(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        (await day.Solve_1()).Should().Be("4");
    }

    [Test]
    public async ValueTask Solve1BTest()
    {
        var day = new Day10(_testInput2.Split("\n", StringSplitOptions.TrimEntries));
        (await day.Solve_1()).Should().Be("8");
    }

    [Test]
    public async ValueTask Solve2_1Test()
    {
        var day = new Day10(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        (await day.Solve_2()).Should().Be("1");
    }

    [Test]
    public async ValueTask Solve2_2Test()
    {
        var day = new Day10(_testInput2.Split("\n", StringSplitOptions.TrimEntries));
        (await day.Solve_2()).Should().Be("1");
    }

    [Test]
    public async ValueTask Solve2_3Test()
    {
        var day = new Day10(_testInput3.Split("\n", StringSplitOptions.TrimEntries));
        (await day.Solve_2()).Should().Be("4");
    }

    [Test]
    public async ValueTask Solve2_4Test()
    {
        var day = new Day10(_testInput4.Split("\n", StringSplitOptions.TrimEntries));
        (await day.Solve_2()).Should().Be("8");
    }

    [Test]
    public async ValueTask Solve2_5Test()
    {
        var day = new Day10(_testInput5.Split("\n", StringSplitOptions.TrimEntries));
        (await day.Solve_2()).Should().Be("10");
    }
}