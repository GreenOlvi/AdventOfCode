using FluentAssertions;
using NUnit.Framework;
using Puzzle20;

namespace Puzzle20Tests
{
    [TestFixture]
    public class SolverTests
    {
        [Test]
        public void Solve1ExampleTest()
        {
            new Solver(9, "5-8", "0-2", "4-7").Solve1().Should().Be(3);
        }

        [Test]
        public void Solve2ExampleTest()
        {
            new Solver(9, "5-8", "0-2", "4-7").Solve2().Should().Be(2);
        }
    }
}
