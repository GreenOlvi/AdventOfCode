using NUnit.Framework;
using AOC2021.Day25;
using FluentAssertions;

namespace AocTests
{
    public class Day25Tests
    {
        private static readonly string[] _testInput = new string[]
        {
            "v...>>.vv>",
            ".vv>>.vv..",
            ">>.>v>...v",
            ">>v>>.>.v.",
            "v>v.vv.v..",
            ">.>>..v...",
            ".vv..>.>v.",
            "v.v..>>v.v",
            "....v..v.>",
        };

        private readonly Puzzle _puzzle = new(_testInput);

        [Test]
        public void Solution1Test()
        {
            _puzzle.Solution1().Should().Be(58);
        }

        [Test]
        public void MapStepTest()
        {
            var map = Map.FromInput(new[]
            {
                "..........",
                ".>v....v..",
                ".......>..",
                ".........."
            });

            var expected = Map.FromInput(new[]
            {
                "..........",
                ".>........",
                "..v....v>.",
                ".........."
            });

            var (_, step) = map.Step();
            step.ToString().Should().Be(expected.ToString());
        }
    }
}