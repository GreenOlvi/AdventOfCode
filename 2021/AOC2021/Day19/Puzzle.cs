using System.Text.RegularExpressions;
using AOC2021.Common;

namespace AOC2021.Day19
{
    public class Puzzle : PuzzleBase<long, long>
    {
        public Puzzle(IEnumerable<string> lines)
        {
            _scanners = ParseScanners(lines).ToArray();
            _orientedScanners = CalculateOrientedScanners(_scanners);
        }

        private static IEnumerable<Scanner> ParseScanners(IEnumerable<string> lines)
        {
            var chunk = new List<string>();
            foreach (var line in lines)
            {
                if (line == string.Empty)
                {
                    yield return ParseScanner(chunk);
                    chunk.Clear();
                }
                else
                {
                    chunk.Add(line);
                }
            }
            yield return ParseScanner(chunk);
        }

        private static Scanner ParseScanner(IEnumerable<string> lines)
        {
            var id = _namePattern.Parse(lines.First(), "id", int.Parse);

            var points = lines.Skip(1)
                .Select(l => _coordsPattern.Parse(l, ("x", int.Parse), ("y", int.Parse), ("z", int.Parse)))
                .Select(c => new Point3(c));

            return new Scanner(id, points);
        }

        private static readonly Regex _namePattern = new(@"--- scanner (?<id>\d+) ---", RegexOptions.Compiled);
        private static readonly Regex _coordsPattern = new(@"(?<x>-?\d+),(?<y>-?\d+),(?<z>-?\d+)", RegexOptions.Compiled);

        private readonly Scanner[] _scanners;
        private readonly Scanner[][] _orientedScanners;

        private static readonly Func<Point3, Point3>[] _orientation = new Func<Point3, Point3>[]
        {
            p => p,                            // +Y up
            p => new Point3(p.Z, p.X, p.Y),    // +X up
            p => new Point3(p.Y, p.Z, p.X),    // +Z up
            p => new Point3(-p.X, -p.Y, -p.Z), // -Y up
            p => new Point3(-p.Z, -p.X, -p.Y), // -X up
            p => new Point3(-p.Y, -p.Z, -p.X), // -Z up
        };

        private static readonly Func<Point3, Point3>[] _rotation = new Func<Point3, Point3>[]
        {
            p => p,
            p => new Point3(p.Z, p.Y, -p.X),
            p => new Point3(-p.X, p.Y, -p.Z),
            p => new Point3(-p.Z, p.Y, p.X),
        };

        private static readonly Func<Point3, Point3>[] _combined =
            _orientation.SelectMany(o => _rotation.Select(r => new Func<Point3, Point3>(p => r(o(p)))))
            .ToArray();

        private static Scanner[][] CalculateOrientedScanners(IEnumerable<Scanner> scanners) =>
            scanners.Select(s => _combined.Select(o => s.Transform(o)).ToArray()).ToArray();

        public override long Solution1()
        {
            var bag = new BeaconBag(_scanners[0].Beacons);
            var scanners = new Queue<Scanner>(_scanners.Skip(1));

            var (success, matching) = TryFindMatch(bag, scanners);
            if (!success)
            {
                throw new Exception("Failed to find match");
            }

            return matching.Count;
        }

