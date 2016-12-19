using System;

namespace Puzzle19
{
    class Program
    {
        static void Main()
        {
            var solver = new Solver(3001330);

            var result1 = solver.Solve1();
            Console.WriteLine(result1);

            var result2 = solver.Solve2();
            Console.WriteLine(result2);

            Console.ReadLine();
        }
    }
}
