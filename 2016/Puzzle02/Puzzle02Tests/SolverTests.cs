using FluentAssertions;
using Puzzle02;
using NUnit.Framework;

namespace Puzzle02Tests
{
    [TestFixture]
    public class SolverTests
    {
        [Test]
        public void Solve1Example()
        {
            var solver = new Solver("ULL", "RRDDD", "LURDL", "UUUUD");
            solver.Solve1().Should().Be("1985");
        }

        [Test]
        public void Solve2Example()
        {
            var solver = new Solver("ULL", "RRDDD", "LURDL", "UUUUD");
            solver.Solve2().Should().Be("5DB3");
        }
    }
}