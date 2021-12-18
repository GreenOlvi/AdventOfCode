using System.Text.RegularExpressions;
using AOC2021.Common;

namespace AOC2021.Day18
{
    public class Puzzle : PuzzleBase<long, long>
    {
        public Puzzle(IEnumerable<string> lines)
        {
            _input = lines.ToArray();
        }

        private readonly string[] _input;

        public static INode ParseNode(string input) =>
            ParseNode(new CharStream(input));

        private static INode ParseNode(CharStream stream)
        {
            while (stream.PeekChar() == ' ')
            {
                stream.Skip();
            }

            return stream.PeekChar() switch
            {
                '[' => ParsePair(stream),
                (>= '0' and <= '9') => ParseValue(stream),
                var c => throw new InvalidDataException($"Not expected '{c}'"),
            };
        }

        private static Pair ParsePair(CharStream stream)
        {
            var lb = stream.ReadChar();
            if (lb != '[') throw new InvalidDataException($"Expected '[', got '{lb}'");

            var l = ParseNode(stream);

            var comma = stream.ReadChar();
            if (comma != ',') throw new InvalidDataException($"Expected ',', got '{comma}'");

            var r = ParseNode(stream);

            var rb = stream.ReadChar();
            if (rb != ']') throw new InvalidDataException($"Expected ']', got '{rb}'");

            return new Pair(l, r);
        }

        private static Number ParseValue(CharStream stream)
        {
            long value = 0;
            while (char.IsDigit(stream.PeekChar()))
            {
                var i = stream.ReadChar() - '0';
                value = value * 10 + i;
            }
            return new Number(value);
        }

        private static INode Add(INode a, INode b) => new Pair(a.Copy(), b.Copy());

        private static INode CheckPair(Pair pair, int level)
        {
            if (level > 4)
            {
                return pair;
            }

            var l = SeekDeep(pair.Left, level + 1);
            if (l is Pair pl)
            {
                return pl;
            }

            var r = SeekDeep(pair.Right, level + 1);
            if (r is Pair pr)
            {
                return pr;
            }

            return l is not null ? l : r;
        }

        private static INode SeekDeep(INode node, int level)
        {
            var fix =  node switch
            {
                Pair p => CheckPair(p, level),
                Number n => n.Value >= 10 ? n : null,
                _ => throw new InvalidOperationException(),
            };
            return fix;
        }

        private static void Explode(Pair p)
        {
            if (TryFindNumberToTheLeft(p, out var nl))
            {
                nl.Value += ((Number)p.Left).Value;
            }

            if (TryFindNumberToTheRight(p, out var nr))
            {
                nr.Value += ((Number)p.Right).Value;
            }

            ReplaceNode(p, new Number(0));
        }

        private static bool TryFindNumberToTheLeft(INode node, out Number n)
        {
            var parent = node.Parent;
            if (parent is null)
            {
                n = null;
                return false;
            }
            var side = parent.GetSide(node);
            if (side == Side.None)
            {
                n = null;
                return false;
            }
            if (side == Side.Left)
            {
                return TryFindNumberToTheLeft(parent, out n);
            }

            var sib = parent.Left;
            while (true)
            {
                if (sib is Number num)
                {
                    n = num;
                    return true;
                }
                else
                {
                    sib = ((Pair)sib).Right;
                }
            }
        }

        private static bool TryFindNumberToTheRight(INode node, out Number n)
        {
            var parent = node.Parent;
            if (parent is null)
            {
                n = null;
                return false;
            }
            var side = parent.GetSide(node);
            if (side == Side.None)
            {
                n = null;
                return false;
            }
            if (side == Side.Right)
            {
                return TryFindNumberToTheRight(parent, out n);
            }

            var sib = parent.Right;
            while (true)
            {
                if (sib is Number num)
                {
                    n = num;
                    return true;
                }
                else
                {
                    sib = ((Pair)sib).Left;
                }
            }
        }

        private static void ReplaceNode(INode node, INode newNode)
        {
            var parent = node.Parent;
            if (parent != null)
            {
                var side = parent.GetSide(node);
                node.Parent = null;

                if (side == Side.Left)
                {
                    parent.Left = newNode;
                }
                else if (side == Side.Right)
                {
                    parent.Right = newNode;
                }
            }
        }

        private static void Split(Number n)
        {
            ReplaceNode(n, new Pair(
                new Number((long)Math.Floor(n.Value / 2.0)),
                new Number((long)Math.Ceiling(n.Value / 2.0))));
        }

        public static INode Reduce(INode node)
        {
            var no = node.Copy();
            var doFix = true;
            do
            {
                var toFix = SeekDeep(no, 1);
                switch (toFix)
                {
                    case null:
                        doFix = false;
                        break;
                    case Pair p:
                        Explode(p);
                        break;
                    case Number n:
                        Split(n);
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
            while (doFix);

            return no;
        }

        public static INode AddAndReduce(INode a, INode b) => Reduce(Add(a, b));

        public static INode AddAndReduce(IEnumerable<INode> nodes) =>
            nodes.Aggregate((a, b) => AddAndReduce(a, b));

        public override long Solution1() =>
            AddAndReduce(_input.Select(ParseNode)).GetMagnitude();

        public override long Solution2() =>
            _input.Select(ParseNode).GetAllPairs()
                .Select(p => AddAndReduce(p.Item1, p.Item2).GetMagnitude())
                .Max();
    }
}
