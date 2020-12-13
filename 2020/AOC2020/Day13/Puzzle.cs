using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC2020.Day13
{
    public class Puzzle : PuzzleBase<long, long>
    {
        public Puzzle(IEnumerable<string> input)
        {
            var lines = input.ToArray();
            if (!long.TryParse(lines[0], out _timestamp))
            {
                throw new PuzzleException($"Invalid input: {lines[0]}");
            }

            var split = lines[1].Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            _ids = split.Where(e => e != "x")
                .Select(e => int.Parse(e))
                .ToArray();

            _idsOffset = split.Select((e, i) => (e, i))
                .Where(p => p.e != "x")
                .Select(p => (int.Parse(p.e), p.i))
                .ToArray();
        }

        private readonly long _timestamp;
        private readonly int[] _ids;
        private readonly (int Id, int Offset)[] _idsOffset;

        public override long Solution1()
        {
            var (id, time) = _ids.Select(id => (id, id - _timestamp % id))
                .OrderBy(p => p.Item2)
                .First();
            return id * time;
        }

        private static long InvMod(long a, long n)
        {
            var b = a % n;
            for (var x = 1; x < n; x++)
            {
                if (Utils.Modulo(b * x, n) == 1)
                {
                    return x;
                }
            }
            return 1;
        }

        public override long Solution2()
        {
            (int a, int n)[] aMod = _idsOffset
                .Select(p => (Utils.Modulo(0 - p.Offset, p.Id), p.Id))
                .ToArray();

            var N = aMod.Aggregate(1L, (s, p) => s * p.n);

            //foreach(var (a, n) in aMod)
            //{
            //    Console.WriteLine($"t ≡ {a} (mod {n})");
            //}
            //Console.WriteLine($"N = {N}");

            var result = 0L;
            foreach (var (a, n) in aMod)
            {
                var b = N / n;
                result += a * b * InvMod(b, n);
            }
            result = Utils.Modulo(result, N);

            return result;
        }
    }
}
