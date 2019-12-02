using NUnit.Framework;
using AoC2019.Puzzle02;
using FluentAssertions;

namespace AoCTests
{
    [TestFixture]
    public class Puzzle02Tests
    {
        [Test]
        [TestCase(new long[] { 1, 0, 0, 0, 99 }, 2)]
        [TestCase(new long[] { 2, 3, 0, 3, 99 }, 2)]
        [TestCase(new long[] { 2, 4, 4, 5, 99, 0 }, 2)]
        [TestCase(new long[] { 1, 1, 1, 4, 99, 5, 6, 0, 99 }, 30)]
        [TestCase(new long[] { 1, 9, 10, 3, 2, 3, 11, 0, 99, 30, 40, 50 }, 3500)]
        public void Solve1Test(long[] input, int result)
        {
            Solution.Solve1(input, input[1], input[2]).Should().Be(result);
        }

        [Test]
        [TestCase(new long[] { 1, 0, 0, 0, 99 }, 0,
            new long[] { 2, 0, 0, 0, 99 }, 4, true)]
        [TestCase(new long[] { 1, 9, 10, 3, 2, 3, 11, 0, 99, 30, 40, 50 }, 0,
            new long[] { 1, 9, 10, 70, 2, 3, 11, 0, 99, 30, 40, 50 }, 4, true)]
        [TestCase(new long[] { 1, 9, 10, 70, 2, 3, 11, 0, 99, 30, 40, 50 }, 4,
            new long[] { 3500, 9, 10, 70, 2, 3, 11, 0, 99, 30, 40, 50 }, 8, true)]
        [TestCase(new long[] { 3500, 9, 10, 70, 2, 3, 11, 0, 99, 30, 40, 50 }, 8,
            new long[] { 3500, 9, 10, 70, 2, 3, 11, 0, 99, 30, 40, 50 }, 9, false)]
        [TestCase(new long[] { 1, 0, 0, 0, 99 }, 0,
            new long[] { 2, 0, 0, 0, 99 }, 4, true)]
        [TestCase(new long[] { 2, 3, 0, 3, 99 }, 0,
            new long[] { 2, 3, 0, 6, 99 }, 4, true)]
        [TestCase(new long[] { 2, 4, 4, 5, 99, 0 }, 0,
            new long[] { 2, 4, 4, 5, 99, 9801 }, 4, true)]
        [TestCase(new long[] { 1, 1, 1, 4, 99, 5, 6, 0, 99 }, 0,
            new long[] { 1, 1, 1, 4, 2, 5, 6, 0, 99 }, 4, true)]
        [TestCase(new long[] { 1, 1, 1, 4, 2, 5, 6, 0, 99 }, 4,
            new long[] { 30, 1, 1, 4, 2, 5, 6, 0, 99 }, 8, true)]
        [TestCase(new long[] { 30, 1, 1, 4, 2, 5, 6, 0, 99 }, 8,
            new long[] { 30, 1, 1, 4, 2, 5, 6, 0, 99 }, 9, false)]
        public void RunTest(long[] memory, int ip, long[] after, int ipAfter, bool runAfter)
        {
            var run = true;
            Solution.RunInstruction(ref memory, ref ip, ref run);

            memory.Should().BeEquivalentTo(after);
            ip.Should().Be(ipAfter);
            run.Should().Be(runAfter);
        }

        [Test]
        [TestCase(new long[] { 1, 0, 0, 0, 99 }, 2, 0)]
        [TestCase(new long[] { 1, 0, 0, 3, 2, 3, 11, 0, 99, 30, 40, 50 }, 3500, 910)]
        public void Solve2Test(long[] input, int searched, int values)
        {
            Solution.Solve2(input, searched).Should().Be(values);
        }
    }
}
