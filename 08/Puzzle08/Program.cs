using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Puzzle08
{
    internal class Program
    {
        private static readonly Regex OuterQuoteRegex = new Regex("^\"|\"$");
        private static readonly Regex HexRegex = new Regex(@"[0-9a-zA-Z]{2}");

        private static void Main(string[] args)
        {
            foreach (var filename in args)
            {
                var words = GetInput(filename).ToList();
                var length = words.Select(s => s.Length).Sum();
                var memLength = words.Select(MemLength).Sum();

                Console.WriteLine("Difference: {0}", length - memLength);

                var escLength = words.Select(EscLength).Sum();

                Console.WriteLine("Difference 2: {0}", escLength - length);
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

        private static int MemLength(string input)
        {
            var s = OuterQuoteRegex.Replace(input, "");

            var iter = s.GetEnumerator();
            var l = 0;

            while (iter.MoveNext())
            {
                l++;
                var c = iter.Current;
                if (c == '\\')
                {
                    var x = GetNext(iter);
                    switch (x)
                    {
                        case '\\':
                            break;
                        case '\"':
                            break;
                        case 'x':
                            if (!HexRegex.IsMatch((GetNext(iter) + GetNext(iter)).ToString())) l += 2;
                            break;
                        default:
                            l++;
                            break;
                    }
                }
            }

            return l;
        }

        private static char GetNext(CharEnumerator iter)
        {
            iter.MoveNext();
            return iter.Current;
        }

        private static int EscLength(string input)
        {
            var specials = input.ToCharArray().Count(c => c == '\\' || c == '"');
            return input.Length + specials + 2;
        }
    }
}