        private (bool, BeaconBag) TryFindMatch(BeaconBag bag, Queue<Scanner> scanners)
        {
            var skipped = 0;

            while (scanners.Count > 0)
            {
                if (skipped == scanners.Count)
                {
                    return (false, null);
                }

                var s = scanners.Dequeue();
                Console.WriteLine(s);

                var candidates = new List<(Scanner, Point3, int)>();

                for (var o = 0; o < _combined.Length; o++)
                {
                    var st = _orientedScanners[s.Id][o];
                    var scannerPos = new HashSet<Point3>();

                    var found = false;
                    var foundBeacons = 0;
                    for (var i = 0; i < st.Beacons.Count(); i++)
                    {
                        var dists = st.GetDiffs(i);
                        var matches = bag.GetMatching(dists).ToArray();

                        //if (matches.Length > 1)
                        //{
                        //    Console.WriteLine($"Multiple matches: {matches.Length}");
                        //}

                        if (matches.Length >= 11)
                        {
                            found = true;
                            var beaconAbsolute = matches.First();
                            var scannerAbsolute = beaconAbsolute - st.GetBeacon(i);
                            scannerPos.Add(scannerAbsolute);
                            foundBeacons++;

                            //Console.Write($"\tOrientation {o}, Beacon {i}, ");
                            //Console.Write($"Scanner position: {scannerAbsolute}, ");
                            //Console.WriteLine($"Beacon position: {beaconAbsolute}, ");

                        }
                    }

                    if (found)
                    {
                        candidates.Add((st, scannerPos.Single(), foundBeacons));
                    }
                }

                if (candidates.Count == 0)
                {
                    scanners.Enqueue(s);
                    skipped++;
                }
                else
                {
                    if (candidates.Count == 1)
                    {
                        var (st, abs, _) = candidates.Single();
                        var newBeacons = st.Beacons.Select(b => abs + b).ToArray();
                        bag = bag.AddRange(newBeacons);
                        Console.WriteLine($"Removed. {scanners.Count} left");
                        skipped = 0;
                    }
                    else
                    {
                        var (st, abs, _) = candidates.MaxBy(c => c.Item3);
                        var newBeacons = st.Beacons.Select(b => abs + b).ToArray();
                        bag = bag.AddRange(newBeacons);
                        Console.WriteLine($"Removed. {scanners.Count} left");
                        skipped = 0;
                    }
                }
            }

            return (true, bag);
        }

        public override long Solution2()
        {
            throw new NotImplementedException();
        }
    }

    public class Scanner
    {
        public Scanner(int id, IEnumerable<Point3> beacons)
        {
            Id = id;
            _beacons = beacons.ToArray();
        }

        private readonly Point3[] _beacons;
        private readonly Dictionary<int, HashSet<Point3>> _diffs = new Dictionary<int, HashSet<Point3>>();

        public IEnumerable<Point3> Beacons => _beacons;

        public int Id { get; }

        public Point3 GetBeacon(int id) => _beacons[id];

        public Scanner Transform(Func<Point3, Point3> f) => new(Id, _beacons.Select(f));

        public HashSet<Point3> GetDiffs(int beacon)
        {
            if (_diffs.TryGetValue(beacon, out var diffs))
            {
                return diffs;
            }

            var set = new HashSet<Point3>();
            var b = _beacons[beacon];
            for (var i = 0; i < _beacons.Length; i++)
            {
                if (i == beacon)
                {
                    continue;
                }

                set.Add(_beacons[i] - b);
            }

            _diffs[beacon] = set;
            return set;
        }

        public override string ToString() => $"Scanner {Id} ({_beacons.Length})";
    }

    public class BeaconBag
    {
        public BeaconBag(IEnumerable<Point3> beacons)
        {
            _beacons = beacons.Distinct().ToArray();
            _distances = CalculateDistances(_beacons);
        }

        private const int MaxDistance = 1000;

        private readonly Point3[] _beacons;
        private readonly Dictionary<int, HashSet<Point3>> _distances;

        public int Count => _beacons.Length;

        private static Dictionary<int, HashSet<Point3>> CalculateDistances(Point3[] beacons)
        {
            var distances = new Dictionary<int, HashSet<Point3>>();
            for (var i = 0; i < beacons.Length; i++)
            {
                var set = new HashSet<Point3>();
                for (var j = 0; j < beacons.Length; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }

                    var diff = beacons[j] - beacons[i];
                    if (diff.X <= MaxDistance && diff.X >= -MaxDistance &&
                       diff.Y <= MaxDistance && diff.Y >= -MaxDistance &&
                       diff.Z <= MaxDistance && diff.Z >= -MaxDistance)
                    {
                        set.Add(diff);
                    }
                }
                distances[i] = set;
            }
            return distances;
        }

        public IEnumerable<Point3> GetMatching(HashSet<Point3> dists)
        {
            foreach (var p in _distances)
            {
                var matching = p.Value.Intersect(dists);
                if (matching.Any())
                //if (matching.Count() > 1)
                {
                    yield return _beacons[p.Key];
                }
            }
        }

        public bool Contains(Point3 point) => _beacons.Contains(point);

        public BeaconBag AddRange(IEnumerable<Point3> beacons) => new(_beacons.Concat(beacons));

        public override string ToString() => $"Bag ({_beacons.Length})";
    }
}
