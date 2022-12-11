using NUnit.Framework.Internal;

namespace Tests;

[TestFixture]
public class Day11Tests
{
    private readonly IEnumerable<string> _testInput = new string[]
    {
        "Monkey 0:",
        "  Starting items: 79, 98",
        "  Operation: new = old * 19",
        "  Test: divisible by 23",
        "    If true: throw to monkey 2",
        "    If false: throw to monkey 3",
        "",
        "Monkey 1:",
        "  Starting items: 54, 65, 75, 74",
        "  Operation: new = old + 6",
        "  Test: divisible by 19",
        "    If true: throw to monkey 2",
        "    If false: throw to monkey 0",
        "",
        "Monkey 2:",
        "  Starting items: 79, 60, 97",
        "  Operation: new = old * old",
        "  Test: divisible by 13",
        "    If true: throw to monkey 1",
        "    If false: throw to monkey 3",
        "",
        "Monkey 3:",
        "  Starting items: 74",
        "  Operation: new = old + 3",
        "  Test: divisible by 17",
        "    If true: throw to monkey 0",
        "    If false: throw to monkey 1",
    };

    [Test]
    public async ValueTask Solve1Test()
    {
        var puzzle = new Day11(_testInput);
        (await puzzle.Solve_1()).Should().Be("10605");
    }

    [Test]
    public async ValueTask Solve2Test()
    {
        var puzzle = new Day11(_testInput);
        (await puzzle.Solve_2()).Should().Be("2713310158");
    }
}