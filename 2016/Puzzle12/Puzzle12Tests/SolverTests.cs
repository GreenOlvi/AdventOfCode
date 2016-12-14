using System;
using FluentAssertions;
using NUnit.Framework;
using Puzzle12;

namespace Puzzle12Tests
{
    [TestFixture]
    public class SolverTests
    {
        [Test]
        public void Solve1Example()
        {
            new Solver(
                "cpy 41 a",
                "inc a",
                "inc a",
                "dec a",
                "jnz a 2",
                "dec a"
                ).Solve1().Should().Be(42);
        }
    }
}