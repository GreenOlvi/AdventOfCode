using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AoC2019.Common;

namespace AoC2019.Puzzle18
{
    public class Map
    {
        public Map(char[] input, int width)
        {
            _map = input;
            _width = width;
            _height = _map.Length / width;

            var indexed = input.Select((c, i) => (c, i)).ToArray();

            Start = Position.From(indexed.First(p => p.c == '@').i, _width);

            _keys = indexed.Where(p => p.c >= 'a' && p.c <= 'z')
                .Select(p => (p.c, pos: Position.From(p.i, _width)))
                .ToDictionary(p => p.c, p => p.pos);

            _doors = indexed.Where(p => p.c >= 'A' && p.c <= 'Z')
                .Select(p => (p.c, pos: Position.From(p.i, _width)))
                .ToDictionary(p => p.c, p => p.pos);

            _keyDeps = AnalyzeKeyDependencies(Start).ToArray();
        }

        private readonly char[] _map;
        private readonly int _width;
        private readonly int _height;

        private readonly Dictionary<char, Position> _keys;
        private readonly Dictionary<char, Position> _doors;

        public Position Start { get; }
        public IEnumerable<Position> AllKeys => _keys.OrderBy(k => k.Key).Select(k => k.Value);
        public IEnumerable<Position> AllDoors => _doors.OrderBy(k => k.Key).Select(k => k.Value);

        public static uint KeyHash(IEnumerable<char> keys)
        {
            var hash = 0u;
            foreach (var k in keys)
            {
                var i = (k & 0x1f) - 1;
                hash |= 1u << i;
            }
            return hash;
        }

        public static ulong CombinedHash(IEnumerable<char> keys, char pos) => KeyHash(keys) | (((ulong)pos) << 32);

        public CacheStats ShortestWayStats = new CacheStats();

        private readonly Dictionary<ulong, (char[], int)> _shortestWayCache = new Dictionary<ulong, (char[], int)>();

        public (char[] Path, int Steps) FindShortestWay(char from, char[] hasKeys)
        {
            var hash = CombinedHash(hasKeys, from);
            if (_shortestWayCache.TryGetValue(hash, out var way))
            {
                ShortestWayStats.IncHit();
                return way;
            }

            var available = AvailableKeysFast(hasKeys);
            if (!available.Any())
            {
                return (new[] { from }, 0);
            }

            var min = (Path: Array.Empty<char>(), Steps: int.MaxValue);
            foreach (var key in available)
            {
                var distance = GetDistance(from, key);
                var (path, pathSteps) = FindShortestWay(key, hasKeys.Append(key).ToArray());
                var totalSteps = pathSteps + distance;
                if (totalSteps < min.Steps)
                {
                    min = (path.Prepend(from).ToArray(), totalSteps);
                }
            }
            _shortestWayCache.Add(hash, min);
            ShortestWayStats.IncMiss();
            return min;
        }


        public CacheStats AvailableKeysStats = new CacheStats();

        private readonly Dictionary<uint, char[]> _availableKeys = new Dictionary<uint, char[]>();
        private char[] AvailableKeysCached(char[] hasKeys)
        {
            var hash = KeyHash(hasKeys);
            if (_availableKeys.TryGetValue(hash, out var keys))
            {
                AvailableKeysStats.IncHit();
                return keys;
            }

            var available = AvailableKeys(Start, hasKeys).OrderBy(k => k).ToArray();
            _availableKeys.Add(hash, available);
            AvailableKeysStats.IncMiss();
            return available;
        }

        public IEnumerable<char> AvailableKeys(Position from, char[] hasKeys)
        {
            var openDoors = hasKeys.Select(k => (char)(k - 0x20)).ToHashSet();

            var visited = new HashSet<Position>();
            var stack = new Stack<Position>();
            stack.Push(from);

            while (stack.Any())
            {
                var p = stack.Pop();
                visited.Add(p);

                foreach (var dir in DirectionExtensions.Directions)
                {
                    var newPos = p.Move(dir);
                    var c = Get(newPos);

                    if (visited.Contains(newPos) || IsWall(c) || (IsDoor(c) && !openDoors.Contains(c)))
                    {
                        continue;
                    }

                    if (IsKey(c) && !openDoors.Contains((char)(c - 0x20)))
                    {
                        yield return c;
                    }

                    stack.Push(newPos);
                }
            }
        }


