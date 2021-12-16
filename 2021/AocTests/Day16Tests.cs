using NUnit.Framework;
using AOC2021.Day16;
using FluentAssertions;

namespace AocTests
{
    public class Day16Tests
    {
        [Test]
        public void ParseLiteralValuePacketTest()
        {
            var stream = new BitStream(Puzzle.DecodeBits("D2FE28"));
            var packet = Puzzle.ParsePacket(stream) as LiteralValue;
            packet.Should().NotBeNull();
            packet.Should().Be(new LiteralValue
            {
                Version = 6,
                Value = 2021,
            });
        }

        [Test]
        public void ParseOperatorPacketBitLengthTest()
        {
            var stream = new BitStream(Puzzle.DecodeBits("38006F45291200"));
            var packet = Puzzle.ParsePacket(stream) as Operator;
            packet.Should().NotBeNull();
            packet.Version.Should().Be(1);
            packet.Type.Should().Be((PacketType)6);
            packet.LengthType.Should().Be(0);
            packet.Packets.Should().BeEquivalentTo(
                new PacketBase[] {
                    new LiteralValue { Version = 6, Value = 10 },
                    new LiteralValue { Version = 2, Value = 20 },
                });
        }

        [Test]
        public void ParseOperatorPacketPacketLengthTest()
        {
            var stream = new BitStream(Puzzle.DecodeBits("EE00D40C823060"));
            var packet = Puzzle.ParsePacket(stream) as Operator;
            packet.Should().NotBeNull();
            packet.Version.Should().Be(7);
            packet.Type.Should().Be((PacketType)3);
            packet.LengthType.Should().Be(1);
            packet.Packets.Should().BeEquivalentTo(
                new PacketBase[] {
                    new LiteralValue { Version = 2, Value = 1 },
                    new LiteralValue { Version = 4, Value = 2 },
                    new LiteralValue { Version = 1, Value = 3 },
                });
        }

        [TestCase("8A004A801A8002F478", 16)]
        [TestCase("620080001611562C8802118E34", 12)]
        [TestCase("C0015000016115A2E0802F182340", 23)]
        [TestCase("A0016C880162017C3686B18A3D4780", 31)]
        public void Solution1Test(string input, long expected)
        {
            new Puzzle(new[] { input }).Solution1().Should().Be(expected);
        }

        [TestCase("C200B40A82", 3)]
        [TestCase("04005AC33890", 54)]
        [TestCase("880086C3E88112", 7)]
        [TestCase("CE00C43D881120", 9)]
        [TestCase("D8005AC2A8F0", 1)]
        [TestCase("F600BC2D8F", 0)]
        [TestCase("9C005AC2F8F0", 0)]
        [TestCase("9C0141080250320F1802104A08", 1)]
        public void Solution2Test(string input, long expected)
        {
            new Puzzle(new[] { input }).Solution2().Should().Be(expected);
        }
    }
}