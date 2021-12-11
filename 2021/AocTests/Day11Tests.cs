using NUnit.Framework;
using AOC2021.Day11;
using FluentAssertions;

namespace AocTests
{
    public class Day11Tests
    {
        private static readonly string[] _testInput = new string[]
        {
            "5483143223",
            "2745854711",
            "5264556173",
            "6141336146",
            "6357385478",
            "4167524645",
            "2176841721",
            "6882881134",
            "4846848554",
            "5283751526",
        };

        private readonly Puzzle _puzzle = new(_testInput);

        [Test]
        public void Solution1Test()
        {
            _puzzle.Solution1().Should().Be(1656);
        }

        [Test]
        public void Solution2Test()
        {
            _puzzle.Solution2().Should().Be(195);
        }
    }
}