        private readonly (char Key, uint Hash)[] _keyDeps;

        private IEnumerable<(char Key, uint Hash)> AnalyzeKeyDependencies(Position from)
        {
            var visited = new HashSet<Position>();
            var queue = new Queue<(Position Position, uint Keys)>();
            queue.Enqueue((from, 0u));

            while (queue.Any())
            {
                var (p, k) = queue.Dequeue();
                visited.Add(p);

                foreach (var newPos in p.Neighbours())
                {
                    var c = Get(newPos);

                    if (visited.Contains(newPos) || IsWall(c))
                    {
                        continue;
                    }

                    if (IsDoor(c))
                    {
                        k = AddKey(k, c);
                    }

                    if (IsKey(c))
                    {
                        yield return (c, k);
                    }

                    queue.Enqueue((newPos, k));
                }
            }
        }

        private uint AddKey(uint k, char c)
        {
            var i = (c & 0x1f) - 1;
            return k | (1u << i);
        }

        private bool HasKey(uint k, char c)
        {
            var i = (c & 0x1f) - 1;
            return (k & i) > 0;
        }

        public IEnumerable<char> AvailableKeysFast(char[] hasKeys)
        {
            var hash = KeyHash(hasKeys);
            return _keyDeps.Where(kd => ((kd.Hash & hash) == kd.Hash) && ((AddKey(0u, kd.Key) & hash) == 0))
                .Select(kd => kd.Key);
        }


        private readonly Dictionary<char, Dictionary<char, int>> _distances = new Dictionary<char, Dictionary<char, int>>();
        public int GetDistance(char from, char to)
        {
            if (_distances.TryGetValue(from, out var dist))
            {
                return dist[to];
            }

            var pos = from == '@' ? Start : _keys[from];
            var dists = GetDistancesDijkstra(pos).ToDictionary(d => d.Key, d => d.Distance);
            _distances.Add(from, dists);
            return dists[to];
        }

        public IEnumerable<(char Key, int Distance)> GetDistancesDijkstra(Position from)
        {
            var dist = new Dictionary<Position, int>()
            {
                { from, 0 },
            };

            var queue = new Queue<Position>();
            queue.Enqueue(from);

            while (queue.Any())
            {
                var p = queue.Dequeue();
                var d = dist[p];

                foreach (var dir in DirectionExtensions.Directions)
                {
                    var newPos = p.Move(dir);
                    var c = Get(newPos);

                    if (IsWall(c))
                    {
                        continue;
                    }

                    if (dist.TryGetValue(newPos, out var currDist))
                    {
                        if (currDist > d + 1)
                        {
                            queue.Enqueue(newPos);
                            dist[newPos] = d + 1;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        queue.Enqueue(newPos);
                        dist[newPos] = d + 1;
                    }
                }
            }

            return _keys.Select(kv => (kv.Key, dist[kv.Value]));
        }


        public char Get(Position position) => _map[position.Y * _width + position.X];

        private bool IsWall(char c) => c == '#';
        private bool IsKey(char c) => c >= 'a' && c <= 'z';
        private bool IsDoor(char c) => c >= 'A' && c <= 'Z';

        public static Map Parse(IEnumerable<string> input)
        {
            var ch = new List<char>();
            var width = -1;
            foreach (var line in input.Select(l => l.Trim()))
            {
                if (width == -1)
                {
                    width = line.Length;
                }

                ch.AddRange(line);
            }
            return new Map(ch.ToArray(), width);
        }

        public static Map ModifyAndParse(IEnumerable<string> input)
        {
            var lines = input.ToArray();
            var w = lines[0].Trim().Length;

            var midLine = lines.Select((l, i) => (l, i)).First(line => line.l.Contains(".@."));
            var x = midLine.l.IndexOf('@');
            var y = midLine.i;

            lines[y - 1] = lines[y - 1].Remove(x - 1, 3).Insert(x - 1, "@#@");
            lines[y] = lines[y].Remove(x - 1, 3).Insert(x - 1, "###");
            lines[y + 1] = lines[y + 1].Remove(x - 1, 3).Insert(x - 1, "@#@");

            return Parse(lines);
        }

        public string Draw()
        {
            var sb = new StringBuilder();
            foreach (var y in Enumerable.Range(0, _height))
            {
                foreach (var x in Enumerable.Range(0, _width))
                {
                    sb.Append(_map[y * _width + x]);
                }
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }
    }
}
