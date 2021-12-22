using System.Collections;
using System.Text.RegularExpressions;
using AOC2021.Common;

namespace AOC2021.Day21
{
    public class Puzzle : PuzzleBase<long, long>
    {
        public Puzzle(IEnumerable<string> lines)
        {
            _player1 = _inputPattern.Parse(lines.ElementAt(0), "pos", int.Parse);
            _player2 = _inputPattern.Parse(lines.ElementAt(1), "pos", int.Parse);
        }

        private static readonly Regex _inputPattern = new(@"^Player \d+ starting position: (?<pos>\d+)$", RegexOptions.Compiled);

        private readonly int _player1;
        private readonly int _player2;

        public override long Solution1()
        {
            var positions = new[] { _player1 - 1, _player2 - 1 };
            var score = new[] { 0, 0 };
            var player = 0;

            var die = new DeterministicDie();

            while (score[0] < 1000 && score[1] < 1000)
            {
                positions[player] = (positions[player] + die.Take(3).Sum()).Modulo(10);
                score[player] += positions[player] + 1;
                player = (player + 1) % 2;
            }

            return score[player] * die.RolledCount;
        }

        public override long Solution2()
        {
            throw new NotImplementedException();
        }

        private class DeterministicDie
        {
            public int RolledCount { get; private set; } = 0;
            private int _current = 1;

            public int Next()
            {
                RolledCount++;
                var c = _current;
                _current++;
                if (_current > 100)
                {
                    _current = 1;
                }
                return c;
            }

            public IEnumerable<int> Take(int count) =>
                Enumerable.Range(0, count).Select(i => Next());
        }
    }
}
