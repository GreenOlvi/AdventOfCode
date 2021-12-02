using NUnit.Framework;
using AOC2021.Day02;
using FluentAssertions;

namespace AocTests
{
    public class Day02Tests
    {
        private static readonly string[] _testInput = new string[]
        {
            "forward 5",
            "down 5",
            "forward 8",
            "up 3",
            "down 8",
            "forward 2"
        };

        private readonly Puzzle _puzzle = new(_testInput);

        [Test]
        public void Solution1Test()
        {
            _puzzle.Solution1().Should().Be(150);
        }

        [Test]
        public void Solution2Test()
        {
            _puzzle.Solution2().Should().Be(900);
        }
    }
}