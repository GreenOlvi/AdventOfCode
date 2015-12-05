using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Puzzle04
{
    class Program
    {
        private static MD5 _md5;

        static void Main(string[] args)
        {
            _md5 = MD5.Create();
            const string key = "bgvyzdsv";

            var i = 0;
            byte[] hash;
            string str;
            do
            {
                i++;
                str = key + i;
                hash = ToMd5(str);
            } while (!Has5Zeros(hash));

            Console.WriteLine(@"{0} - {1}", str, HashString(hash));

            do
            {
                i++;
                str = key + i;
                hash = ToMd5(str);
            } while (!Has6Zeros(hash));

            Console.WriteLine(@"{0} - {1}", str, HashString(hash));

            Console.ReadLine();
        }

        public static bool Has5Zeros(byte[] hash)
        {
            if (hash[0] == 0 && hash[1] == 0 && hash[2] < 16)
                return true;

            return false;
        }

        public static bool Has6Zeros(byte[] hash)
        {
            if (hash[0] == 0 && hash[1] == 0 && hash[2] == 0)
                return true;

            return false;
        }

        public static string HashString(byte[] hash)
        {
            return hash.Select(x => x.ToString("X2")).Aggregate((a, b) => a + b);
        }

        public static byte[] ToMd5(string input)
        {
            return _md5.ComputeHash(Encoding.ASCII.GetBytes(input));
        }
    }
}
