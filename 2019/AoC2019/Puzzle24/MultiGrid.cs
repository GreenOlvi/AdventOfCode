using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC2019.Puzzle24
{
    public class MultiGrid
    {
        public MultiGrid(uint initial)
        {
            _grids.Add(0, initial);
        }

        private MultiGrid(Dictionary<int, uint> grids)
        {
            _grids = grids;
        }

        private readonly Dictionary<int, uint> _grids = new Dictionary<int, uint>();

        public IEnumerable<uint> Grids() =>
            _grids.OrderBy(kv => kv.Key).Select(kv => kv.Value);

        public MultiGrid Steps(int steps)
        {
            var grid = this;
            for (var i = 0; i < steps; i++)
            {
                grid = grid.Step();
            }
            return grid;
        }

        public MultiGrid Step()
        {
            var newGrids = new Dictionary<int, uint>();
            foreach (var depth in _grids.Keys)
            {
                var newGrid = StepGrid(depth);
                newGrids.Add(depth, newGrid);
            }

            var first = _grids.Keys.Min();
            if (_grids[first] != 0)
            {
                var newFirst = StepGrid(first - 1);
                if (newFirst != 0)
                {
                    newGrids.Add(first - 1, newFirst);
                }
            }

            var last = _grids.Keys.Max();
            if (_grids[last] != 0)
            {
                var newLast = StepGrid(last + 1);
                if (newLast != 0)
                {
                    newGrids.Add(last + 1, newLast);
                }
            }

            return new MultiGrid(newGrids);
        }

        private uint StepGrid(int depth) =>
            _neighbours.Keys
                .Select(i => StepField(depth, i))
                .Aggregate(0u, (a, b) => a | b);

        private uint StepField(int depth, int i) =>
            IsNewSet(IsSet(depth, i) == 1, CountNeighbours(depth, i)) ? 1u << i : 0;

        private bool IsNewSet(bool isSet, int neighbours) =>
            (isSet && neighbours == 1) ||
            (!isSet && (neighbours == 1 || neighbours == 2));

        private int CountNeighbours(int depth, int field) =>
            _neighbours[field].Select(p => IsSet(depth + p.Depth, p.Field)).Sum();

        private int IsSet(int depth, int field)
        {
            if (!_grids.TryGetValue(depth, out var grid))
            {
                return 0;
            }
            return (grid & (1 << field)) > 0 ? 1 : 0;
        }

        private static readonly Dictionary<int, (int Depth, int Field)[]> _neighbours = new Dictionary<int, (int, int)[]>()
        {
            // Up Right Down Left
            { 0, new[] { (-1, 7), (0, 1), (0, 5), (-1, 11) } },
            { 1, new[] { (-1, 7), (0, 2), (0, 6), (0, 0) } },
            { 2, new[] { (-1, 7), (0, 3), (0, 7), (0, 1) } },
            { 3, new[] { (-1, 7), (0, 4), (0, 8), (0, 2) } },
            { 4, new[] { (-1, 7), (-1, 13), (0, 9), (0, 3) } },

            { 5, new[] { (0, 0), (0, 6), (0, 10), (-1, 11) } },
            { 6, new[] { (0, 1), (0, 7), (0, 11), (0, 5) } },
            { 7, new[] { (0, 2), (0, 8), (1, 0), (1, 1), (1, 2), (1, 3), (1, 4), (0, 6) } },
            { 8, new[] { (0, 3), (0, 9), (0, 13), (0, 7) } },
            { 9, new[] { (0, 4), (-1, 13), (0, 14), (0, 8) } },

            { 10, new[] { (0, 5), (0, 11), (0, 15), (-1, 11) } },
            { 11, new[] { (0, 6), (1, 0), (1, 5), (1, 10), (1, 15), (1, 20), (0, 16), (0, 10) } },
            // 12 => +1
            { 13, new[] { (0, 8), (0, 14), (0, 18), (1, 4), (1, 9), (1, 14), (1, 19), (1, 24) } },
            { 14, new[] { (0, 9), (-1, 13), (0, 19), (0, 13) } },

            { 15, new[] { (0, 10), (0, 16), (0, 20), (-1, 11) } },
            { 16, new[] { (0, 11), (0, 17), (0, 21), (0, 15) } },
            { 17, new[] { (1, 20), (1, 21), (1, 22), (1, 23), (1, 24), (0, 18), (0, 22), (0, 16) } },
            { 18, new[] { (0, 13), (0, 19), (0, 23), (0, 17) } },
            { 19, new[] { (0, 14), (-1, 13), (0, 24), (0, 18) } },

            { 20, new[] { (0, 15), (0, 21), (-1, 17), (-1, 11) } },
            { 21, new[] { (0, 16), (0, 22), (-1, 17), (0, 20) } },
            { 22, new[] { (0, 17), (0, 23), (-1, 17), (0, 21) } },
            { 23, new[] { (0, 18), (0, 24), (-1, 17), (0, 22) } },
            { 24, new[] { (0, 19), (-1, 13), (-1, 17), (0, 23) } },
        };

        public int CountBugs()
        {
            var bugs = 0;
            foreach (var g in _grids.Values)
            {
                for (var i = 0; i < 25; i++)
                {
                    if (((1u << i) & g) > 0)
                    {
                        bugs++;
                    }
                }
            }
            return bugs;
        }

        public string Draw()
        {
            var sb = new StringBuilder();
            foreach (var d in _grids.Keys.OrderBy(k => k))
            {
                sb.AppendLine($"Depth = {d}");
                DrawGrid(ref sb, _grids[d]);
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }

        private void DrawGrid(ref StringBuilder sb, uint grid)
        {
            var bit = 1u;
            for (var i = 0; i < 25; i++)
            {
                if (i == 12)
                {
                    sb.Append('?');
                }
                else
                {
                    sb.Append((grid & bit) == 0 ? '.' : '#');
                    bit <<= 1;
                    if (i % 5 == 4)
                    {
                        sb.Append(Environment.NewLine);
                    }
                }
            }
        }
    }
}
