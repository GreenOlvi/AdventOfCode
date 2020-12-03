using NUnit.Framework;
using FluentAssertions;
using AOC2020.Puzzle03;

namespace Tests
{
    [TestFixture]
    public class Puzzle03Tests
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

        private readonly P03 _example = new P03(ExampleData);

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
