using System.Collections.Generic;

namespace AOC2020
{
    public static class Utils
    {
        public static IEnumerable<int> ParseInts(this IEnumerable<string> lines)
        {
            foreach (var line in lines)
            {
                yield return int.Parse(line);
            }
        }
    }
}
