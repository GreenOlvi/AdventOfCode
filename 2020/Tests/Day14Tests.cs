using NUnit.Framework;
using FluentAssertions;
using AOC2020.Day14;

namespace Tests
{
    [TestFixture]
    public class Day14Tests
    {
        private readonly Puzzle _example1 = new Puzzle(new[] 
        {
            "mask = XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X",
            "mem[8] = 11",
            "mem[7] = 101",
            "mem[8] = 0",
        });

        private readonly Puzzle _example2 = new Puzzle(new[]
        {
            "mask = 000000000000000000000000000000X1001X",
            "mem[42] = 100",
            "mask = 00000000000000000000000000000000X0XX",
            "mem[26] = 1",
        });

        [Test]
        public void Solution1Test()
        {
            _example1.Solution1().Should().Be(165);
        }

        [Test]
        public void Solution2Test()
        {
            _example2.Solution2().Should().Be(208);
        }
    }
}
