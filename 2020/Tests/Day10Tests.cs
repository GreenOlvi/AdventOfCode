using NUnit.Framework;
using FluentAssertions;
using AOC2020.Day10;

namespace Tests
{
    [TestFixture]
    public class Day10Tests
    {
        private static readonly int[] ExampleData = { 16, 10, 15, 5, 1, 11, 7, 19, 6, 12, 4, };

        private static readonly int[] LongerData =
        {
            28, 33, 18, 42, 31, 14, 46, 20, 48, 47, 24, 23, 49, 45, 19, 38,
            39, 11, 1, 32, 25, 35, 8, 17, 7, 9, 4, 2, 34, 10, 3,
        };

        private readonly Puzzle _example = new Puzzle(ExampleData);
        private readonly Puzzle _longerExample = new Puzzle(LongerData);

        [Test]
        public void Solution1Test()
        {
            _example.Solution1().Should().Be(35);
            _longerExample.Solution1().Should().Be(220);
        }

        [Test]
        public void Solution2Test()
        {
            _example.Solution2().Should().Be(8);
            _longerExample.Solution2().Should().Be(19208);
        }
    }
}
