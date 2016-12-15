using System;
using System.IO;

namespace Puzzle09
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllText(args[0]).Trim();
            var solver = new Solver(input);

            var result1 = solver.Solve1();
            Console.WriteLine(result1);

            var result2 = solver.Solve2();
            Console.WriteLine(result2);

            Console.ReadLine();
        }
    }
}
