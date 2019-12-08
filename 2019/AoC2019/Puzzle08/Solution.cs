using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2019.Puzzle08
{
    public partial class Solution : IPuzzle
    {
        public Solution(string input)
        {
            _input = ParseInput(input);
        }

        public static int[] ParseInput(string input) =>
            input.Select(c => c - '0').ToArray();

        private readonly int[] _input;

        public static string Draw(int[] image, int width, int height)
        {
            var sb = new StringBuilder();
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var p = image[y * width + x] == 0 ? "  " : "██";
                    sb.Append(p);
                }
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }

        public static int Solve1(int[] input, int width, int height)
        {
            var image = new LayeredImage(input, width, height);
            return image.Checksum();
        }

        public static string Solve2(int[] input, int width, int height)
        {
            var image = new LayeredImage(input, width, height);
            return Draw(image.Flatten().ToArray(), width, height);
        }

        public Task<string> Solve1Async()
            => Task.Run(() => Solve1(_input, 25, 6).ToString());

        public Task<string> Solve2Async()
            => Task.Run(() => Environment.NewLine + Solve2(_input, 25, 6).ToString());
    }
}
