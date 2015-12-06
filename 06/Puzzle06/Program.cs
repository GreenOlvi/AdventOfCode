using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Puzzle06
{
    class Program
    {
        private static readonly Regex CommandRegex =
            new Regex(@"^(turn on|turn off|toggle) (\d+),(\d+) through (\d+),(\d+)$");

        static void Main(string[] args)
        {
            foreach (var filename in args)
            {
                var grid1 = new BoolGrid();
                var grid2 = new IntGrid();

                var input = GetInput(filename);
                foreach (var match in input.Select(x => CommandRegex.Match(x)).Where(x => x.Success))
                {
                    var command = ParseCommand(match.Groups[1].Value);
                    var x1 = int.Parse(match.Groups[2].Value);
                    var y1 = int.Parse(match.Groups[3].Value);
                    var x2 = int.Parse(match.Groups[4].Value);
                    var y2 = int.Parse(match.Groups[5].Value);

                    grid1.RunCommand(command, x1, y1, x2, y2);
                    grid2.RunCommand(command, x1, y1, x2, y2);
                }
                
                Console.WriteLine(@"Grid 1: {0}", grid1.CountLit());
                Console.WriteLine(@"Grid 2: {0}", grid2.CountLit());
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

        private enum Command { TurnOn, TurnOff, Toggle }
        private static Command ParseCommand(string command) {
            switch (command)
            {
                case "turn on":
                    return Command.TurnOn;
                case "turn off":
                    return Command.TurnOff;
                case "toggle":
                    return Command.Toggle;
                default:
                    throw new ArgumentException("Invalid command!");
            }
        }

        private abstract class Grid
        {
            protected const int Side = 1000;

            public abstract int CountLit();
            public void RunCommand(Command command, int x1, int y1, int x2, int y2)
            {
                switch (command)
                {
                    case Command.TurnOn:
                        TurnOn(x1, y1, x2, y2);
                        break;
                    case Command.TurnOff:
                        TurnOff(x1, y1, x2, y2);
                        break;
                    case Command.Toggle:
                        Toggle(x1, y1, x2, y2);
                        break;
                }
            }

            protected abstract void TurnOn(int x1, int y1, int x2, int y2);
            protected abstract void TurnOff(int x1, int y1, int x2, int y2);
            protected abstract void Toggle(int x1, int y1, int x2, int y2);

            private int Index(int x, int y)
            {
                return x + y * Side;   
            }
            protected IEnumerable<int> GetRect(int x1, int y1, int x2, int y2)
            {
                var startX = x1 <= x2 ? x1 : x2;
                var countX = Math.Abs(x1 - x2) + 1;
                var startY = y1 <= y2 ? y1 : y2;
                var countY = Math.Abs(y1 - y2) + 1;

                return from y in Enumerable.Range(startY, countY)
                       from x in Enumerable.Range(startX, countX)
                       select Index(x, y);
            } 
        }

        private class BoolGrid : Grid
        {
            private bool[] Lights { get; } = new bool[Side * Side];

            public override int CountLit()
            {
                return Lights.Count(x => x);
            }

            protected override void TurnOn(int x1, int y1, int x2, int y2)
            {
                GetRect(x1, y1, x2, y2).ToList().ForEach(i => Lights[i] = true);
            }

            protected override void TurnOff(int x1, int y1, int x2, int y2)
            {
                GetRect(x1, y1, x2, y2).ToList().ForEach(i => Lights[i] = false);
            }

            protected override void Toggle(int x1, int y1, int x2, int y2)
            {
                GetRect(x1, y1, x2, y2).ToList().ForEach(i => Lights[i] = !Lights[i]);
            }
        }

        private class IntGrid : Grid
        {
            private int[] Lights { get; } = new int[Side * Side];

            public override int CountLit()
            {
                return Lights.Sum();
            }

            protected override void TurnOn(int x1, int y1, int x2, int y2)
            {
                GetRect(x1, y1, x2, y2).ToList().ForEach(i => Lights[i]++);
            }

            protected override void TurnOff(int x1, int y1, int x2, int y2)
            {
                GetRect(x1, y1, x2, y2).Where(x => Lights[x] > 0).ToList().ForEach(i => Lights[i]--);
            }

            protected override void Toggle(int x1, int y1, int x2, int y2)
            {
                GetRect(x1, y1, x2, y2).ToList().ForEach(i => Lights[i] += 2);
            }
        }
    }
}
