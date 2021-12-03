using NUnit.Framework;
using AOC2021.Day03;
using FluentAssertions;

namespace AocTests
{
    public class Day03Tests
    {
        private static readonly string[] _testInput = new string[]
        {
            "00100",
            "11110",
            "10110",
            "10111",
            "10101",
            "01111",
            "00111",
            "11100",
            "10000",
            "11001",
            "00010",
            "01010",
        };

        private Puzzle _puzzle = new(_testInput);

        [Test]
        public void Solution1Test()
        {
            _puzzle.Solution1().Should().Be(198);
        }

        [Test]
        public void Solution2Test()
        {
            _puzzle.Solution2().Should().Be(230);
        }
    }
}