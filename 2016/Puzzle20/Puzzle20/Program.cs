using System;
using System.IO;

namespace Puzzle20
{
    class Program
    {
        static void Main(string[] args)
        {
            var solver = new Solver(File.ReadAllLines(args[0]));

            var result1 = solver.Solve1();
            Console.WriteLine(result1);

            var result2 = solver.Solve2();
            Console.WriteLine(result2);

            Console.ReadLine();
        }
    }
}
