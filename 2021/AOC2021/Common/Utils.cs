using System.Text.RegularExpressions;

namespace AOC2021.Common
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

        public static long Product(this IEnumerable<int> numbers) => numbers.Aggregate(1L, (a, b) => a * b);

        public static IEnumerable<IEnumerable<T>> SlidingWindow<T>(this IEnumerable<T> input, int n)
        {
            var arr = input.ToArray();
            for (var i = 0; i <= arr.Length - n; i++)
            {
                yield return arr.Skip(i).Take(n);
            }
        }

        public static string Print(this IEnumerable<Point> points)
        {
            var pointArray = points.ToArray();
            var minX = points.Min(p => p.X);
            var minY = points.Min(p => p.Y);
            var maxX = points.Max(p => p.X);
            var maxY = points.Max(p => p.Y);

            var r = Enumerable.Range(0, (int)(maxY - minY + 1))
                .Select(i => Enumerable.Range(0, (int)(maxX - minX + 1)).Select(j => ' ').ToArray())
                .ToArray();

            foreach (var point in pointArray)
            {
                var y = point.Y - minY;
                var x = (int)(point.X - minX);
                r[y][x] = '#';
            }

            return "\n" + string.Join("\n", r.Select(l => new string(l)));
        }
    }
}
