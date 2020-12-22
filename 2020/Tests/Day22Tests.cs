using NUnit.Framework;
using FluentAssertions;
using AOC2020.Day22;

namespace Tests
{
    [TestFixture]
    public class Day22Tests
    {
        private static readonly string[] ExampleData =
        {
            "Player 1:",
            "9",
            "2",
            "6",
            "3",
            "1",
            "",
            "Player 2:",
            "5",
            "8",
            "4",
            "7",
            "10",
        };

        private readonly Puzzle _example = new Puzzle(ExampleData);

        [Test]
        public void Solution1Test()
        {
            _example.Solution1().Should().Be(306);
        }

        [Test]
        [Ignore("Not solved yet")]
        public void Solution2Test()
        {
            _example.Solution2().Should().Be(291);
        }
    }
}
