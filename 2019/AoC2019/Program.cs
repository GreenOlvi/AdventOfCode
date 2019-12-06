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
            { 1, f => new Puzzle01.Solution(File.ReadLines(f)) },
            { 2, f => new Puzzle02.Solution(File.ReadAllText(f)) },
            { 3, f => new Puzzle03.Solution(File.ReadLines(f)) },
            { 4, f => new Puzzle04.Solution(File.ReadAllText(f)) },
            { 5, f => new Puzzle05.Solution(File.ReadAllText(f)) },
            { 6, f => new Puzzle06.Solution(File.ReadLines(f)) },
        };

        internal static void Main(string[] args)
        {
            Console.WriteLine("Advent of Code 2019");

            RunPuzzle(6).Wait();

            Console.ReadLine();
        }

        private static async Task RunPuzzle(int id)
        {
            Console.WriteLine($"Running solution for Puzzle {id:d2}");

            var file = GetInput(id);
            Console.WriteLine($"Reading from {file}");

            var puzzle = Puzzles[id](file);
            await Run(1, () => puzzle.Solve1Async());
            await Run(2, () => puzzle.Solve2Async());
        }

        private static async Task Run(int part, Func<Task<string>> solve)
        {
            Console.WriteLine();
            Console.WriteLine($"Calculating {part}...");

            var stopwatch = Stopwatch.StartNew();
            var result = await solve();
            stopwatch.Stop();

            Console.WriteLine($"Result {part} = {result}");
            Console.WriteLine($"Took {stopwatch.Elapsed}");
        }

        private static string GetInput(int id)
        {
            return $"input/{id:d2}.txt";
        }
    }
}
