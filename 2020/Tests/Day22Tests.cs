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

        private static readonly string[] InfiniteLoopExample =
        {
            "Player 1:",
            "43",
            "19",
            "",
            "Player 2:",
            "2",
            "29",
            "14",
        };

        private readonly Puzzle _example = new Puzzle(ExampleData);

        [Test]
        public void Solution1Test()
        {
            _example.Solution1().Should().Be(306);
        }

        [Test]
        public void Solution2Test()
        {
            _example.Solution2().Should().Be(291);
        }

        [Test]
        public void InfiniteLoopAvoidintTest()
        {
            new Puzzle(InfiniteLoopExample).Solution2().Should().Be(105);
        }
    }
}
