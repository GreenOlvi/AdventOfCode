using System;

namespace Puzzle19
{
    public class Solver
    {
        public Solver(int input)
        {
            Input = input;
        }

        private int Input { get; set; }

        public int Solve1()
        {
            var l = Input - HighestPowerOf2(Input);
            return 2*l + 1;
        }

        public int Solve2()
        {
            var m = HighestPowerOf3(Input);
            if (Input == m)
            {
                return Input;
            }

            if (Input <= 2*m)
            {
                return Input - m;
            }

            return 2*Input - 3*m;
        }

        private int HighestPowerOf2(long number)
        {
            var n = number;
            var i = 0;
            while (n > 0)
            {
                n = n >> 1;
                i++;
            }
            i--;

            return  (int) Math.Pow(2, i);
        }

        private int HighestPowerOf3(int number)
        {
            var n = number;
            var i = 0;
            while (n > 0)
            {
                n /= 3;
                i++;
            }
            i--;
            return (int) Math.Pow(3, i);
        }
    }
}
