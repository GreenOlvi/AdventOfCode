using System;
using System.Collections.Generic;
using BenchmarkDotNet.Running;

namespace AOC2020Benchmarks
{
    internal class Program
    {
        internal const string InputPath = "input";

        private static readonly Dictionary<int, Action[]> _benchmarks = new Dictionary<int, Action[]>
        {
            { 1, new Action[] { () => BenchmarkRunner.Run<Puzzle01Benchmark>() } },
        };

        internal static int Main(string[] args)
        {
            if (!TryParseArgs(args, out var day))
            {
                return -1;
            }

            if (!_benchmarks.ContainsKey(day))
            {
                Console.WriteLine($"No day {day:00} puzzle benchmark defined");
                return -1;
            }

            foreach (var b in _benchmarks[day])
            {
                b();
            }

            return 0;
        }

        private static bool TryParseArgs(string[] args, out int day)
        {
            day = 0;

            if (args.Length == 0)
            {
                Console.WriteLine("Day number is required");
                return false;
            }

            if (!int.TryParse(args[0], out var d) || d < 1 || d > 25)
            {
                Console.WriteLine("Day should be a number between [1-25]");
                return false;
            }

            if (args.Length == 1)
            {
                day = d;
                return true;
            }

            return false;
        }

    }
}
