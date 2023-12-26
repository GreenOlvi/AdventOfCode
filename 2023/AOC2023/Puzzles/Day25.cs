
namespace AOC2023.Puzzles;
public class Day25 : CustomBaseDay
{
    private readonly (string, string)[] _edges;

    public Day25()
    {
        _edges = ReadLinesFromFile().SelectMany(ParseLine).ToArray();
    }

    public Day25(IEnumerable<string> lines)
    {
        _edges = lines.SelectMany(ParseLine).ToArray();
    }

    private static readonly char[] separator = [':', ' '];
    private IEnumerable<(string, string)> ParseLine(string line)
    {
        var nodes = line.Split(separator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        foreach (var n in nodes.Skip(1))
        {
            yield return (nodes[0], n);
        }
    }

    public override ValueTask<string> Solve_1()
    {
        var g = new Graph();
        g.AddEdgesRange(_edges);

        //var s = g.ToDot();

        // Example
        //g.RemoveEdge(("cmg", "bvb"));
        //g.RemoveEdge(("nvd", "jqt"));
        //g.RemoveEdge(("pzl", "hfx"));

        // Input
        g.RemoveEdge(("zhb", "vxr"));
        g.RemoveEdge(("jbx", "sml"));
        g.RemoveEdge(("szh", "vqj"));

        var counts = g.CountSubgraphs().ToArray();

        if (counts.Length != 2)
        {
            throw new InvalidOperationException("Supposed to be two subgraphs");
        }

        return (counts[0] * counts[1]).ToResult();
    }

    public override ValueTask<string> Solve_2() => string.Empty.ToResult();

    private class Graph
    {
        private readonly List<(string, string)> _edges = [];

        public void AddEdge((string, string) edge)
        {
            _edges.Add(edge);
        }

        public void AddEdgesRange(IEnumerable<(string, string)> edges)
        {
            _edges.AddRange(edges);
        }

        public void RemoveEdge((string, string) edge)
        {
            _edges.Remove(edge);
            _edges.Remove((edge.Item2, edge.Item1));
        }

        public IEnumerable<long> CountSubgraphs()
        {
            var allNodes = _edges.Select(e => e.Item1)
                .Concat(_edges.Select(e => e.Item2))
                .ToHashSet();

            var neighbours = _edges.Concat(_edges.Select(e => (e.Item2, e.Item1)))
                .GroupBy(e => e.Item1)
                .ToDictionary(e => e.Key, e => e.Select(e => e.Item2).ToArray());

            var queue = new Queue<string>();
            var subgraph = new HashSet<string>();

            while (allNodes.Count > 0)
            {
                queue.Clear();
                queue.Enqueue(allNodes.First());
                subgraph.Clear();

                while (queue.Count > 0)
                {
                    var n = queue.Dequeue();
                    if (subgraph.Contains(n))
                    {
                        continue;
                    }
                    subgraph.Add(n);
                    allNodes.Remove(n);

                    var nei = neighbours[n].Where(ne => !subgraph.Contains(ne));
                    foreach(var neighbour in nei)
                    {
                        queue.Enqueue(neighbour);
                    }
                }

                yield return subgraph.Count;
            }
        }

        public string ToDot()
        {
            var sb = new StringBuilder();
            sb.AppendLine("graph {");
            foreach(var e in _edges)
            {
                sb.AppendLine($"\t{e.Item1} -- {e.Item2}");
            }
            sb.AppendLine("}");
            return sb.ToString();
        }
    }
}
