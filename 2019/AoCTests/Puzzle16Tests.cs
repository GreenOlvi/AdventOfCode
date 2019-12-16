using NUnit.Framework;
using FluentAssertions;
using AoC2019.Puzzle16;
using System.Linq;
using System.Collections.Generic;

namespace AoCTests
{
    [TestFixture]
    public class Puzzle16Tests
    {
        [TestCase(1, new[] { 1, 0, -1, 0, 1, 0, -1, 0, 1, 0 })]
        [TestCase(2, new[] { 0, 1, 1, 0, 0, -1, -1, 0, 0, 1 })]
        [TestCase(3, new[] { 0, 0, 1, 1, 1, 0, 0, 0, -1, -1 })]
        [TestCase(4, new[] { 0, 0, 0, 1, 1, 1, 1, 0, 0, 0 })]
        public void PatternTests(int n, int[] expected)
        {
            Solution.Pattern(n).Take(10).Should().BeEquivalentTo(expected, c => c.WithStrictOrdering());
        }

        [TestCase(new[] { 1, 2, 3, 4, 5, 6, 7, 8 }, new[] { 4, 8, 2, 2, 6, 1, 5, 8 })]
        [TestCase(new[] { 4, 8, 2, 2, 6, 1, 5, 8 }, new[] { 3, 4, 0, 4, 0, 4, 3, 8 })]
        [TestCase(new[] { 3, 4, 0, 4, 0, 4, 3, 8 }, new[] { 0, 3, 4, 1, 5, 5, 1, 8 })]
        public void FFTTests(IEnumerable<int> input, IEnumerable<int> expected)
        {
            Solution.FFT(input).Should().BeEquivalentTo(expected, c => c.WithoutStrictOrdering());
        }


        [TestCase("80871224585914546619083218645595", "24176176")]
        [TestCase("19617804207202209144916044189917", "73745418")]
        [TestCase("69317163492948606335995924319873", "52432133")]
        public void Solve1Tests(string input, string expected)
        {
            Solution.Solve1(input).Should().Be(expected);
        }

        [Ignore("Not solvable in sane time")]
        [TestCase("03036732577212944063491565474664", "84462026")]
        [TestCase("02935109699940807407585447034323", "78725270")]
        [TestCase("03081770884921959731165446850517", "53553731")]
        public void Solve2Tests(string input, string expected)
        {
            Solution.Solve2(input).Should().Be(expected);
        }
    }
}
