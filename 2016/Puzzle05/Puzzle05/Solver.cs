using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Puzzle05
{
    public class Solver
    {
        public Solver(string input)
        {
            Input = input;
        }

        public string Input { get; private set; }

        public string Solve1()
        {
            var pass = new HashGenerator(Input)
                .Where(h => StartsWith5Zeros(h))
                .Take(8)
                .Select(h => HashGenerator.HashString(h)[5])
                .Aggregate(String.Empty, (s, c) => s + c);

            return pass;
        }

        public string Solve2()
        {
            var hashes = new HashGenerator(Input).Where(h => StartsWith5Zeros(h))
                .Select(h => HashGenerator.HashString(h));

            var pass = new char?[8];
            foreach (var hash in hashes.Where(h => IsValidPosition(h[5])))
            {
                var position = int.Parse(hash[5].ToString());
                if (pass[position] != null)
                    continue;

                pass[position] = hash[6];

                Print(pass);

                if (pass.All(l => l != null))
                    break;
            }

            return pass.Aggregate(String.Empty, (s, c) => s + c.Value);
        }

        private static void Print(char?[] str)
        {
            var result = new StringBuilder();

            foreach (var c in str)
            {
                if (c == null)
                    result.Append("_");
                else
                    result.Append(c);
            }

            Console.WriteLine(result);
        }

        private static bool IsValidPosition(char l)
        {
            return l >= '0' && l < '8';
        }

        public static bool StartsWith5Zeros(byte[] hash)
        {
            return (hash[0] == 0 && hash[1] == 0 && hash[2] < 16);
        }

        public class HashGenerator : IEnumerable<byte[]>
        {
            private readonly MD5 _md5 = MD5.Create();

            public HashGenerator(string key, long startIndex = 0)
            {
                Key = key;
                Index = startIndex;
            }

            public string Key { get; }
            public long Index { get; private set; }

            public IEnumerator<byte[]> GetEnumerator()
            {
                while (true)
                {
                    yield return ToMd5(Key + Index);
                    Index++;
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            private byte[] ToMd5(string text)
            {
                return _md5.ComputeHash(Encoding.ASCII.GetBytes(text));
            }

            public static string HashString(byte[] hash)
            {
                return hash.Select(x => x.ToString("x2")).Aggregate((a, b) => a + b);
            }
        }
    }
}
