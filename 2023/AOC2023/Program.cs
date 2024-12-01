using Spectre.Console;

internal class Program
{
    private static readonly Action<SolverConfiguration> _solverConfig = c =>
    {
        c.ClearConsole = false;
        c.ShowConstructorElapsedTime = true;
        c.ShowTotalElapsedTimePerDay = true;
        c.ShowOverallResults = true;
    };

    private static Task Main(string[] args)
    {
        if (!TryParseCommandLine(args, out var options, out var error))
        {
            AnsiConsole.MarkupLineInterpolated($"[red]Error[/]: {error}");
            return Task.CompletedTask;
        }

        return options.DayNumber switch
        {
            null => Solver.SolveAll(_solverConfig),
            _ => Solver.Solve(new[] { options.DayNumber.Value }, _solverConfig),
        };
    }

    private static bool TryParseCommandLine(string[] args, out CommandLineOptions options, out string error)
    {
        error = string.Empty;
        options = new CommandLineOptions();

        if (args.Length == 0)
        {
            return true;
        }

        if (!uint.TryParse(args[0], out var day))
        {
            error = "Day should be a number";
            return false;
        }

        if (day < 0 || day > 25)
        {
            error = "Day should be a number between [1-25]";
            return false;
        }

        if (args.Length == 1)
        {
            var filePath = Path.Combine($"Inputs", $"{day:00}.txt");
            if (!File.Exists(filePath))
            {
                error = $"Input file '{filePath}' does not exist";
                return false;
            }

            options = new CommandLineOptions { DayNumber = day, Input = filePath };
            return true;
        }

        if (File.Exists(args[1]))
        {
            options = new CommandLineOptions { DayNumber = day, Input = args[1] };
            return true;
        }

        error = $"Could not find file '{args[1]}'";
        return false;

    }

    private readonly struct CommandLineOptions
    {
        public CommandLineOptions()
        {
        }

        public uint? DayNumber { get; init; }

        public string? Input { get; init; }
    }

}
