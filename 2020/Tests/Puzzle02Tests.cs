using NUnit.Framework;
using FluentAssertions;
using AOC2020.Puzzle02;

namespace Tests
{
    [TestFixture]
    public class Puzzle02Tests
    {
        private static readonly string[] ExampleData =
        {
            @"1-3 a: abcde",
            @"1-3 b: cdefg",
            @"2-9 c: ccccccccc",
        };

        [Test]
        public void Solution1Test()
        {
            var p = new P02(ExampleData);
            p.Solution1().Should().Be(2);
        }

        [TestCase(1, 3, 'a', "abcde", true)]
        [TestCase(1, 3, 'b', "cdefg", false)]
        [TestCase(2, 9, 'c', "ccccccccc", true)]
        public void Rule1Tests(int p1, int p2, char letter, string pass, bool expected)
        {
            P02.Rule1((p1, p2, letter, pass)).Should().Be(expected);
        }

        [Test]
        public void Solution2Test()
        {
            var p = new P02(ExampleData);
            p.Solution2().Should().Be(1);
        }

        [TestCase(1, 3, 'a', "abcde", true)]
        [TestCase(1, 3, 'b', "cdefg", false)]
        [TestCase(2, 9, 'c', "ccccccccc", false)]
        public void Rule2Tests(int p1, int p2, char letter, string pass, bool expected)
        {
            P02.Rule2((p1, p2, letter, pass)).Should().Be(expected);
        }
    }
}
