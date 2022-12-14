namespace AOC2022.Puzzles;

public class Day13 : CustomBaseDay
{
    private readonly PacketElement[][] _packets;

    public Day13()
    {
        _packets = ParseInput(ReadLinesFromFile()).ToArray();
    }

    public Day13(IEnumerable<string> lines)
    {
        _packets = ParseInput(lines).ToArray();
    }

    private static IEnumerable<PacketElement[]> ParseInput(IEnumerable<string> lines) => lines.SplitGroups(ParseList);

    public static PacketElement ParseList(string line)
    {
        var queue = new Queue<char>(line);

        if (!TryParseList(queue, out var list))
        {
            throw new InvalidDataException(line);
        }

        return list;
    }

    private static bool TryParseList(Queue<char> queue, out PacketList list)
    {
        if (queue.Dequeue() != '[')
        {
            list = new PacketList();
            return false;
        }

        var elements = new List<PacketElement>();
        while (queue.TryPeek(out var ch))
        {
            if (ch == ' ' || ch == ',')
            {
                queue.Dequeue();
                continue;
            }

            if (ch >= '0' && ch <= '9' && TryParseNumber(queue, out var number))
            {
                elements.Add(number);
                continue;
            }

            if (ch == '[' && TryParseList(queue, out var packetList))
            {
                elements.Add(packetList);
                continue;
            }

            if (ch == ']')
            {
                queue.Dequeue();
                break;
            }

            throw new InvalidDataException();
        }

        list = new PacketList(elements.ToArray());
        return true;
    }

    private static bool TryParseNumber(Queue<char> queue, out PacketNumber number)
    {
        var value = 0;
        while (queue.TryPeek(out var ch))
        {
            if (ch >= '0' && ch <= '9')
            {
                value = value * 10 + (ch - '0');
                queue.Dequeue();
            }
            else
            {
                break;
            }
        }

        number = new PacketNumber(value);
        return true;
    }

    public override ValueTask<string> Solve_1()
    {
        var indices = _packets.Select((p, i) => (p, i + 1))
            .Where(p => PacketElementComparer.Instance.Compare(p.p[0], p.p[1]) < 0)
            .Select(p => p.Item2);

        return indices.Sum().ToResult();
    }

    private static readonly PacketElement[] DividerPackets = new PacketElement[]
    {
        ParseList("[[2]]"),
        ParseList("[[6]]"),
    };

    public override ValueTask<string> Solve_2()
    {
        var allPackets = _packets.SelectMany(p => p)
            .Concat(DividerPackets)
            .Order(PacketElementComparer.Instance)
            .ToArray();

        var indices = allPackets.Select((p, i) => (p, i + 1))
            .Where(pair => pair.p == DividerPackets[0] || pair.p == DividerPackets[1])
            .Select(pair => pair.Item2);

        return indices.Product().ToResult();
    }

    public readonly record struct PacketNumber(int Value) : PacketElement;

    public readonly record struct PacketList : PacketElement
    {
        public PacketList(params PacketElement[] elements) : this()
        {
            Elements = elements;
        }

        public readonly PacketElement[] Elements;
    }

    public interface PacketElement
    {
    }

    public class PacketElementComparer : IComparer<PacketElement>
    {
        public static readonly PacketElementComparer Instance = new();

        public int Compare(PacketElement? x, PacketElement? y)
        {
            if (ReferenceEquals(x, y))
            {
                return 0;
            }

            if (x is null)
            {
                return -1;
            }

            if (y is null)
            {
                return 1;
            }

            return CompareNonNull(x, y);
        }

        private static int CompareNonNull(PacketElement x, PacketElement y)
        {
            if (x is PacketNumber xn && y is PacketNumber yn)
            {
                return CompareNumbers(xn, yn);
            }

            if (x is PacketList xl && y is PacketList yl)
            {
                return CompareLists(xl, yl);
            }

            if (x is PacketNumber xn1 && y is PacketList yl1)
            {
                return CompareMixed(xn1, yl1);
            }

            if (x is PacketList xl1 && y is PacketNumber yn1)
            {
                return CompareMixed(xl1, yn1);
            }

            throw new NotImplementedException();
        }

        private static int CompareMixed(PacketNumber xn1, PacketList yl1) =>
            CompareLists(new PacketList(new PacketElement[] { xn1 }), yl1);

        private static int CompareMixed(PacketList xl1, PacketNumber yn1) => -1 * CompareMixed(yn1, xl1);

        private static int CompareLists(PacketList x, PacketList y)
        {
            var xels = x.Elements;
            var yels = y.Elements;

            foreach (var (First, Second) in xels.Zip(yels))
            {
                var r = CompareNonNull(First, Second);
                if (r != 0)
                {
                    return r;
                }
            }

            return Math.Sign(xels.Length - yels.Length);
        }

        private static int CompareNumbers(PacketNumber x, PacketNumber y) => Math.Sign(x.Value - y.Value);

    }
}
