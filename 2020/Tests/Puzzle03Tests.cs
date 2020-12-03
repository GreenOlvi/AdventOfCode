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

        [Test]
        public void Solution1Test()
        {
            var p = new P03(ExampleData);
            p.Solution1().Should().Be(7);
        }

        [Test]
        public void Solution2Test()
        {
            var p = new P03(ExampleData);
            p.Solution2().Should().Be(336);
        }
    }
}
