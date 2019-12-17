using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2019.Puzzle17
{
    public class Compressor
    {
        private readonly Dict _dict = new Dict();

        public string[] Compress(IEnumerable<string> path)
        {
            var chain = InitDictionaryAndReplaceInput(path);

            while (!IsEnough(chain))
            {
                var pair = MostCommonPairs(chain).First(p => _dict.CanCombine(p.Item1, p.Item2));
                var c = _dict.Combine(pair.Item1, pair.Item2);
                chain = ReplacePair(chain, pair, c).ToArray();
            }

            var main = new string(chain);
            Console.WriteLine($"Main = {main}");
            var fun = chain.Distinct()
                .Select(c => $"{c} = {string.Join(',', _dict.Get(c))}")
                .ToArray();
            foreach (var f in fun)
            {
                Console.WriteLine(fun);
            }

            return new string[0];
        }

        private bool IsEnough(char[] chain) =>
            chain.Distinct().Count() <= 3 && string.Join(',', chain).Length <= 20;

        private char[] InitDictionaryAndReplaceInput(IEnumerable<string> chain)
        {
            var input = chain.ToArray();
            var revDict = new Dictionary<string, char>();

            foreach (var i in input.Distinct())
            {
                var n = _dict.Add(i);
                revDict.Add(i, n);
            }

            return input.Select(e => revDict[e]).ToArray();
        }

        private static IEnumerable<(char, char)> MostCommonPairs(char[] input) =>
            input.Zip(input.Skip(1))
                .GroupBy(p => p)
                .Select(g => (Pair: g.Key, Count: g.Count()))
                .OrderByDescending(p => p.Count)
                .Select(p => p.Pair);

        private IEnumerable<char> ReplacePair(IEnumerable<char> chain, (char, char) pair, char c)
        {
            var (a, b) = pair;
            var iter = chain.GetEnumerator();
            while (iter.MoveNext())
            {
                if (iter.Current != a)
                {
                    yield return iter.Current;
                }
                else
                {
                    if (!iter.MoveNext())
                    {
                        yield return a;
                        yield break;
                    }
                    else
                    {
                        if (iter.Current == b)
                        {
                            yield return c;
                        }
                        else
                        {
                            yield return a;
                            yield return iter.Current;
                        }
                    }
                }
            }
        }

        private class Dict
        {
            private readonly Dictionary<char, string[]> _dictionary = new Dictionary<char, string[]>();

            public char Add(string entry)
            {
                var n = NewName();
                _dictionary.Add(n, new[] { entry });
                return n;
            }

            public bool CanCombine(char a, char b)
            {
                var concat = string.Join(",", _dictionary[a].Concat(_dictionary[b]));
                return concat.Length <= 20;
            }

            public char Combine(char a, char b)
            {
                var n = NewName();
                _dictionary.Add(n, _dictionary[a].Concat(_dictionary[b]).ToArray());
                return n;
            }

            public IEnumerable<string> Get(char c) => _dictionary[c];

            private char _nextName = 'A';
            private char NewName()
            {
                if (_nextName == 'Z')
                {
                    _nextName = 'a';
                }
                return _nextName++;
            }
        }
    }
}
