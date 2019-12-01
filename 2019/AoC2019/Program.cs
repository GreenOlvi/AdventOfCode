using System;
using System.Collections.Generic;
using System.IO;

namespace AoC2019
{
    internal static class Program
    {
        private static readonly IDictionary<int, Func<string, IPuzzle>> Puzzles = new Dictionary<int, Func<string, IPuzzle>>()
        {
            { 1, f => new Puzzle01.Solution(ReadLines(f)) },
        };

        internal static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2019");

            var file = "input/01.txt";
            var p01 = new Puzzle01.Solution(ReadLines(file));

            var result1 = p01.Solve1Async().Result;
            Console.WriteLine($"Result 1 = {result1}");

            var result2 = p01.Solve2Async().Result;
            Console.WriteLine($"Result 2 = {result2}");

            Console.ReadLine();
        }

        private static IEnumerable<string> ReadLines(string fileName)
        {
            using var reader = new StreamReader(fileName);
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (line != null)
                {
                    yield return line;
                }
            }
        }
    }
}
