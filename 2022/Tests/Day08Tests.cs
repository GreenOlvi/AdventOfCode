namespace Tests;

[TestFixture]
public class Day08Tests
{
    private readonly IEnumerable<string> _testInput = new string[]
    {
        "30373",
        "25512",
        "65332",
        "33549",
        "35390",
    };

    [Test]
    public async ValueTask Solve1Test()
    {
        var puzzle = new Day08(_testInput);
        (await puzzle.Solve_1()).Should().Be("21");
    }

    [Test]
    public async ValueTask Solve2Test()
    {
        var puzzle = new Day08(_testInput);
        (await puzzle.Solve_2()).Should().Be("8");
    }

    [Test]
    public void CountTreesTest()
    {
        var grid = Day08.ParseInput(_testInput);
        Day08.CountTrees(grid, new Point2(2, 1), Direction.Up).Should().Be(1);
        Day08.CountTrees(grid, new Point2(2, 1), Direction.Left).Should().Be(1);
        Day08.CountTrees(grid, new Point2(2, 1), Direction.Right).Should().Be(2);
        Day08.CountTrees(grid, new Point2(2, 1), Direction.Down).Should().Be(2);
    }
}