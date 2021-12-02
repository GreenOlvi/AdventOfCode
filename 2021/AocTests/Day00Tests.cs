using NUnit.Framework;
using AOC2021.Day00;
using FluentAssertions;

namespace AocTests
{
    public class Day00Tests
    {
        private static readonly string[] _testInput = new string[]
        {
        };

        private Puzzle _puzzle = new(_testInput);

        [Test]
        public void Solution1Test()
        {
            _puzzle.Solution1().Should().Be(0);
        }

        [Test]
        public void Solution2Test()
        {
            _puzzle.Solution2().Should().Be(0);
        }
    }
}