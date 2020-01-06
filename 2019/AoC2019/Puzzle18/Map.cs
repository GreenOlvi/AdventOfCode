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

            Start = indexed.Where(p => p.c == '@').Select(p => Position.From(p.i, _width)).ToArray();

            _keys = indexed.Where(p => p.c >= 'a' && p.c <= 'z')
                .Select(p => (p.c, pos: Position.From(p.i, _width)))
                .ToDictionary(p => p.c, p => p.pos);

            _doors = indexed.Where(p => p.c >= 'A' && p.c <= 'Z')
                .Select(p => (p.c, pos: Position.From(p.i, _width)))
                .ToDictionary(p => p.c, p => p.pos);

            var deps = Start.Select(p => BuildKeyDependencies(p).ToArray()).ToArray();
            _keySection = deps.SelectMany((d, i) => d.Select(k => (k.Key, i))).ToDictionary(k => k.Key, k => k.i);
            _keyDeps = deps.SelectMany(d => d).ToArray();
        }

        private readonly char[] _map;
        private readonly int _width;
        private readonly int _height;

        private readonly Dictionary<char, Position> _keys;
        private readonly Dictionary<char, Position> _doors;
        private readonly Dictionary<char, int> _keySection;


        public Position[] Start { get; }
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
        public static ulong CombinedHash(IEnumerable<char> keys, IEnumerable<char> pos)
        {
            var posHash = pos.Aggregate(0ul, (h, p) => (h << 8) | p);
            return KeyHash(keys) | (posHash << 32);
        }

        public CacheStats ShortestWayStats = new CacheStats();

        private readonly Dictionary<ulong, (char[], int)> _shortestWayCache = new Dictionary<ulong, (char[], int)>();

        public (char[] Path, int Steps) FindShortestWay() => FindShortestWay('@', Array.Empty<char>());

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


        private readonly Dictionary<ulong, (string[], int)> _shortestWaySectionsCache = new Dictionary<ulong, (string[], int)>();
        public (string[] Path, int Steps) FindShortestWayMultiple() =>
            FindShortestWayMultiple("@@@@", Array.Empty<char>());

        public (string[] Path, int Steps) FindShortestWayMultiple(string from, char[] hasKeys)
        {
            var hash = CombinedHash(hasKeys, from);
            if (_shortestWaySectionsCache.TryGetValue(hash, out var way))
            {
                ShortestWayStats.IncHit();
                return way;
            }

            var available = AvailableKeysFast(hasKeys);
            if (!available.Any())
            {
                return (new[] { from }, 0);
            }

            var min = (Path: Array.Empty<string>(), Steps: int.MaxValue);
            foreach (var key in available)
            {
                var section = _keySection[key];
                var distance = GetDistance(from[section], key);
                var newFrom = ReplaceCharAt(from, section, key);

                var (path, pathSteps) = FindShortestWayMultiple(newFrom, hasKeys.Append(key).ToArray());
                var totalSteps = pathSteps + distance;
                if (totalSteps < min.Steps)
                {
                    min = (path.Prepend(from).ToArray(), totalSteps);
                }
            }
            _shortestWaySectionsCache.Add(hash, min);
            ShortestWayStats.IncMiss();
            return min;
        }

        private string ReplaceCharAt(string str, int index, char value) =>
            new StringBuilder(str).Remove(index, 1).Insert(index, value).ToString();


        private readonly (char Key, uint Hash)[] _keyDeps;

        private IEnumerable<(char Key, uint Hash)> BuildKeyDependencies(Position from)
        {
            var visited = new HashSet<Position>();
            var queue = new Queue<(Position Position, uint Keys)>();
            queue.Enqueue((from, 0u));
            visited.Add(from);

            while (queue.Any())
            {
                var (p, k) = queue.Dequeue();

                foreach (var newPos in p.Neighbours())
                {
                    var c = Get(newPos);
                    if (visited.Contains(newPos) || IsWall(c))
                    {
                        continue;
                    }
                    visited.Add(newPos);

                    var keyReq = IsDoor(c) ? AddKey(k, c) : k;

                    if (IsKey(c))
                    {
                        yield return (c, keyReq);
                    }

                    queue.Enqueue((newPos, keyReq));
                }
            }
        }

        private uint AddKey(uint k, char c)
        {
            var i = (c & 0x1f) - 1;
            return k | (1u << i);
        }

        public IEnumerable<char> AvailableKeysFast(char[] hasKeys)
        {
            var hash = KeyHash(hasKeys);
            return _keyDeps.Where(kd => ((kd.Hash & hash) == kd.Hash) && ((AddKey(0u, kd.Key) & hash) == 0))
                .Select(kd => kd.Key);
        }


        private readonly Dictionary<Position, Dictionary<char, int>> _distances = new Dictionary<Position, Dictionary<char, int>>();
        public int GetDistance(char from, char to)
        {
            var pos = from == '@' ? Start[_keySection[to]] : _keys[from];
            if (_distances.TryGetValue(pos, out var dist))
            {
                return dist[to];
            }

            var dists = GetDistancesDijkstra(pos).ToDictionary(d => d.Key, d => d.Distance);
            _distances.Add(pos, dists);
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

            return _keys.Where(kv => dist.ContainsKey(kv.Value))
                .Select(kv => (kv.Key, dist[kv.Value]));
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

            if (!lines.Any(l => l.Contains("@#@"))) // skip if already modified
            {
                var midLine = lines.Select((l, i) => (l, i)).First(line => line.l.Contains(".@."));
                var x = midLine.l.IndexOf('@');
                var y = midLine.i;

                lines[y - 1] = lines[y - 1].Remove(x - 1, 3).Insert(x - 1, "@#@");
                lines[y] = lines[y].Remove(x - 1, 3).Insert(x - 1, "###");
                lines[y + 1] = lines[y + 1].Remove(x - 1, 3).Insert(x - 1, "@#@");
            }

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
