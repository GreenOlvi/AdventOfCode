using NUnit.Framework;
using AOC2021.Day06;
using FluentAssertions;

namespace AocTests
{
    public class Day06Tests
    {
        private static readonly string[] _testInput = new string[]
        {
            "3,4,3,1,2",
        };

        private readonly Puzzle _puzzle = new(_testInput);

        [Test]
        public void Solution1Test()
        {
            _puzzle.Solution1().Should().Be(5934);
        }

        [Test]
        public void Solution2Test()
        {
            _puzzle.Solution2().Should().Be(26984457539);
        }
    }
}