using NUnit.Framework;
using FluentAssertions;
using AOC2020.Day09;

namespace Tests
{
    [TestFixture]
    public class Day09Tests
    {
        private static readonly long[] ExampleData =
        {
            35,
            20,
            15,
            25,
            47,
            40,
            62,
            55,
            65,
            95,
            102,
            117,
            150,
            182,
            127,
            219,
            299,
            277,
            309,
            576,
        };

        private readonly Puzzle _example = new Puzzle(ExampleData, 5);

        [Test]
        public void Solution1Test()
        {
            _example.Solution1().Should().Be(127);
        }

        [Test]
        public void Solution2Test()
        {
            _example.Solution2().Should().Be(62);
        }
    }
}
