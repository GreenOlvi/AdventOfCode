using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Puzzle10
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = @"1321131112";

            var gen = LookAndSayGenerator(input);

            var fourty = gen.Take(40).Last();
            Console.WriteLine(fourty.Length);

            var fifty = LookAndSayGenerator(fourty).Take(10).Last();
            Console.WriteLine(fifty.Length);

            Console.ReadLine();
        }

        private static IEnumerable<string> LookAndSayGenerator(string seed)
        {
            var current = seed;
            while (true)
            {
                current = LookAndSay(current);
                yield return current;
            }
        } 

        private static Regex sameLetters = new Regex(@"((.)\2*)");
        private static string LookAndSay(string input)
        {
            var matches = sameLetters.Matches(input);

            var s = new StringBuilder();
            foreach (var m in matches)
            {
                var match = m as Match;

                var c = match?.Value[0] ?? ' ';
                var l = match?.Value.Length ?? 0;

                s.Append(l);
                s.Append(c);
            }

            return s.ToString();
        }
    }
}
