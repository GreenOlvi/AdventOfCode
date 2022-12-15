namespace Tests;

[TestFixture]
public class Day15Tests
{
    private readonly IEnumerable<string> _testInput = new string[]
    {
        "Sensor at x=2, y=18: closest beacon is at x=-2, y=15",
        "Sensor at x=9, y=16: closest beacon is at x=10, y=16",
        "Sensor at x=13, y=2: closest beacon is at x=15, y=3",
        "Sensor at x=12, y=14: closest beacon is at x=10, y=16",
        "Sensor at x=10, y=20: closest beacon is at x=10, y=16",
        "Sensor at x=14, y=17: closest beacon is at x=10, y=16",
        "Sensor at x=8, y=7: closest beacon is at x=2, y=10",
        "Sensor at x=2, y=0: closest beacon is at x=2, y=10",
        "Sensor at x=0, y=11: closest beacon is at x=2, y=10",
        "Sensor at x=20, y=14: closest beacon is at x=25, y=17",
        "Sensor at x=17, y=20: closest beacon is at x=21, y=22",
        "Sensor at x=16, y=7: closest beacon is at x=15, y=3",
        "Sensor at x=14, y=3: closest beacon is at x=15, y=3",
        "Sensor at x=20, y=1: closest beacon is at x=15, y=3",
    };

    [Test]
    public async ValueTask Solve1Test()
    {
        var puzzle = new Day15(_testInput) { CheckLevel = 10 };
        (await puzzle.Solve_1()).Should().Be("26");
    }

    [Test]
    public async ValueTask Solve2Test()
    {
        var puzzle = new Day15(_testInput) { MaxCoord = 20 };
        (await puzzle.Solve_2()).Should().Be("56000011");
    }
}