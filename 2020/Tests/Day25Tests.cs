using NUnit.Framework;
using FluentAssertions;
using AOC2020.Day25;

namespace Tests
{
    [TestFixture]
    public class Day25Tests
    {
        private static readonly long[] ExampleData =
        {
            5764801,
            17807724,
        };

        private readonly Puzzle _example = new Puzzle(ExampleData);

        [Test]
        public void Solution1Test()
        {
            _example.Solution1().Should().Be(14897079);
        }
    }
}
