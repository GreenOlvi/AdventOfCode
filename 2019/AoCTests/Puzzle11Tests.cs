using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using FluentAssertions;
using AoC2019.Puzzle11;
using AoC2019.Common;
using System.Linq;

namespace AoCTests
{
    [TestFixture]
    public class Puzzle11Tests
    {
        [Test]
        public void PositionHasHashCodeImplementedTest()
        {
            var set = new HashSet<Position>();

            foreach (var _ in Enumerable.Range(0, 2))
            {
                set.Add(new Position(0, 0));
                set.Add(new Position(1, 0));
                set.Add(new Position(0, 1));
                set.Add(new Position(1, 1));
                set.Add(new Position(-1, -1));
            }

            set.Count.Should().Be(5);
        }
    }
}
