using System;
using System.Collections.Generic;
using System.Linq;
using AOC2020.Common;

namespace AOC2020.Day17
{
    public class Vector
    {
        public Vector(params int[] coords)
        {
            N = coords.Length;
            _coords = coords;
        }

        public Vector(int n, Point point)
        {
            N = n;
            if (n < 2)
                throw new InvalidOperationException("Dimensions should be at least 2");

            _coords = new[] { point.X, point.Y }
                .Concat(Enumerable.Range(0, N - 2).Select(i => 0))
                .ToArray();
        }

        public Vector(IEnumerable<int> coords) : this(coords.ToArray())
        {
        }

        private readonly int[] _coords;

        public int N { get; }
        public IReadOnlyCollection<int> Coords => Array.AsReadOnly(_coords);

        private Vector Op(Vector p, Func<int, int, int> op)
        {
            if (p.N != N)
            {
                throw new InvalidOperationException("Point dimensions have to be equal");
            }
            return new Vector(Coords.Zip(p.Coords, op));
        }

        public Vector Add(Vector p) => Op(p, (a, b) => a + b);

        public static Vector operator +(Vector a, Vector b) => a.Add(b);
        public static Vector operator -(Vector a, Vector b) => a.Add(-1 * b);
        public static Vector operator *(Vector a, int b) => new Vector(a.Coords.Select(c => b * c));
        public static Vector operator *(int b, Vector a) => new Vector(a.Coords.Select(c => b * c));
        public static bool operator ==(Vector a, Vector b) => a.Equals(b);
        public static bool operator !=(Vector a, Vector b) => !a.Equals(b);

        public int this[int index] => _coords[index];

        public static Vector Zero(int n) => new Vector(Enumerable.Range(0, n).Select(n => 0));
        public static Vector One(int n) => new Vector(Enumerable.Range(0, n).Select(n => 1));

        public override string ToString() => string.Join(",", Coords.Select((c, i) => $"[{i}]={c}"));

        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            var other = (Vector)obj;
            return N == other.N && _coords.SequenceEqual(other._coords);
        }

        public override int GetHashCode() =>
            _coords.Aggregate(N.GetHashCode(), (a, b) => HashCode.Combine(a, b));
    }
}
