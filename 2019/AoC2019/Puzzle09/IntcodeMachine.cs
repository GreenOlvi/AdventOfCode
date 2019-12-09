using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2019.Puzzle09
{
    public class IntcodeMachine
    {
        public IntcodeMachine(IEnumerable<long> memory)
        {
            _memory = new Memory(memory);
            _input = new Queue<long>();
            Output = new Queue<long>();
            IsHalted = false;
        }

        private readonly Memory _memory;
        private int _ip = 0;
        private bool _run;
        private long _relativeBase = 0L;
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
            { 9, 1 },
            { 99, 0 },
        };

        public static (long, ParamMode[]) Decode(long instruction)
        {
            var opcode = instruction % 100;

            var par = instruction / 100;
            var param = new List<ParamMode>();
            foreach (var i in Enumerable.Range(0, InstructionParamCount[opcode]))
            {
                param.Add((ParamMode)(par % 10));
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
                        var a = ParamReader(_ip + 1, paramMode[0]);
                        var b = ParamReader(_ip + 2, paramMode[1]);
                        ParamWriter(_ip + 3, paramMode[2], a + b);
                        _ip += 1 + pCount;
                        break;
                    }
                case 2:
                    {   //Multiply
                        var a = ParamReader(_ip + 1, paramMode[0]);
                        var b = ParamReader(_ip + 2, paramMode[1]);
                        ParamWriter(_ip + 3, paramMode[2], a * b);
                        _ip += 1 + pCount;
                        break;
                    }
                case 3:
                    {   //Input
                        if (_input.Any())
                        {
                            ParamWriter(_ip + 1, paramMode[0], _input.Dequeue());
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
                        ParamWriter(_ip + 3, paramMode[2], a < b ? 1 : 0);
                        _ip += 1 + pCount;
                        break;
                    }
                case 8:
                    {   //Equals
                        var a = ParamReader(_ip + 1, paramMode[0]);
                        var b = ParamReader(_ip + 2, paramMode[1]);
                        ParamWriter(_ip + 3, paramMode[2], a == b ? 1 : 0);
                        _ip += 1 + pCount;
                        break;
                    }
                case 9:
                    {   //Adjust relative base
                        var a = ParamReader(_ip + 1, paramMode[0]);
                        _relativeBase += a;
                        _ip += 1 + pCount;
                        break;
                    }
                case 99:
                    {   //Halt
                        _run = false;
                        IsHalted = true;
                        _ip += 1 + pCount;
                        break;
                    }
                default:
                    throw new ArgumentException($"Invalid opcode {opcode}");
            }
        }

        private long ParamReader(int pos, ParamMode mode)
        {
            var par = _memory[pos];
            return mode switch
            {
                ParamMode.Position => _memory[par],
                ParamMode.Immediate => par,
                ParamMode.Relative => _memory[_relativeBase + par],
                _ => throw new ArgumentOutOfRangeException(nameof(mode)),
            };
        }

        private void ParamWriter(int pos, ParamMode mode, long value)
        {
            var par = _memory[pos];
            var addr = mode switch
            {
                ParamMode.Position => par,
                ParamMode.Immediate => throw new InvalidOperationException("Cannot use ParamMode.Immediate to write"),
                ParamMode.Relative => _relativeBase + par,
                _ => throw new ArgumentOutOfRangeException(nameof(mode)),
            };
            _memory[addr] = value;
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
