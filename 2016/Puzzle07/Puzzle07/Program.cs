using System;
using System.IO;

namespace Puzzle07
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines(args[0]);
            var solver = new Solver(input);

            var result1 = solver.Solve1();
            Console.WriteLine(result1);

            Console.ReadLine();
        }
    }
}
