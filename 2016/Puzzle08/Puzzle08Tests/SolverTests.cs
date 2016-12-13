using System;
using FluentAssertions;
using NUnit.Framework;
using Puzzle08;

namespace Puzzle08Tests
{
    [TestFixture]
    public class SolverTests
    {
        [Test]
        public void Solve1Example()
        {
            var solver = new Solver(
                "rect 3x2",
                "rotate column x=1 by 1",
                "rotate row y=0 by 4",
                "rotate column x=1 by 1"
                ).Solve1().Should().Be(6);
        }

        [Test]
        public void DisplayTests()
        {
            var display = new Display(7, 3);
            display.ToString().Should().Be(Picture(
                ".......",
                ".......",
                "......."));

            display.Rect(3, 2);
            display.ToString().Should().Be(Picture(
                "###....",
                "###....",
                "......."));

            display.RotateColumn(1, 1);
            display.ToString().Should().Be(Picture(
                "#.#....",
                "###....",
                ".#....."));

            display.RotateRow(0, 4);
            display.ToString().Should().Be(Picture(
                "....#.#",
                "###....",
                ".#....."));

            display.RotateColumn(1, 1);
            display.ToString().Should().Be(Picture(
                ".#..#.#",
                "#.#....",
                ".#....."));
        }

        private static string Picture(params string[] lines)
        {
            return String.Join(Environment.NewLine, lines) + Environment.NewLine;
        }
    }
}