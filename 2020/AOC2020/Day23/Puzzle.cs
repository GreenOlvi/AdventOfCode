using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AOC2020.Day23
{
    public class Puzzle : PuzzleBase<string, long>
    {
        public Puzzle(string input)
        {
            _input = ParseInput(input);
        }

        private static int[] ParseInput(string input) => input.Select(c => c - '0').ToArray();

        private readonly int[] _input;

        private const int Moves = 100;
        private const long ExtendedMoves = 10_000_000;

        private static IEnumerable<int> ExtendedInput(int[] input)
        {
            foreach (var i in input)
            {
                yield return i;
            }

            for (int i = input.Length + 1; i <= 1_000_000; i++)
            {
                yield return i;
            }
        }

        private static int PickDestination(int[] s, int current, int p)
        {
            var dest = current - 1;
            while (dest == 0 || p == dest || s[p] == dest || s[s[p]] == dest)
            {
                dest--;
                if (dest <= 0)
                {
                    dest = s.Length - 1;
                }
            }
            return dest;
        }

        private static int[] PlayFaster(IEnumerable<int> input, long moves)
        {
            var s = BuildSuccessorList(input);
            var current = input.First();

            for (var i = 0L; i < moves; i++)
            {
                var p1 = s[current];        // first picked
                var p3 = s[s[s[current]]];  // last picked

                s[current] = s[p3];         // snip

                var destination = PickDestination(s, current, p1);

                s[p3] = s[destination];     // paste
                s[destination] = p1;

                current = s[current];
            }

            return s;
        }

        public static int[] BuildSuccessorList(IEnumerable<int> input)
        {
            var tmp = input.ToArray();
            var s = new int[tmp.Length + 1];

            s[0] = -1;
            for (var i = 0; i < tmp.Length - 1; i++)
            {
                s[tmp[i]] = tmp[i + 1];
            }
            s[tmp[^1]] = tmp[0];

            return s;
        }

        private static string PrintSuccessorList(int[] list, int start = 1, int count = 10, string? separator = default)
        {
            var sb = new StringBuilder();
            var curr = start;

            for (var i = 0; i < count - 1; i++)
            {
                sb.Append(curr);
                if (separator != null)
                {
                    sb.Append(separator);
                }
                curr = list[curr];
            }
            sb.Append(curr);

            return sb.ToString();
        }

        public override string Solution1()
        {
            var list = PlayFaster(_input, Moves);
            return PrintSuccessorList(list, list[1], 8);
        }

        public override long Solution2()
        {
            var list = PlayFaster(ExtendedInput(_input), ExtendedMoves);
            return list[1] * (long)list[list[1]];
        }
    }
}
