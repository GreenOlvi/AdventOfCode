using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AoC2019.Puzzle02
{
    public class Solution : IPuzzle
    {
        public Solution(string input)
        {
            _input = ParseInput(input).ToList();
        }

        private static IEnumerable<long> ParseInput(string input) =>
            input.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse);

        private readonly List<long> _input;

        public static void RunInstruction(ref long[] memory, ref int ip, ref bool run)
        {
            var opcode = memory[ip];
            switch (opcode)
            {
                case 1:
                    var a1 = memory[ip + 1];
                    var a2 = memory[ip + 2];
                    var a3 = memory[ip + 3];
                    memory[a3] = memory[a1] + memory[a2];
                    ip += 4;
                    break;
                case 2:
                    var b1 = memory[ip + 1];
                    var b2 = memory[ip + 2];
                    var b3 = memory[ip + 3];
                    memory[b3] = memory[b1] * memory[b2];
                    ip += 4;
                    break;
                case 99:
                    run = false;
                    ip += 1;
                    break;
                default:
                    throw new ArgumentException($"Invalid opcode {opcode}");
            }
        }

        public static long Run(long[] input, long noun, long verb)
        {
            var mem = new long[input.Length];
            Array.Copy(input, mem, input.Length);

            mem[1] = noun;
            mem[2] = verb;

            var ip = 0;
            var run = true;

            while (run)
            {
                RunInstruction(ref mem, ref ip, ref run);
            }

            return mem[0];
        }

        public static long Solve1(IEnumerable<long> input, long noun, long verb)
        {
            return Run(input.ToArray(), noun, verb);
        }

        public static long Solve2(IEnumerable<long> input, long searched)
        {
            var mem = input as long[] ?? input.ToArray();

            var tries = 0;
            foreach (var noun in Enumerable.Range(0, 99))
            {
                foreach (var verb in Enumerable.Range(0, 99))
                {
                    try
                    {
                        var result = Run(mem, noun, verb);
                        if (result == searched)
                        {
                            Console.WriteLine($"{tries} tries.");
                            return 100 * noun + verb;
                        }
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        Console.WriteLine($"[{noun}, {verb}] Out of range: {e.Message}");
                    }
                    tries++;
                }
            }

            throw new InvalidOperationException("Values not found");
        }

        public async Task<string> Solve1Async() =>
            await Task.Run(() => Solve1(_input, 12, 2).ToString());

        public async Task<string> Solve2Async() =>
            await Task.Run(() => Solve2(_input, 19690720).ToString());
    }
}
