using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Puzzle09
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var filename in args)
            {
                var travel = new TravelPlan(filename);

                var min = travel.FindShortestRoute();
                Console.WriteLine("Shortest: {0}", min);

                var max = travel.FindLongestRoute();
                Console.WriteLine("Longest: {0}", max);
            }

            Console.ReadLine();
        }


        private class TravelPlan
        {
            public string[] Places;
            public int[,] Distances;
            public int Length => Places.Length;

            public TravelPlan(string filename)
            {
                LoadInput(filename);
            }

            private static readonly Regex InputSplit = new Regex(@"^(\S+) to (\S+) = (\d+)$");

            private void LoadInput(string filename)
            {
                var places = new HashSet<string>();
                var dist = new List<Tuple<string, string, int>>();

                using (var reader = new StreamReader(filename))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var match = InputSplit.Match(line);

                        if (match.Success)
                        {
                            var a = match.Groups[1].Value;
                            var b = match.Groups[2].Value;
                            var d = int.Parse(match.Groups[3].Value);

                            places.Add(a);
                            places.Add(b);

                            dist.Add(new Tuple<string, string, int>(a, b, d));
                        }
                    }
                }

                Places = places.OrderBy(x => x).ToArray();
                var i = 0;
                var dict = Places.ToDictionary(k => k, k => i++);

                Distances = new int[Places.Length, Places.Length];
                foreach (var entry in dist)
                {
                    Distances[dict[entry.Item1], dict[entry.Item2]] = entry.Item3;
                    Distances[dict[entry.Item2], dict[entry.Item1]] = entry.Item3;
                }
            }

            public int FindShortestRoute()
            {
                return Permutations.GetEnumerator(Length).Select(CalculateDistance).Min();
            }

            public int FindLongestRoute()
            {
                return Permutations.GetEnumerator(Length).Select(CalculateDistance).Max();
            }

            private int CalculateDistance(int[] route)
            {
                var d = 0;
                for (int i = 0; i < route.Length - 1; i++)
                {
                    d += Distances[route[i], route[i + 1]];
                }
                return d;
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
                var d = (r%Factorial(j + 1))/Factorial(j);
                r = r - d*Factorial(j);
                p[n - j - 1] = (int) d;
                for (var i = n - j + 1; i <= n; i++)
                {
                    if (p[i-1] >= d)
                        p[i-1] = p[i-1] + 1;
                }
            }

            return p;
        }

        private static readonly Dictionary<int, long> _factorial = new Dictionary<int, long>();
        private static long Factorial(int i)
        {
            if (!_factorial.ContainsKey(i))
                _factorial.Add(i, Enumerable.Range(1, i).Aggregate((x, y) => x*y));

            return _factorial[i];
        }
    }
}
