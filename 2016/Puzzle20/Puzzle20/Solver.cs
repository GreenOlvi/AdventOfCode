using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Puzzle20
{
    public class Solver
    {
        public Solver(uint max, params string[] input)
        {
            Max = max;
            Input = input;
            AllowedRanges = RemoveRanges(Input.Select(line => ParseLine(line)));
        }

        public Solver(params string[] input) : this(4294967295, input)
        {
        }

        public uint Max { get; }
        public string[] Input { get; }
        public List<Range> AllowedRanges { get; }

        public uint Solve1()
        {
            return AllowedRanges.OrderBy(r => r.Low).First().Low;
        }

        private List<Range> RemoveRanges(IEnumerable<Range> ranges)
        {
            var addresses = new List<Range> {new Range(0, Max)};

            foreach (var range in ranges)
            {
                var newRange = new List<Range>();
                foreach (var address in addresses)
                {
                    newRange.AddRange(address.Subtract(range));
                }
                addresses = newRange;
            }

            return addresses;
        }

        public uint Solve2()
        {
            return (uint) AllowedRanges.Sum(r => r.Count());
        }

        private static readonly Regex RangeRegex = new Regex(@"(?<a>\d+)-(?<b>\d+)");
        private static Range ParseLine(string line)
        {
            var match = RangeRegex.Match(line);
            if (!match.Success)
                throw new ArgumentException("Wring line format '" + line + "'", "line");

            var a = uint.Parse(match.Groups["a"].Value);
            var b = uint.Parse(match.Groups["b"].Value);

            return new Range(a, b);
        }

        public class Range
        {
            public Range(uint a, uint b)
            {
                if (a < b)
                {
                    Low = a;
                    High = b;
                }
                else
                {
                    Low = b;
                    High = a;
                }
            }

            public uint Low { get; }
            public uint High { get; }

            public IEnumerable<Range> Subtract(Range other)
            {
                if (other.High < Low || High < other.Low)
                {
                    return new[] {this};
                }

                if (other.Low <= Low && High <= other.High)
                {
                    return Enumerable.Empty<Range>();
                }

                var results = new List<Range>();
                if (Low <= other.High && other.High < High)
                {
                    results.Add(new Range(other.High + 1, High));
                }

                if (Low < other.Low && other.Low <= High)
                {
                    results.Add(new Range(Low, other.Low - 1));
                }

                return results;
            }

            public uint Count()
            {
                return High - Low + 1;
            }

            public override string ToString()
            {
                return String.Format(@"({0}, {1})", Low, High);
            }
        }
    }
}
