using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AOC2020.Day14
{
    public class Puzzle : PuzzleBase<ulong, ulong>
    {
        public Puzzle(IEnumerable<string> input)
        {
            _input = input.Select(ParseInstruction).ToArray();
        }

        private readonly Instruction[] _input;

        private static readonly Regex MaskRegex = new Regex(@"^mask = (?<value>[01X]{36})$");
        private static readonly Regex MemRegex = new Regex(@"^mem\[(?<address>\d+)\] = (?<value>\d+)$");

        private static Instruction ParseInstruction(string line)
        {
            if (MaskRegex.TryMatch(line, out var maskMatch))
            {
                return new MaskInstruction(maskMatch.Groups["value"].Value);
            }

            if (MemRegex.TryMatch(line, out var memMatch))
            {
                return new MemInstruction(
                    uint.Parse(memMatch.Groups["address"].Value),
                    ulong.Parse(memMatch.Groups["value"].Value));
            }

            throw new PuzzleException($"Invalid input: {line}");
        }

        public override ulong Solution1()
        {
            var currentMask = MaskInstruction.Neutral1;
            var memory = new Dictionary<uint, ulong>();
            foreach (var instruction in _input)
            {
                switch (instruction)
                {
                    case MaskInstruction mask:
                        currentMask = mask;
                        break;
                    case MemInstruction mem:
                        memory[mem.Address] = currentMask.ApplyToValue(mem.Value);
                        break;
                }
            }

            return memory.Aggregate(0UL, (s, kv) => s + kv.Value);
        }

        public override ulong Solution2()
        {
            var currentMask = MaskInstruction.Neutral2;
            var memory = new Dictionary<ulong, ulong>();
            foreach (var instruction in _input)
            {
                switch (instruction)
                {
                    case MaskInstruction mask:
                        currentMask = mask;
                        break;
                    case MemInstruction mem:
                        foreach (var addr in currentMask.GetAddresses(mem.Address))
                        {
                            memory[addr] = mem.Value;
                        }
                        break;
                }
            }

            return memory.Aggregate(0UL, (s, kv) => s + kv.Value);
        }
    }

    public record Instruction();

    public record MaskInstruction : Instruction
    {
        public static readonly MaskInstruction Neutral1 = new MaskInstruction(@"XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");
        public static readonly MaskInstruction Neutral2 = new MaskInstruction(@"000000000000000000000000000000000000");

        public MaskInstruction(string mask)
        {
            Mask = mask;

            var zeros = ~0UL;
            var ones = 0UL;
            var xs = 0;
            var addrMask = ~0UL;

            var xPos = new List<int>();

            for (var i = 0; i < 36; i++)
            {
                if (Mask[i] == '0')
                {
                    zeros ^= 1UL << (35 - i);
                }

                if (Mask[i] == '1')
                {
                    ones ^= 1UL << (35 - i);
                }

                if (Mask[i] == 'X')
                {
                    addrMask ^= 1UL << (35 - i);
                    xs++;
                    xPos.Add(35 - i);
                }
            }

            _onesMask = ones;
            _zerosMask = zeros;
            _xCount = xs;
            _addrMask = addrMask;
            _xPos = xPos.OrderBy(p => p).ToArray();
        }

        public string Mask { get; }

        private readonly ulong _zerosMask;
        private readonly ulong _onesMask;

        private readonly ulong _addrMask;
        private readonly int _xCount;
        private readonly int[] _xPos;

        public ulong ApplyToValue(ulong value) => (value | _onesMask) & _zerosMask;
        public ulong ApplyToAddress(ulong address) => (address | _onesMask) & _addrMask;

        public IEnumerable<ulong> GetAddresses(ulong address)
        {
            var addr = ApplyToAddress(address);
            for (var i = 0UL; i < Math.Pow(2, _xCount); i++)
            {
                var a = 0UL;
                for (var o = 0; o < _xPos.Length; o++)
                {
                    if ((i & (1UL << o)) > 0)
                    {
                        a |= 1UL << _xPos[o];
                    }
                }

                yield return addr | a;
            }
        }

        public int MatchingXs(MaskInstruction mask) => Mask.Zip(mask.Mask).Count(c => c.First == 'X' && c.First == c.Second);

        public override string ToString() => $"mask = {Mask}";
    };

    public record MemInstruction(uint Address, ulong Value) : Instruction;
}
