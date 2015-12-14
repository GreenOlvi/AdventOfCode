using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Puzzle14
{
    class Program
    {
        private static readonly int Time = 2503;

        private static void Main(string[] args)
        {
            foreach (var filename in args)
            {
                var reindeers = GetInput(filename).Select(x => new Reindeer(x));

                var best = reindeers.Select(r => r.GetDistance(Time)).Max();
                Console.WriteLine("Best reindeer: {0}", best);
            }

            Console.ReadLine();
        }

        private static IEnumerable<string> GetInput(string filename)
        {
            using (var input = new StreamReader(filename))
            {
                while (!input.EndOfStream)
                {
                    yield return input.ReadLine();
                }
            }
        } 

        private class Reindeer
        {
            public string Name { get; private set; }
            public int Velocity { get; private set; }
            public int RunTime { get; private set; }
            public int WaitTime { get; private set; }

            private static readonly Regex LineRegex =
                new Regex(@"^(?<name>\w+) can fly (?<velocity>\d+) km/s for (?<run_time>\d+) seconds, but then must rest for (?<wait_time>\d+) seconds.$");

            public Reindeer(string input)
            {
                var match = LineRegex.Match(input);

                if (!match.Success)
                    throw new ArgumentException("Wrong input format!", "input");

                Name = match.Groups["name"].Value;
                Velocity = int.Parse(match.Groups["velocity"].Value);
                RunTime = int.Parse(match.Groups["run_time"].Value);
                WaitTime = int.Parse(match.Groups["wait_time"].Value);
            }

            public int GetDistance(int time)
            {
                var cycles = time/(RunTime + WaitTime);
                var r = Math.Min(time%(RunTime + WaitTime), RunTime);

                return cycles * RunTime * Velocity + r * Velocity;
            }
        }
    }
}
