using System;
using System.Threading.Tasks;

namespace Puzzle05
{
    class Program
    {
        static void Main(string[] args)
        {
            const string input = "ojvtpuvg";
            var solver = new Solver(input);

            var task1 = new Task<string>(() => solver.Solve1());
            var task2 = new Task<string>(() => solver.Solve2());

            task1.Start();
            task2.Start();

            var result1 = task1.Result;
            Console.WriteLine("1 " + result1);

            var result2 = task2.Result;
            Console.WriteLine("2 " + result2);

            Console.ReadLine();
        }
    }
}
