using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace AOC2020
{
    internal static class Program
    {
        private const string InputPath = "input";

        private static readonly Dictionary<int, Func<string, IPuzzle>> _puzzles = new Dictionary<int, Func<string, IPuzzle>>
        {
            {  1, i => new Day01.Puzzle(File.ReadAllLines(i).ParseInts()) },
            {  2, i => new Day02.Puzzle(File.ReadAllLines(i)) },
            {  3, i => new Day03.Puzzle(File.ReadAllLines(i)) },
            {  4, i => new Day04.Puzzle(File.ReadAllLines(i)) },
            {  5, i => new Day05.Puzzle(File.ReadAllLines(i)) },
            {  6, i => new Day06.Puzzle(File.ReadAllLines(i)) },
            {  7, i => new Day07.Puzzle(File.ReadAllLines(i)) },
            {  8, i => new Day08.Puzzle(File.ReadAllLines(i)) },
            {  9, i => new Day09.Puzzle(File.ReadAllLines(i).ParseLongs()) },
            { 10, i => new Day10.Puzzle(File.ReadAllLines(i).ParseInts()) },
            { 11, i => new Day11.Puzzle(File.ReadAllLines(i)) },
            { 12, i => new Day12.Puzzle(File.ReadAllLines(i)) },
            { 13, i => new Day13.Puzzle(File.ReadLines(i).Take(2)) },
            { 14, i => new Day14.Puzzle(File.ReadLines(i)) },
            { 15, i => new Day15.Puzzle(File.ReadLines(i).First().Split(",").ParseInts()) },
            { 16, i => new Day16.Puzzle(File.ReadLines(i)) },
            { 17, i => new Day17.Puzzle(File.ReadLines(i)) },
            { 18, i => new Day18.Puzzle(File.ReadLines(i)) },
            { 19, i => new Day19.Puzzle(File.ReadLines(i)) },
            { 20, i => new Day20.Puzzle(File.ReadLines(i)) },
            { 22, i => new Day22.Puzzle(File.ReadLines(i)) },
            { 24, i => new Day24.Puzzle(File.ReadLines(i)) },
        };

        private static readonly TimeSpan ProgressTimerDelay = TimeSpan.FromSeconds(10);

        internal static async Task<int> Main(string[] args)
        {
            if (!TryParseArgs(args, out var input))
            {
                return -1;
            }

            var (day, file) = input;
            if (!_puzzles.ContainsKey(day))
            {
                Console.WriteLine($"No day {day:00} puzzle solution defined");
                return -1;
            }

            Console.WriteLine($"Running day {day:00}, input {file}");
            Console.WriteLine();

            var stopwatch = Stopwatch.StartNew();
            IPuzzle puzzle = _puzzles[day](file);
            stopwatch.Stop();
            Console.WriteLine($"Initialization took {stopwatch.Elapsed}");

            using (var timer = SetTimer(() => puzzle.GetProgress1()))
            {
                Console.WriteLine($"Solving part 1...");
                stopwatch.Restart();
                var result1 = await puzzle.Solve1();
                stopwatch.Stop();
                timer.Stop();
                Console.WriteLine();
                Console.WriteLine($"Result 1 = {result1}");
                Console.WriteLine($"Took {stopwatch.Elapsed}");
            }

            Console.WriteLine();

            using (var timer = SetTimer(() => puzzle.GetProgress2()))
            {
                Console.WriteLine($"Solving part 2...");
                stopwatch.Restart();
                var result2 = await puzzle.Solve2();
                stopwatch.Stop();
                timer.Stop();
                Console.WriteLine();
                Console.WriteLine($"Result 2 = {result2}");
                Console.WriteLine($"Took {stopwatch.Elapsed}");
            }

            return 0;
        }

        private static Timer SetTimer(Func<string?> progressMethod)
        {
            var timer = new Timer(ProgressTimerDelay.TotalMilliseconds)
            {
                AutoReset = true,
                Enabled = true,
            };
            timer.Elapsed += (sender, e) =>
            {
                var p = progressMethod();
                if (p != null)
                {
                    Console.WriteLine(p);
                }
                else
                {
                    Console.Write(".");
                }
            };
            return timer;
        }

        private static bool TryParseArgs(string[] args, out (int Day, string Path) inputArgs)
        {
            inputArgs = (0, string.Empty);

            if (args.Length == 0)
            {
                Console.WriteLine("Day number is required");
                return false;
            }

            if (!int.TryParse(args[0], out var day) || day < 1 || day > 25)
            {
                Console.WriteLine("Day should be a number between [1-25]");
                return false;
            }

            if (args.Length == 1)
            {
                var filePath = Path.Combine(InputPath, $"{day:00}.txt");
                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"Input file '{filePath}' does not exist");
                    return false;
                }
                inputArgs = (day, filePath);
                return true;
            }

            if (File.Exists(args[1]))
            {
                inputArgs = (day, args[1]);
                return true;
            }

            var file = Path.Combine(InputPath, args[1]);
            if (File.Exists(file))
            {
                inputArgs = (day, file);
                return true;
            }

            Console.WriteLine($"Could not find file '{args[1]}'");
            return false;
        }
    }
}
