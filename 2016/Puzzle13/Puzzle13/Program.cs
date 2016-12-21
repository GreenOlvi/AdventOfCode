using System;
using System.Threading.Tasks;

namespace Puzzle13
{
    class Program
    {
        static void Main()
        {
            var solver = new Solver(1364);

            var task1 = new Task<int>(() => solver.Solve1());
            var task2 = new Task<int>(() => solver.Solve2());

            task1.Start();
            task2.Start();

            Console.WriteLine(task1.Result);
            Console.WriteLine(task2.Result);

            Console.ReadLine();
        }
    }
}
