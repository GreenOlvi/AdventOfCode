using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using Puzzle19;

namespace Puzzle19Tests
{
    [TestFixture]
    public class SolverTests
    {
        [Test]
        public void Solve1Example()
        {
            new Solver(5).Solve1().Should().Be(3);
        }

        [Test]
        public void Solve2Example()
        {
            var tests = new Dictionary<int, int>
            {
                {1, 1},
                {2, 1},
                {3, 3},
                {4, 1},
                {5, 2},
                {6, 3},
                {7, 5},
                {8, 7},
                {9, 9},
                {10, 1},
            };

            foreach (var test in tests)
            {
                new Solver(test.Key).Solve2().Should().Be(test.Value);
            }
        }
    }
}
