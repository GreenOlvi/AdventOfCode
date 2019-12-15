using AoC2019.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AoC2019.Puzzle13
{
    public partial class Solution : IPuzzle
    {
        public Solution(string input)
        {
            _input = ParseInput(input);
        }

        private readonly long[] _input;

        public static long[] ParseInput(string input) =>
            input.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(long.Parse)
                .ToArray();

        private static IEnumerable<Tile> ToTiles(IEnumerable<long> input)
        {
            var e = input.GetEnumerator();
            while (e.MoveNext())
            {
                var x = e.Current;
                e.MoveNext();
                var y = e.Current;
                e.MoveNext();
                var b = e.Current;
                yield return new Tile((int)x, (int)y, (int)b);
            }
        }

        private static void Draw(Screen screen)
        {
            Console.Clear();
            Console.WriteLine(screen.Draw());
        }
        
        private static long ManualInput()
        {
            var key = Console.ReadKey();
            return key.Key switch
            {
                ConsoleKey.LeftArrow => -1,
                ConsoleKey.RightArrow => 1,
                _ => 0,
            };
        }

        private static long AutoInput(Screen screen)
        {
            if (screen.Paddle.X > screen.Ball.X)
            {
                return -1;
            }
            if (screen.Paddle.X < screen.Ball.X)
            {
                return 1;
            }
            return 0;
        }

        private static int Solve1(IEnumerable<long> input)
        {
            var m = new IntcodeMachine(input);
            m.Run();

            var blocks = ToTiles(m.GetAllOutput()).ToList();
            return blocks.Count(b => b.Type == 2);
        }

        private static long Solve2(IEnumerable<long> input)
        {
            var modifiedInput = new[] { 2L }.Concat(input.Skip(1));
            var m = new IntcodeMachine(modifiedInput);
            m.Run();

            var screen = new Screen(ToTiles(m.GetAllOutput()));
            Draw(screen);

            while (!m.IsHalted)
            {
                //var i = ManualInput();
                var i = AutoInput(screen);
                m.AddInputAndRun(i);
                screen.Update(ToTiles(m.GetAllOutput()));
                //Draw(screen);
                //Thread.Sleep(100);
            }

            return screen.Score;
        }

        public Task<string> Solve1Async() =>
            Task.Run(() => Solve1(_input).ToString());

        public Task<string> Solve2Async() =>
            Task.Run(() => Solve2(_input).ToString());
    }
}
