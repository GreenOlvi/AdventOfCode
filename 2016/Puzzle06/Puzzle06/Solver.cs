using System;
using System.Collections.Generic;
using System.Linq;

namespace Puzzle06
{
    public class Solver
    {
        public Solver(params string[] input)
        {
            Input = input;

            var length = Input.Max(l => l.Length);
            analyzer = new Analyzer(length);

            foreach (var line in Input)
            {
                analyzer.Add(line);
            }
        }

        public string[] Input { get; }
        private Analyzer analyzer { get; }

        public string Solve1()
        {
            return analyzer.GetPassword();
        }

        public string Solve2()
        {
            return analyzer.GetModifiedPassword();
        }

        private class Analyzer
        {
            public Analyzer(int length)
            {
                Length = length;

                Dictionary = new List<LetterCounter>(Length);
                for (var i = 0; i < Length; i++)
                {
                    Dictionary.Add(new LetterCounter());
                }
            }

            public int Length { get; }

            public List<LetterCounter> Dictionary { get; private set; }

            public void Add(string message)
            {
                for (int i = 0; i < Length; i++)
                {
                    Dictionary[i].Add(message[i]);
                }
            }

            public string GetPassword()
            {
                return Dictionary.Select(c => c.GetMostCommon())
                    .Aggregate(String.Empty, (s, c) => s + c);
            }

            public string GetModifiedPassword()
            {
                return Dictionary.Select(c => c.GetLeastCommon())
                    .Aggregate(String.Empty, (s, c) => s + c);
            }
        }

        private class LetterCounter
        {
            public Dictionary<char, int> Letters { get; } = new Dictionary<char, int>();

            public void Add(char letter)
            {
                if (Letters.ContainsKey(letter))
                {
                    Letters[letter]++;
                }
                else
                {
                    Letters.Add(letter, 1);
                }
            }

            public char GetMostCommon()
            {
                return Letters.OrderByDescending(kv => kv.Value)
                    .Select(kv => kv.Key)
                    .First();
            }

            public char GetLeastCommon()
            {
                return Letters.OrderBy(kv => kv.Value)
                    .Select(kv => kv.Key)
                    .First();
            }
        }
    }
}
