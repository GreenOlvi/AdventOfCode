using FluentAssertions;
using NUnit.Framework;
using Puzzle03;

namespace Puzzle03Tests
{
    [TestFixture]
    public class SolverTests
    {
        [Test]
        public void Solve1Example()
        {
            new Solver("5 10 25").Solve1().Should().Be(0);
            new Solver("3 3 5").Solve1().Should().Be(1);
            new Solver("5 10 25", "3 3 5").Solve1().Should().Be(1);
        }

        [Test]
        public void Solve2Example()
        {
            new Solver(
                "101 301 501",
                "102 302 502",
                "103 303 503",
                "201 401 601",
                "202 402 602",
                "203 403 603")
                .Solve2().Should().Be(6);
        }
    }
}
