using NUnit.Framework;
using FluentAssertions;
using AOC2020.Day12;

namespace Tests
{
    [TestFixture]
    public class Day12Tests
    {
        private static readonly string[] ExampleData =
        {
            "F10",
            "N3",
            "F7",
            "R90",
            "F11",
        };

        private readonly Puzzle _example = new Puzzle(ExampleData);

        [Test]
        public void Solution1Test()
        {
            _example.Solution1().Should().Be(25);
        }

        [Test]
        public void Solution2Test()
        {
            _example.Solution2().Should().Be(286);
        }
    }
}
