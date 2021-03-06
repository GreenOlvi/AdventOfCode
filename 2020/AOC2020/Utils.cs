﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AOC2020
{
    public static class Utils
    {
        public static IEnumerable<int> ParseInts(this IEnumerable<string> lines)
            => lines.Select(int.Parse);

        public static IEnumerable<long> ParseLongs(this IEnumerable<string> lines)
            => lines.Select(long.Parse);

        public static IEnumerable<string[]> SplitGroups(this IEnumerable<string> input)
        {
            var current = new List<string>();
            foreach (var line in input)
            {
                if (line == string.Empty && current.Any())
                {
                    yield return current.ToArray();
                    current.Clear();
                }
                else
                {
                    current.Add(line);
                }
            }

            if (current.Any())
            {
                yield return current.ToArray();
            }
        }

        public static IEnumerable<T[]> SplitGroups<T>(this IEnumerable<string> input, Func<string, T> converter)
        {
            var current = new List<T>();
            foreach (var line in input)
            {
                if (line == string.Empty && current.Any())
                {
                    yield return current.ToArray();
                    current.Clear();
                }
                else
                {
                    current.Add(converter(line));
                }
            }

            if (current.Any())
            {
                yield return current.ToArray();
            }
        }

        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<(TKey, TValue)> pairs)
            where TKey : notnull
            where TValue : notnull
                => pairs.ToDictionary(p => p.Item1, p => p.Item2);

        public static int Modulo(this int a, int n) => (int)Modulo((long)a, n);

        public static long Modulo(this long a, long n)
        {
            while (a < 0)
            {
                a += n;
            }
            return a % n;
        }

        public static bool TryMatch(this Regex regex, string input, out Match match)
        {
            match = regex.Match(input);
            return match.Success;
        }

        public static long Product(this IEnumerable<int> numbers) =>
            numbers.Aggregate(1L, (a, b) => a * b);
    }
}
