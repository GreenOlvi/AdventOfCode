using NUnit.Framework;
using FluentAssertions;
using AOC2020.Day03;

namespace Tests
{
    [TestFixture]
    public class Day03Tests
    {
        private static readonly string[] ExampleData =
        {
            "..##.......",
            "#...#...#..",
            ".#....#..#.",
            "..#.#...#.#",
            ".#...##..#.",
            "..#.##.....",
            ".#.#.#....#",
            ".#........#",
            "#.##...#...",
            "#...##....#",
            ".#..#...#.#",
        };

        private readonly Puzzle _example = new Puzzle(ExampleData);

        [Test]
        public void Solution1Test()
        {
            _example.Solution1().Should().Be(7);
        }

        [TestCase(1, 1, 2)]
        [TestCase(3, 1, 7)]
        [TestCase(5, 1, 3)]
        [TestCase(7, 1, 4)]
        [TestCase(1, 2, 2)]
        public void CountTreesTests(int dx, int dy, int expected)
        {
            _example.CountTrees(dx, dy).Should().Be(expected);
        }

        [Test]
        public void Solution2Test()
        {
            _example.Solution2().Should().Be(336);
        }
    }
}
