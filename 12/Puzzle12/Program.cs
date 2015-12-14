using System;
using System.IO;
using Newtonsoft.Json;

namespace Puzzle12
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var filename in args)
            {
                using (var reader = new JsonTextReader(new StreamReader(filename)))
                {
                    long sum = 0;
                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonToken.Integer)
                        {
                            sum += (long) reader.Value;
                        }
                    }

                    Console.WriteLine("Sum: {0}", sum);
                }
            }

            Console.ReadLine();
        }
    }
}
