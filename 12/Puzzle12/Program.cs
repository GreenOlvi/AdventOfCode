using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

                using (var reader = new JsonTextReader(new StreamReader(filename)))
                {
                    JToken t = JToken.ReadFrom(reader);

                    var sum = CountToken(t);

                    Console.WriteLine("Sum (red): {0}", sum);
                }
            }

            Console.ReadLine();
        }

        private static long CountObject(JObject o)
        {
            if (o.Values().Any(x => x.Type == JTokenType.String && x.Value<string>() == "red"))
            {
                return 0;
            }

            return o.Values().Sum(value => CountToken(value));
        }

        private static long CountArray(JArray a)
        {
            return a.Sum(value => CountToken(value));
        }

        private static long CountToken(JToken t)
        {
            switch (t.Type)
            {
                case JTokenType.Integer:
                    return t.Value<long>();
                case JTokenType.Object:
                    return CountObject(t.Value<JObject>());
                case JTokenType.Array:
                    return CountArray(t.Value<JArray>());
                case JTokenType.String:
                    return 0;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
