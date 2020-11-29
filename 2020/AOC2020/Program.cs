using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace AOC2020
{
    internal static class Program
    {
        private const string InputPath = "input";

        private static readonly Dictionary<int, Func<string, IPuzzle>> _puzzles = new Dictionary<int, Func<string, IPuzzle>>();

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

            var puzzle = _puzzles[day](file);

            Console.WriteLine($"Solving part 1...");
            var stopwatch = Stopwatch.StartNew();
            var result1 = await puzzle.Solve1();
            stopwatch.Stop();
            Console.WriteLine($"Result 1 = {result1}");
            Console.WriteLine($"Took {stopwatch.Elapsed}");
            Console.WriteLine();

            Console.WriteLine($"Solving part 2...");
            stopwatch.Restart();
            var result2 = await puzzle.Solve2();
            stopwatch.Stop();
            Console.WriteLine($"Result 2 = {result2}");
            Console.WriteLine($"Took {stopwatch.Elapsed}");

            return 0;
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
