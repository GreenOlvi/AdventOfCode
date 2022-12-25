namespace Tests;

[TestFixture]
public class Day25Tests
{
    private readonly IEnumerable<string> _testInput = new string[]
    {
        "1=-0-2",
        "12111",
        "2=0=",
        "21",
        "2=01",
        "111",
        "20012",
        "112",
        "1=-1=",
        "1-12",
        "12",
        "1=",
        "122",
    };

    [Test]
    public async ValueTask Solve1Test()
    {
        var puzzle = new Day25(_testInput);
        (await puzzle.Solve_1()).Should().Be("2=-1=0");
    }

    [TestCase("1", 1)]
    [TestCase("2", 2)]
    [TestCase("1=", 3)]
    [TestCase("1-", 4)]
    [TestCase("10", 5)]
    [TestCase("11", 6)]
    [TestCase("12", 7)]
    [TestCase("2=", 8)]
    [TestCase("2-", 9)]
    [TestCase("20", 10)]
    [TestCase("1=0", 15)]
    [TestCase("1-0", 20)]
    [TestCase("1=11-2", 2022)]
    [TestCase("1-0---0", 12345)]
    [TestCase("1121-1110-1=0", 314159265)]
    [TestCase("1=-0-2", 1747)]
    [TestCase("12111", 906)]
    [TestCase("2=0=", 198)]
    [TestCase("21", 11)]
    [TestCase("2=01", 201)]
    [TestCase("111", 31)]
    [TestCase("20012", 1257)]
    [TestCase("112", 32)]
    [TestCase("1=-1=", 353)]
    [TestCase("1-12", 107)]
    [TestCase("122", 37)]
    public void FromSnafuTests(string snafu, long expected)
    {
        Day25.FromSnafu(snafu).Should().Be(expected);
    }

    [TestCase("1", 1)]
    [TestCase("2", 2)]
    [TestCase("1=", 3)]
    [TestCase("1-", 4)]
    [TestCase("10", 5)]
    [TestCase("11", 6)]
    [TestCase("12", 7)]
    [TestCase("2=", 8)]
    [TestCase("2-", 9)]
    [TestCase("20", 10)]
    [TestCase("1=0", 15)]
    [TestCase("1-0", 20)]
    [TestCase("1=11-2", 2022)]
    [TestCase("1-0---0", 12345)]
    [TestCase("1121-1110-1=0", 314159265)]
    [TestCase("1=-0-2", 1747)]
    [TestCase("12111", 906)]
    [TestCase("2=0=", 198)]
    [TestCase("21", 11)]
    [TestCase("2=01", 201)]
    [TestCase("111", 31)]
    [TestCase("20012", 1257)]
    [TestCase("112", 32)]
    [TestCase("1=-1=", 353)]
    [TestCase("1-12", 107)]
    [TestCase("122", 37)]
    public void ToSnafuTests(string expected, long number)
    {
        Day25.ToSnafu(number).Should().Be(expected);
    }
}