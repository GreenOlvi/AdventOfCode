using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Puzzle07
{
    public class Solver
    {
        public Solver(params string[] input)
        {
            Input = input;
        }

        public string[] Input { get; private set; }

        public int Solve1()
        {
            return Input.Select(ip => new Ipv7(ip)).Count(ip => ip.SupportsTls());
        }

        public int Solve2()
        {
            throw new NotImplementedException();
        }

        private class Ipv7
        {
            private static readonly Regex IpParts = new Regex(@"^(\w+)\[(\w+)\](\w+)$", RegexOptions.Compiled);
            public Ipv7(string ip)
            {
                Ip = ip;

                var match = IpParts.Match(ip);
                if (!match.Success)
                    throw new Exception("Not a valid IPv7 address '" + ip + "'");

                Part1 = match.Groups[1].Value;
                Part2 = match.Groups[2].Value;
                Part3 = match.Groups[3].Value;
            }

            public string Ip { get; }

            public string Part1 { get; }
            public string Part2 { get; }
            public string Part3 { get; }

            public bool SupportsTls()
            {
                return (HasAbba(Part1) || HasAbba(Part3)) && !HasAbba(Part2);
            }

            private static readonly Regex AbbaRegex = new Regex(@"(.)(.)(\2)(\1)");
            private static bool HasAbba(string part)
            {
                var match = AbbaRegex.Match(part);
                if (!match.Success)
                    return false;

                return (match.Groups[1].Value != match.Groups[2].Value);
            }
        } 
    }
}
