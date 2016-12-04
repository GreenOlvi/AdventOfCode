using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Puzzle20
{
    class Program
    {
        private const int value = 33100000;

        static void Main(string[] args)
        {
            foreach (var n in GetNumbers())
            {
                var s = GetDividers(n).Sum()*10;

                if (s >= value)
                {
                    Console.WriteLine("{0}: {1}", n, s);
                    break;
                }
            }

            foreach (var n in GetNumbers())
            {
                var s = GetDividers(n).Where(x => n / x <= 50).Sum(x => x * 11);

                if (s >= value)
                {
                    Console.WriteLine("{0}: {1}", n, s);
                    break;
                }
            }

            Console.ReadLine();
        }

        private static IEnumerable<long> GetNumbers()
        {
            long i = 0;
            while (true)
                yield return i++;
        }

        private static IEnumerable<int> GetDividers(long number)
        {
            var sqrt = Math.Sqrt(number);
            var dividers = new HashSet<int>();

            for (var i = 1; i <= sqrt; i++)
            {
                if (number%i == 0)
                {
                    dividers.Add(i);
                    dividers.Add((int) (number / i));
                }
            }

            return dividers.OrderBy(i => i);
        } 
    }
}
