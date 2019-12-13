using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2019.Common
{
    public class IntcodeMachine
    {
        public IntcodeMachine(IEnumerable<long> memory)
        {
            _memory = new Memory(memory);
            _input = new Queue<long>();
            _output = new Queue<long>();
            IsHalted = false;
        }

        private readonly Memory _memory;
        private int _ip = 0;
        private bool _run;
        private long _relativeBase = 0L;
        private readonly Queue<long> _input;
        private readonly Queue<long> _output;

        public bool WaitingForInput { get; private set; }
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
            
            if (!InstructionParamCount.ContainsKey(opcode))
            {
                throw new ArgumentException($"Invalid opcode [{opcode}]", nameof(instruction));
            }

            var par = instruction / 100;
            var param = new List<ParamMode>();
            foreach (var i in Enumerable.Range(0, InstructionParamCount[opcode]))
            {
                param.Add((ParamMode)(par % 10));
                par /= 10;
            }

            return (opcode, param.ToArray());
        }

        public int OpcodeAdd(int ip, ParamMode[] paramMode)
        {
            var a = ParamReader(ip + 1, paramMode[0]);
            var b = ParamReader(ip + 2, paramMode[1]);
            ParamWriter(ip + 3, paramMode[2], a + b);
            return ip + 4;
        }

        public int OpcodeMultiply(int ip, ParamMode[] paramMode)
        {
            var a = ParamReader(ip + 1, paramMode[0]);
            var b = ParamReader(ip + 2, paramMode[1]);
            ParamWriter(ip + 3, paramMode[2], a * b);
            return ip + 4;
        }

        public int OpcodeInput(int ip, ParamMode[] paramMode)
        {
            if (_input.Any())
            {
                ParamWriter(ip + 1, paramMode[0], _input.Dequeue());
                return ip + 2;
            }
            else
            {
                _run = false;
                WaitingForInput = true;
                return ip;
            }
        }

        public int OpcodeOutput(int ip, ParamMode[] paramMode)
        {
            var d = ParamReader(ip + 1, paramMode[0]);
            _output.Enqueue(d);
            return ip + 2;
        }

        public int OpcodeJumpIfTrue(int ip, ParamMode[] paramMode)
        {
            var a = ParamReader(ip + 1, paramMode[0]);
            var t = ParamReader(ip + 2, paramMode[1]);
            return a != 0 ? (int)t : (ip + 3);
        }

        public int OpcodeJumpIfFalse(int ip, ParamMode[] paramMode)
        {
            var a = ParamReader(ip + 1, paramMode[0]);
            var t = ParamReader(ip + 2, paramMode[1]);
            return a == 0 ? (int)t : (ip + 3);
        }

        public int OpcodeLessThan(int ip, ParamMode[] paramMode)
        {
            var a = ParamReader(ip + 1, paramMode[0]);
            var b = ParamReader(ip + 2, paramMode[1]);
            ParamWriter(ip + 3, paramMode[2], a < b ? 1 : 0);
            return ip + 4;
        }

        public int OpcodeEquals(int ip, ParamMode[] paramMode)
        {
            var a = ParamReader(ip + 1, paramMode[0]);
            var b = ParamReader(ip + 2, paramMode[1]);
            ParamWriter(ip + 3, paramMode[2], a == b ? 1 : 0);
            return ip + 4;
        }

        public int OpcodeAdjustRelativeBase(int ip, ParamMode[] paramMode)
        {
            var a = ParamReader(ip + 1, paramMode[0]);
            _relativeBase += a;
            return ip + 2;
        }

        public int OpcodeHalt(int ip, ParamMode[] paramMode)
        {
            _run = false;
            IsHalted = true;
            return ip + 1;
        }

        private int RunInstruction(int ip)
        {
            var (opcode, paramMode) = Decode(_memory[ip]);
            var newIp = opcode switch
            {
                1 => OpcodeAdd(ip, paramMode),
                2 => OpcodeMultiply(ip, paramMode),
                3 => OpcodeInput(ip, paramMode),
                4 => OpcodeOutput(ip, paramMode),
                5 => OpcodeJumpIfTrue(ip, paramMode),
                6 => OpcodeJumpIfFalse(ip, paramMode),
                7 => OpcodeLessThan(ip, paramMode),
                8 => OpcodeEquals(ip, paramMode),
                9 => OpcodeAdjustRelativeBase(ip, paramMode),
                99 => OpcodeHalt(ip, paramMode),
                _ => throw new ArgumentException($"Invalid opcode {opcode}"),
            };
            return newIp;
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
            WaitingForInput = false;
        }

        public long GetOutput() => _output.Dequeue();
        public IEnumerable<long> GetAllOutput()
        {
            while (_output.Any())
            {
                yield return _output.Dequeue();
            }
        }

        public void Run()
        {
            _run = true;
            while (_run)
            {
                _ip = RunInstruction(_ip);
            }
        }

    }
}
