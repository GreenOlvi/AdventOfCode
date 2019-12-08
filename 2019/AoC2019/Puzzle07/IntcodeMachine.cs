using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2019.Puzzle07
{
    public class IntcodeMachine
    {
        public IntcodeMachine(ref long[] memory, IEnumerable<long> input)
        {
            _memory = memory;
            _ip = 0;
            _input = new Queue<long>(input);
            Output = new Queue<long>();
            IsHalted = false;
        }

        public IntcodeMachine(ref long[] memory)
        {
            _memory = memory;
            _ip = 0;
            _input = new Queue<long>();
            Output = new Queue<long>();
            IsHalted = false;
        }

        private readonly long[] _memory;
        private int _ip;
        private bool _run;
        private readonly Queue<long> _input;

        public Queue<long> Output { get; }
        public bool IsHalted { get; private set; }

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

        public void RunInstruction()
        {
            var (opcode, paramMode) = Decode(_memory[_ip]);
            var pCount = InstructionParamCount[opcode];
            switch (opcode)
            {
                case 1:
                    {   //Add
                        var a1 = ParamReader(_ip + 1, paramMode[0]);
                        var a2 = ParamReader(_ip + 2, paramMode[1]);
                        _memory[_memory[_ip + 3]] = a1 + a2;
                        _ip += 1 + pCount;
                        break;
                    }
                case 2:
                    {   //Multiply
                        var b1 = ParamReader(_ip + 1, paramMode[0]);
                        var b2 = ParamReader(_ip + 2, paramMode[1]);
                        _memory[_memory[_ip + 3]] = b1 * b2;
                        _ip += 1 + pCount;
                        break;
                    }
                case 3:
                    {   //Input
                        if (_input.Any())
                        {
                            _memory[_memory[_ip + 1]] = _input.Dequeue();
                            _ip += 1 + pCount;
                        }
                        else
                        {
                            _run = false;
                        }
                        break;
                    }
                case 4:
                    {   //Output
                        var d = ParamReader(_ip + 1, paramMode[0]);
                        Output.Enqueue(d);
                        _ip += 1 + pCount;
                        break;
                    }
                case 5:
                    {   //Jump if true
                        var a = ParamReader(_ip + 1, paramMode[0]);
                        var t = ParamReader(_ip + 2, paramMode[1]);
                        _ip = a != 0 ? (int)t : (_ip + 1 + pCount);
                        break;
                    }
                case 6:
                    {   //Jump if false
                        var a = ParamReader(_ip + 1, paramMode[0]);
                        var t = ParamReader(_ip + 2, paramMode[1]);
                        _ip = a == 0 ? (int)t : (_ip + 1 + pCount);
                        break;
                    }
                case 7:
                    {   //Less than
                        var a = ParamReader(_ip + 1, paramMode[0]);
                        var b = ParamReader(_ip + 2, paramMode[1]);
                        var t = _memory[_ip + 3];
                        _memory[t] = a < b ? 1 : 0;
                        _ip += 1 + pCount;
                        break;
                    }
                case 8:
                    {   //Equals
                        var a = ParamReader(_ip + 1, paramMode[0]);
                        var b = ParamReader(_ip + 2, paramMode[1]);
                        var t = _memory[_ip + 3];
                        _memory[t] = a == b ? 1 : 0;
                        _ip += 1 + pCount;
                        break;
                    }
                case 99:
                    {   //Halt
                        _run = false;
                        IsHalted = true;
                        _ip += 1 + InstructionParamCount[opcode];
                        break;
                    }
                default:
                    throw new ArgumentException($"Invalid opcode {opcode}");
            }
        }

        private long ParamReader(int pos, bool mode)
        {
            var par = _memory[pos];
            return mode == false ? _memory[par] : par;
        }

        public void AddInput(long input)
        {
            _input.Enqueue(input);
        }

        public void Run()
        {
            _run = true;
            while (_run)
            {
                RunInstruction();
            }
        }

    }
}
