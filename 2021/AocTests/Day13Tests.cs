using NUnit.Framework;
using AOC2021.Day13;
using AOC2021.Common;
using FluentAssertions;

namespace AocTests
{
    public class Day13Tests
    {
        private static readonly string[] _testInput = new string[]
        {
            "6,10",
            "0,14",
            "9,10",
            "0,3",
            "10,4",
            "4,11",
            "6,0",
            "6,12",
            "4,1",
            "0,13",
            "10,12",
            "3,4",
            "3,0",
            "8,4",
            "1,10",
            "2,14",
            "8,10",
            "9,0",
            "",
            "fold along y=7",
            "fold along x=5",
        };

        private readonly Puzzle _puzzle = new(_testInput);

        [Test]
        public void Solution1Test()
        {
            _puzzle.Solution1().Should().Be(17);
        }

        [TestCase(0, 5, 0)]
        [TestCase(14, 7, 0)]
        public void FoldYTest(int y, int foldVal, int expectedY)
        {
            Puzzle.FoldY(new Point(0, y), foldVal).Should().Be(new Point(0, expectedY));
        }

        [Test]
        public void Solution2Test()
        {
            _puzzle.Solution2().Should().Be("\n#####\n#   #\n#   #\n#   #\n#####");
        }
    }
}