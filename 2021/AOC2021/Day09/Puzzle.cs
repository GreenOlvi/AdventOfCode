using AOC2021.Common;

namespace AOC2021.Day09
{
    public class Puzzle : PuzzleBase<long, long>
    {
        public Puzzle(IEnumerable<string> lines)
        {
            _map = lines.Select(l => l.Select(c => c - '0').ToArray()).ToArray();
            _width = _map[0].Length;
            _height = _map.Length;
        }

        private readonly int _width;
        private readonly int _height;
        private readonly int[][] _map;

        private static readonly Point Up = new(0, -1);
        private static readonly Point Down = new(0, 1);
        private static readonly Point Left = new(-1, 0);
        private static readonly Point Right = new(1, 0);

        private int GetHeight(Point p) => _map[p.Y][p.X];

        private IEnumerable<int> GetNeighbourHeights(Point p) =>
            GetNeighbours(p).Select(GetHeight);

        private IEnumerable<Point> GetNeighbours(Point p)
        {
            if (p.Y > 0) yield return p + Up;
            if (p.X > 0) yield return p + Left;
            if (p.Y < _height - 1) yield return p + Down;
            if (p.X < _width - 1) yield return p + Right;
        }

        private IEnumerable<Point> GetAllPoints()
        {
            for (var y = 0; y < _height; y++)
            {
                for (var x = 0; x < _width; x++)
                {
                    yield return new Point(x, y);
                }
            }
        }

        private IEnumerable<Point> FindMinimas()
        {
            foreach (var point in GetAllPoints())
            {
                var h = GetHeight(point);
                if (GetNeighbourHeights(point).All(nh => h < nh))
                {
                    yield return point;
                }
            }
        }

        private IEnumerable<HashSet<Point>> FindBasins() => FindMinimas().Select(FindBasinFromPoint);

        private HashSet<Point> FindBasinFromPoint(Point lowPoint)
        {
            var basin = new HashSet<Point>() { lowPoint };
            var todo = new Queue<Point>(basin);

            while (todo.Count > 0)
            {
                var p = todo.Dequeue();

                var neigbours = GetNeighbours(p).Where(n => !basin.Contains(n) && GetHeight(n) < 9);
                foreach (var n in neigbours)
                {
                    basin.Add(n);
                    todo.Enqueue(n);
                }
            }

            return basin;
        }

        public override long Solution1() => FindMinimas().Sum(m => GetHeight(m) + 1);

        public override long Solution2() =>
            FindBasins().Select(b => b.Count)
                .OrderByDescending(c => c)
                .Take(3)
                .Product();

    }
}
