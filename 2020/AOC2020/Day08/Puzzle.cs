using System.Collections.Generic;
using System.Linq;

namespace AOC2020.Day08
{
    public class Puzzle : PuzzleBase<int, int>
    {
        public Puzzle(IEnumerable<string> input)
        {
            _input = input.Select(ParseInstruction).ToArray();
        }

        private readonly (Instruction, int)[] _input;

        private static (Instruction, int) ParseInstruction(string line)
        {
            var parts = line.Split(" ");
            var i = parts[0] switch
            {
                "acc" => Instruction.Acc,
                "jmp" => Instruction.Jmp,
                "nop" => Instruction.Nop,
                _ => throw new PuzzleException($"Invalid input [{parts[0]}]"),
            };

            if (!int.TryParse(parts[1], out var num))
            {
                throw new PuzzleException($"Invalid number [{parts[1]}]");
            }

            return (i, num);
        }

        private static bool TryRun((Instruction, int)[] instructions, out int ret)
        {
            var done = new HashSet<int>();
            var ip = 0;
            var acc = 0;
            while (true)
            {
                var (i, num) = instructions[ip];
                var oldAcc = acc;
                switch (i)
                {
                    case Instruction.Acc:
                        acc += num;
                        ip++;
                        break;
                    case Instruction.Jmp:
                        ip += num;
                        break;
                    case Instruction.Nop:
                        ip++;
                        break;
                }

                if (ip >= instructions.Length)
                {
                    ret = acc;
                    return true;
                }

                if (done.Contains(ip))
                {
                    ret = oldAcc;
                    return false;
                }

                done.Add(ip);
            }
        }

        public override int Solution1()
        {
            TryRun(_input, out var acc);
            return acc;
        }

        public override int Solution2()
        {
            for (var i = 0; i < _input.Length; i++)
            {
                var ins = _input[i].Item1;
                if (ins == Instruction.Acc)
                {
                    continue;
                }

                var copy = new (Instruction, int)[_input.Length];
                _input.CopyTo(copy, 0);

                if (ins == Instruction.Jmp)
                {
                    copy[i] = (Instruction.Nop, _input[i].Item2);
                }

                if (ins == Instruction.Nop)
                {
                    copy[i] = (Instruction.Jmp, _input[i].Item2);
                }

                if (TryRun(copy, out var acc))
                {
                    return acc;
                }
            }
            return 0;
        }

        public enum Instruction { Acc, Jmp, Nop }
    }
}
