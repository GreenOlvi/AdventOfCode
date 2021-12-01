using NUnit.Framework;
using AOC2021.Day01;
using FluentAssertions;

namespace AocTests
{
    public class Puzzle01Tests
    {
        private static readonly string[] _testInput = new[]
        {
            "199",
            "200",
            "208",
            "210",
            "200",
            "207",
            "240",
            "269",
            "260",
            "263",
        };

        private Puzzle _puzzle = new(_testInput);

        [Test]
        public void Solution1Test()
        {
            _puzzle.Solution1().Should().Be(7);
        }

        [Test]
        public void Solution2Test()
        {
            _puzzle.Solution2().Should().Be(5);
        }
    }
}