using NUnit.Framework;
using AoC2019.Puzzle05;
using FluentAssertions;
using System.Collections.Generic;

namespace AoCTests
{
    [TestFixture]
    public class Puzzle05Tests
    {
        [Test]
        [TestCase(4, 4, new[] { false })]
        [TestCase(1002, 2, new[] { false, true, false })]
        [TestCase(99, 99, new bool[0])]
        public void DecodeTest(long instruction, long opcode, bool[] paramMode)
        {
            var i = Solution.Decode(instruction);
            i.Item1.Should().Be(opcode);
            i.Item2.Should().BeEquivalentTo(paramMode);
        }

        [Test]
        [TestCase(new long[] { 3, 0, 4, 0, 99 }, 0, new long[] { 66 }, new long[] { 66, 0, 4, 0, 99 }, 2, true, new long[0])]
        [TestCase(new long[] { 66, 0, 4, 0, 99 }, 2, new long[0], new long[] { 66, 0, 4, 0, 99 }, 4, true, new long[] { 66 })]
        [TestCase(new long[] { 1101, 100, -1, 4, 0 }, 0, new long[0], new long[] { 1101, 100, -1, 4, 99 }, 4, true, new long[0])]
        public void RunInstruction(long[] mem, int ip, long[] input, long[] expMem, int expIp, bool expRun, long[] expOut)
        {
            var output = new Queue<long>();
            var run = true;
            Solution.RunInstruction(ref mem, ref ip, ref run, new Queue<long>(input), output);
            mem.Should().BeEquivalentTo(expMem);
            ip.Should().Be(expIp);
            run.Should().Be(expRun);
            output.Should().BeEquivalentTo(expOut);
        }
    }
}
