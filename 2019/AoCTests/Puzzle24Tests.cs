using NUnit.Framework;
using FluentAssertions;
using AoC2019.Puzzle24;
using System.Linq;
using System.Text;

namespace AoCTests
{
    [TestFixture]
    public class Puzzle24Tests
    {
        [TestCase("....#/#..#./#..##/..#../#....", "#..#./####./###.#/##.##/.##..")]
        [TestCase("#..#./####./###.#/##.##/.##..", "#####/....#/....#/...#./#.###")]
        [TestCase("#####/....#/....#/...#./#.###", "#..../####./...##/#.##./.##.#")]
        [TestCase("#..../####./...##/#.##./.##.#", "####./....#/##..#/...../##...")]
        public void StepTests(string input, string expected)
        {
            var i = Parse(input);
            var e = Parse(expected);
            Solution.Step(i).Should().Be(e);
        }

        [TestCase("....#/#..#./#..##/..#../#....", 2129920u)]
        public void FindDuplicateTest(string input, uint expected)
        {
            Solution.FindDuplicate(Parse(input)).Should().Be(expected);
        }

        [TestCase("....#/#..#./#.?##/..#../#....", 0, 8)]
        [TestCase("....#/#..#./#.?##/..#../#....", 10, 99)]
        public void CountBugsTests(string input, int steps, int expected)
        {
            var grid = new MultiGrid(Parse(input));
            grid.Steps(steps).CountBugs().Should().Be(expected);
        }

        [TestCase(0, "....#/#..#./#.?##/..#../#....", new[] { "....#/#..#./#.?##/..#../#...." })]
        [TestCase(1, "....#/#..#./#.?##/..#../#....", new[] {
            "...../..#../..?#./..#../.....",
            "#..#./####./##?.#/##.##/.##..",
            "....#/....#/..?.#/....#/#####",
        })]
        [TestCase(10, "....#/#..#./#.?##/..#../#....", new[] {
            "..#../.#.#./..?.#/.#.#./..#..", // -5
            "...#./...##/..?../...##/...#.",
            "#.#../.#.../..?../.#.../#.#..",
            ".#.##/....#/..?.#/...##/.###.",
            "#..##/...##/..?../...#./.####",
            ".#.../.#.##/.#?../...../.....", // 0
            ".##../#..##/..?.#/##.##/#####",
            "###../##.#./#.?../.#.##/#.#..",
            "..###/...../#.?../#..../#...#",
            ".###./#..#./#.?../##.#./.....",
            "####./#..#./#.?#./####./.....", // 5
        })]
        public void MultiGridStepsTests(int steps, string input, string[] expected)
        {
            var grid = new MultiGrid(Parse(input));
            var newGrid = grid.Steps(steps);
            newGrid.Grids().Select(g => ToString(g))
                .Should().BeEquivalentTo(expected, o => o.WithStrictOrdering());
        }

        private static uint Parse(string input) =>
            Solution.GetGridHash(Solution.ParseInput(input.Split('/')).ToArray());

        private static string ToString(uint grid)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < 25; i++)
            {
                if (i == 12)
                {
                    sb.Append('?');
                }
                else
                {
                    sb.Append((grid & (1u << i)) > 0 ? '#' : '.');
                    if (i % 5 == 4 && i < 24)
                    {
                        sb.Append('/');
                    }
                }
            }
            return sb.ToString();
        }
    }
}
