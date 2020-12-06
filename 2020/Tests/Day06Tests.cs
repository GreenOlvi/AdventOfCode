using NUnit.Framework;
using FluentAssertions;
using AOC2020.Day06;

namespace Tests
{
    [TestFixture]
    public class Day06Tests
    {
        private static readonly string[] ExampleData =
        {
            "abc",
            "",
            "a",
            "b",
            "c",
            "",
            "ab",
            "ac",
            "",
            "a",
            "a",
            "a",
            "a",
            "",
            "b",
        };

        private readonly Puzzle _example = new Puzzle(ExampleData);

        [Test]
        public void Solution1Test()
        {
            _example.Solution1().Should().Be(11);
        }

        [Test]
        public void Solution2Test()
        {
            _example.Solution2().Should().Be(6);
        }
    }
}
