using System;
using FluentAssertions;
using NUnit.Framework;
using Puzzle09;

namespace Puzzle09Tests
{
    [TestFixture]
    public class SolverTests
    {
        [Test]
        public void Solve1Example1Test()
        {
            new Solver("ADVENT").Solve1().Should().Be(6);
        }

        [Test]
        public void Solve1Example2Test()
        {
            new Solver("A(1x5)BC").Solve1().Should().Be(7);
        }

        [Test]
        public void Solve1Example3Test()
        {
            new Solver("(3x3)XYZ").Solve1().Should().Be(9);
        }

        [Test]
        public void Solve1Example4Test()
        {
            new Solver("A(2x2)BCD(2x2)EFG").Solve1().Should().Be(11);
        }

        [Test]
        public void Solve1Example5Test()
        {
            new Solver("(6x1)(1x3)A").Solve1().Should().Be(6);
        }

        [Test]
        public void Solve1Example6Test()
        {
            new Solver("X(8x2)(3x3)ABCY").Solve1().Should().Be(18);
        }

        [Test]
        public void RepeatStringTests()
        {
            Solver.RepeatString("a", 5).Should().Be("aaaaa");
        }

        [Test]
        public void Solve2Example1Test()
        {
            new Solver("(3x3)XYZ").Solve2().Should().Be(9);
        }

        [Test]
        public void Solve2Example2Test()
        {
            new Solver("X(8x2)(3x3)ABCY").Solve2().Should().Be(20);
        }

        [Test]
        public void Solve2Example3Test()
        {
            new Solver("(27x12)(20x12)(13x14)(7x10)(1x12)A").Solve2().Should().Be(241920);
        }

        [Test]
        public void Solve2Example4Test()
        {
            new Solver("(25x3)(3x3)ABC(2x3)XY(5x2)PQRSTX(18x9)(3x2)TWO(5x7)SEVEN").Solve2().Should().Be(445);
        }
    }
}
