using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace Puzzle12
{
    public class Solver
    {
        public Solver(params string[] input)
        {
            instructions = input.Select(line => ParseLine(line)).ToList();
        }

        private IList<Instruction> instructions;

        public int Solve1()
        {
            var machine = new Machine(instructions);
            machine.Run();
            return machine.GetRegisterValue(Register.a);
        }

        public int Solve2()
        {
            var machine = new Machine(instructions);
            machine.SetRegisterValue(Register.c, 1);
            machine.Run();
            return machine.GetRegisterValue(Register.a);
        }

        private static readonly Regex CpyRegex = new Regex(@"cpy (-?\d+|\w) (\w)", RegexOptions.Compiled);
        private static readonly Regex IncRegex = new Regex(@"inc (\w)", RegexOptions.Compiled);
        private static readonly Regex DecRegex = new Regex(@"dec (\w)", RegexOptions.Compiled);
        private static readonly Regex JnzRegex = new Regex(@"jnz (-?\d+|\w) (-?\d+)", RegexOptions.Compiled);

        private Instruction ParseLine(string line)
        {
            if (CpyRegex.IsMatch(line))
            {
                var match = CpyRegex.Match(line);
                var arg1 = ParseArgument(match.Groups[1].Value);
                var arg2 = ParseArgument(match.Groups[2].Value);
                return new Instruction(InstructionType.Cpy, arg1, arg2);
            }

            if (IncRegex.IsMatch(line))
            {
                var match = IncRegex.Match(line);
                var arg = ParseArgument(match.Groups[1].Value);
                return new Instruction(InstructionType.Inc, arg);
            }

            if (DecRegex.IsMatch(line))
            {
                var match = DecRegex.Match(line);
                var arg = ParseArgument(match.Groups[1].Value);
                return new Instruction(InstructionType.Dec, arg);
            }

            if (JnzRegex.IsMatch(line))
            {
                var match = JnzRegex.Match(line);
                var arg1 = ParseArgument(match.Groups[1].Value);
                var arg2 = ParseArgument(match.Groups[2].Value);
                return new Instruction(InstructionType.Jnz, arg1, arg2);
            }

            throw new ArgumentException(line);
        }

        private ArgumentObject ParseArgument(string argument)
        {
            int i;
            if (int.TryParse(argument, out i))
            {
                return new ArgumentObject(i);
            }

            Register r;
            if (Enum.TryParse(argument, out r))
            {
                return new ArgumentObject(r);
            }

            throw new ArgumentException();
        }
    }

    public class Machine
    {
        public Machine(IEnumerable<Instruction> instructions)
        {
            Registers = new Dictionary<Register, int>()
            {
                {Register.a, 0},
                {Register.b, 0},
                {Register.c, 0},
                {Register.d, 0},
            };

            Instructions = instructions.ToList();
        }

        private Dictionary<Register, int> Registers { get; }
        private int IP = 0;
        private List<Instruction> Instructions { get; }

        public int GetRegisterValue(Register register)
        {
            if (register == Register.Invalid)
                throw new ArgumentException("Invalid register", "register");

            return Registers[register];
        }

        public void SetRegisterValue(Register register, int value)
        {
            if (register == Register.Invalid)
                throw new ArgumentException("Invalid register", "register");

            Registers[register] = value;
        }

        public void Run()
        {
            while (IP < Instructions.Count)
            {
                RunInstruction(Instructions[IP]);
                IP++;
            }
        }

        private void RunInstruction(Instruction instruction)
        {
            switch (instruction.Type)
            {
                case InstructionType.Cpy:
                    RunCpy(instruction.Arguments[0], instruction.Arguments[1]);
                    break;
                case InstructionType.Inc:
                    RunInc(instruction.Arguments[0]);
                    break;
                case InstructionType.Dec:
                    RunDec(instruction.Arguments[0]);
                    break;
                case InstructionType.Jnz:
                    RunJnz(instruction.Arguments[0], instruction.Arguments[1]);
                    break;
                default:
                    throw new ArgumentException("Invalid instruction", "instruction");
            }
        }

        private void RunCpy(ArgumentObject arg1, ArgumentObject arg2)
        {
            if (arg1.Type == ArgumentType.Register)
            {
                SetRegisterValue(arg2.Register, GetRegisterValue(arg1.Register));
            }
            else
            {
                SetRegisterValue(arg2.Register, arg1.Value.Value);
            }
        }

        private void RunInc(ArgumentObject arg)
        {
            SetRegisterValue(arg.Register, GetRegisterValue(arg.Register) + 1);
        }

        private void RunDec(ArgumentObject arg)
        {
            SetRegisterValue(arg.Register, GetRegisterValue(arg.Register) - 1);
        }

        private void RunJnz(ArgumentObject arg1, ArgumentObject arg2)
        {
            int value;
            if (arg1.Type == ArgumentType.Register)
            {
                value = GetRegisterValue(arg1.Register);
            }
            else
            {
                value = arg1.Value.Value;
            }

            if (value != 0)
                IP += arg2.Value.Value - 1;
        }
    }

    public struct Instruction
    {
        public Instruction(InstructionType type, params ArgumentObject[] arguments)
        {
            Type = type;
            Arguments = arguments;
        }

        public InstructionType Type { get; }

        public ArgumentObject[] Arguments { get; }
    }

    public struct ArgumentObject
    {
        public ArgumentObject(int value)
        {
            Type = ArgumentType.Value;
            Value = value;
            Register = Register.Invalid;
        }

        public ArgumentObject(Register register)
        {
            Type = ArgumentType.Register;
            Value = null;
            Register = register;
        }

        public ArgumentType Type { get; }
        public int? Value { get; }
        public Register Register { get; }
    }

    public enum ArgumentType
    {
        Register,
        Value,
    }

    public enum Register
    {
        Invalid = 0,
        a,
        b,
        c,
        d,
    }

    public enum InstructionType
    {
        Cpy,
        Inc,
        Dec,
        Jnz,
    }
}
