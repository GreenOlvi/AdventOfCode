using System;
using NUnit.Framework;
using FluentAssertions;
using AoC2019.Puzzle10;
using System.Linq;
using AoC2019.Common;
using System.Collections.Generic;

namespace AoCTests
{
    [TestFixture]
    public class Puzzle10Tests
    {
        [Test]
        [TestCase(new[] { ".#..#", ".....", "#####", "....#", "...##" },
            new[] { "1,0", "4,0", "0,2", "1,2", "2,2", "3,2", "4,2", "4,3", "3,4", "4,4" })]
        public void ParseInputTest(string[] input, string[] result)
        {
            var expected = result.Select(s => s.Split(',', StringSplitOptions.RemoveEmptyEntries))
                .Select(a => new Position(int.Parse(a[0]), int.Parse(a[1])));
            Solution.ParseInput(input)
                .Should().BeEquivalentTo(expected);
        }

        #region Test data
        private static readonly Grid _smallGrid = new Grid(Solution.ParseInput(new[]{
            ".#..#",
            ".....",
            "#####",
            "....#",
            "...##",
        }));

        private static readonly Grid _grid2 = new Grid(Solution.ParseInput(new[]
        {
            "......#.#.",
            "#..#.#....",
            "..#######.",
            ".#.#.###..",
            ".#..#.....",
            "..#....#.#",
            "#..#....#.",
            ".##.#..###",
            "##...#..#.",
            ".#....####",
        }));

        private static readonly Grid _grid3 = new Grid(Solution.ParseInput(new[]
        {
            "#.#...#.#.",
            ".###....#.",
            ".#....#...",
            "##.#.#.#.#",
            "....#.#.#.",
            ".##..###.#",
            "..#...##..",
            "..##....##",
            "......#...",
            ".####.###.",
        }));

        private static readonly Grid _grid4 = new Grid(Solution.ParseInput(new[]
        {
            ".#..#..###",
            "####.###.#",
            "....###.#.",
            "..###.##.#",
            "##.##.#.#.",
            "....###..#",
            "..#.#..#.#",
            "#..#.#.###",
            ".##...##.#",
            ".....#.#..",
        }));

        private static readonly Grid _grid5 = new Grid(Solution.ParseInput(new[]
        {
            ".#..##.###...#######",
            "##.############..##.",
            ".#.######.########.#",
            ".###.#######.####.#.",
            "#####.##.#.##.###.##",
            "..#####..#.#########",
            "####################",
            "#.####....###.#.#.##",
            "##.#################",
            "#####.##.###..####..",
            "..######..##.#######",
            "####.##.####...##..#",
            ".#####..#.######.###",
            "##...#.##########...",
            "#.##########.#######",
            ".####.#.###.###.#.##",
            "....##.##.###..#####",
            ".#.#.###########.###",
            "#.#.#.#####.####.###",
            "###.##.####.##.#..##",
        }));
        #endregion

        private static readonly TestCaseData[] IsVisibleTestCases = new[]
        {
            new TestCaseData(false, (1, 0), (1, 0), _smallGrid),
            new TestCaseData(true, (1, 0), (4, 0), _smallGrid),
            new TestCaseData(true, (4, 0), (1, 0), _smallGrid),
            new TestCaseData(true, (4, 0), (4, 2), _smallGrid),
            new TestCaseData(false, (4, 0), (4, 4), _smallGrid),
            new TestCaseData(false, (1, 0), (3, 4), _smallGrid),
            new TestCaseData(true, (1, 0), (4, 4), _smallGrid),
            new TestCaseData(false, (3, 4), (1, 0), _smallGrid),
        };

        [TestCaseSource(nameof(IsVisibleTestCases))]
        public void IsVisibleTest(bool expected, (int, int) a, (int, int) b, Grid grid)
        {
            grid.IsVisible(Position.From(a), Position.From(b)).Should().Be(expected);
        }

        [TestCase(1, 5, new[] { 2, 3, 4 })]
        [TestCase(5, 1, new[] { 4, 3, 2 })]
        [TestCase(1, 1, new int[0])]
        [TestCase(1, 2, new int[0])]
        [TestCase(2, 1, new int[0])]
        public void RangeTest(int a, int b, int[] result)
        {
            Grid.Between(a, b).Should().BeEquivalentTo(result);
        }

        private static readonly IEnumerable<TestCaseData> Solve1TestCases = new[]
        {
            new TestCaseData(_smallGrid, 8),
            new TestCaseData(_grid2, 33),
            new TestCaseData(_grid3, 35),
            new TestCaseData(_grid4, 41),
            new TestCaseData(_grid5, 210),
        };

        [TestCaseSource(nameof(Solve1TestCases))]
        public void Solve1Tests(Grid grid, int expected)
        {
            Solution.Solve1(grid).Should().Be(expected);
        }

        private static readonly IEnumerable<TestCaseData> Solve2TestCases = new[]
        {
            new TestCaseData(_smallGrid, 1, 302),
            new TestCaseData(_smallGrid, 2, 400),
            new TestCaseData(_grid5, 200, 802),
        };

        [TestCaseSource(nameof(Solve2TestCases))]
        public void Solve2Test(Grid grid, int n, int expected)
        {
            Solution.Solve2(grid, n).Should().Be(expected);
        }

        [Test]
        public void Solve1OnBigGrid()
        {
            var grid = _grid5;
            var p = new Position(11, 13);

            grid.Contains(p).Should().BeTrue();

            var visible = grid.Asteroids.Where(a => grid.IsVisible(p, a)).ToList();
            visible.Count.Should().Be(210);
        }
    }
}
