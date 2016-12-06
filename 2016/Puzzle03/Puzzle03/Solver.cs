using System;
using System.Collections.Generic;
using System.Linq;

namespace Puzzle03
{
    public class Solver
    {
        public Solver(params string[] input)
        {
            Input = input;
        }

        public string[] Input { get; }

        public int Solve1()
        {
            return ParseInput1().Count(t => t.IsValid());
        }

        public int Solve2()
        {
            return ParseInput2().Count(t => t.IsValid());
        }

        private IEnumerable<Triangle> ParseInput1()
        {
            return Input.Select(line => ParseInputLine(line))
                .Select(vals => new Triangle(vals[0], vals[1], vals[2]));
        }

        private IEnumerable<Triangle> ParseInput2()
        {
            var buffer = new List<int[]>(3);
            foreach (var row in Input.Select(line => ParseInputLine(line)))
            {
                buffer.Add(row);

                if (buffer.Count == 3)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        yield return new Triangle(buffer[0][i], buffer[1][i], buffer[2][i]);
                    }
                    buffer.Clear();
                }
            }
        }

        private int[] ParseInputLine(string line)
        {
            return line.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => int.Parse(s))
                .ToArray();
        }

        private struct Triangle
        {
            public Triangle(int a, int b, int c)
            {
                A = a;
                B = b;
                C = c;
            }

            public int A { get; private set; }
            public int B { get; private set; }
            public int C { get; private set; }

            public bool IsValid()
            {
                return (A + B > C) && (B + C > A) && (C + A > B);
            }
        }
    }
}
