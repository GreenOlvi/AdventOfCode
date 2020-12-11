using NUnit.Framework;
using FluentAssertions;
using AOC2020.Day11;

namespace Tests
{
    [TestFixture]
    public class Day11Tests
    {
        private static readonly string[] ExampleData =
        {
            "L.LL.LL.LL",
            "LLLLLLL.LL",
            "L.L.L..L..",
            "LLLL.LL.LL",
            "L.LL.LL.LL",
            "L.LLLLL.LL",
            "..L.L.....",
            "LLLLLLLLLL",
            "L.LLLLLL.L",
            "L.LLLLL.LL",
        };

        private readonly Puzzle _example = new Puzzle(ExampleData);

        [Test]
        public void Solution1Test()
        {
            _example.Solution1().Should().Be(37);
        }

        [Test]
        public void GetOccupiedNeighboursTest()
        {
            var p = new Puzzle(new[]
            {
                "L.LL",
                "##..",
                "####",
                "LLLL",
            });

            var expected = new[]
            {
                2, 2, 1, 0,
                3, 4, 4, 2,
                3, 4, 3, 1,
                2, 3, 3, 2,
            };

            for (var i = 0; i < expected.Length; i++)
            {
                p.CountOccupiedNeighbours(p.Input, i).Should().Be(expected[i]);
            }
        }

        [Test]
        public void TransformedBoardTest()
        {
            var b1 = Puzzle.ParseInput(new[]
            {
                "#.##.##.##",
                "#######.##",
                "#.#.#..#..",
                "####.##.##",
                "#.##.##.##",
                "#.#####.##",
                "..#.#.....",
                "##########",
                "#.######.#",
                "#.#####.##",
            });
            Puzzle.TransformBoard(_example.Input, _example.TransformField1).Should().BeEquivalentTo(b1);

            var b2 = Puzzle.ParseInput(new[]
            {
                "#.LL.L#.##",
                "#LLLLLL.L#",
                "L.L.L..L..",
                "#LLL.LL.L#",
                "#.LL.LL.LL",
                "#.LLLL#.##",
                "..L.L.....",
                "#LLLLLLLL#",
                "#.LLLLLL.L",
                "#.#LLLL.##"
            });
            Puzzle.TransformBoard(b1, _example.TransformField1).Should().BeEquivalentTo(b2);
        }

        [Test]
        public void Solution2Test()
        {
            _example.Solution2().Should().Be(26);
        }
    }
}
