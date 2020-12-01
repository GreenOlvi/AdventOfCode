using System.IO;
using System.Linq;
using BenchmarkDotNet.Attributes;
using AOC2020.Puzzle01;

namespace AOC2020Benchmarks
{
    [SimpleJob(launchCount: 3, warmupCount: 10, targetCount: 30)]
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    public class Puzzle01Benchmark
    {
        public Puzzle01Benchmark()
        {
            _input = File.ReadAllLines(Path.Combine(Program.InputPath, "01.txt"))
                .Select(line => int.Parse(line))
                .ToArray();
            _p01 = new P01(_input);
            _p01a = new P01a(_input);
        }

        private const int Sum = 2020;

        private readonly int[] _input;
        private readonly P01 _p01;
        private readonly P01a _p01a;

        [Benchmark]
        public (int, int)[] P01GetPairs() => _p01.GetPairs(Sum).ToArray();

        [Benchmark]
        public (int, int)[] P01aGetPairs() => _p01a.GetPairs().ToArray();

        [Benchmark]
        public (int, int, int)[] P01GetThrees() => _p01.GetThrees(Sum).ToArray();

        [Benchmark]
        public (int, int, int)[] P01aGetThrees() => _p01a.GetThrees().ToArray();

    }
}
