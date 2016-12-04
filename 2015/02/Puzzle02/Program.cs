using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Puzzle02
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var filename in args)
            {
                var presents = GetPresents(filename).ToList();

                var paperSum = presents.Select(p => p.WrapArea()).Sum();
                var ribbonSum = presents.Select(p => p.RibbonLength()).Sum();
      
                Console.WriteLine("Paper sum: {0}", paperSum);
                Console.WriteLine("Ribbon sum: {0}", ribbonSum);
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

            public int RibbonLength()
            {
                var sides = new List<int>() {Length, Width, Height};
                sides.Sort();
                var a = sides[0];
                var b = sides[1];

                return (a + b)*2 + Volume();
            }

            public int Volume()
            {
                return Length*Width*Height;
            }
        }
    }
}
