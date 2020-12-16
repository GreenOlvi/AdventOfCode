using NUnit.Framework;
using FluentAssertions;
using AOC2020.Day16;

namespace Tests
{
    [TestFixture]
    public class Day16Tests
    {
        private static readonly string[] ExampleData =
        {
            "class: 1-3 or 5-7",
            "row: 6-11 or 33-44",
            "seat: 13-40 or 45-50",
            "",
            "your ticket:",
            "7,1,14",
            "",
            "nearby tickets:",
            "7,3,47",
            "40,4,50",
            "55,2,20",
            "38,6,12",
        };

        private readonly Puzzle _example = new Puzzle(ExampleData);

        private static readonly string[] Example2Data =
        {
            "class: 0-1 or 4-19",
            "row: 0-5 or 8-19",
            "seat: 0-13 or 16-19",
            "",
            "your ticket:",
            "11,12,13",
            "",
            "nearby tickets:",
            "3,9,18",
            "15,1,5",
            "5,14,9",
        };

        private readonly Puzzle _example2 = new Puzzle(Example2Data, r => r == "row");

        [Test]
        public void Solution1Test()
        {
            _example.Solution1().Should().Be(71);
        }

        [Test]
        public void Solution2Test()
        {
            _example2.Solution2().Should().Be(11);
        }
    }
}
