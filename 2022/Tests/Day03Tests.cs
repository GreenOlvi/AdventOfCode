namespace Tests;

[TestFixture]
public class Day03Tests
{
    private readonly IEnumerable<string> _testInput = new string[]
    {
        "vJrwpWtwJgWrhcsFMMfFFhFp",
        "jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL",
        "PmmdzqPrVvPwwTWBwg",
        "wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn",
        "ttgJtRGJQctTZtZT",
        "CrZsJsPPZsGzwwsLwLmpwMDw",
    };

    [Test]
    public async ValueTask Solve1Test()
    {
        var puzzle = new Day03(_testInput);
        (await puzzle.Solve_1()).Should().Be("157");
    }

    [Test]
    public async ValueTask Solve2Test()
    {
        var puzzle = new Day03(_testInput);
        (await puzzle.Solve_2()).Should().Be("70");
    }

    [TestCase('a', 1ul)]
    [TestCase('b', 2ul)]
    [TestCase('z', 0x200_0000ul)]
    [TestCase('A', 0x400_0000ul)]
    [TestCase('Z', 0x8_0000_0000_0000ul)]
    public void GetBitTest(char letter, ulong result)
    {
        Day03.GetBit(letter).Should().Be(result);
    }

    [TestCase(1ul, 1)]
    [TestCase(2ul, 2)]
    [TestCase(0x200_0000ul, 26)]
    [TestCase(0x400_0000ul, 27)]
    [TestCase(0x8_0000_0000_0000ul, 52)]
    public void GetPriority(ulong value, int prioity)
    {
        Day03.GetPriority(value).Should().Be(prioity);
    }

    [TestCase("vJrwpWtwJgWrhcsFMMfFFhFp", "vJrwpWtwJgWr", "hcsFMMfFFhFp")]
    public void SplitStringTests(string input, string expected1, string expected2)
    {
        Day03.SplitString(input).Should().Be((expected1, expected2));
    }
}