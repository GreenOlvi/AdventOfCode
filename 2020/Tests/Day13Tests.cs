using NUnit.Framework;
using FluentAssertions;
using AOC2020.Day13;

namespace Tests
{
    [TestFixture]
    public class Day13Tests
    {
        private static readonly string[] ExampleData =
        {
            "939",
            "7,13,x,x,59,x,31,19"
        };

        private readonly Puzzle _example = new Puzzle(ExampleData);

        [Test]
        public void Solution1Test()
        {
            _example.Solution1().Should().Be(295);
        }

        [Test]
        public void Solution2Test()
        {
            _example.Solution2().Should().Be(1068781);
        }

        [TestCase("17,x,13,19", 3417)]
        [TestCase("67,7,59,61", 754018)]
        [TestCase("67,x,7,59,61", 779210)]
        [TestCase("67,7,x,59,61", 1261476)]
        [TestCase("1789,37,47,1889", 1202161486)]
        public void Solution2Examples(string ids, long expected)
        {
            new Puzzle(new[] { "0", ids }).Solution2()
                .Should().Be(expected);
        }
    }
}
