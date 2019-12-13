using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC2019.Puzzle12
{
    public class Solution : IPuzzle
    {
        public Solution(string input)
        {
            _moons = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(l => ParseVector(l))
                .ToArray();
        }

        private static readonly Regex VectorRegex = new Regex(@"^<x=(?<x>-?\d+), y=(?<y>-?\d+), z=(?<z>-?\d+)>$", RegexOptions.Compiled);
        public static Vector3 ParseVector(string line)
        {
            var match = VectorRegex.Match(line);
            if (!match.Success)
            {
                throw new ArgumentException("Invalid input format", nameof(line));
            }

            var x = int.Parse(match.Groups["x"].Value);
            var y = int.Parse(match.Groups["y"].Value);
            var z = int.Parse(match.Groups["z"].Value);
            return new Vector3(x, y, z);
        }

        private readonly Vector3[] _moons;

        private static Vector3[] ZeroVectors =>
            new[] { Vector3.Zero, Vector3.Zero, Vector3.Zero, Vector3.Zero };

        private static IEnumerable<Vector3> SumVectors(IEnumerable<Vector3> left, IEnumerable<Vector3> right) =>
            left.Zip(right).Select(t => t.First.Add(t.Second));

        public static (Vector3[] pos, Vector3[] vel) Step(Vector3[] pos, Vector3[] vel)
        {
            var xs = pos.Select(m => m.X).ToArray();
            var ys = pos.Select(m => m.Y).ToArray();
            var zs = pos.Select(m => m.Z).ToArray();

            var grav = pos.Select(m =>
            {
                var gx = xs.Select(x => Math.Sign(x - m.X)).Sum();
                var gy = ys.Select(y => Math.Sign(y - m.Y)).Sum();
                var gz = zs.Select(z => Math.Sign(z - m.Z)).Sum();
                return new Vector3(gx, gy, gz);
            });

            var newVel = SumVectors(vel, grav).ToArray();
            var newPos = SumVectors(pos, newVel).ToArray();
            return (newPos, newVel);
        }

        public static (Vector3[] pos, Vector3[] vel) Steps(Vector3[] pos, Vector3[] vel, int steps) =>
            Enumerable.Range(0, steps)
                .Aggregate((pos, vel), (t, _) => Step(t.pos, t.vel));

        public static int Energy(Vector3 vec) =>
            Math.Abs(vec.X) + Math.Abs(vec.Y) + Math.Abs(vec.Z);

        public static int TotalEnergy(IEnumerable<Vector3> pos, IEnumerable<Vector3> vel) =>
            pos.Zip(vel).Select(m => Energy(m.First) * Energy(m.Second)).Sum();

        private static int Solve1(Vector3[] moons, int steps)
        {
            var (p, v) = Steps(moons, ZeroVectors, steps);
            return TotalEnergy(p, v);
        }

        public static ulong HashCode(int[] ints)
        {
            ulong result = 0;
            foreach (var i in ints)
            {
                result <<= 8;
                result |= (byte)i;
            }

            return result;
        }

        private static ulong GetHashCodeX((Vector3[] pos, Vector3[] vel) state)
        {
            return HashCode(new[] {
                state.pos[0].X, state.vel[0].X,
                state.pos[1].X, state.vel[1].X,
                state.pos[2].X, state.vel[2].X,
                state.pos[3].X, state.vel[3].X });
        }

        private static ulong GetHashCodeY((Vector3[] pos, Vector3[] vel) state)
        {
            return HashCode(new[] {
                state.pos[0].Y, state.vel[0].Y,
                state.pos[1].Y, state.vel[1].Y,
                state.pos[2].Y, state.vel[2].Y,
                state.pos[3].Y, state.vel[3].Y });
        }

        private static ulong GetHashCodeZ((Vector3[] pos, Vector3[] vel) state)
        {
            return HashCode(new[] {
                state.pos[0].Z, state.vel[0].Z,
                state.pos[1].Z, state.vel[1].Z,
                state.pos[2].Z, state.vel[2].Z,
                state.pos[3].Z, state.vel[3].Z });
        }

        private static long GCD(long a, long b)
        {
            while (a != b)
            {
                if (a % b == 0) return b;
                if (b % a == 0) return a;
                if (a > b)
                    a -= b;
                if (b > a)
                    b -= a;
            }
            return a;
        }

        private static long LCM(long a, long b) =>
            Math.Abs(a * b) / GCD(a, b);

        public static long Solve2(Vector3[] moons)
        {
            var cycleX = new CycleFinder<(Vector3[], Vector3[]), ulong>(GetHashCodeX);
            var cycleY = new CycleFinder<(Vector3[], Vector3[]), ulong>(GetHashCodeY);
            var cycleZ = new CycleFinder<(Vector3[], Vector3[]), ulong>(GetHashCodeZ);

            var state = (pos: moons, vel: ZeroVectors);

            while (!(cycleX.Found && cycleY.Found && cycleZ.Found))
            {
                cycleX.Add(state);
                cycleY.Add(state);
                cycleZ.Add(state);
                state = Step(state.pos, state.vel);
            }

            return LCM(LCM(cycleX.CycleLength, cycleY.CycleLength), cycleZ.CycleLength);
        }

        public Task<string> Solve1Async() =>
            Task.Run(() => Solve1(_moons, 1000).ToString());

        public Task<string> Solve2Async() =>
            Task.Run(() => Solve2(_moons).ToString());
    }
}
