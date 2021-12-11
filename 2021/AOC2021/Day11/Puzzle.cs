using System.Text.RegularExpressions;
using AOC2021.Common;

namespace AOC2021.Day11
{
    public class Puzzle : PuzzleBase<long, long>
    {
        public Puzzle(IEnumerable<string> lines)
        {
            var lineArray = lines.ToArray();
            _width = lineArray[0].Length;
            _height = lineArray.Length;
            _input = lineArray.SelectMany(l => l.Select(c => c - '0')).ToArray();
        }

        private readonly int _width;
        private readonly int _height;
        private readonly int[] _input;

        private static IEnumerable<Point> GetAllNeighbours(Point p) => new[]
            {
                new Point(p.X - 1, p.Y - 1),
                new Point(p.X - 1, p.Y),
                new Point(p.X - 1, p.Y + 1),
                new Point(p.X, p.Y + 1),
                new Point(p.X + 1, p.Y + 1),
                new Point(p.X + 1, p.Y),
                new Point(p.X + 1, p.Y - 1),
                new Point(p.X, p.Y - 1),
            };

        private bool IsInBounds(Point p) => p.X >= 0 && p.Y >= 0 && p.X < _width && p.Y < _height;

        private IEnumerable<int> GetNeighbours(int p) =>
            GetAllNeighbours(new Point(p % _width, p / _width))
                .Where(IsInBounds)
                .Select(p => (int)(p.Y * _width + p.X));

        private (int[] Board, int flashes) NextStep(int[] board)
        {
            var newBoard = new int[board.Length];
            var flashes = 0;
            var toFlash = new Queue<int>();

            for (int i = 0; i < board.Length; i++)
            {
                newBoard[i] = board[i] + 1;
                if (newBoard[i] == 10)
                {
                    toFlash.Enqueue(i);
                }
            }

            while (toFlash.Count > 0)
            {
                var f = toFlash.Dequeue();
                flashes++;
                foreach (var n in GetNeighbours(f))
                {
                    newBoard[n]++;
                    if (newBoard[n] == 10)
                    {
                        toFlash.Enqueue(n);
                    }
                }
            }

            for (int i = 0; i < board.Length; i++)
            {
                if (newBoard[i] > 9)
                {
                    newBoard[i] = 0;
                }
            }

            return (newBoard, flashes);
        }

        private void PrintBoard(int[] board)
        {
            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    var v = board[y * _width + x];
                    if (v == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    }
                    Console.Write(v);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public void PrintSteps(int steps)
        {
            var board = _input;
            var flashes = 0L;

            Console.WriteLine("Before any steps:");
            PrintBoard(board);

            for (int i = 1; i <= steps; i++)
            {
                var (n, f) = NextStep(board);

                Console.WriteLine($"After step {i}: flashes {f}");
                PrintBoard(n);

                flashes += f;
                board = n;
            }
        }

        public override long Solution1()
        {
            var board = _input;
            var flashes = 0L;

            for (int i = 1; i <= 100; i++)
            {
                var (n, f) = NextStep(board);
                flashes += f;
                board = n;
            }

            return flashes;
        }

        public override long Solution2()
        {
            var board = _input;
            var step = 0L;

            while (true)
            {
                step++;
                var (n, f) = NextStep(board);

                if (f == _width * _height)
                {
                    break;
                }

                board = n;
            }

            return step;
        }
    }
}
