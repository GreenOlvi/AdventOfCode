using AoC2019.Puzzle01;
using FluentAssertions;
using NUnit.Framework;

namespace AoCTests
{
    public class Tests
    {
        [Test]
        [TestCase(12, 2)]
        [TestCase(14, 2)]
        [TestCase(1969, 654)]
        [TestCase(100756, 33583)]
        public void Solve1Tests(int input, int expected)
        {
            Solution.Solve1(new[] { input }).Should().Be(expected);
        }

        [Test]
        [TestCase(12, 2)]
        [TestCase(1969, 966)]
        [TestCase(100756, 50346)]
        public void Solve2Tests(int input, int expected)
        {
            Solution.Solve2(new[] { input }).Should().Be(expected);
        }
    }
}