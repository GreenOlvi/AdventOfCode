using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Puzzle17
{
    class Program
    {
        private const int Capacity = 150;

        static void Main(string[] args)
        {
            foreach (var filename in args)
            {
                var containers = GetContainers(filename).ToArray();

                var combinations = Subsets.GetSubsets(containers).Count(s => s.Sum() == Capacity);
                Console.WriteLine("Combinations: {0}", combinations);

                var min = containers.Length;
                var count = 0;
                foreach (var subset in Subsets.GetSubsets(containers).Where(s => s.Sum() == Capacity))
                {
                    if (min > subset.Length)
                    {
                        min = subset.Length;
                        count = 1;
                    }
                    else
                    {
                        if (min == subset.Length)
                            count++;
                    }
                }

                Console.WriteLine("Minimal: {0}, combinations: {1}", min, count);

            }
            Console.ReadLine();
        }

        private static IEnumerable<int> GetContainers(string filename)
        {
            using (var reader = new StreamReader(filename))
            {
                while (!reader.EndOfStream)
                {
                    yield return int.Parse(reader.ReadLine());
                }
            }
        }

    }

    static class Subsets
    {
        public static IEnumerable<int[]> GetSubsets(int length)
        {
            var max = Math.Pow(2, length);
            for (ulong i = 0; i < max; i++)
            {
                yield return Unrank(i, length);
            }
        }

        public static IEnumerable<T[]> GetSubsets<T>(T[] elements)
        {
            return GetSubsets(elements.Length).Select(s => s.Select(e => elements[e]).ToArray());
        }

        public static int[] Unrank(ulong r, int n)
        {
            var rank = r;
            var set = new List<int>();
            for (var i = 0; i < n; i++)
            {
                if ((rank & 1) > 0)
                {
                    set.Add(i);
                }

                rank >>= 1;
            }

            return set.ToArray();
        }

        public static string ToString<T>(T[] set)
        {
            var s = new StringBuilder("[");

            if (set.Length > 0)
            {
                s.Append(set.Select(x => x.ToString()).Aggregate((x, y) => x + ", " + y));
            }

            s.Append("]");

            return s.ToString();
        }
    }
}
