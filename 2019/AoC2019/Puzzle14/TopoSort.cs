using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2019.Puzzle14
{
    public static class TopoSort
    {
        public static IEnumerable<T> TopologicalSort<T>(IEnumerable<(T, T)> edgeList) where T : IEquatable<T>
        {
            var edges = new HashSet<(T, T)>(edgeList);
            var nodes = new HashSet<T>(edges.SelectMany(e => new[] { e.Item1, e.Item2 }));

            var L = new List<T>();
            var S = new HashSet<T>(nodes.Where(n => edges.All(e => e.Item2.Equals(n) == false)));

            while (S.Any())
            {
                var n = S.First();
                S.Remove(n);

                L.Add(n);

                foreach (var e in edges.Where(e => e.Item1.Equals(n)).ToList())
                {
                    var m = e.Item2;
                    edges.Remove(e);

                    if (edges.All(me => me.Item2.Equals(m) == false))
                    {
                        S.Add(m);
                    }
                }
            }

            if (edges.Any())
            {
                throw new InvalidOperationException("Graph has cycles");
            }
            return L;
        }
    }
}
