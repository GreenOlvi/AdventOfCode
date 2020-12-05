using NUnit.Framework;
using FluentAssertions;
using AOC2020.Puzzle05;

namespace Tests
{
    [TestFixture]
    public class Puzzle05Tests
    {
        [TestCase("FBFBBFFRLR", 357)]
        [TestCase("BFFFBBFRRR", 567)]
        [TestCase("FFFBBBFRRR", 119)]
        [TestCase("BBFFBBFRLL", 820)]
        public void GetSeatIdTests(string line, int id)
        {
            P05.GetSeatId(line).Should().Be(id);
        }
    }
}
