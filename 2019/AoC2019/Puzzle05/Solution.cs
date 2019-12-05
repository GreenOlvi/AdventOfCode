using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AoC2019.Puzzle05
{
    public class Solution : IPuzzle
    {
        public Solution(string input)
        {
            _input = ParseInput(input).ToList();
        }

        private readonly List<long> _input;

        private static IEnumerable<long> ParseInput(string input) =>
            input.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse);

        private static readonly Dictionary<long, int> InstructionParamCount = new Dictionary<long, int>()
        {
            { 1, 3 },
            { 2, 3 },
            { 3, 1 },
            { 4, 1 },
            { 5, 2 },
            { 6, 2 },
            { 7, 3 },
            { 8, 3 },
            { 99, 0 },
        };

        public static (long, bool[]) Decode(long instruction)
        {
            var opcode = instruction % 100;

            var par = instruction / 100;
            List<bool> param = new List<bool>();
            foreach (var i in Enumerable.Range(0, InstructionParamCount[opcode]))
            {
                param.Add(par % 10 != 0);
                par /= 10;
            }

            return (opcode, param.ToArray());
        }

        public static void RunInstruction(ref long[] memory, ref int ip, ref bool run, Queue<long> input, Queue<long> output)
        {
            var (opcode, paramMode) = Decode(memory[ip]);
            var pCount = InstructionParamCount[opcode];
            switch (opcode)
            {
                case 1:
                    {
                        var a1 = ParamReader(ref memory, ip + 1, paramMode[0]);
                        var a2 = ParamReader(ref memory, ip + 2, paramMode[1]);
                        memory[memory[ip + 3]] = a1 + a2;
                        ip += 1 + pCount;
                        break;
                    }
                case 2:
                    {
                        var b1 = ParamReader(ref memory, ip + 1, paramMode[0]);
                        var b2 = ParamReader(ref memory, ip + 2, paramMode[1]);
                        memory[memory[ip + 3]] = b1 * b2;
                        ip += 1 + pCount;
                        break;
                    }
                case 3:
                    {
                        memory[memory[ip + 1]] = input.Dequeue();
                        ip += 1 + pCount;
                        break;
                    }
                case 4:
                    {
                        var d = ParamReader(ref memory, ip + 1, paramMode[0]);
                        output.Enqueue(d);
                        ip += 1 + pCount;
                        break;
                    }
                case 5:
                    {
                        var a = ParamReader(ref memory, ip + 1, paramMode[0]);
                        var t = ParamReader(ref memory, ip + 2, paramMode[1]);
                        ip = a != 0 ? (int)t : (ip + 1 + pCount);
                        break;
                    }
                case 6:
                    {
                        var a = ParamReader(ref memory, ip + 1, paramMode[0]);
                        var t = ParamReader(ref memory, ip + 2, paramMode[1]);
                        ip = a == 0 ? (int)t : (ip + 1 + pCount);
                        break;
                    }
                case 7:
                    {
                        var a = ParamReader(ref memory, ip + 1, paramMode[0]);
                        var b = ParamReader(ref memory, ip + 2, paramMode[1]);
                        var t = memory[ip + 3];
                        memory[t] = a < b ? 1 : 0;
                        ip += 1 + pCount;
                        break;
                    }
                case 8:
                    {
                        var a = ParamReader(ref memory, ip + 1, paramMode[0]);
                        var b = ParamReader(ref memory, ip + 2, paramMode[1]);
                        var t = memory[ip + 3];
                        memory[t] = a == b ? 1 : 0;
                        ip += 1 + pCount;
                        break;
                    }
                case 99:
                    run = false;
                    ip += 1 + InstructionParamCount[opcode];
                    break;
                default:
                    throw new ArgumentException($"Invalid opcode {opcode}");
            }
        }

        private static long ParamReader(ref long[] memory, int pos, bool mode)
        {
            var par = memory[pos];
            return mode == false ? memory[par] : par;
        }

        public static (long, Queue<long>) Run(long[] memory, long[] input)
        {
            var mem = new long[memory.Length];
            Array.Copy(memory, mem, memory.Length);

            var inp = new Queue<long>(input);
            var outp = new Queue<long>();

            var ip = 0;
            var run = true;

            while (run)
            {
                RunInstruction(ref mem, ref ip, ref run, inp, outp);
            }

            return (mem[0], outp);
        }

        public static long Solve1(IEnumerable<long> input)
        {
            var (_, outp) = Run(input.ToArray(), new long[] { 1 });
            return outp.Last();
        }

        public static long Solve2(IEnumerable<long> input)
        {
            var (_, outp) = Run(input.ToArray(), new long[] { 5 });
            return outp.Last();
        }

        public Task<string> Solve1Async() =>
            Task.Run(() => Solve1(_input).ToString());

        public Task<string> Solve2Async() =>
            Task.Run(() => Solve2(_input).ToString());
    }
}
