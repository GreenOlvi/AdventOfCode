using NUnit.Framework;
using FluentAssertions;
using AOC2020.Day15;
using AOC2020;

namespace Tests
{
    [TestFixture]
    public class Day15Tests
    {
        [TestCase("0,3,6", 436)]
        [TestCase("1,3,2", 1)]
        [TestCase("2,1,3", 10)]
        [TestCase("1,2,3", 27)]
        [TestCase("2,3,1", 78)]
        [TestCase("3,2,1", 438)]
        [TestCase("3,1,2", 1836)]
        public void Solution1Test(string input, int expected)
        {
            new Puzzle(input.Split(",").ParseInts()).Solution1().Should().Be(expected);
        }

        [TestCase("0,3,6", 175594)]
        [TestCase("1,3,2", 2578)]
        [TestCase("2,1,3", 3544142)]
        [TestCase("1,2,3", 261214)]
        [TestCase("2,3,1", 6895259)]
        [TestCase("3,2,1", 18)]
        [TestCase("3,1,2", 362)]
        public void Solution2Test(string input, int expected)
        {
            new Puzzle(input.Split(",").ParseInts()).Solution2().Should().Be(expected);
        }
    }
}
