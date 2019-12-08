using NUnit.Framework;
using FluentAssertions;
using AoC2019.Puzzle07;
using System.Linq;

namespace AoCTests
{
    [TestFixture]
    public class Puzzle07Tests
    {
        [Test]
        [TestCase("3,15,3,16,1002,16,10,16,1,16,15,15,4,15,99,0,0", new[] { 4, 3, 2, 1, 0 }, 43210)]
        [TestCase("3,23,3,24,1002,24,10,24,1002,23,-1,23,101,5,23,23,1,24,23,23,4,23,99,0,0", new[] { 0, 1, 2, 3, 4 }, 54321)]
        [TestCase("3,31,3,32,1002,32,10,32,1001,31,-2,31,1007,31,0,33,1002,33,7,33,1,33,31,31,1,32,31,31,4,31,99,0,0,0", new[] { 1, 0, 4, 3, 2 }, 65210)]
        public void RunApmsTest(string acs, int[] settings, long result)
        {
            var code = Solution.ParseInput(acs);
            Solution.RunAmps(code.ToArray(), settings).Should().Be(result);
        }

        [Test]
        public void PermLexUnrankTest()
        {
            Enumerable.Range(0, 6)
                .Select(i => Solution.PermLexUnrank(3, i))
                .Should().BeEquivalentTo(new[]
                {
                    new[] { 0, 1, 2 },
                    new[] { 0, 2, 1 },
                    new[] { 1, 0, 2 },
                    new[] { 1, 2, 0 },
                    new[] { 2, 0, 1 },
                    new[] { 2, 1, 0 },
                });
        }

        [Test]
        public void PermutateTest()
        {
            Solution.Permutate(new[] { "a", "b", "c" })
                .Should().BeEquivalentTo(new[] {
                    new[] { "a", "b", "c" },
                    new[] { "a", "c", "b" },
                    new[] { "b", "a", "c" },
                    new[] { "b", "c", "a" },
                    new[] { "c", "a", "b" },
                    new[] { "c", "b", "a" },
                });
        }

        [Test]
        [TestCase("3,26,1001,26,-4,26,3,27,1002,27,2,27,1,27,26,27,4,27,1001,28,-1,28,1005,28,6,99,0,0,5",
            new[] { 9, 8, 7, 6, 5 }, 139629729L)]
        [TestCase("3,52,1001,52,-5,52,3,53,1,52,56,54,1007,54,5,55,1005,55,26,1001,54,-5,54,1105,1,12,1,53,54,53,1008,54,0,55,1001,55,1,55,2,53,55,53,4,53,1001,56,-1,56,1005,56,6,99,0,0,0,0,10",
            new[] { 9, 7, 8, 5, 6 }, 18216L)]
        public void RunAmpsLoopedTest(string acs, int[] settings, long result)
        {
            var code = Solution.ParseInput(acs);
            Solution.RunLoopedAmps(code.ToArray(), settings).Should().Be(result);
        }
    }
}
