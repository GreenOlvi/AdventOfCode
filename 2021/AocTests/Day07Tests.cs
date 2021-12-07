using NUnit.Framework;
using AOC2021.Day07;
using FluentAssertions;

namespace AocTests
{
    public class Day07Tests
    {
        private static readonly string[] _testInput = new string[]
        {
            "16,1,2,0,4,2,7,1,2,14",
        };

        private readonly Puzzle _puzzle = new(_testInput);

        [Test]
        public void Solution1Test()
        {
            _puzzle.Solution1().Should().Be(37);
        }

        [Test]
        public void Solution2Test()
        {
            _puzzle.Solution2().Should().Be(168);
        }
    }
}