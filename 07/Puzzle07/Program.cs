using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Puzzle07
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var filename in args)
            {
                var instructions = GetInput(filename);
                var device = new Device(instructions);

                var wireA = device.GetWireValue("a");
                Console.WriteLine(@"Value: {0}", wireA);

                device.ResetAllValues();
                device.Wires["b"] = new ResetableLazy<ushort>(() => wireA);
                var wireA2 = device.GetWireValue("a");

                Console.WriteLine(@"New value: {0}", wireA2);
            }

            Console.ReadLine();
        }

        private static IEnumerable<string> GetInput(string filename)
        {
            using (var input = new StreamReader(filename))
            {
                while (!input.EndOfStream)
                {
                    yield return input.ReadLine();
                }
            }
        }

        class Device
        {
            public Device(IEnumerable<string> instructions)
            {
                foreach (var i in instructions)
                {
                    AddInstruction(i);
                }
            }

            public ushort GetWireValue(string arg)
            {
                return Wires[arg].Value;
            }

            public readonly IDictionary<string, ResetableLazy<ushort>> Wires = new Dictionary<string, ResetableLazy<ushort>>();

            private readonly Dictionary<string, Func<ushort[], ushort>> _operations = new Dictionary<string, Func<ushort[], ushort>>(StringComparer.OrdinalIgnoreCase)
            {
                { "Assign", a => a[0] },
                { "And", a => (ushort) (a[0] & a[1]) },
                { "Or", a => (ushort) (a[0] | a[1]) },
                { "Lshift", a => (ushort) (a[0] << a[1]) },
                { "Rshift", a => (ushort) (a[0] >> a[1]) },
                { "Not", a => (ushort) ~a[0] },
            };

            private static readonly Regex InstructionRegex = new Regex(@"((?<argument1>[a-z\d]+)|(?<operation>[A-Z]+) (?<argument1>[a-z\d]+)|(?<argument1>[a-z\d]+) (?<operation>[A-Z]+) (?<argument2>[a-z\d]+)) -> (?<outputName>[a-z]+)");
            private static readonly Regex NumberValueRegex = new Regex(@"\d+");

            private void AddInstruction(string input)
            {
                var match = InstructionRegex.Match(input);
                if (match.Success)
                {
                    var op = match.Groups["operation"].Success ? match.Groups["operation"].Value : "Assign";
                    if (!_operations.ContainsKey(op))
                    {
                        throw new ArgumentException("Unknown operation");
                    }
                    var operation = _operations[op];

                    var args = new List<string>
                    {
                        match.Groups["argument1"].Value,
                    };

                    if (match.Groups["argument2"].Success)
                    {
                        args.Add(match.Groups["argument2"].Value);
                    }

                    var output = match.Groups["outputName"].Value;

                    Wires.Add(output, new ResetableLazy<ushort>(() => operation(args.Select(GetValue).ToArray())));
                }
                else
                {
                    throw new ArgumentException("Could not parse [" + input + "]!");
                }
            }

            public void ResetAllValues()
            {
                foreach (var v in Wires.Values)
                {
                    v.Reset();
                }
            }

            private ushort GetValue(string arg)
            {
                if (!NumberValueRegex.IsMatch(arg))
                    return Wires[arg].Value;

                ushort v;
                ushort.TryParse(arg, out v);
                return v;
            }
        }

        class ResetableLazy<T>
        {
            public ResetableLazy(Func<T> valueFactory)
            {
                ValueFactory = valueFactory;
            }

            private Func<T> ValueFactory { get; }
            public bool IsConstructed { get; private set; } = false;

            private T _value;
            public T Value {
                get {
                    if (!IsConstructed)
                    {
                        _value = ValueFactory();
                        IsConstructed = true;
                    }
                    return _value;
                }
            }

            public void Reset()
            {
                IsConstructed = false;
            }
        }
    }
}