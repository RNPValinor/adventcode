using _2023.Utils;

namespace _2023.Days;

public class Day20() : Day(20)
{
    private readonly HashSet<(string name, ModuleType type, List<string> outputs)> _moduleData = [];

    private readonly HashSet<FlipFlop> _flipFlops = [];
    private readonly HashSet<Conjunction> _conjunctions = [];
    private Broadcaster? _broadcaster;

    private Conjunction _outputModule;
    
    protected override void ProcessInputLine(string line)
    {
        var moduleData = line[0] switch
        {
            'b' => ("broadcaster", ModuleType.Broadcast, line[15..].Split(", ").ToList()),
            '%' => (line.Substring(1, 2), ModuleType.FlipFlop, line[7..].Split(", ").ToList()),
            '&' => (line.Substring(1, 2), ModuleType.Conjunction, line[7..].Split(", ").ToList()),
            _ => throw new ArgumentException($"Unexpected module definition: {line}", nameof(line))
        };

        this._moduleData.Add(moduleData);
    }

    private void CreateModules()
    {
        var moduleLookup = new Dictionary<string, Module>();

        var partModules = new HashSet<(Module module, List<string> outputs)>();
        var sourceModuleLookup = new Dictionary<string, HashSet<Module>>();

        foreach (var moduleData in this._moduleData)
        {
            Module module = moduleData.type switch
            {
                ModuleType.Broadcast => new Broadcaster(),
                ModuleType.FlipFlop => new FlipFlop(moduleData.name),
                ModuleType.Conjunction => new Conjunction(moduleData.name),
                _ => throw new ArgumentOutOfRangeException()
            };

            switch (module)
            {
                case Broadcaster broadcaster:
                    this._broadcaster = broadcaster;
                    break;
                case FlipFlop flipFlop:
                    this._flipFlops.Add(flipFlop);
                    break;
                case Conjunction conjunction:
                    this._conjunctions.Add(conjunction);
                    break;
            }

            moduleLookup.Add(moduleData.name, module);
            partModules.Add((module, moduleData.outputs));

            foreach (var output in moduleData.outputs)
            {
                if (sourceModuleLookup.TryGetValue(output, out var sources))
                {
                    sources.Add(module);
                }
                else
                {
                    sourceModuleLookup[output] = [module];
                }
            }
        }
        
        this.HookupModules(moduleLookup, partModules, sourceModuleLookup);
    }

    private void HookupModules(
        IReadOnlyDictionary<string, Module> lookup,
        HashSet<(Module module, List<string> outputs)> modules,
        IReadOnlyDictionary<string, HashSet<Module>> sourceModuleLookup)
    {
        foreach (var (module, outputs) in modules)
        {
            var outputModules = new List<Module>();
            var nonModuleOutputs = new List<string>();

            foreach (var name in outputs)
            {
                if (lookup.TryGetValue(name, out var outputModule))
                {
                    outputModules.Add(outputModule);
                }
                else
                {
                    nonModuleOutputs.Add(name);
                }
            }

            module.SetOutputs(outputModules, nonModuleOutputs);

            if (module is Conjunction conjunction)
            {
                conjunction.SetInputs(sourceModuleLookup[conjunction.Name]);

                if (conjunction.GetNumNonModuleOutputs() > 0)
                {
                    this._outputModule = conjunction;
                }
            }
        }
    }

    protected override void SolvePart1()
    {
        this.CreateModules();
        
        var numLowPulses = 0L;
        var numHighPulses = 0L;

        for (var i = 0; i < 1000; i++)
        {
            var (dLow, dHigh) = this.PushTheButton(i, new(), null);

            numLowPulses += dLow;
            numHighPulses += dHigh;
        }

        this.Part1Solution = (numLowPulses * numHighPulses).ToString();
    }

