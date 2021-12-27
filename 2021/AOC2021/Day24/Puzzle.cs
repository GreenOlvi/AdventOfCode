using System.Text;
using System.Text.RegularExpressions;
using AOC2021.Common;

namespace AOC2021.Day24
{
    public class Puzzle : PuzzleBase<long, long>
    {
        public Puzzle(IEnumerable<string> lines)
        {
            Coeffs = ChopByInput(lines).Select(ExtractCoefficients).ToArray();
        }

        private static IEnumerable<string> ChopByInput(IEnumerable<string> lines)
        {
            var sb = new StringBuilder();
            foreach (string line in lines)
            {
                if (line.StartsWith("inp"))
                {
                    if (sb.Length > 0)
                    {
                        yield return sb.ToString();
                        sb.Clear();
                    }
                }
                sb.AppendLine(line);
            }
            yield return sb.ToString();
        }

        private static (int, int, int) ExtractCoefficients(string chunk) =>
            CoeffPattern.Parse(chunk, ("A", int.Parse), ("B", int.Parse), ("C", int.Parse));

        private static readonly Regex CoeffPattern =
            new(@"inp w\s*" +
                @"mul x 0\s*" +
                @"add x z\s*" +
                @"mod x 26\s*" +
                @"div z (?<A>\d+)\s*" +
                @"add x (?<B>-?\d+)\s*" +
                @"eql x w\s*" +
                @"eql x 0\s*" +
                @"mul y 0\s*" +
                @"add y 25\s*" +
                @"mul y x\s*" +
                @"add y 1\s*" +
                @"mul z y\s*" +
                @"mul y 0\s*" +
                @"add y w\s*" +
                @"add y (?<C>-?\d+)\s*" +
                @"mul y x\s*" +
                @"add z y\s*",
                RegexOptions.Compiled | RegexOptions.Singleline);

        private readonly (int A, int B, int C)[] Coeffs;

        private static long PushPart(int w, long z, int a, int b, int c)
        {
            var x = (z % 26 + b) != w ? 1 : 0;
            var y = 25 * x + 1;
            return z / a * y + x * (w + c);
        }

        private static bool PopPart(long z, int a, int b, int c, out int w, out long zn)
        {
            w = (int)(z % 26 + b);
            if (w >= 1 && w <= 9)
            {
                zn = PushPart(w, z, a, b, c);
                return true;
            }
            zn = -1;
            return false;
        }

        private bool TryMonad(int[] input, out int wrongDigit, out int[] allDigits)
        {
            allDigits = new int[14];
            var z = 0L;
            var skipped = 0;
            for (var i = 0; i < Coeffs.Length; i++)
            {
                var (a, b, c) = Coeffs[i];
                if (a == 1)
                {
                    z = PushPart(input[i - skipped], z, a, b, c);
                    allDigits[i] = input[i - skipped];
                }
                else
                {
                    if (PopPart(z, a, b, c, out var w, out z))
                    {
                        allDigits[i] = w;
                        skipped++;
                    }
                    else
                    {
                        wrongDigit = i - skipped;
                        allDigits = null;
                        return false;
                    }
                }
            }
            wrongDigit = default;
            return z == 0;
        }

        private static void Succ(int[] arr)
        {
            for (var i = arr.Length - 1; i >= 0; i--)
            {
                if (arr[i] < 9)
                {
                    arr[i]++;
                    for (var j = i + 1; j < arr.Length; j++)
                    {
                        arr[j] = 1;
                    }
                    return;
                }
            }
        }

        private static void Pred(int[] arr)
        {
            for (var i = arr.Length - 1; i >= 0; i--)
            {
                if (arr[i] > 1)
                {
                    arr[i]--;
                    for (var j = i + 1; j < arr.Length; j++)
                    {
                        arr[j] = 9;
                    }
                    return;
                }
            }
        }

        public override long Solution1()
        {
            int[] all;
            var input = Enumerable.Repeat(9, 7).ToArray();
            while (true)
            {
                if (TryMonad(input, out var d, out all))
                    break;
                Pred(input);
            }
            return all.Aggregate(0L, (a, b) => 10 * a + b);
        }

        public override long Solution2()
        {
            int[] all;
            var input = Enumerable.Repeat(1, 7).ToArray();
            while (true)
            {
                if (TryMonad(input, out var d, out all))
                    break;
                Succ(input);
            }
            return all.Aggregate(0L, (a, b) => 10 * a + b);
        }
    }
}
