using System;
using FluentAssertions;
using NUnit.Framework;
using Puzzle04;

namespace Puzzle04Tests
{
    [TestFixture]
    public class SolverTests
    {
        [Test]
        public void Solve1Example1()
        {
            new Solver("aaaaa-bbb-z-y-x-123[abxyz]").Solve1().Should().Be(123);
        }

        [Test]
        public void Solve1Example2()
        {
            new Solver("a-b-c-d-e-f-g-h-987[abcde]").Solve1().Should().Be(987);
        }

        [Test]
        public void Solve1Example3()
        {
            new Solver("not-a-real-room-404[oarel]").Solve1().Should().Be(404);
        }

        [Test]
        public void Solve1Example4()
        {
            new Solver("totally-real-room-200[decoy]").Solve1().Should().Be(0);
        }

        [Test]
        public void Solve1ExampleSum()
        {
            new Solver(
                "aaaaa-bbb-z-y-x-123[abxyz]",
                "a-b-c-d-e-f-g-h-987[abcde]",
                "not-a-real-room-404[oarel]",
                "totally-real-room-200[decoy]"
            ).Solve1().Should().Be(1514);
        }

        [Test]
        public void Solve2DecodeTest()
        {
            var roomName = "qzmt-zixmtkozy-ivhz";
            var id = "343";
            var checksum = Solver.Room.GenerateChecksum(roomName);
            var fullName = String.Format("{0}-{1}[{2}]", roomName, id, checksum);
            new Solver.Room(fullName).Decode().Should().Be("very encrypted name");
        }
    }
}