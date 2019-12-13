using System;

namespace AoC2019.Puzzle12
{
    public struct Vector3
    {
        public Vector3(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public int X { get; }
        public int Y { get; }
        public int Z { get; }

        public Vector3 Add(Vector3 vector) =>
            new Vector3(X + vector.X, Y + vector.Y, Z + vector.Z);

        public override string ToString() => $"<{X},{Y},{Z}>";

        public override bool Equals(object? obj) =>
            obj is Vector3 vector &&
                X == vector.X &&
                Y == vector.Y &&
                Z == vector.Z;

        public override int GetHashCode() => HashCode.Combine(X, Y, Z);

        public static bool operator ==(Vector3 left, Vector3 right) =>
            left.Equals(right);

        public static bool operator !=(Vector3 left, Vector3 right) =>
            !(left == right);

        public static Vector3 Zero = new Vector3(0, 0, 0);
    }
}
