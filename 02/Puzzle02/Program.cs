using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Puzzle02
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var filename in args)
            {
                var presents = GetPresents(filename).ToList();

                var sum = presents.Select(p => p.WrapArea()).Sum();
      
                Console.WriteLine("Sum: {0}", sum);
            }

            Console.ReadLine();
        }

        public static IEnumerable<Present> GetPresents(string filename)
        {
            using (var file = new StreamReader(filename))
            {
                while (!file.EndOfStream)
                {
                    var line = file.ReadLine();

                    var split = line.Split('x').Select(s => int.Parse(s)).ToArray();

                    yield return new Present(split[0], split[1], split[2]);
                }
            }
        }

        internal class Present
        {
            public int Length { get; private set; }
            public int Width { get; private set; }
            public int Height { get; private set; }

            public Present(int width, int length, int height)
            {
                Length = length;
                Width = width;
                Height = height;
            }

            public int WrapArea()
            {
                var sides = new List<int>() {Length*Width, Width*Height, Height*Length};
                return sides.Sum()*2 + sides.Min();
            }
        }
    }
}
