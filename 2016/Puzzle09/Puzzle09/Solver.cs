using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Puzzle09
{
    public class Solver
    {
        public Solver(string input)
        {
            Input = input;
        }

        private string Input { get; }

        public int Solve1()
        {
            return Uncompress(Input).Length;
        }

        public long Solve2()
        {
            return UncompressRecursively(Input);
        }

        private string Uncompress(string input)
        {
            var stream = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(input)));
            var output = new StringBuilder();

            while (!stream.EndOfStream)
            {
                string token;
                if (!stream.ReadUntil('(', out token))
                {
                    output.Append(token);
                }
                else
                {
                    output.Append(token);
                    string marker;
                    if (stream.ReadUntil(')', out marker))
                    {
                        var m = ParseMarker(marker);
                        var rep = stream.ReadString(m.Chars);
                        output.Append(RepeatString(rep, m.Repeat));
                    }
                }
            }

            return output.ToString();
        }

        private long UncompressRecursively(string input)
        {
            var stream = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(input)));
            var output = 0L;

            while (!stream.EndOfStream)
            {
                string token;
                if (stream.ReadUntil('(', out token))
                {
                    string marker;
                    if (stream.ReadUntil(')', out marker))
                    {
                        var m = ParseMarker(marker);
                        var rep = stream.ReadString(m.Chars);
                        output += UncompressRecursively(rep)*m.Repeat;
                    }
                }
                output += token.Length;
            }

            return output;
        }

        public static string RepeatString(string text, int count)
        {
            return String.Concat(Enumerable.Repeat(text, count));
        }

        private static readonly Regex MarkerRegex = new Regex(@"(\d+)x(\d+)", RegexOptions.Compiled);
        private Marker ParseMarker(string marker)
        {
            var match = MarkerRegex.Match(marker);

            if (!match.Success)
                throw new ArgumentException("Not a valid marker '" + marker + "'", "marker");

            var chars = int.Parse(match.Groups[1].Value);
            var count = int.Parse(match.Groups[2].Value);

            return new Marker() {Chars = chars, Repeat = count};
        }

        private struct Marker
        {
            public int Chars { get; set; }
            public int Repeat { get; set; } 
        }
    }

    public static class StreamReaderExtensions
    {
        public static bool ReadChar(this StreamReader reader, out char result)
        {
            if (reader.EndOfStream)
            {
                result = ' ';
                return false;
            }
            var buffer = new char[1];
            var read = reader.Read(buffer, 0, 1);
            result = buffer[0];
            return read > 0;
        }

        public static string ReadString(this StreamReader reader, int count)
        {
            var buffer = new char[count];
            reader.Read(buffer, 0, count);
            return String.Concat(buffer);
        }

        public static bool ReadUntil(this StreamReader reader, char end, out string result)
        {
            var resultBuilder = new StringBuilder();

            char next;
            while (reader.ReadChar(out next))
            {
                if (next == end)
                {
                    result = resultBuilder.ToString();
                    return true;
                }

                resultBuilder.Append(next);
            }

            result = resultBuilder.ToString();
            return false;
        }
    }
}
