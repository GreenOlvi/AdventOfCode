using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Puzzle05
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var filename in args)
            {
                var strings = GetInput(filename).ToList();

                var count1 = strings.Where(Part1.IsNice).Count();
                Console.WriteLine(@"Part1: {0}", count1);

                var count2 = strings.Where(Part2.IsNice).Count();
                Console.WriteLine(@"Part2: {0}", count2);
            }

            Console.ReadLine();
        }

        private static IEnumerable<string> GetInput(string filename)
        {
            using (var reader = new StreamReader(filename))
            {
                while (!reader.EndOfStream)
                {
                    yield return reader.ReadLine();
                }
            }
        }

        private static class Part1
        {
            private static readonly Regex[] Rules =
            {
                new Regex(@"[aeiou].*[aeiou].*[aeiou]"),
                new Regex(@"(.)\1"),
            };

            private static readonly Regex[] NegRules =
            {
                new Regex(@"(ab|cd|pq|xy)"),
            };

            public static bool IsNice(string input)
            {
                if (Rules.All(x => x.IsMatch(input)) && NegRules.All(x => !x.IsMatch(input)))
                    return true;

                return false;
            }
        }

        private static class Part2
        {
            private static readonly Regex[] Rules =
            {
                new Regex(@"(..).*\1"),
                new Regex(@"(.).\1"),
            };

            public static bool IsNice(string input)
            {
                if (Rules.All(x => x.IsMatch(input)))
                    return true;

                return false;
            }
        }
    }
}
