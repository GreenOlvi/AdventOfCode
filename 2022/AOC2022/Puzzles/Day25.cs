using System.Diagnostics.CodeAnalysis;

namespace AOC2022.Puzzles;

public class Day25 : CustomBaseDay
{
    private readonly string[] _lines;

    public Day25()
    {
        _lines = ReadLinesFromFile().ToArray();
    }

    public Day25(IEnumerable<string> lines)
    {
        _lines = lines.ToArray();
    }

    public override ValueTask<string> Solve_1()
    {
        var sum = _lines.Select(FromSnafu).Sum();
        return ToSnafu(sum).ToResult();
    }

    public override ValueTask<string> Solve_2() => string.Empty.ToResult();

    public static long FromSnafu(string snafu)
    {
        var result = 0L;
        var queue = new Queue<char>(snafu.Trim());
        while (queue.TryDequeue(out char c))
        {
            var n = c switch
            {
                '2' => 2,
                '1' => 1,
                '0' => 0,
                '-' => -1,
                '=' => -2,
                _ => throw new ArgumentOutOfRangeException(c.ToString()),
            };
            result = result * 5 + n;
        }

        return result;
    }

    public static string ToSnafu(long number)
    {
        var stack = new Stack<char>();
        var carry = 0;
        while (number > 0)
        {
            var d = (number + carry) % 5;

            char c;
            (c, carry) = d switch
            {
                0 => ('0', carry),
                1 => ('1', 0),
                2 => ('2', 0),
                3 => ('=', 1),
                4 => ('-', 1),
                _ => throw new InvalidOperationException(),
            };

            stack.Push(c);
            number /= 5;
        }
        if (carry > 0)
        {
            stack.Push('1');
        }
        return new string(stack.ToArray());
    }
}
