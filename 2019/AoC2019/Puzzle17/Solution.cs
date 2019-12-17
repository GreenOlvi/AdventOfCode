using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AoC2019.Common;

namespace AoC2019.Puzzle17
{
    public class Solution : IPuzzle
    {
        public Solution(string input)
        {
            _input = IntcodeMachine.ParseInput(input).ToArray();
        }

        private readonly long[] _input;

        public static long Solve1(IEnumerable<long> input)
        {
            var m = new IntcodeMachine(input);
            m.Run();

            var grid = new Grid(m.GetAllOutput().Select(o => (char)o));
            return grid.GetIntersections().Select(p => p.X * p.Y).Sum();
        }

        public static long Solve2(IEnumerable<long> input)
        {
            var mod = new[] { 2L }.Concat(input.Skip(1));
            var m = new IntcodeMachine(mod);
            var r = new Robot(m);

            var path = r.FindPath().ToArray();

            var pathString = string.Join(",", path);
            Console.WriteLine(pathString);

            //var c = new Compressor();
            //var compressed = c.Compress(path);

            //Console.WriteLine(r.Draw());

            m.AddInput("A,B,A,C,A,B,C,C,A,B\n");
            m.AddInput("R,8,L,10,R,8\n");
            m.AddInput("R,12,R,8,L,8,L,12\n");
            m.AddInput("L,12,L,10,L,8\n");
            m.AddInput("y\n");
            m.Run();

            var o = m.GetAllOutput().ToList();
            var dust = o[o.Count - 1];
            o.RemoveAt(o.Count - 1);

            Console.WriteLine(o.Select(c => (char)c).ToArray());

            return dust;
        }

        public Task<string> Solve1Async() =>
            Task.Run(() => Solve1(_input).ToString());

        public Task<string> Solve2Async() =>
            Task.Run(() => Solve2(_input).ToString());
    }
}