    private (long numLow, long numHigh) PushTheButton(int iterationNumber, Dictionary<Module, List<long>> watchlist, Module? targetModule)
    {
        var numLowPulses = 1L;
        var numHighPulses = 0L;

        var pulseQueue = new Queue<(Module source, Pulse pulse, Module module)>();

        foreach (var outputModule in this._broadcaster!.GetOutputs())
        {
            pulseQueue.Enqueue((source: this._broadcaster, pulse: Pulse.Low, module: outputModule));
        }

        while (pulseQueue.TryDequeue(out var pulseData))
        {
            var (source, pulse, module) = pulseData;
            
            switch (pulse)
            {
                case Pulse.Low:
                    numLowPulses++;
                    break;
                case Pulse.High:
                    numHighPulses++;

                    if (module == targetModule)
                    {
                        watchlist[source].Add(iterationNumber);
                    }
                    break;
                case Pulse.None:
                default:
                    throw new ArgumentException($"Unexpected pulse type {pulse}");
            }

            var outputPulse = module.GivePulse(pulse, source);

            switch (outputPulse)
            {
                case Pulse.None:
                    continue;
                case Pulse.Low:
                    numLowPulses += module.GetNumNonModuleOutputs();
                    break;
                case Pulse.High:
                    numHighPulses += module.GetNumNonModuleOutputs();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            foreach (var outputModule in module.GetOutputs())
            {
                pulseQueue.Enqueue((source: module, pulse: outputPulse, module: outputModule));
            }
        }

        return (numLowPulses, numHighPulses);
    }

    protected override void SolvePart2()
    {
        foreach (var flipFlop in this._flipFlops)
        {
            flipFlop.Reset();
        }

        foreach (var conjunction in this._conjunctions)
        {
            conjunction.Reset();
        }

        var outputInputHighPulseOccurrences = this._outputModule
            .GetInputs()
            .ToDictionary<Module, Module, List<long>>(input => input, _ => []);

        for (var iterationNumber = 0; outputInputHighPulseOccurrences.Values.Any(v => v.Count < 2); iterationNumber++)
        {
            this.PushTheButton(iterationNumber, outputInputHighPulseOccurrences, this._outputModule);
        }

        var periods = outputInputHighPulseOccurrences.Values.Select(v => v[1] - v[0]);

        this.Part2Solution = periods.Aggregate(Maths.Lcm).ToString();
    }

    private abstract class Module(string name)
    {
        public readonly string Name = name;
        private List<Module> _outputs = [];
        private List<string> _nonModuleOutputs = [];

        public void SetOutputs(List<Module> targetModules, List<string> nonModuleOutputs)
        {
            this._outputs = targetModules;
            this._nonModuleOutputs = nonModuleOutputs;
        }

        public List<Module> GetOutputs()
        {
            return this._outputs;
        }

        public int GetNumNonModuleOutputs()
        {
            return this._nonModuleOutputs.Count;
        }

        public abstract Pulse GivePulse(Pulse pulse, Module input);
    }

    private class Broadcaster() : Module("Broadcaster")
    {
        public override Pulse GivePulse(Pulse pulse, Module input)
        {
            return pulse;
        }
    }

    private class FlipFlop(string name) : Module(name)
    {
        private bool _isOn;

        public void Reset()
        {
            this._isOn = false;
        }
        
        public override Pulse GivePulse(Pulse pulse, Module input)
        {
            if (pulse is Pulse.High)
            {
                return Pulse.None;
            }

            this._isOn = !this._isOn;

            return this._isOn ? Pulse.High : Pulse.Low;
        }
    }

    private class Conjunction(string name) : Module(name)
    {
        private Dictionary<Module, Pulse> _receivedPulses = new();

        public void Reset()
        {
            this._receivedPulses = this._receivedPulses.ToDictionary(kvp => kvp.Key, _ => Pulse.Low);
        }
        
        public void SetInputs(IEnumerable<Module> inputs)
        {
            foreach (var module in inputs)
            {
                this._receivedPulses.Add(module, Pulse.Low);
            }
        }

        public List<Module> GetInputs()
        {
            return this._receivedPulses.Keys.ToList();
        }
        
        public override Pulse GivePulse(Pulse pulse, Module input)
        {
            this._receivedPulses[input] = pulse;

            var outputPulse = Pulse.High;

            if (this._receivedPulses.Values.All(p => p is Pulse.High))
            {
                outputPulse = Pulse.Low;
            }

            return outputPulse;
        }
    }

    private enum ModuleType
    {
        Broadcast,
        FlipFlop,
        Conjunction
    }

    private enum Pulse
    {
        None,
        Low,
        High
    }
}