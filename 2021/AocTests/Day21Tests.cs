using NUnit.Framework;
using AOC2021.Day21;
using FluentAssertions;

namespace AocTests
{
    public class Day21Tests
    {
        private static readonly string[] _testInput = new string[]
        {
            "Player 1 starting position: 4",
            "Player 2 starting position: 8",
        };

        private readonly Puzzle _puzzle = new(_testInput);

        [Test]
        public void Solution1Test()
        {
            _puzzle.Solution1().Should().Be(739785);
        }

        [Test]
        public void Solution2Test()
        {
            _puzzle.Solution2().Should().Be(444356092776315);
        }
    }
}