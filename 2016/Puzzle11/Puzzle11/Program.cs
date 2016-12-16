using System;
using System.IO;
using System.Threading.Tasks;

namespace Puzzle11
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines(args[0]);
            var solver = new Solver(input);

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
