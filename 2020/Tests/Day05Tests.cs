using NUnit.Framework;
using FluentAssertions;
using AOC2020.Day05;

namespace Tests
{
    [TestFixture]
    public class Day05Tests
    {
        [TestCase("FBFBBFFRLR", 357)]
        [TestCase("BFFFBBFRRR", 567)]
        [TestCase("FFFBBBFRRR", 119)]
        [TestCase("BBFFBBFRLL", 820)]
        public void GetSeatIdTests(string line, int id)
        {
            Puzzle.GetSeatId(line).Should().Be(id);
        }
    }
}
