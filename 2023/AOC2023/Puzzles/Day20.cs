namespace AOC2023.Puzzles;
public class Day20 : CustomBaseDay
{
    private readonly CableDefinition[] _cables;

    public Day20()
    {
        _cables = ReadLinesFromFile().Select(ParseCable).ToArray();
    }

    public Day20(IEnumerable<string> lines)
    {
        _cables = lines.Select(ParseCable).ToArray();
    }

    private static CableDefinition ParseCable(string line)
    {
        var parts = line.Split(" -> ", StringSplitOptions.TrimEntries);
        var (module, type) = parts[0] switch
        {
            "broadcaster" => (parts[0], ModuleType.Broadcast),
            string s when s.StartsWith('%') => (parts[0][1..], ModuleType.FlipFlop),
            string s when s.StartsWith('&') => (parts[0][1..], ModuleType.Conjuction),
            _ => (parts[0], ModuleType.Generic),
        };

        var outputs = parts[1].Split(", ", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        return new CableDefinition(new ModuleDefinition(module, type), outputs);
    }

    private static IEnumerable<IModule> BuildModules(CableDefinition[] cables)
    {
        var connections = cables.SelectMany(c => c.Outputs.Select(o => (c.Module.Name, o))).ToList();
        foreach (var cable in cables)
        {
            var m = cable.Module;
            var inputs = connections.Where(c => c.o == m.Name).Select(c => c.Name);
            yield return m.Type switch
            {
                ModuleType.FlipFlop => new FlipFlop(m.Name, cable.Outputs),
                ModuleType.Conjuction => new Conjuction(m.Name, inputs, cable.Outputs),
                ModuleType.Broadcast => new Broadcaster(m.Name, cable.Outputs),
                _ => throw new InvalidOperationException(),
            };
        }
        var generic = connections.Select(c => c.o)
            .Distinct()
            .Where(o => !cables.Any(cab => cab.Module.Name == o));

        foreach (var g in generic)
        {
            yield return new Generic(g);
        }
    }


    public override ValueTask<string> Solve_1()
    {
        var modules = BuildModules(_cables).ToDictionary(m => m.Name);
        var bus = new Queue<Pulse>();

        var (high, low) = (0L, 0L);

        for (var i = 0; i < 1000; i++)
        {
            bus.Enqueue(new Pulse("button", "broadcaster", false));
            while (bus.TryDequeue(out var pulse))
            {
                if (pulse.State)
                {
                    high++;
                }
                else
                {
                    low++;
                }

                var results = modules[pulse.To].PushPulse(pulse);
                foreach (var result in results)
                {
                    bus.Enqueue(result);
                }
            }
        }

        return (high * low).ToResult();
    }

    public override ValueTask<string> Solve_2()
    {
        return "result 2".ToResult();
    }

    private readonly record struct CableDefinition(ModuleDefinition Module, IReadOnlyList<string> Outputs);

    private readonly record struct ModuleDefinition(string Name, ModuleType Type);

    private enum ModuleType
    {
        Generic = 0,
        FlipFlop,
        Conjuction,
        Broadcast,
        Button,
    }

    private readonly record struct Pulse(string From, string To, bool State);

    private interface IModule
    {
        string Name { get; }
        IEnumerable<Pulse> PushPulse(Pulse pulse);
    }

    private class Generic(string Name) : IModule
    {
        public string Name { get; } = Name;

        private readonly List<Pulse> _pulses = [];

        public IEnumerable<Pulse> PushPulse(Pulse pulse)
        {
            _pulses.Add(pulse);
            return Enumerable.Empty<Pulse>();
        }

        public IEnumerable<Pulse> GetPulses() => _pulses.AsReadOnly();
        public bool AnyLowPulses() => _pulses.Any(p => !p.State);
    }

    private class FlipFlop(string Name, IEnumerable<string> Outputs) : IModule
    {
        public string Name { get; } = Name;

        private readonly string[] _outputs = Outputs.ToArray();

        private bool _state = false;

        public IEnumerable<Pulse> PushPulse(Pulse pulse)
        {
            if (pulse.State)
            {
                yield break;
            }

            _state = !_state;
            foreach (var output in _outputs)
            {
                yield return new Pulse { From = Name, To = output, State = _state };
            }
        }
    }

    private class Conjuction(string Name, IEnumerable<string> Inputs, IEnumerable<string> Outputs) : IModule
    {
        public string Name { get; } = Name;

        private readonly string[] _outputs = Outputs.ToArray();
        private readonly Dictionary<string, bool> _memory = Inputs.Select(i => (i, false)).ToDictionary();

        public IEnumerable<Pulse> PushPulse(Pulse pulse)
        {
            _memory[pulse.From] = pulse.State;

            var result = _memory.Values.All(s => s);
            foreach (var output in _outputs)
            {
                yield return new Pulse { From = Name, To = output, State = !result };
            }
        }
    }

    private class Broadcaster(string Name, IEnumerable<string> Outputs) : IModule
    {
        public string Name { get; } = Name;

        private readonly string[] _outputs = Outputs.ToArray();

        public IEnumerable<Pulse> PushPulse(Pulse pulse)
        {
            foreach (var output in _outputs)
            {
                yield return new Pulse { From = Name, To = output, State = pulse.State };
            }
        }
    }
}
