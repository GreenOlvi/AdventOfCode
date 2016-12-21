using System.Collections.Generic;
using System.Linq;

namespace Puzzle13
{
    public class Walker
    {
        private readonly Dictionary<string, Node> _nodes = new Dictionary<string, Node>();

        public Walker(Maze maze)
        {
            Maze = maze;
        }

        public Maze Maze { get; }

        private Dictionary<Point, int> SeenLocations; 

        public int WalkedInRadius(Point location, int radius)
        {
            SeenLocations = new Dictionary<Point, int>()
            {
                {location, 0},
            };
            Visit(location, radius);
            return SeenLocations.Count;
        }

        private void Visit(Point currentLocation, int radius)
        {
            var queue = new Queue<Point>();
            queue.Enqueue(currentLocation);

            while (queue.Any())
            {
                var l = queue.Dequeue();

                var dist = SeenLocations[l];
                if (dist >= radius)
                    continue;

                foreach (var loc in GetAdjacentNotSeenLocations(l))
                {
                    SeenLocations.Add(loc, dist + 1);
                    queue.Enqueue(loc);
                }
            }
        }

        public override string ToString()
        {
            var path = SeenLocations.Keys;
            return Maze.ToString(path);
        }

        //private void Visit2(Point currentLocation, int radius)
        //{
        //    if (radius <= 0)
        //    {
        //        return;
        //    }

        //    var adjacent = GetAdjacentNotSeenLocations(currentLocation).ToArray();
        //    foreach (var loc in adjacent)
        //    {
        //        SeenLocations.Add(loc.ToShortString());
        //        Visit2(loc, radius - 1);
        //    }
        //}

        private IEnumerable<Point> GetAdjacentNotSeenLocations(Point location)
        {
            var locs = Maze.GetAdjacentLocations(location).ToArray();
            return locs.Where(l => Maze.IsWalkable(l) && !SeenLocations.ContainsKey(l));
        }

        private IEnumerable<Node> GetAdjacentWalkableNodes(Node fromNode, int stepsLeft)
        {
            if (stepsLeft <= 0)
                yield break;
            ;
            var adjacentNodes = Maze.GetAdjacentLocations(fromNode.Location)
                .Select(l => GetNode(l))
                .Where(n => n.IsWalkable);

            foreach (var node in adjacentNodes)
            {
                if (node.Visited)
                {
                    if (node.StepsLeft < stepsLeft)
                    {
                        node.StepsLeft = stepsLeft;
                        yield return node;
                    } 
                }
                else
                {
                    node.Visited = true;
                    yield return node;
                }
            }
        }

        private Node GetNode(Point location)
        {
            var key = location.ToShortString();
            if (!_nodes.ContainsKey(key))
            {
                _nodes.Add(key, new Node(location, Maze.IsWalkable(location)));
            }
            return _nodes[key];
        }

        private class Node
        {
            public Node(Point location, bool isWalkable)
            {
                Location = location;
                IsWalkable = isWalkable;
                Visited = false;
                StepsLeft = 0;
            }

            public Point Location { get; }
            public bool IsWalkable { get; }
            public bool Visited { get; set; }
            public int StepsLeft { get; set; }
        }
    }
}
