using AOC2020.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2020.Day11
{
    public class Puzzle : PuzzleBase<int, int>
    {
        public Puzzle(IEnumerable<string> input)
        {
            var i = input.ToArray();
            _width = i[0].Length;
            _height = i.Length;
            Input = ParseInput(i);
        }

        private readonly int _width;
        private readonly int _height;
        public readonly Layout[] Input;

        public static Layout[] ParseInput(IEnumerable<string> input) =>
            input.SelectMany(ParseLine).ToArray();

        private static IEnumerable<Layout> ParseLine(string line)
        {
            return line.Select(c => c switch
            {
                '.' => Layout.Floor,
                'L' => Layout.Empty,
                '#' => Layout.Occupied,
                _ => throw new PuzzleException("Invalid input"),
            });
        }

        private Point GetPoint(int pos) => new Point(pos % _width, pos / _width);

        private int GetPos(Point p) => _width * p.Y + p.X;

        private bool IsInBoard(Point p) => p.X >= 0 && p.Y >= 0 && p.X < _width && p.Y < _height;

        public int CountOccupiedNeighbours(Layout[] board, int pos)
        {
            var p = GetPoint(pos);
            return _directions.Select(p.Add)
                .Where(IsInBoard)
                .Select(GetPos)
                .Count(pos => board[pos] == Layout.Occupied);
        }

        private static readonly Point[] _directions =
            {
                new Point(-1, -1),
                new Point(0, -1),
                new Point(1, -1),
                new Point(-1, 0),
                new Point(1, 0),
                new Point(-1, 1),
                new Point(0, 1),
                new Point(1, 1),
            };

        private bool CheckOccupiedLine(Layout[] board, int pos, Point dir)
        {
            var p = GetPoint(pos);
            while (true)
            {
                p = p.Add(dir);
                
                if (!IsInBoard(p))
                {
                    return false;
                }

                var l = board[GetPos(p)];
                if (l == Layout.Empty)
                {
                    return false;
                }

                if (l == Layout.Occupied)
                {
                    return true;
                }
            }
        }

        public int CountOccupiedNeighbours2(Layout[] board, int pos) =>
            _directions.Count(d => CheckOccupiedLine(board, pos, d));

        public static Layout[] TransformBoard(Layout[] board, Func<Layout[], int, Layout> transformFunc) =>
            Enumerable.Range(0, board.Length).Select(i => transformFunc(board, i)).ToArray();

        public static char LayoutToChar(Layout l) =>
                l switch
                {
                    Layout.Floor => '.',
                    Layout.Empty => 'L',
                    Layout.Occupied => '#',
                    _ => ' ',
                };

        private static string ToHash(Layout[] board) =>
            new string(board.Select(LayoutToChar).ToArray());

        private static Layout[] TransformUntilStable(Layout[] board, Func<Layout[], int, Layout> transformFunc)
        {
            var current = board;
            var hash = ToHash(current);
            while (true)
            {
                var newBoard = TransformBoard(current, transformFunc);
                var newHash = ToHash(newBoard);
                if (newHash == hash)
                {
                    return newBoard;
                }
                current = newBoard;
                hash = newHash;
            }
        }

        public Layout TransformField1(Layout[] board, int pos)
        {
            var current = board[pos];
            if (current == Layout.Floor)
            {
                return Layout.Floor;
            }

            var p = GetPoint(pos);
            var occupiedNeighbours = _directions.Select(p.Add)
                .Where(IsInBoard)
                .Select(GetPos)
                .Count(pos => board[pos] == Layout.Occupied);

            if (current == Layout.Empty && occupiedNeighbours == 0)
            {
                return Layout.Occupied;
            }

            if (current == Layout.Occupied && occupiedNeighbours >= 4)
            {
                return Layout.Empty;
            }

            return current;
        }

        private Layout TransformField2(Layout[] board, int pos)
        {
            var current = board[pos];
            if (current == Layout.Floor)
            {
                return Layout.Floor;
            }

            var occupiedNeighbours = CountOccupiedNeighbours2(board, pos);

            if (current == Layout.Empty && occupiedNeighbours == 0)
            {
                return Layout.Occupied;
            }

            if (current == Layout.Occupied && occupiedNeighbours >= 5)
            {
                return Layout.Empty;
            }

            return current;
        }

        public override int Solution1()
        {
            var stable = TransformUntilStable(Input, TransformField1);
            return stable.Count(s => s == Layout.Occupied);
        }

        public override int Solution2()
        {
            var stable = TransformUntilStable(Input, TransformField2);
            return stable.Count(s => s == Layout.Occupied);
        }

        public enum Layout { Floor, Empty, Occupied }
    }
}
