
namespace AOC2025.Puzzles;

public class Day08 : CustomBaseProblem<long>
{
    private readonly Point3[] _input;

    public int Connections { get; init; } = 1000;

    public Day08()
    {
        _input = [.. ParseInput(ReadLinesFromFile())];
    }

    public Day08(IEnumerable<string> lines)
    {
        _input = [.. ParseInput(lines)];
    }

    private static IEnumerable<Point3> ParseInput(IEnumerable<string> lines)
    {
        foreach (var line in lines)
        {
            var s = line.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            yield return new Point3(long.Parse(s[0]), long.Parse(s[1]), long.Parse(s[2]));
        }
    }

    public override long Solve1()
    {
        var connections = _input.EachPair()
            .Select(p => (p, p.Item1.DistanceSquaredTo(p.Item2)))
            .OrderBy(d => d.Item2)
            .Select(p => p.p)
            .Take(Connections);

        var circuits = new HashSet<Circuit>();
        foreach (var conn in connections)
        {
            var c1 = circuits.SingleOrDefault(c => c.HasNode(conn.Item1));
            var c2 = circuits.SingleOrDefault(c => c.HasNode(conn.Item2));

            if (c1 is not null && c2 is not null)
            {
                if (c1.Id == c2.Id)
                {
                    c1.AddEdge(conn);
                }
                else
                {
                    circuits.Remove(c1);
                    circuits.Remove(c2);
                    circuits.Add(c1.Merge(c2));
                }
                continue;
            }

            if (c1 is not null && c2 is null)
            {
                c1.AddEdge(conn);
                continue;
            }

            if (c1 is null && c2 is not null)
            {
                c2.AddEdge(conn);
                continue;
            }

            var g = new Circuit([conn]);
            circuits.Add(g);
        }

        return circuits.Select(c => c.Nodes.Count)
            .OrderByDescending(c => c)
            .Take(3)
            .Product();
    }

    public override long Solve2()
    {
        var allNodes = _input.Length;
        var connections = _input.EachPair()
            .Select(p => (p, p.Item1.DistanceSquaredTo(p.Item2)))
            .OrderBy(d => d.Item2)
            .Select(p => p.p);

        (Point3, Point3)? lastConnection = null;
        var circuits = new HashSet<Circuit>();
        foreach (var conn in connections)
        {
            var c1 = circuits.SingleOrDefault(c => c.HasNode(conn.Item1));
            var c2 = circuits.SingleOrDefault(c => c.HasNode(conn.Item2));

            if (c1 is not null && c2 is not null)
            {
                if (c1.Id == c2.Id)
                {
                    c1.AddEdge(conn);
                }
                else
                {
                    circuits.Remove(c1);
                    circuits.Remove(c2);
                    circuits.Add(c1.Merge(c2));

                }
            }

            if (c1 is not null && c2 is null)
            {
                c1.AddEdge(conn);
            }

            if (c1 is null && c2 is not null)
            {
                c2.AddEdge(conn);
            }

            if (c1 is null && c2 is null)
            {
                var g = new Circuit([conn]);
                circuits.Add(g);
            }

            if (circuits.Count == 1 && circuits.First().Nodes.Count == allNodes)
            {
                lastConnection = conn;
                break;
            }
        }

        return lastConnection!.Value.Item1.X * lastConnection!.Value.Item2.X;
    }

    private class Circuit
    {
        public Guid Id = Guid.NewGuid();

        public IReadOnlyCollection<Point3> Nodes => _nodes;

        private readonly HashSet<Point3> _nodes = [];

        public Circuit()
        {
        }

        public Circuit(IEnumerable<Point3> nodes)
        {
            foreach (var n in nodes)
            {
                _nodes.Add(n);
            }
        }

        public Circuit(IEnumerable<(Point3, Point3)> edges)
        {
            AddEdgeRange(edges);
        }

        public void AddEdgeRange(IEnumerable<(Point3, Point3)> edges)
        {
            foreach (var e in edges)
            {
                AddEdge(e.Item1, e.Item2);
            }
        }

        public void AddEdge((Point3, Point3) e) => AddEdge(e.Item1, e.Item2);

        public void AddEdge(Point3 a, Point3 b)
        {
            _nodes.Add(a);
            _nodes.Add(b);
        }

        public bool HasNode(Point3 a) => _nodes.Contains(a);

        public Circuit Merge(Circuit g) => new(_nodes.Concat(g._nodes));
    }
}
