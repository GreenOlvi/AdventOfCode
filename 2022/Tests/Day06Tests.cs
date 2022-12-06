using FluentAssertions.Execution;

namespace Tests;

[TestFixture]
public class Day06Tests
{
    private readonly IEnumerable<string> _testInput = new string[]
    {
        "mjqjpqmgbljsphdztnvjfqwrcgsmlb",
    };

    [TestCase("bvwbjplbgvbhsrlpgdmjqwftvncz", 5, 23)]
    [TestCase("nppdvjthqldpwncqszvftbrmjlhg", 6, 23)]
    [TestCase("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", 10, 29)]
    [TestCase("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 11, 26)]
    public void FirstDifferentTest(string input, int expected4, int expected14)
    {
        using var ctx = new AssertionScope(input);
        Day06.FirstDifferent(input, 4).Should().Be(expected4);
        Day06.FirstDifferent(input, 14).Should().Be(expected14);
    }

    [Test]
    public async ValueTask Solve1Test()
    {
        var puzzle = new Day06(_testInput);
        (await puzzle.Solve_1()).Should().Be("7");
    }

    [Test]
    public async ValueTask Solve2Test()
    {
        var puzzle = new Day06(_testInput);
        (await puzzle.Solve_2()).Should().Be("19");
    }
}