using System;
using System.Collections.Generic;
using System.Linq;

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

        public int Moves { get; set; } = 100;

        private static List<int> PickUp(List<int> list, int current, out int[] picked)
        {
            picked = list.Skip(current + 1).Take(3).ToArray();
            list.RemoveRange(current + 1, 3);
            return list;
        }

        private static int PickDestination(List<int> list, int current)
        {
            var curr = list[current] - 1;
            while (curr >= list.Min())
            {
                if (list.Contains(curr))
                {
                    return list.IndexOf(curr);
                }
                curr--;
            }
            return list.IndexOf(list.Max());
        }

        private static List<int> InsertAfter(List<int> list, int destination, int[] elements)
        {
            list.InsertRange(destination + 1, elements);
            return list;
        }

        private static List<int> ReindexList(List<int> list, int newStart)
        {
            var wrapped = list.Take(newStart).ToArray();
            list.RemoveRange(0, newStart);
            list.AddRange(wrapped);
            return list;
        }

        private static string PrintResult(List<int> list)
        {
            var copy = new List<int>(list);
            ReindexList(copy, copy.IndexOf(1));
            return string.Join("", copy.Skip(1));
        }

        public override string Solution1()
        {
            var list = new List<int>(_input);
            var currentIndex = 0;

            for (var i = 0; i < Moves; i++)
            {
                list = PickUp(list, currentIndex, out var picked);
                var destination = PickDestination(list, currentIndex);
                list = InsertAfter(list, destination, picked);
                list = ReindexList(list, 1);
            }

            return PrintResult(list);
        }

        public override long Solution2()
        {
            return 0;
        }
    }
}
