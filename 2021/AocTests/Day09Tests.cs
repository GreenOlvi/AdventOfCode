using NUnit.Framework;
using AOC2021.Day09;
using FluentAssertions;

namespace AocTests
{
    public class Day09Tests
    {
        private static readonly string[] _testInput = new string[]
        {
            "2199943210",
            "3987894921",
            "9856789892",
            "8767896789",
            "9899965678",
        };

        private readonly Puzzle _puzzle = new(_testInput);

        [Test]
        public void Solution1Test()
        {
            _puzzle.Solution1().Should().Be(15);
        }

        [Test]
        public void Solution2Test()
        {
            _puzzle.Solution2().Should().Be(1134);
        }
    }
}