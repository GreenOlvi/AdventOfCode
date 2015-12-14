using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Puzzle13
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var filename in args)
            {
                var table = new TablePlanner(filename);

                var best = table.FindMostHappiness();
                Console.WriteLine("Best: {0}", best);
            }

            Console.ReadLine();
        }

        private class TablePlanner { 
            public string[] People { get; private set; }
            public int[,] Happiness;
            public int Count => People.Length;

            public TablePlanner(string filename)
            {
                FromFile(filename);
            }

            private static readonly Regex LineRegex =
                new Regex(@"^(?<person1>\w+) would (?<sign>gain|lose) (?<value>\d+) happiness units by sitting next to (?<person2>\w+).$");

            private void FromFile(string filename)
            {
                var people = new HashSet<string>();
                var happiness = new List<Tuple<string, string, int>>();

                foreach (var line in GetInput(filename))
                {
                    var match = LineRegex.Match(line);

                    if (!match.Success)
                        throw new ArgumentException("Invalid input!");

                    var a = match.Groups["person1"].Value;
                    var b = match.Groups["person2"].Value;
                    var sign = match.Groups["sign"].Value == "gain" ? 1 : -1;
                    var value = sign * int.Parse(match.Groups["value"].Value);

                    people.Add(a);
                    people.Add(b);

                    happiness.Add(new Tuple<string, string, int>(a, b, value));
                }

                People = people.OrderBy(x => x).ToArray();
                var i = 0;
                var dict = People.ToDictionary(k => k, k => i++);

                Happiness = new int[People.Length, People.Length];
                foreach (var tuple in happiness)
                {
                    Happiness[dict[tuple.Item1], dict[tuple.Item2]] = tuple.Item3;
                }
            }

            private static IEnumerable<string> GetInput(string filename)
            {
                using (var input = new StreamReader(filename))
                {
                    while (!input.EndOfStream)
                    {
                        yield return input.ReadLine();
                    }
                }
            }

            public int FindMostHappiness()
            {
                return Permutations.GetEnumerator(Count).Select(CalculateHappiness).Max();
            }

            private int CalculateHappiness(int[] setting)
            {
                var sum = Happiness[setting[0], setting[Count - 1]] + Happiness[setting[Count - 1], setting[0]];
                for (var i = 0; i < Count - 1; i++)
                {
                    sum += Happiness[setting[i], setting[i + 1]] + Happiness[setting[i + 1], setting[i]];
                }

                return sum;
            }
        }
    }

    static class Permutations
    {
        public static IEnumerable<int[]> GetEnumerator(int length)
        {
            var all = Factorial(length);

            for (long i = 0; i < all; i++)
            {
                yield return Unrank(length, i);
            }
        }

        public static string ToString(int[] p)
        {
            return p.Select(x => x.ToString()).Aggregate((x, y) => x + ", " + y);
        }

        private static int[] Unrank(int n, long r)
        {
            var p = new int[n];
            p[n - 1] = 0;
            for (var j = 1; j <= n - 1; j++)
            {
                var d = (r % Factorial(j + 1)) / Factorial(j);
                r = r - d * Factorial(j);
                p[n - j - 1] = (int)d;
                for (var i = n - j + 1; i <= n; i++)
                {
                    if (p[i - 1] >= d)
                        p[i - 1] = p[i - 1] + 1;
                }
            }

            return p;
        }

        private static readonly Dictionary<int, long> _factorial = new Dictionary<int, long>();
        private static long Factorial(int i)
        {
            if (!_factorial.ContainsKey(i))
                _factorial.Add(i, Enumerable.Range(1, i).Aggregate((x, y) => x * y));

            return _factorial[i];
        }
    }
}
