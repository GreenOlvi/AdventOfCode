namespace AOC2025Tests.Puzzles;

public class Day08Test
{
    private static readonly string _testInput1 = """
162,817,812
57,618,57
906,360,560
592,479,940
352,342,300
466,668,158
542,29,236
431,825,988
739,650,466
52,470,668
216,146,977
819,987,18
117,168,530
805,96,715
346,949,466
970,615,88
941,993,340
862,61,35
984,92,344
425,690,689
""";

    [Test]
    public void Solve1Test()
    {
        var day = new Day08(_testInput1.Split("\n", StringSplitOptions.TrimEntries))
        {
            Connections = 10
        };
        _ = day.Solve1().Should().Be(40);
    }

    [Test]
    public void Solve2Test()
    {
        var day = new Day08(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        _ = day.Solve2().Should().Be(25272);
    }
}
