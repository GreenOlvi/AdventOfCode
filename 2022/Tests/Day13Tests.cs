using static AOC2022.Puzzles.Day13;

namespace Tests;

[TestFixture]
public class Day13Tests
{
    private readonly IEnumerable<string> _testInput = new string[]
    {
        "[1,1,3,1,1]",
        "[1,1,5,1,1]",
        "",
        "[[1],[2,3,4]]",
        "[[1],4]",
        "",
        "[9]",
        "[[8,7,6]]",
        "",
        "[[4,4],4,4]",
        "[[4,4],4,4,4]",
        "",
        "[7,7,7,7]",
        "[7,7,7]",
        "",
        "[]",
        "[3]",
        "",
        "[[[]]]",
        "[[]]",
        "",
        "[1,[2,[3,[4,[5,6,7]]]],8,9]",
        "[1,[2,[3,[4,[5,6,0]]]],8,9]",
    };

    [Test]
    public async ValueTask Solve1Test()
    {
        var puzzle = new Day13(_testInput);
        (await puzzle.Solve_1()).Should().Be("13");
    }

    [Test]
    public async ValueTask Solve2Test()
    {
        var puzzle = new Day13(_testInput);
        (await puzzle.Solve_2()).Should().Be("140");
    }

    [TestCase("[1,1,3,1,1]", "[1,1,5,1,1]", -1)]
    [TestCase("[[1],[2,3,4]]", "[[1],4]", -1)]
    [TestCase("[9]", "[[8,7,6]]", 1)]
    [TestCase("[[4,4],4,4]", "[[4,4],4,4,4]", -1)]
    [TestCase("[7,7,7,7]", "[7,7,7]", 1)]
    [TestCase("[]", "[3]", -1)]
    [TestCase("[[[]]]", "[[]]", 1)]
    [TestCase("[1,[2,[3,[4,[5,6,7]]]],8,9]", "[1,[2,[3,[4,[5,6,0]]]],8,9]", 1)]
    public void ComparerTests(string x, string y, int expected)
    {
        var comparer = new PacketElementComparer();
        var xp = ParseList(x);
        var yp = ParseList(y);
        comparer.Compare(xp, yp).Should().Be(expected);
    }
}