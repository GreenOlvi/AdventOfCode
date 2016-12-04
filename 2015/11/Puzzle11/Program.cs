using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Puzzle11
{
    class Program
    {
        static void Main(string[] args)
        {
            const string input = @"hxbxwxba";

            var pwGen = GetPasswordGenerator(input);

            foreach (var pw in pwGen.Where(IsValidPassword).Take(2))
            {
                Console.WriteLine(pw);
            }
    
            Console.ReadLine();
        }

        private static IEnumerable<string> GetPasswordGenerator(string seed = @"aaaaaaaa")
        {
            if (seed.Length != 8)
                throw new ArgumentException("Works only for length 8!");

            var sp = seed.ToCharArray();

            while (true)
            {
                var c = true;
                var i = sp.Length - 1;

                while (c && i >= 0)
                {
                    if (sp[i] == 'z')
                    {
                        sp[i] = 'a';
                    }
                    else
                    {
                        sp[i]++;
                        c = false;
                    }
                    i--;
                }
                yield return string.Concat(sp);
            }
        }


        private static bool IsValidPassword(string password)
        {
            return !InvalidChars.IsMatch(password) && DoubleChars.IsMatch(password)
                   && HasThreeIncreasingLetters(password);
        }

        private static readonly Regex InvalidChars = new Regex(@"(i|o|l)");
        private static readonly Regex DoubleChars = new Regex(@"([a-z])\1.*([a-z])\2");

        private static bool HasThreeIncreasingLetters(string password)
        {
            var c = password.ToCharArray();

            for (int i = 0; i < c.Length - 3; i++)
            {
                if (c[i + 1] == c[i] + 1 && c[i + 2] == c[i] + 2)
                    return true;
            }

            return false;
        }
    }
}
