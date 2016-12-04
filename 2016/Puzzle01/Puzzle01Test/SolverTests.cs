using FluentAssertions;
using NUnit.Framework;
using Puzzle01;

namespace Puzzle01Test
{
    [TestFixture]
    public class SolverTests
    {
        [Test]
        public void Solve1Example1()
        {
            new Solver("R2, L3").Solve1().Should().Be(5);
        }

        [Test]
        public void Solve1Example2()
        {
            new Solver("R2, R2, R2").Solve1().Should().Be(2);
        }

        [Test]
        public void Solve1Example3()
        {
            new Solver("R5, L5, R5, R3").Solve1().Should().Be(12);
        }

        [Test]
        public void Solve2Example1()
        {
            new Solver("R8, R4, R4, R8").Solve2().Should().Be(4);
        }
    }
}
