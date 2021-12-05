using NUnit.Framework;
using AOC2021.Day05;
using FluentAssertions;

namespace AocTests
{
    public class Day05Tests
    {
        private static readonly string[] _testInput = new string[]
        {
            "0,9 -> 5,9",
            "8,0 -> 0,8",
            "9,4 -> 3,4",
            "2,2 -> 2,1",
            "7,0 -> 7,4",
            "6,4 -> 2,0",
            "0,9 -> 2,9",
            "3,4 -> 1,4",
            "0,0 -> 8,8",
            "5,5 -> 8,2",
        };

        private readonly Puzzle _puzzle = new(_testInput);

        [Test]
        public void Solution1Test()
        {
            _puzzle.Solution1().Should().Be(5);
        }

        [Test]
        public void Solution2Test()
        {
            _puzzle.Solution2().Should().Be(12);
        }
    }
}