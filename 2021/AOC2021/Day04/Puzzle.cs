using AOC2021.Common;

namespace AOC2021.Day04
{
    public class Puzzle : PuzzleBase<long, long>
    {
        public Puzzle(IEnumerable<string> lines)
        {
            _drawn = lines.First().Split(",").ParseInts().ToArray();

            _boardNumbers = lines.Skip(2).SplitGroups()
                .Select(g =>
                    g.Select(l =>
                        l.Split(" ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                            .ParseInts()
                            .ToArray())
                    .ToArray())
                .ToList();
        }

        private readonly int[] _drawn;
        private readonly List<int[][]> _boardNumbers;

        private static bool TryMarkAll(int number, IEnumerable<Board> boards, out IEnumerable<Board> winning)
        {
            var win = new List<Board>();
            foreach (var b in boards)
            {
                if (b.MarkAndCheck(number))
                {
                    win.Add(b);
                }
            }

            winning = win;
            return win.Any();
        }

        private static IEnumerable<(Board, int)> GetWinningOrder(IEnumerable<Board> boards, IEnumerable<int> drawn)
        {
            var boardList = boards.ToList();
            foreach (var number in drawn)
            {
                if (TryMarkAll(number, boardList, out var winning))
                {
                    foreach (var b in winning)
                    {
                        yield return (b, number);
                        boardList.Remove(b);
                    }
                }
            }
        }

        public override long Solution1()
        {
            var (winning, lastNumber) = GetWinningOrder(_boardNumbers.Select(b => new Board(b)), _drawn).First();
            return winning.GetScore() * lastNumber;
        }

        public override long Solution2()
        {
            var (board, number) = GetWinningOrder(_boardNumbers.Select(b => new Board(b)), _drawn).Last();
            return board.GetScore() * number;
        }

        private class Board
        {
            public Board(int[][] numbers)
            {
                _numbers = numbers.Select(row => row.Select(n => (n, false)).ToArray()).ToArray();

                _index = new Dictionary<int, (int, int)>();
                for (int r = 0; r < 5; r++)
                {
                    for (int c = 0; c < 5; c++)
                    {
                        _index[numbers[r][c]] = (r, c);
                    }
                }
            }

            private readonly (int N, bool Marked)[][] _numbers;
            private readonly Dictionary<int, (int, int)> _index;

            public IEnumerable<(int N, bool Marked)> Row(int i) => _numbers[i];

            public IEnumerable<(int N, bool Marked)> Col(int i) => _numbers.Select(row => row[i]);

            public bool MarkAndCheck(int number)
            {
                if (!_index.TryGetValue(number, out (int, int) pos))
                {
                    return false;
                }

                var (row, col) = pos;
                _numbers[row][col] = (number, true);

                return Row(row).All(n => n.Marked) || Col(col).All(n => n.Marked);
            }

            public long GetScore() =>
                _numbers.SelectMany(row => row)
                    .Where(n => !n.Marked)
                    .Sum(n => n.N);
        }
    }
}
