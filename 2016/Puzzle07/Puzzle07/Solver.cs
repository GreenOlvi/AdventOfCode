using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text.RegularExpressions;

namespace Puzzle07
{
    public class Solver
    {
        public Solver(params string[] input)
        {
            Ips = input.Select(ip => new Ipv7(ip));
        }

        public IEnumerable<Ipv7> Ips { get; private set; }

        public int Solve1()
        {
            return Ips.Count(ip => ip.SupportsTls());
        }

        public int Solve2()
        {
            return Ips.Count(ip => ip.SupportsSsl());
        }

        public class Ipv7
        {
            public Ipv7(string ip)
            {
                Ip = ip;
                var parts = ip.Split('[', ']');

                PartsInside = new List<string>();
                PartsOutside = new List<string>();

                var i = 0;
                foreach (var part in parts)
                {
                    if (i%2 == 0)
                    {
                        PartsOutside.Add(part);
                    }
                    else
                    {
                        PartsInside.Add(part);
                    }
                    i++;
                }

            }

            public string Ip { get; }

            public List<string> PartsOutside { get; }
            public List<string> PartsInside { get; }

            public bool SupportsTls()
            {
                return PartsOutside.Any(p => HasAbba(p)) && !PartsInside.Any(p => HasAbba(p));
            }

            private static readonly Regex AbaBabRegex = new Regex(@"(\w)(\w)(\1).*=.*(\2)(\1)(\2)");
            public bool SupportsSsl()
            {
                var connected = String.Join("-", PartsOutside) + "=" + String.Join("-", PartsInside);
                return AbaBabRegex.IsMatch(connected);
            }

            private static readonly Regex AbbaRegex = new Regex(@"(\w)(\w)(\2)(\1)");
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
