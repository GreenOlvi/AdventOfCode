using System.Text.RegularExpressions;
using AOC2021.Common;
using MoreLinq;

namespace AOC2021.Day10
{
    public class Puzzle : PuzzleBase<long, long>
    {
        public Puzzle(IEnumerable<string> lines)
        {
            _input = lines.ToArray();
        }

        private readonly string[] _input;

        private static readonly Dictionary<char, char> _matchClosing = new()
        {
            { '(', ')' },
            { '[', ']' },
            { '{', '}' },
            { '<', '>' },
        };

        private static readonly Dictionary<char, int> _scoresInvalid = new()
        {
            { ')', 3 },
            { ']', 57 },
            { '}', 1197 },
            { '>', 25137 },
        };

        private static readonly Dictionary<char, int> _scoresRemaining = new()
        {
            { '(', 1 },
            { '[', 2 },
            { '{', 3 },
            { '<', 4 },
            { ')', 1 },
            { ']', 2 },
            { '}', 3 },
            { '>', 4 },
        };

        public static bool IsValid(string line, out char corrupted, out Stack<char> remaining)
        {
            remaining = new Stack<char>();

            foreach (char c in line)
            {
                switch (c)
                {
                    case '(':
                    case '[':
                    case '{':
                    case '<':
                        remaining.Push(_matchClosing[c]);
                        break;
                    case ')':
                    case ']':
                    case '}':
                    case '>':
                        if (remaining.Pop() != c)
                        {
                            corrupted = c;
                            return false;
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(c));
                }
            }

            corrupted = default;
            return true;
        }

        private static long GetCompletionScore(IEnumerable<char> remaining) =>
            remaining.Aggregate(0L, (s, r) => s * 5 + _scoresRemaining[r]);

        public override long Solution1() =>
            _input.Choose(l => (!IsValid(l, out var c, out var _), c))
                .Sum(c => _scoresInvalid[c]);

        public override long Solution2()
        {
            var scores = _input.Choose(l => (IsValid(l, out var _, out var r), r))
                .Select(r => GetCompletionScore(r))
                .OrderBy(s => s)
                .ToArray();
            return scores[scores.Length / 2];
        }
    }
}
