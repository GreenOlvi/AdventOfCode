using System.Text.RegularExpressions;
using AOC2021.Common;

namespace AOC2021.Day16
{
    public class Puzzle : PuzzleBase<long, long>
    {
        public Puzzle(IEnumerable<string> lines)
        {
            _input = lines.First();
            _bits = DecodeBits(_input).ToArray();
        }

        private readonly string _input;
        private readonly byte[] _bits;

        private static readonly Dictionary<char, string> Hex2Bits = new()
        {
            { '0', "0000" },
            { '1', "0001" },
            { '2', "0010" },
            { '3', "0011" },
            { '4', "0100" },
            { '5', "0101" },
            { '6', "0110" },
            { '7', "0111" },
            { '8', "1000" },
            { '9', "1001" },
            { 'A', "1010" },
            { 'B', "1011" },
            { 'C', "1100" },
            { 'D', "1101" },
            { 'E', "1110" },
            { 'F', "1111" },
        };

        public static IEnumerable<byte> DecodeBits(string input)
        {
            foreach (var c in input)
            {
                var bin = Hex2Bits[c];
                foreach (var b in bin)
                {
                    yield return (byte)(b - '0');
                }
            }
        }

        public static PacketBase ParsePacket(BitStream stream)
        {
            var version = stream.ReadBits(3).ToByte();
            var type = (PacketType)stream.ReadBits(3).ToByte();

            if (type == PacketType.LiteralValue)
            {
                var valueBits = new List<byte>();
                bool keepReading;
                do
                {
                    keepReading = stream.ReadBit() == 1;
                    valueBits.AddRange(stream.ReadBits(4));
                }
                while (keepReading);

                return new LiteralValue
                {
                    Version = version,
                    Value = valueBits.ToLong(),
                };
            }
            else
            {
                var lengthType = stream.ReadBit();
                var packets = new List<PacketBase>();
                if (lengthType == 0)
                {
                    var bitLength = stream.ReadBits(15).ToInt();
                    var startPos = stream.Position;
                    while (stream.Position - startPos < bitLength)
                    {
                        packets.Add(ParsePacket(stream));
                    }
                }
                else
                {
                    var packetLength = stream.ReadBits(11).ToInt();
                    for (var i = 0; i < packetLength; i++)
                    {
                        packets.Add(ParsePacket(stream));
                    }
                }

                return new Operator
                {
                    Type = type,
                    Version = version,
                    LengthType = lengthType,
                    Packets = packets.ToArray(),
                };
            }

        }

        private static IEnumerable<PacketBase> ParsePackets(BitStream stream)
        {
            while (!stream.IsEmpty)
            {
                yield return ParsePacket(stream);
            }
        }

        private long SumPacketVersions(IEnumerable<PacketBase> packets)
        {
            var sum = 0L;
            foreach (var packet in packets)
            {
                sum += packet.Version;
                if (packet is Operator op)
                {
                    sum += SumPacketVersions(op.Packets);
                }
            }
            return sum;
        }

        private long EvaluatePacket(PacketBase packet)
        {
            long LogicOp(Operator o, Func<long, long, bool> func) =>
                func(EvaluatePacket(o.Packets[0]), EvaluatePacket(o.Packets[1])) ? 1 : 0;

            return packet switch
            {
                LiteralValue val => val.Value,
                Operator op => op.Type switch
                {
                    PacketType.Sum => op.Packets.Sum(p => EvaluatePacket(p)),
                    PacketType.Product => op.Packets.Product(p => EvaluatePacket(p)),
                    PacketType.Minimum => op.Packets.Min(p => EvaluatePacket(p)),
                    PacketType.Maximum => op.Packets.Max(p => EvaluatePacket(p)),
                    PacketType.GreaterThan => LogicOp(op, (a, b) => a > b),
                    PacketType.LessThan => LogicOp(op, (a, b) => a < b),
                    PacketType.EqualTo => LogicOp(op, (a, b) => a == b),
                    _ => throw new InvalidOperationException(),
                },
                _ => throw new InvalidDataException()
            };
        }

        public override long Solution1()
        {
            var packets = ParsePackets(new BitStream(_bits)).ToArray();
            return SumPacketVersions(packets);
        }

        public override long Solution2()
        {
            var packet = ParsePackets(new BitStream(_bits)).Single();
            return EvaluatePacket(packet);
        }
    }
}
