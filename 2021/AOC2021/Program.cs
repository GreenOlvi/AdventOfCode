using System.Diagnostics;

namespace AOC2021
{
    internal static class Program
    {
        private const string InputPath = "input";

        private static readonly Dictionary<int, Func<string, IPuzzle>> _puzzles = new()
        {
            { 1, i => new Day01.Puzzle(ReadLines(i)) }
        };

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

            await Run(puzzle, 1);
            await Run(puzzle, 2);

            return 0;
        }

        private static async Task<bool> Run(IPuzzle puzzle, int part)
        {
            Func<string> method = part switch
            {
                1 => puzzle.Solve1,
                2 => puzzle.Solve2,
                _ => throw new InvalidOperationException(),
            };

            Console.WriteLine($"Solving part {part}...");
            var stopwatch = Stopwatch.StartNew();
            var result = await Task.Run(method);
            stopwatch.Stop();
            Console.WriteLine();
            Console.WriteLine($"Result {part} = {result}");
            Console.WriteLine($"Took {stopwatch.Elapsed}");
            return result != null;
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

        private static IEnumerable<string> ReadLines(string filepath) => File.ReadLines(filepath);
    }
}