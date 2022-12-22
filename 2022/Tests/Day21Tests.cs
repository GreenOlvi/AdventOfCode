namespace Tests;

[TestFixture]
public class Day21Tests
{
    private readonly IEnumerable<string> _testInput = new string[]
    {
        "root: pppw + sjmn",
        "dbpl: 5",
        "cczh: sllz + lgvd",
        "zczc: 2",
        "ptdq: humn - dvpt",
        "dvpt: 3",
        "lfqf: 4",
        "humn: 5",
        "ljgn: 2",
        "sjmn: drzm * dbpl",
        "sllz: 4",
        "pppw: cczh / lfqf",
        "lgvd: ljgn * ptdq",
        "drzm: hmdt - zczc",
        "hmdt: 32",
    };

    [Test]
    public async ValueTask Solve1Test()
    {
        var puzzle = new Day21(_testInput);
        (await puzzle.Solve_1()).Should().Be("152");
    }

    [Test]
    public async ValueTask Solve2Test()
    {
        var puzzle = new Day21(_testInput);
        (await puzzle.Solve_2()).Should().Be("301");
    }
}