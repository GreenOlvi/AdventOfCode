using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Puzzle04
{
    public class Solver
    {
        public Solver(params string[] input)
        {
            Input = input;
        }

        public string[] Input { get; private set; }

        public long Solve1()
        {
            return ParseInput(Input).Where(r => r.IsValid()).Sum(r => r.Id);
        }

        public int Solve2()
        {
            var name = ParseInput(Input).Where(r => r.IsValid())
                .FirstOrDefault(n => n.Decode().Contains("northpole"));

            return name?.Id ?? -1;
        }

        private IEnumerable<Room> ParseInput(IEnumerable<string> input)
        {
            return input.Select(line => new Room(line));
        }

        //                                                      1111111111222222
        //                                            01234567890123456789012345
        private static readonly List<char> Letters = "abcdefghijklmnopqrstuvwxyz".ToCharArray().ToList();

        public class Room
        {
            private static readonly Regex NameRegex =
                new Regex(@"^(?<letters>[a-z\-]+)-(?<id>\d+)\[(?<checksum>[a-z]+)\]$", RegexOptions.Compiled);

            public Room(string name)
            {
                Name = name;

                var match = NameRegex.Match(name);
                if (!match.Success)
                    throw new ArgumentException("Not a room name", "name");

                LettersPart = match.Groups["letters"].Value;
                Id = int.Parse(match.Groups["id"].Value);
                Checksum = match.Groups["checksum"].Value;

                IdModLetters = Id%Letters.Count;
            }

            public string Name { get; private set; }

            public string LettersPart { get; set; }
            public int Id { get; private set; }
            public string Checksum { get; private set; }

            private int IdModLetters { get; }

            public static string GenerateChecksum(string name)
            {
                var letterDict = new Dictionary<char, int>();
                foreach (var l in name.Where(l => l != '-'))
                {
                    if (letterDict.ContainsKey(l))
                    {
                        letterDict[l]++;
                    }
                    else
                    {
                        letterDict.Add(l, 1);
                    }
                }

                var ordered = letterDict.OrderByDescending(kv => kv.Value)
                    .ThenBy(kv => kv.Key)
                    .Select(kv => kv.Key.ToString());

                return String.Join("", ordered.Take(5));
            }

            public bool IsValid()
            {
                return Checksum == GenerateChecksum(LettersPart);
            }

            public string Decode()
            {
                return LettersPart.Select(l => DecodeLetter(l)).Aggregate("", (s, c) => s + c);
            }

            private char DecodeLetter(char letter)
            {
                if (letter == '-') return ' ';

                var index = (Letters.IndexOf(letter) + IdModLetters) % Letters.Count;
                return Letters[index];
            }
        }
    }
}
