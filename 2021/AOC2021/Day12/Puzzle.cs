using System.Text.RegularExpressions;
using AOC2021.Common;

namespace AOC2021.Day12
{
    public class Puzzle : PuzzleBase<long, long>
    {
        public Puzzle(IEnumerable<string> lines)
        {
            var caves = lines.Select(l =>
            {
                return ParseLine(l);
            }).ToArray();

            _tunnels = ProcessTunnels(caves);
        }

        private static readonly Regex _line = new(@"^(?<c1>\w+)-(?<c2>\w+)$", RegexOptions.Compiled);
        private readonly Dictionary<string, List<string>> _tunnels;

        private static (string, string) ParseLine(string l)
        {
            if (_line.TryMatch(l, new[] { "c1", "c2" }, out var r))
            {
                return (r[0], r[1]);
            }
            throw new InvalidDataException(l);
        }

        private static Dictionary<string, List<string>> ProcessTunnels(IEnumerable<(string, string)> caves)
        {
            var tunnels = new Dictionary<string, List<string>>();
            foreach (var (c1, c2) in caves)
            {
                if (!tunnels.ContainsKey(c1))
                {
                    tunnels.Add(c1, new List<string> { c2 });
                }
                else
                {
                    tunnels[c1].Add(c2);
                }

                if (!tunnels.ContainsKey(c2))
                {
                    tunnels.Add(c2, new List<string> { c1 });
                }
                else
                {
                    tunnels[c2].Add(c1);
                }
            }
            return tunnels;
        }

        private static bool IsBig(string cave) => cave[0] >= 'A' && cave[0] <= 'Z';

        private bool GetAllPaths(IEnumerable<string> takenPath, Func<string, string[], bool> canGo, out List<string[]> result)
        {
            var path = takenPath.ToArray();
            var from = path.Last();

            if (from == "end")
            {
                result = new List<string[]> { path };
                return true;
            }

            var tunnels = _tunnels[from]
                .Where(t => canGo(t, path))
                .OrderBy(t => t)
                .ToArray();

            result = new List<string[]>();
            var reachedEnd = false;
            foreach (var tunnel in tunnels)
            {
                if (GetAllPaths(path.Append(tunnel), canGo, out var paths))
                {
                    reachedEnd = true;
                    result.AddRange(paths);
                }
            }

            return reachedEnd;
        }

        private static bool CanGo1(string t, string[] currentPath)
        {
            if (t == "start")
            {
                return false;
            }

            if (!currentPath.Any() || t == "end" || IsBig(t))
            {
                return true;
            }

            return currentPath.All(p => p != t);
        }

        private static bool CanGo2(string t, string[] currentPath)
        {
            if (t == "start")
            {
                return false;
            }

            if (!currentPath.Any() || t == "end" || IsBig(t) || !currentPath.Contains(t))
            {
                return true;
            } 

            return currentPath.Select(p => p)
                .Where(c => !IsBig(c))
                .GroupBy(c => c)
                .All(g => g.Count() < 2);
        }

        public override long Solution1()
        {
            GetAllPaths(new[] { "start" }, CanGo1, out var paths);
            return paths.Count;
        }

        public override long Solution2()
        {
            GetAllPaths(new[] { "start" }, CanGo2, out var paths);
            return paths.Count;
        }
    }
}
