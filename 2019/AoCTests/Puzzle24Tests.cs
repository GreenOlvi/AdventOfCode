using NUnit.Framework;
using FluentAssertions;
using AoC2019.Puzzle24;
using System.Linq;

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
            var i = Solution.GetGridHash(Solution.ParseInput(input.Split('/')).ToArray());
            var e = Solution.GetGridHash(Solution.ParseInput(expected.Split('/')).ToArray());
            Solution.Step(i).Should().Be(e);
        }

        [TestCase("....#/#..#./#..##/..#../#....", 2129920u)]
        public void FindDuplicateTest(string input, uint expected)
        {
            var i = Solution.GetGridHash(Solution.ParseInput(input.Split('/')).ToArray());
            Solution.FindDuplicate(i).Should().Be(expected);
        }
    }
}
