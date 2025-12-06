using System.Linq;
using NoAlloq;

namespace AOC2025.Puzzles;

public partial class Day06 : CustomBaseProblem<long>
{
    private readonly Problem[] _problems;

    public Day06()
    {
        _problems = ParseInput(ReadLinesFromFile());
    }

    public Day06(IEnumerable<string> lines)
    {
        _problems = ParseInput(lines);
    }

    private static Problem[] ParseInput(IEnumerable<string> lines)
    {
        var linesArray = lines.ToArray();
        ReadOnlySpan<string> numberLines = linesArray.AsSpan()[0..^1];
        var problems = new List<Problem>();

        var opLine = linesArray.Last();
        var opIndex = 0;
        var lastOp = false;
        while (!lastOp)
        {
            var nextOpIndex = opLine.IndexOfAny(['+', '*'], opIndex + 1);
            if (nextOpIndex == -1)
            {
                nextOpIndex = opLine.Length + 1;
                lastOp = true;
            }

            var op = opLine[opIndex] switch
            {
                '+' => Operation.Add,
                '*' => Operation.Multiply,
                _ => throw new InvalidDataException(),
            };

            var numbers = numberLines.Select(line => line[opIndex..(nextOpIndex - 1)]).ToArray();
            problems.Add(new Problem(numbers, op));

            opIndex = nextOpIndex;
        }
        return [.. problems];
    }

    public override long Solve1() => _problems.Sum(SolveSimpleProblem);

    private long SolveSimpleProblem(Problem problem) =>
        RunOpOnNumbers(problem.Numbers.ParseLines<long>(), problem.Operation);

    private static long RunOpOnNumbers(IEnumerable<long> numbers, Operation op) => op switch
    {
        Operation.Add => numbers.Sum(),
        Operation.Multiply => numbers.Product(),
        _ => throw new InvalidOperationException(),
    };

    public override long Solve2() => _problems.Sum(SolveRotatedProblem);

    private long SolveRotatedProblem(Problem problem) =>
        RunOpOnNumbers(GetRotatedNumbers(problem.Numbers), problem.Operation);

    private static IEnumerable<long> GetRotatedNumbers(string[] numbers)
    {
        var cols = numbers[0].Length;
        var rows = numbers.Length;

        for (var c = cols - 1; c >= 0; c--)
        {
            var number = 0L;
            for (var r = 0; r < rows; r++)
            {
                var d = numbers[r][c];
                if (char.IsAsciiDigit(d))
                {
                    var i = d - '0';
                    number = number * 10 + i;
                }
            }
            yield return number;
        }
    }

    private readonly record struct Problem(string[] Numbers, Operation Operation)
    {
        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var l in Numbers)
            {
                sb.AppendLine($"[{l}]");
            }

            var op = Operation switch
            {
                Operation.Add => '+',
                Operation.Multiply => '*',
                _ => '?',
            };
            sb.Append(op);
            sb.AppendLine();
            return sb.ToString();
        }
    }

    private enum Operation
    {
        None = 0,
        Add,
        Multiply,
    }
}
