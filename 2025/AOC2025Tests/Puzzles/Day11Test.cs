namespace AOC2025Tests.Puzzles;

public class Day11Test
{
    private static readonly string _testInput1 = """
aaa: you hhh
you: bbb ccc
bbb: ddd eee
ccc: ddd eee fff
ddd: ggg
eee: out
fff: out
ggg: out
hhh: ccc fff iii
iii: out
""";

    [Test]
    public void Solve1Test()
    {
        var day = new Day11(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        _ = day.Solve1().Should().Be(5);
    }

    private static readonly string _testInput2 = """
svr: aaa bbb
aaa: fft
fft: ccc
bbb: tty
tty: ccc
ccc: ddd eee
ddd: hub
hub: fff
eee: dac
dac: fff
fff: ggg hhh
ggg: out
hhh: out
""";

    [Test]
    public void Solve2Test()
    {
        var day = new Day11(_testInput2.Split("\n", StringSplitOptions.TrimEntries));
        _ = day.Solve2().Should().Be(2);
    }
}
