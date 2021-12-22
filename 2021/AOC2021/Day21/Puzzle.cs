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

        private static readonly (int Sum, int Count)[] Dirac3Sums = new[]
        {
            (3, 1),
            (4, 3),
            (5, 6),
            (6, 7),
            (7, 6),
            (8, 3),
            (9, 1),
        };

        private static PlayerState MovePlayer(PlayerState state, int sum)
        {
            var pos = (state.Position + sum).Modulo(10);
            var score = state.Score + pos + 1;
            return new PlayerState
            {
                Position = pos,
                Score = score,
            };
        }

        private static State MoveDeterministic(State state, DeterministicDie die) =>
            state.Player switch
            {
                1 => state with { Player1 = MovePlayer(state.Player1, die.Take(3).Sum()), Player = 2 },
                2 => state with { Player2 = MovePlayer(state.Player2, die.Take(3).Sum()), Player = 1 },
                _ => throw new InvalidOperationException(),
            };

        private static IEnumerable<(State State, int Count)> MoveDirac(State state) =>
            state.Player switch
            {
                1 => Dirac3Sums.Select(pair => (state with { Player1 = MovePlayer(state.Player1, pair.Sum), Player = 2 }, pair.Count)),
                2 => Dirac3Sums.Select(pair => (state with { Player2 = MovePlayer(state.Player2, pair.Sum), Player = 1 }, pair.Count)),
                _ => throw new InvalidOperationException(),
            };

        public override long Solution1()
        {
            var current = new State
            {
                Player1 = new PlayerState { Position = _player1 - 1 },
                Player2 = new PlayerState { Position = _player2 - 1 },
                Player = 1,
            };

            var die = new DeterministicDie();
            while (current.Player1.Score < 1000 && current.Player2.Score < 1000)
            {
                current = MoveDeterministic(current, die);
            }

            var loser = current.Player1.Score > current.Player2.Score ? current.Player2 : current.Player1;

            return loser.Score * die.RollCount;
        }

        public override long Solution2()
        {
            var initialState = new State
            {
                Player1 = new PlayerState { Position = _player1 - 1 },
                Player2 = new PlayerState { Position = _player2 - 1 },
                Player = 1,
            };

            var won1 = 0L;
            var won2 = 0L;

            var todo = new Stack<(State State, long Count)>();
            todo.Push((initialState, 1));

            var next = new Dictionary<State, long>();
            while (todo.Any()) {
                var current = todo.Pop();

                var results = MoveDirac(current.State);
                next.Clear();
                foreach (var r in results)
                {
                    next.TryUpdate(r.State, c => c + r.Count * current.Count, r.Count * current.Count);
                }

                foreach (var kv in next)
                {
                    var state = kv.Key;
                    if (state.Player1.Score >= 21)
                    {
                        won1 += kv.Value;
                    }
                    else if (state.Player2.Score >= 21)
                    {
                        won2 += kv.Value;
                    }
                    else
                    {
                        todo.Push((kv.Key, kv.Value));
                    }
                }
            }

            return Math.Max(won1, won2);
        }

        private readonly record struct PlayerState
        {
            public int Position { get; init; }
            public int Score { get; init; }
        }

        private readonly record struct State
        {
            public PlayerState Player1 { get; init; }
            public PlayerState Player2 { get; init; }
            public byte Player { get; init; }
        }

        private class DeterministicDie
        {
            public int RollCount { get; private set; } = 0;
            private int _current = 1;

            public int Next()
            {
                RollCount++;
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
