namespace AOC2025Tests.Puzzles;

public class Day02Test
{
    private static readonly string _testInput1 = """11-22,95-115,998-1012,1188511880-1188511890,222220-222224,1698522-1698528,446443-446449,38593856-38593862,565653-565659,824824821-824824827,2121212118-2121212124""";

    [Test]
    public void Solve1Test()
    {
        var day = new Day02(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        _ = day.Solve1().Should().Be(1227775554);
    }

    [Test]
    public void Solve2Test()
    {
        var day = new Day02(_testInput1.Split("\n", StringSplitOptions.TrimEntries));
        _ = day.Solve2().Should().Be(4174379265);
    }
}
