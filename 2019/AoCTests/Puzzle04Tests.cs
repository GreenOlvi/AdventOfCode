using NUnit.Framework;
using AoC2019.Puzzle04;
using FluentAssertions;

namespace AoCTests
{
    [TestFixture]
    public class Puzzle04Tests
    {
        [Test]
        [TestCase(111111, true)]
        [TestCase(223450, false)]
        [TestCase(123789, false)]
        public void IsValid1Test(int number, bool result)
        {
            Solution.IsValid1(number).Should().Be(result);
        }

        [Test]
        [TestCase(123456, new[] { 1, 2, 3, 4, 5, 6 })]
        public void SplitTest(int number, int[] expected)
        {
            Solution.Split(number).Should().BeEquivalentTo(expected);
        }

        [Test]
        [TestCase(new[] { 1, 2, 3, 4, 5, 6 }, new[] { 1, 1, 1, 1, 1, 1 })]
        [TestCase(new[] { 1, 1, 1, 1, 2, 2 }, new[] { 4, 2 })]
        public void RepeatedLengthsTest(int[] n, int[] expected)
        {
            Solution.RepeatedLengths(n).Should().BeEquivalentTo(expected);
        }

        [Test]
        [TestCase(111111, false)]
        [TestCase(223450, false)]
        [TestCase(123789, false)]
        [TestCase(112233, true)]
        [TestCase(123444, false)]
        [TestCase(111122, true)]
        public void IsValid2Test(int number, bool result)
        {
            Solution.IsValid2(number).Should().Be(result);
        }
    }
}
