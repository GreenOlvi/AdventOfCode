namespace AOC2024Tests.Puzzles;

public class Day23Test
{
    private static readonly string _testInput1 = """
kh-tc
qp-kh
de-cg
ka-co
yn-aq
qp-ub
cg-tb
vc-aq
tb-ka
wh-tc
yn-cg
kh-ub
ta-co
de-co
tc-td
tb-wq
wh-td
ta-ka
td-qp
aq-cg
wq-ub
ub-vc
de-ta
wq-aq
wq-vc
wh-yn
ka-de
kh-ta
co-tc
wh-qp
tb-vc
td-yn
""";

    [Test]
    public void Solve1Test()
    {
        var day = new Day23(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        _ = day.Solve1().Should().Be(7);
    }

    [Test]
    public void Solve2Test()
    {
        var day = new Day23(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        _ = day.Solve2().Should().Be("co,de,ka,ta");
    }
}
