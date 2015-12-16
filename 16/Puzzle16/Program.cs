using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Puzzle16
{
    class Program
    {
        static readonly Aunt SearchedAunt = new Aunt("Searched", "children: 3\r\ncats: 7\r\nsamoyeds: 2\r\npomeranians: 3\r\nakitas: 0\r\nvizslas: 0\r\ngoldfish: 5\r\ntrees: 3\r\ncars: 2\r\nperfumes: 1");

        static void Main(string[] args)
        {

            foreach (var filename in args)
            {
                var auntList = GetAunts(filename).ToList();

                auntList.Where(a => SearchedAunt.MatchExact(a)).ToList().ForEach(a => Console.WriteLine("Matched: {0}", a.Name));

                Console.WriteLine();

                auntList.Where(a => SearchedAunt.MatchRanges(a)).ToList().ForEach(a => Console.WriteLine("Matched: {0}", a.Name));
            }

            Console.ReadLine();
        }

        private static IEnumerable<Aunt> GetAunts(string filename)
        {
            using (var reader = new StreamReader(filename))
            {
                while (!reader.EndOfStream)
                {
                    yield return new Aunt(reader.ReadLine());
                }
            }
        }

        class Aunt
        {
            private static readonly string[] Properties =
            {
                "children",
                "cats",
                "samoyeds",
                "pomeranians",
                "akitas",
                "vizslas",
                "goldfish",
                "trees",
                "cars",
                "perfumes",
            };

            private static readonly Dictionary<string, Func<int, int, bool>> Ranges = new Dictionary<string, Func<int, int, bool>>
            {
                { "cats", (a, b) => a < b },
                { "trees", (a, b) => a < b },
                { "pomeranians", (a, b) => a > b },
                { "goldfish", (a, b) => a > b },
            }; 

            private static readonly Regex LineRegex = new Regex(@"^(?<name>Sue \d+): (?<things>.+)$");
            private static readonly Regex PropertyRegex = new Regex(@"^(?<property>" + Properties.Aggregate((a, b) => a + "|" + b) + @"): (?<amount>\d+)$");

            public string Name { get; }
            private Dictionary<string, int?> Property { get; } = Properties.ToDictionary(s => s, s => (int?) null);

            public Aunt(string input)
            {
                var match = LineRegex.Match(input);

                if (!match.Success)
                    throw new ArgumentException("Wrong input format!", "input");

                Name = match.Groups["name"].Value;

                var things = match.Groups["things"].Value.Split(new [] {", "}, StringSplitOptions.RemoveEmptyEntries);
                AddThings(things);
            }

            public Aunt(string name, string things)
            {
                Name = name;
                var t = things.Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);
                AddThings(t);
            }

            private void AddThings(IEnumerable<string> things)
            {
                foreach (var thing in things)
                {
                    var m = PropertyRegex.Match(thing);

                    if (!m.Success)
                        throw new ArgumentException("Wrong input format!", thing);

                    Property[m.Groups["property"].Value] = int.Parse(m.Groups["amount"].Value);
                }
            }

            public bool MatchExact(Aunt other)
            {
                return Properties.All(property => !other.Property[property].HasValue || !Property[property].HasValue ||
                    other.Property[property].Value == Property[property].Value);
            }

            public bool MatchRanges(Aunt other)
            {
                foreach (var p in Properties.Where(p => Property[p].HasValue && other.Property[p].HasValue))
                {
                    if (Ranges.ContainsKey(p))
                    {
                        if (!Ranges[p](Property[p].Value, other.Property[p].Value))
                            return false;
                    }
                    else
                    {
                        if (Property[p] != other.Property[p])
                            return false;
                    }
                }
                return true;
            }
        }
    }
}
