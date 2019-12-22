using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2019.Puzzle20
{
    public class Solution : IPuzzle
    {
        public Solution(IEnumerable<string> input)
        {
            _input = input.ToArray();
        }

        private readonly string[] _input;

        public static int Solve1(string[] input)
        {
            return 0;
        }

        public Task<string> Solve1Async() =>
            Task.Run(() => Solve1(_input).ToString());

        public Task<string> Solve2Async()
        {
            throw new NotImplementedException();
        }
    }

    public class Map
    {
        public Map(string[] input)
        {
            var top = input[0].IndexOf('#') == -1 ? 2 : 0;
            var left = input[top].IndexOf('#');
            var right = input[top].LastIndexOf('#');
            var bottom = input.Select((l, i) => (l, i)).Last(line => line.l.Contains('#')).i;
            _width = right - left;
            _height = bottom - top;
            _map = new char[_width * _height];

            foreach (var y in Enumerable.Range(0, _height))
            {
                foreach (var x in Enumerable.Range(0, _width))
                {
                    var c = input[y + top][x + left];
                    switch (c)
                    {
                        case '#':
                            _map[x + y * _width] = '#';
                            break;
                        case '.':
                            _map[x + y * _width] = '.';
                    }
                }
            }
        }

        private readonly char[] _map;
        private readonly int _width;
        private readonly int _height;
    }
}
