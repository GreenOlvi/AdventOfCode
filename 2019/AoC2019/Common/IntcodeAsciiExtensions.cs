using System.Linq;
using System.Text;

namespace AoC2019.Common
{
    public static class IntcodeAsciiExtensions
    {
        public static void AddAsciiInput(this IntcodeMachine machine, string input)
        {
            machine.AddInput(input.Select(c => (long)c).ToArray());
            machine.AddInputAndRun(0x0a);
        }

        public static string GetAsciiOutput(this IntcodeMachine machine)
        {
            var sb = new StringBuilder();
            while (machine.HasOutput() && machine.PeekOutput() < 128)
            {
                sb.Append((char)machine.GetOutput());
            }
            return sb.ToString();
        }
    }
}
