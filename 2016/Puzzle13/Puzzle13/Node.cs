using System;
using System.Collections.Generic;

namespace Puzzle13
{
    public class Node
    {
        private Node _parentNode;

        public Node(Point location, bool isWalkable, Point endLocation) : this(location, isWalkable)
        {
            H = GetTraversalCost(Location, endLocation);
            G = 0;
        }

        public Node(Point location, bool isWalkable)
        {
            Location = location;
            State = NodeState.Untested;
            IsWalkable = isWalkable;
        }

        public Point Location { get; }
        public bool IsWalkable { get; }
        public float G { get; private set; }
        public float H { get; private set; }
        public float F => G + H;
        public NodeState State { get; set; }

        public Node ParentNode
        {
            get { return _parentNode; }
            set
            {
                _parentNode = value;
                G = _parentNode.G + GetTraversalCost(Location, _parentNode.Location);
            }
        }

        public List<Point> GetPath()
        {
            var path = new List<Point>();
            var node = this;
            while (node.ParentNode != null)
            {
                path.Add(node.Location);
                node = node.ParentNode;
            }
            return path;
        }

        public int GetPathLength()
        {
            var length = 0;
            var node = this;
            while (node.ParentNode != null)
            {
                length++;
                node = node.ParentNode;
            }
            return length;
        }

        public override string ToString()
        {
            return String.Format(@"{0}, {1}: {2}", Location.X, Location.Y, State);
        }


        internal static float GetTraversalCost(Point location, Point otherLocation)
        {
            return GetTraversalPlain(location, otherLocation);
        }

        private static float GetTraversalPlain(Point location, Point otherLocation)
        {
            float deltaX = otherLocation.X - location.X;
            float deltaY = otherLocation.Y - location.Y;
            return (float) Math.Sqrt(deltaX*deltaX + deltaY*deltaY);
        }

        private static float GetTraversalMetropolitan(Point location, Point otherLocation)
        {
            int deltaX = otherLocation.X - location.X;
            int deltaY = otherLocation.Y - location.Y;
            return deltaX + deltaY;
        }
    }
}