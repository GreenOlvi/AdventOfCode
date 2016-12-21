using System.Collections.Generic;
using System.Linq;

namespace Puzzle13
{
    public class PathFinder
    {
        private readonly Dictionary<string, Node> _nodes = new Dictionary<string, Node>();

        public PathFinder(Maze maze)
        {
            Maze = maze;
        }

        public Maze Maze { get; }
        private Point StartLocation { get; set; }
        private Point EndLocation { get; set; }

        public List<Point> FindPath(Point startLocation, Point endLocation)
        {
            StartLocation = startLocation;
            EndLocation = endLocation;
            var path = new List<Point>();
            var startNode = GetNode(StartLocation);
            startNode.State = NodeState.Open;

            var success = SearchBfs(startNode);
            if (success)
            {
                path = GetNode(EndLocation).GetPath();
                path.Reverse();
            }

            return path;
        }

        private bool Search(Node currentNode)
        {
            currentNode.State = NodeState.Closed;
            var nextNodes = GetAdjacentWalkableNodes(currentNode).OrderBy(n => n.F);

            foreach (var nextNode in nextNodes)
            {
                if (nextNode.Location.Equals(EndLocation))
                    return true;

                if (Search(nextNode))
                    return true;
            }

            return false;
        }

        private bool SearchBfs(Node startNode)
        {
            var pQueue = new PriorityQueue<Node>(n => n.F);
            pQueue.Enqueue(startNode);
            var success = false;

            while (pQueue.Any())
            {
                var nextNode = pQueue.Dequeue();

                if (nextNode.Location.Equals(EndLocation))
                    success = true;

                nextNode.State = NodeState.Closed;

                pQueue.EnqueueRange(GetAdjacentWalkableNodes(nextNode));
            }

            return success;
        }

        private Node GetNode(Point location)
        {
            var key = location.ToShortString();
            if (!_nodes.ContainsKey(key))
            {
                _nodes.Add(key, new Node(location, Maze.IsWalkable(location), EndLocation));
            }

            return _nodes[key];
        }

        private IEnumerable<Node> GetAdjacentWalkableNodes(Node fromNode)
        {
            var nextLocations = Maze.GetAdjacentLocations(fromNode.Location);
            foreach (var node in nextLocations.Select(location => GetNode(location))
                .Where(node => node.IsWalkable && node.State != NodeState.Closed))
            {
                if (node.State == NodeState.Open)
                {
                    var traversalCost = Node.GetTraversalCost(node.Location, node.ParentNode.Location);
                    var gTemp = fromNode.G + traversalCost;
                    if (gTemp < node.G)
                    {
                        node.ParentNode = fromNode;
                        yield return node;
                    }
                }
                else
                {
                    node.ParentNode = fromNode;
                    node.State = NodeState.Open;
                    yield return node;
                }
            }
        }
    }
}