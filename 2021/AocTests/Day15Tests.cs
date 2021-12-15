using NUnit.Framework;
using AOC2021.Day15;
using FluentAssertions;

namespace AocTests
{
    public class Day15Tests
    {
        private static readonly string[] _testInput = new string[]
        {
            "1163751742",
            "1381373672",
            "2136511328",
            "3694931569",
            "7463417111",
            "1319128137",
            "1359912421",
            "3125421639",
            "1293138521",
            "2311944581",
        };

        private readonly Puzzle _puzzle = new(_testInput);

        [Test]
        public void Solution1Test()
        {
            _puzzle.Solution1().Should().Be(40);
        }

        [Test]
        public void Solution2Test()
        {
            _puzzle.Solution2().Should().Be(0);
        }
    }
}