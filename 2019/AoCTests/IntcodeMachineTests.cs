using System;
using System.Collections.Generic;
using NUnit.Framework;
using FluentAssertions;
using AoC2019.Common;
using System.Linq;

namespace AoCTests
{
    [TestFixture]
    public class IntcodeMachineTests
    {
        [TestCase(2101, 1, new[] { ParamMode.Relative, ParamMode.Immediate, ParamMode.Position })]
        [TestCase(102, 2, new[] { ParamMode.Position, ParamMode.Immediate, ParamMode.Position })]
        [TestCase(204, 4, new[] { ParamMode.Relative })]
        public void DecodeTest(int instruction, int opcode, ParamMode[] paramModes)
        {
            var (op, param) = IntcodeMachine.Decode(instruction);
            op.Should().Be(opcode);
            param.Should().BeEquivalentTo(paramModes);
        }

        //Halt
        [TestCase("99", Description = "Just halt")]

        //Output
        [TestCase("104,42,99", 42)]
        [TestCase("4,0,99", 4)]
        [TestCase("4,100,99", 0)]
        [TestCase("204,2,99", 99)]

        //Add
        [TestCase("1101,12,34,3,4,3,99", 46)]
        [TestCase("1001,0,34,3,4,3,99", 1035)]
        [TestCase("1,3,4,3,4,3,99", 7)]
        [TestCase("21101,14,1,3,4,3,99", 15)]

        //Multiply
        [TestCase("1102,12,34,3,4,3,99", 408)]
        [TestCase("1002,0,34,3,4,3,99", 34068)]
        [TestCase("2,3,4,3,4,3,99", 12)]
        [TestCase("21102,14,2,3,4,3,99", 28)]
        
        //Jump if true
        [TestCase("5,9,10,104,0,99,104,1,99,0,6", 0)]
        [TestCase("5,9,10,104,0,99,104,1,99,1,6", 1)]
        [TestCase("105,0,9,104,0,99,104,1,99,6", 0)]
        [TestCase("105,9,9,104,0,99,104,1,99,6", 1)]
        [TestCase("1105,0,6,104,0,99,104,1,99", 0)]
        [TestCase("1105,-4,6,104,0,99,104,1,99", 1)]

        //Jump if false
        [TestCase("6,9,10,104,0,99,104,1,99,0,6", 1)]
        [TestCase("6,9,10,104,0,99,104,1,99,1,6", 0)]
        [TestCase("106,0,9,104,0,99,104,1,99,6", 1)]
        [TestCase("106,9,9,104,0,99,104,1,99,6", 0)]
        [TestCase("1106,0,6,104,0,99,104,1,99", 1)]
        [TestCase("1106,-4,6,104,0,99,104,1,99", 0)]

        //Less than
        [TestCase("7,8,9,7,4,7,99,-1,1,2", 1)]
        [TestCase("7,8,9,7,4,7,99,-1,1,1", 0)]
        [TestCase("7,8,9,7,4,7,99,-1,5,3", 0)]
        [TestCase("1107,1,2,7,4,7,99,-1", 1)]
        [TestCase("1107,1,1,7,4,7,99,-1", 0)]
        [TestCase("1107,4,2,7,4,7,99,-1", 0)]

        //Equals
        [TestCase("1108,1,3,7,4,7,99,-1", 0)]
        [TestCase("1108,1,1,7,4,7,99,-1", 1)]
        [TestCase("1108,3,1,7,4,7,99,-1", 0)]
        [TestCase("8,8,9,7,4,7,99,-1,1,3", 0)]
        [TestCase("8,8,9,7,4,7,99,-1,2,2", 1)]
        [TestCase("8,8,9,7,4,7,99,-1,3,1", 0)]

        //Adjust relative base
        [TestCase("9,2,204,-200,99", 99)]
        [TestCase("109,-100,204,102,99", 204)]
        [TestCase("109,100,204,-98,99", 204)]
        [TestCase("209,5,204,-100,99,100,150", 209)]
        public void RunTests(string prog, params long[] output)
        {
            var m = new IntcodeMachine(ParseProgram(prog));
            m.Run();
            m.IsHalted.Should().BeTrue();
            m.GetAllOutput().Should().BeEquivalentTo(output);
        }

        [TestCase("3,5,4,5,99,0", 21, 21, Description = "Return input")]
        [TestCase("203,5,4,5,99,0", 21, 21, Description = "Return input (relative)")]
        public void RunWithInputTests(string prog, long input, params long[] output)
        {
            var m = new IntcodeMachine(ParseProgram(prog));

            m.Run();
            m.WaitingForInput.Should().BeTrue();

            m.AddInput(input);
            m.WaitingForInput.Should().BeFalse();

            m.Run();

            m.GetAllOutput().Should().BeEquivalentTo(output);
        }

        private static IEnumerable<long> ParseProgram(string input) =>
            input.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => long.Parse(s));
    }
}
