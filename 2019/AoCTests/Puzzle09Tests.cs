using NUnit.Framework;
using FluentAssertions;
using AoC2019.Puzzle09;
using AoC2019.Common;

namespace AoCTests
{
    [TestFixture]
    public class Puzzle09Tests
    {
        [Test]
        [TestCase("109,1,204,-1,1001,100,1,100,1008,100,16,101,1006,101,0,99",
            "109,1,204,-1,1001,100,1,100,1008,100,16,101,1006,101,0,99")]
        [TestCase("1102,34915192,34915192,7,4,7,99,0", "1219070632396864")]
        [TestCase("104,1125899906842624,99", "1125899906842624")]
        public void IntcodeMachineTests(string input, string output)
        {
            var mem = Solution.ParseInput(input);
            var m = new IntcodeMachine(mem);
            m.Run();
            m.GetAllOutput().Should().BeEquivalentTo(Solution.ParseInput(output));
        }
    }
}
