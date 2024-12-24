using System.Text.RegularExpressions;

namespace AOC2024.Puzzles;

public partial class Day24 : CustomBaseProblem<long>
{
    private readonly Dictionary<string, INode> _nodes;

    public Day24()
    {
        _nodes = ParseInput(ReadLinesFromFile());
    }

    public Day24(IEnumerable<string> lines)
    {
        _nodes = ParseInput(lines);
    }


    [GeneratedRegex(@"^(?<name>.+): (?<value>[01])$")]
    private static partial Regex ResolvePatternGenerator();
    private static readonly Regex ResolvedPattern = ResolvePatternGenerator();

    [GeneratedRegex(@"^(?<name1>.+) (?<op>AND|OR|XOR) (?<name2>.+) -> (?<name3>.+)$")]
    private static partial Regex OperationPatternGenerator();
    private static readonly Regex OperationPattern = OperationPatternGenerator();

    private static Dictionary<string, INode> ParseInput(IEnumerable<string> lines)
    {
        var nodes = new Dictionary<string, INode>();
        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            var mR = ResolvedPattern.Match(line);
            if (mR.Success)
            {
                var val = mR.Groups["value"].Value == "1" ? NodeValue.True : NodeValue.False;
                var node = new ResolvedNode(mR.Groups["name"].Value, val);
                nodes[node.Name] = node;

                continue;
            }

            var mOp = OperationPattern.Match(line);
            if (mOp.Success)
            {
                var node1 = mOp.Groups["name1"].Value;
                var node2 = mOp.Groups["name2"].Value;
                var node3 = mOp.Groups["name3"].Value;
                var op = Enum.Parse<Operation>(mOp.Groups["op"].Value, true);

                nodes[node3] = new NotResolvedNode(node3, op, node1, node2);

                continue;
            }

            throw new InvalidOperationException("Invalid line format");
        }

        return nodes;
    }

    private static NodeValue GetNodeValue(string name, Dictionary<string, INode> _nodes)
    {
        if (_nodes[name] is ResolvedNode r)
        {
            return r.Value;
        }

        var node = (NotResolvedNode)_nodes[name];
        var val = node.Op switch
        {
            Operation.And => ResolveAnd(node.A, node.B, _nodes),
            Operation.Or => ResolveOr(node.A, node.B, _nodes),
            Operation.Xor => ResolveXor(node.A, node.B, _nodes),
            _ => throw new InvalidOperationException(),
        };

        _nodes[name] = new ResolvedNode(name, val);
        return val;
    }

    private static NodeValue ResolveOr(string nodeA, string nodeB, Dictionary<string, INode> _nodes) =>
        GetNodeValue(nodeA, _nodes) == NodeValue.True ? NodeValue.True : GetNodeValue(nodeB, _nodes);

    private static NodeValue ResolveAnd(string nodeA, string nodeB, Dictionary<string, INode> _nodes) =>
        GetNodeValue(nodeA, _nodes) == NodeValue.False ? NodeValue.False : GetNodeValue(nodeB, _nodes);

    private static NodeValue ResolveXor(string nodeA, string nodeB, Dictionary<string, INode> _nodes) =>
        GetNodeValue(nodeA, _nodes) != GetNodeValue(nodeB, _nodes) ? NodeValue.True : NodeValue.False;

    private static long BuildValue(NodeValue[] values)
    {
        var sum = 0L;
        foreach (var v in values)
        {
            sum = (sum * 2) + (v == NodeValue.True ? 1 : 0);
        }

        return sum;
    }

    public override long Solve1()
    {
        var nodes = new Dictionary<string, INode>(_nodes);
        var zNodes = nodes.Keys.Where(static k => k.StartsWith('z')).Order().Reverse();
        var zValues = zNodes.Select(z => GetNodeValue(z, nodes)).ToArray();
        return BuildValue(zValues);
    }

    public override long Solve2()
    {
        return default;
    }

    private interface INode
    {
        string Name { get; }
    }

    private readonly record struct ResolvedNode(string Name, NodeValue Value) : INode
    {
    }

    private readonly record struct NotResolvedNode(string Name, Operation Op, string A, string B) : INode
    {
    }

    private enum NodeValue : byte
    {
        NotResolved = 0,
        False,
        True,
    }

    private enum Operation
    {
        None = 0,
        And,
        Or,
        Xor,
    }
}
