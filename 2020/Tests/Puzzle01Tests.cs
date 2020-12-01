using NUnit.Framework;
using FluentAssertions;
using AOC2020.Puzzle01;

namespace Tests
{
    [TestFixture]
    public class Puzzle01Tests
    {
        private static readonly int[] ExampleReport =
        {
            1721,
            979,
            366,
            299,
            675,
            1456,
        };

        [Test]
        public void Solution1Test()
        {
            var p = new P01(ExampleReport);
            p.Solution1().Should().Be(514579);
        }

        [Test]
        public void Solution2Test()
        {
            var p = new P01(ExampleReport);
            p.Solution2().Should().Be(241861950);
        }
    }
}
