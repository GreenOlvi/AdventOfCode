using NUnit.Framework;
using FluentAssertions;
using AoC2019.Puzzle08;
using System.Linq;

namespace AoCTests
{
    [TestFixture]
    public class Puzzle08Tests
    {
        [Test]
        public void LayersTest()
        {
            var img = Solution.ParseInput("123456789012");
            var image = new LayeredImage(img, 3, 2);
            image.Layers().Should().BeEquivalentTo(new[]
            {
                new[] { 1, 2, 3, 4, 5, 6 },
                new[] { 7, 8, 9, 0, 1, 2 },
            });
        }

        [Test]
        public void FlattenTests()
        {
            var input = "0222112222120000".Select(c => c - '0').ToArray();
            var image = new LayeredImage(input, 2, 2);
            image.Flatten().Should().BeEquivalentTo(new[] { 0, 1, 1, 0 });
        }
    }
}
