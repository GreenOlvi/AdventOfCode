using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace AoC2019
{
    internal static class Program
    {
        private static readonly IDictionary<int, Func<string, IPuzzle>> Puzzles = new Dictionary<int, Func<string, IPuzzle>>()
        {
            { 1, f => new Puzzle01.Solution(ReadLines(f)) },
            { 2, f => new Puzzle02.Solution(File.ReadAllText(f)) },
            { 3, f => new Puzzle03.Solution(ReadLines(f)) },
        };

        internal static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2019");

            RunPuzzle(3).Wait();

            Console.ReadLine();
        }

        private static async Task RunPuzzle(int id)
        {
            Console.WriteLine($"Running solution for Puzzle {id:d2}");

            var file = GetInput(id);
            Console.WriteLine($"Reading from {file}");

            var puzzle = Puzzles[id](file);

            var stopwatch = new Stopwatch();

            Console.WriteLine();
            Console.WriteLine("Calculating 1...");
            stopwatch.Start();
            var result1 = await puzzle.Solve1Async();
            stopwatch.Stop();
            Console.WriteLine($"Result 1 = {result1}");
            Console.WriteLine($"Took {stopwatch.Elapsed}");

            try
            {
                Console.WriteLine();
                Console.WriteLine("Calculating 2...");
                stopwatch.Restart();
                var result2 = await puzzle.Solve2Async();
                stopwatch.Stop();
                Console.WriteLine($"Result 2 = {result2}");
                Console.WriteLine($"Took {stopwatch.Elapsed}");
            }
            catch (AggregateException e)
            {
                Console.WriteLine(e.Flatten().ToString());
            }
        }

        private static string GetInput(int id)
        {
            return $"input/{id:d2}.txt";
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
