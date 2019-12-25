using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AoC2019.Common;

namespace AoC2019.Puzzle25
{
    public class Solution : IPuzzle
    {
        public Solution(string input)
        {
            _input = IntcodeMachine.ParseInput(input).ToArray();
        }

        private readonly long[] _input;

        private static readonly Regex SolutionRegex =
            new Regex(@"""Oh, hello! You should be able to get in by typing (?<code>\d+) on the keypad at the main airlock.""", RegexOptions.Multiline | RegexOptions.Compiled);

        private static string Solve(IntcodeMachine m)
        {
            South(m);
            Take(m, "space heater");
            South(m);
            East(m);
            Take(m, "loom");
            West(m);
            North(m);
            West(m);
            Take(m, "wreath");
            South(m);
            Take(m, "space law space brochure");
            South(m);
            Take(m, "pointer");
            North(m);
            North(m);
            East(m);
            North(m);
            North(m);
            North(m);
            Take(m, "sand");
            South(m);
            South(m);
            West(m);
            South(m);
            Take(m, "planetoid");
            North(m);
            West(m);
            Take(m, "festive hat");
            South(m);
            West(m);
            Drop(m, "space heater");
            Drop(m, "loom");
            Drop(m, "space law space brochure");
            Drop(m, "festive hat");
            North(m);
            var output = m.GetAsciiOutput();
            var match = SolutionRegex.Match(output);
            if (match.Success)
            {
                return match.Groups["code"].Value;
            }
            return output;
        }

        private static void North(IntcodeMachine m)
        {
            m.AddAsciiInput("north");
        }
        private static void South(IntcodeMachine m)
        {
            m.AddAsciiInput("south");
        }
        private static void East(IntcodeMachine m)
        {
            m.AddAsciiInput("east");
        }
        private static void West(IntcodeMachine m)
        {
            m.AddAsciiInput("west");
        }
        private static void Take(IntcodeMachine m, string thing)
        {
            m.AddAsciiInput($"take {thing}");
        }
        private static void Drop(IntcodeMachine m, string thing)
        {
            m.AddAsciiInput($"drop {thing}");
        }

        private static void ManualExplore(IntcodeMachine m)
        {
            Console.WriteLine(m.GetAsciiOutput());
            while (!m.IsHalted)
            {
                var cmd = Console.ReadLine().Trim();
                m.AddAsciiInput(cmd);
                Console.WriteLine(m.GetAsciiOutput());
            }

            Console.WriteLine("Drone has halted.");
        }

        private static string Solve1(IEnumerable<long> input)
        {
            var m = new IntcodeMachine(input);
            m.Run();
            //ManualExplore(m);
            return Solve(m);
        }

        public Task<string> Solve1Async() =>
            Task.Run(() => Solve1(_input));

        public Task<string> Solve2Async() =>
            Task.Run(() => string.Empty);
    }
}
