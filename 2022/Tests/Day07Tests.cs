namespace Tests;

[TestFixture]
public class Day07Tests
{
    private readonly IEnumerable<string> _testInput = new string[]
    {
        "$ cd /",
        "$ ls",
        "dir a",
        "14848514 b.txt",
        "8504156 c.dat",
        "dir d",
        "$ cd a",
        "$ ls",
        "dir e",
        "29116 f",
        "2557 g",
        "62596 h.lst",
        "$ cd e",
        "$ ls",
        "584 i",
        "$ cd ..",
        "$ cd ..",
        "$ cd d",
        "$ ls",
        "4060174 j",
        "8033020 d.log",
        "5626152 d.ext",
        "7214296 k",
    };

    [Test]
    public async ValueTask Solve1Test()
    {
        var puzzle = new Day07(_testInput);
        (await puzzle.Solve_1()).Should().Be("95437");
    }

    [Test]
    public async ValueTask Solve2Test()
    {
        var puzzle = new Day07(_testInput);
        (await puzzle.Solve_2()).Should().Be("24933642");
    }
}