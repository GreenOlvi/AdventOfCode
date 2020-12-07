using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AOC2020.Day07
{
    public class Puzzle : PuzzleBase<int, long>
    {
        public Puzzle(IEnumerable<string> input)
        {
            _bagContents = input.Select(ParseLine).ToDictionary();
        }

        private const string Target = "shiny gold";
        private static readonly Regex contentsRegex = new Regex(@"(?<count>\d+) (?<type>\w+ \w+) bags?", RegexOptions.Compiled);

        private readonly Dictionary<string, (int, string)[]> _bagContents;

        private static (string Container, (int Count, string Bag)[] Contents) ParseLine(string line)
        {
            var parts = line.Split(" bags contain ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var container = parts[0];

            if (parts[1] == "no other bags.")
            {
                return (container, Array.Empty<(int, string)>());
            }

            var content = parts[1].Split(", ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var contents = content.Select(s =>
            {
                var m = contentsRegex.Match(s);
                if (!m.Success)
                {
                    throw new PuzzleException("Invalid input");
                }

                return (int.Parse(m.Groups["count"].Value), m.Groups["type"].Value);
            });

            return (container, contents.ToArray());
        }

        public override int Solution1()
        {
            Dictionary<string, string[]> bagsContaining = _bagContents
                .SelectMany(l => l.Value.Select(contained => (contained.Item2, l.Key)))
                .GroupBy(p => p.Item1)
                .ToDictionary(g => g.Key, g => g.Select(p => p.Item2).ToArray());

            var queue = new Queue<string>();
            queue.Enqueue(Target);

            var canContain = new HashSet<string>();
            while (queue.Any())
            {
                var bag = queue.Dequeue();
                if (!bagsContaining.TryGetValue(bag, out var bags))
                {
                    continue;
                }

                foreach (var container in bags.Where(b => !canContain.Contains(b))) {
                    canContain.Add(container);
                    queue.Enqueue(container);
                }
            }

            return canContain.Count;
        }

        private long CountBagsInside(string baseColour)
        {
            var sum = 0L;
            var bagsInside = _bagContents[baseColour];
            foreach (var (count, colour) in bagsInside)
            {
                sum += (1 + CountBagsInside(colour)) * count;
            }
            return sum;
        }

        public override long Solution2() => CountBagsInside(Target);
    }
}
