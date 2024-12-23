namespace AOC2024.Puzzles;

public class Day23 : CustomBaseProblem<long, string>
{
    private readonly Graph _graph;

    public Day23()
    {
        _graph = ParseInput(ReadLinesFromFile());
    }

    public Day23(IEnumerable<string> lines)
    {
        _graph = ParseInput(lines);
    }

    private static Graph ParseInput(IEnumerable<string> lines)
    {
        var graph = new Graph();
        foreach (var line in lines)
        {
            var n = line.Split('-', StringSplitOptions.TrimEntries);

            graph.AddEdge(n[0], n[1]);
        }

        return graph;
    }

    private static string BuildPassword(params IEnumerable<string> nodes) =>
        string.Join(',', nodes.Order());

    private static string FindLargestClique(Graph graph, IEnumerable<string> clique, IEnumerable<string> other, HashSet<string> seen)
    {
        var pass = BuildPassword(clique);
        if (seen.Contains(pass))
        {
            return pass;
        }

        _ = seen.Add(pass);

        foreach (var node in other)
        {
            if (!clique.All(c => graph.HasEdge(c, node)))
            {
                continue;
            }

            var p = FindLargestClique(graph, clique.Append(node), other.Where(n => n != node), seen);
            if (p.Length > pass.Length)
            {
                pass = p;
            }
        }

        return pass;
    }

    public override long Solve1()
    {
        var tNodes = _graph.Nodes.Where(static n => n.StartsWith('t')).ToArray();
        var cliques = new HashSet<string>();
        foreach (var node in tNodes)
        {
            var tN = _graph.GetNeighbours(node).ToHashSet();
            foreach (var n in tN)
            {
                var common = _graph.GetNeighbours(n).Where(tN.Contains).ToArray();
                foreach (var c in common)
                {
                    _ = cliques.Add(BuildPassword(node, n, c));
                }
            }
        }
        return cliques.Count;
    }

    public override string Solve2() =>
        FindLargestClique(_graph, [], _graph.Nodes.Order().ToArray(), []);

    private class Graph
    {
        private readonly Dictionary<string, Dictionary<string, bool>> _edges = [];
        private readonly HashSet<string> _nodes = [];

        public IEnumerable<string> Nodes => _nodes;

        public void AddEdge(string a, string b)
        {
            _ = _nodes.Add(a);
            _ = _nodes.Add(b);
            AddSingleEdge(a, b);
            AddSingleEdge(b, a);
        }

        private void AddSingleEdge(string a, string b)
        {
            if (!_edges.TryGetValue(a, out var aEdges))
            {
                aEdges = [];
                _edges[a] = aEdges;
            }
            aEdges[b] = true;
        }

        public bool HasEdge(string a, string b) =>
            _edges.TryGetValue(a, out var aEdges) && aEdges.ContainsKey(b);

        public IEnumerable<string> GetNeighbours(string a) =>
            _edges.TryGetValue(a, out var aEdges) ? aEdges.Keys : ([]);

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var node in _nodes.OrderBy(static n => n))
            {
                sb.AppendFormat("{0}: {1}", node, string.Join(", ", _edges[node].Keys.OrderBy(n => n)));
                _ = sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
