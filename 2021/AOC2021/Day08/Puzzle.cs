namespace AOC2021.Day08
{
    public class Puzzle : PuzzleBase<long, long>
    {
        public Puzzle(IEnumerable<string> lines)
        {
            _input = lines.Select(ParseLine).ToArray();
        }

        public static (string[], string[]) ParseLine(string line)
        {
            var digits = line.Split(new[] { '|', ' '}, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            return (digits[0..10], digits[10..14]);
        }

        private readonly (string[] Definitions, string[] Digits)[] _input;

        public static long DecodeNumber((string[] Definitions, string[] Digits) line)
        {
            var dict = FindDigits(line.Definitions.Select(s => (s.Length, ToBits(s))));
            return line.Digits
                .Select(ToBits)
                .Select(n => dict[n])
                .Aggregate(0, (s, a) => s * 10 + a);
        }

        private static Dictionary<byte, int> FindDigits(IEnumerable<(int Length, byte)> left)
        {
            var defs = left.ToArray();

            // 2 segments => 1
            var n1 = defs.Single(p => p.Length == 2).Item2;

            // 3 segments => 7
            var n7 = defs.Single(p => p.Length == 3).Item2;

            // 4 segments => 4
            var n4 = defs.Single(p => p.Length == 4).Item2;

            // 7 segments => 8
            var n8 = defs.Single(p => p.Length == 7).Item2;

            // segment a is the only difference between 7 and 1
            //var a = (byte)(n7 ^ n1);

            // 5 segments => 2, 3, 5
            var l5 = defs.Where(p => p.Length == 5).Select(p => p.Item2).ToArray();

            // 6 segments => 0, 6, 9
            var l6 = defs.Where(p => p.Length == 6).Select(p => p.Item2).ToArray();

            // 3 is the only one with 5 segments that has all segments from 7
            var n3 = l5.Single(l => (l & n7) == n7);

            // 9 is all segments from 4 and all segments from 7 and segment e
            var n9 = l6.Single(l => CountBits(l & (n4 | n7)) == 5);

            // segment g is segments from 9 minus combined segments from 4 and 7
            //var g = n9 ^ (n4 | n7);

            // segment e is the only difference between 8 and 9
            var e = (byte)(n8 ^ n9);

            // from length 5 only 2 has segment e
            var n2 = l5.Single(l => (l & e) != 0);

            // 5 is only one left in length 5
            var n5 = l5.Single(l => l != n2 && l != n3);

            // 6 is just 5 with e
            var n6 = (byte)(n5 | e);

            // 0 is only one left in length 6
            var n0 = l6.Single(l => l != n9 && l != n6);

            return new Dictionary<byte, int>
            {
                { n0, 0 },
                { n1, 1 },
                { n2, 2 },
                { n3, 3 },
                { n4, 4 },
                { n5, 5 },
                { n6, 6 },
                { n7, 7 },
                { n8, 8 },
                { n9, 9 },
            };
        }

        private static byte CountBits(int n) =>
            (byte)Enumerable.Range(0, 7).Sum(b => (n & (1 << b)) == 0 ? 0 : 1);

        private static byte ToBits(string d) => (byte)d.Select(c => 1 << (c - 'a')).Sum();

        public override long Solution1() =>
            _input.SelectMany(i => i.Digits).Count(n => n.Length == 2 || n.Length == 3 || n.Length == 4 || n.Length == 7);

        public override long Solution2() => _input.Sum(DecodeNumber);
    }
}
