using System.Text;
using System.Text.RegularExpressions;
using AOC2021.Common;

namespace AOC2021.Day15
{
    public class Puzzle : PuzzleBase<long, long>
    {
        public Puzzle(IEnumerable<string> lines)
        {
            var input = lines.ToArray();
            _width = input[0].Length;
            _height = input.Length;
            _risk = lines.SelectMany(l => l.Select(c => c - '0')).ToArray();
        }

        private readonly int _width;
        private readonly int _height;
        private readonly int[] _risk;

        private static string PrintMap(Map map, bool debug = false)
        {
            var path = GetShortestPath(map).ToHashSet();

            var sb = new StringBuilder();
            for (var y = 0; y < map.Height; y++)
            {
                sb.AppendLine();
                for (var x = 0; x < map.Width; x++)
                {
                    var p = new Point(x, y);
                    var d = map.GetData(p);
                    if (debug)
                    {
                        if (path.Contains(p))
                        {
                            sb.Append($"[{d.Risk}, {d.GetDir()}, {d.Sum,3}] ");
                        }
                        else
                        {
                            sb.Append($" {d.Risk}, {d.GetDir()}, {d.Sum,3}  ");
                        }
                    }
                    else
                    {
                        if (path.Contains(p))
                        {
                            sb.Append($"[{d.Risk}]");
                        }
                        else
                        {
                            sb.Append($" {d.Risk} ");
                        }
                    }
                }
            }
            return sb.ToString();
        }

        private static IEnumerable<Point> GetShortestPath(Map map)
        {
            var point = map.GetStart();
            do
            {
                yield return point.Pos;
                point = point.Next;
            }
            while (point != null);
        }

        private static bool Update(Map map, Map.PointData point)
        {
            var best = map.GetNeighbours(point)
                .Where(n => n.Visited)
                .OrderBy(n => n.Sum)
                .First();

            if (!point.Visited)
            {
                point.Sum = best.Sum + point.Risk;
                point.Next = best;
                point.Visited = true;
                return true;
            }
            else
            {
                if (point.Sum > best.Sum + point.Risk)
                {
                    point.Sum = best.Sum + point.Risk;
                    point.Next = best;
                    return true;
                }
            }

            return false;
        }

        private static long FindLowestRisk(Map map) {
            var start = map.GetStart();
            start.Sum = start.Risk;
            start.Visited = true;

            var todo = new PriorityQueue<Map.PointData>(p => (int)(p.Pos.X + p.Pos.Y));
            todo.EnqueueRange(map.GetNeighbours(start));

            while (todo.Count > 0)
            {
                var p = todo.Dequeue();
                if (Update(map, p))
                {
                    todo.EnqueueRange(map.GetNeighbours(p));
                }
            }

            var end = map.GetEnd();
            return end.Sum - end.Risk;
        }

        public override long Solution1()
        {
            var map = new Map(_width, _height, _risk);
            return FindLowestRisk(map);
        }

        public override long Solution2()
        {
            var map = Map.FromRiskX25(_width, _height, _risk);
            return FindLowestRisk(map);
        }

        private class Map
        {
            private readonly PointData[] _map;
            public int Width { get; }
            public int Height { get; }

            public Map(int width, int height, int[] risk)
            {
                Width = width;
                Height = height;
                _map = risk.Select((r, i) =>
                    new PointData
                    {
                        Risk = r,
                        Pos = new Point(i % Width, i / Width),
                    })
                    .ToArray();
            }

            public static Map FromRiskX25(int width, int height, int[] risk)
            {
                var map = new int[width * height * 25];
                for (var y = 0; y < height; y++)
                {
                    for (var x = 0; x < width; x++)
                    {
                        for (var dy = 0; dy < 5; dy++)
                        {
                            for (var dx = 0; dx < 5; dx++)
                            {
                                map[(y + dy * height) * width * 5 + (x + dx * width)] = (risk[y * width + x] + dx + dy - 1) % 9 + 1;
                            }
                        }
                    }
                }
                return new Map(width * 5, height * 5, map);
            }

            public PointData GetData(Point p) => _map[p.Y * Width + p.X];
            public PointData GetStart() => _map[0];
            public PointData GetEnd() => _map.Last();

            public IEnumerable<Point> GetNeighbourPoints(Point p)
            {
                if (p.X > 0) yield return p + Point.Left;
                if (p.Y < Height - 1) yield return p + Point.Down;
                if (p.X < Width - 1) yield return p + Point.Right;
                if (p.Y > 0) yield return p + Point.Up;
            }

            public IEnumerable<PointData> GetNeighbours(PointData p) =>
                GetNeighbourPoints(p.Pos).Select(GetData);

            public class PointData
            {
                public int Risk { get; init; }
                public long Sum { get; set; }
                public Point Pos { get; init; }
                public PointData Next { get; set; }
                public bool Visited { get; set; } = false;

                public char GetDir()
                {
                    return Next switch
                    {
                        null => '•',
                        { Pos.Y: var ny } when ny == Pos.Y - 1  => '↑',
                        { Pos.Y: var ny } when ny == Pos.Y + 1  => '↓',
                        { Pos.X: var nx } when nx == Pos.X - 1  => '←',
                        { Pos.X: var nx } when nx == Pos.X + 1  => '→',
                        _ => throw new NotImplementedException(),
                    };
                }

                public override string ToString() =>
                    $"[{Pos}, {Risk}, " + (Visited ? $"{Sum}, {GetDir()}" : "-") + "]";

                public override bool Equals(object obj)
                {
                    if (obj is null)
                    {
                        return false;
                    }

                    if (ReferenceEquals(this, obj))
                    {
                        return true;
                    }

                    if (obj is not PointData o)
                    {
                        return false;
                    }

                    return Equals(o);
                }

                private bool Equals(PointData o) => Pos == o.Pos;

                public override int GetHashCode() => Pos.GetHashCode();
            }
        }

        private class PriorityQueue<T>
        {
            public PriorityQueue(Func<T, int> prioritySelector)
            {
                _elements = new HashSet<T>();
                _prioritySelector = prioritySelector;
            }

            private readonly HashSet<T> _elements;
            private readonly Func<T, int> _prioritySelector;

            public void Enqueue(T element)
            {
                _elements.Add(element);
            }

            public void EnqueueRange(IEnumerable<T> elements)
            {
                foreach (var e in elements)
                {
                    Enqueue(e);
                }
            }

            public T Dequeue()
            {
                var first = _elements.OrderBy(e => _prioritySelector(e)).First();
                _elements.Remove(first);
                return first;
            }

            public int Count => _elements.Count;

            public override string ToString() => $"Count: {_elements.Count}";
        }
    }
}
