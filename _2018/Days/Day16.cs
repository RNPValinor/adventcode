using System;
using System.Collections.Generic;
using System.Linq;
using _2018.Utils;

namespace _2018.Days
{
    public class Day16 : Day
    {
        private readonly IDictionary<byte, HashSet<string>> _possibleMatches = new Dictionary<byte, HashSet<string>>();
        private readonly IDictionary<string, Func<IList<ushort>, byte, byte, ushort>> _instructions = new Dictionary<string, Func<IList<ushort>, byte, byte, ushort>>
        {
            { "addi", (registers, a, b) => (ushort) (registers[a] + b) },
            { "addr", (registers, a, b) => (ushort) (registers[a] + registers[b]) },
            { "muli", (registers, a, b) => (ushort) (registers[a] * b) },
            { "mulr", (registers, a, b) => (ushort) (registers[a] * registers[b]) },
            { "bani", (registers, a, b) => (ushort) (registers[a] & b) },
            { "banr", (registers, a, b) => (ushort) (registers[a] & registers[b]) },
            { "bori", (registers, a, b) => (ushort) (registers[a] | b) },
            { "borr", (registers, a, b) => (ushort) (registers[a] | registers[b]) },
            { "seti", (registers, a, b) => (ushort) (a) },
            { "setr", (registers, a, b) => registers[a] },
            { "gtir", (registers, a, b) => (ushort) (a > registers[b] ? 1 : 0) },
            { "gtri", (registers, a, b) => (ushort) (registers[a] > b ? 1 : 0) },
            { "gtrr", (registers, a, b) => (ushort) (registers[a] > registers[b] ? 1 : 0) },
            { "eqir", (registers, a, b) => (ushort) (a == registers[b] ? 1 : 0) },
            { "eqri", (registers, a, b) => (ushort) (registers[a] == b ? 1 : 0) },
            { "eqrr", (registers, a, b) => (ushort) (registers[a] == registers[b] ? 1 : 0) }
        };
        
        protected override void DoPart1()
        {
            var examples = QuestionLoader.Load(16).Split(Environment.NewLine + Environment.NewLine);
            var numThreesomes = 0;

            foreach (var example in examples)
            {
                var data = example.Split(Environment.NewLine);

                var initialRegisters = data[0].Substring(9, 10).Split(',').Select(ushort.Parse).ToList();
                var targetRegisters = data[2].Substring(9, 10).Split(',').Select(ushort.Parse).ToList();

                var instData = data[1].Split(' ');
                var opCode = byte.Parse(instData[0]);
                var a = byte.Parse(instData[1]);
                var b = byte.Parse(instData[2]);
                var c = byte.Parse(instData[3]);

                var numMatches = 0;
                var currentMatches = new HashSet<string>();

                foreach (var instructionEntry in this._instructions)
                {
                    var registers = new List<ushort>(initialRegisters);

                    registers[c] = instructionEntry.Value(registers, a, b);

                    if (!registers.SequenceEqual(targetRegisters)) continue;

                    numMatches++;
                    currentMatches.Add(instructionEntry.Key);
                }

                if (!this._possibleMatches.ContainsKey(opCode))
                {
                    this._possibleMatches.Add(opCode, currentMatches);
                }
                else
                {
                    this._possibleMatches[opCode].IntersectWith(currentMatches);
                }

                if (numMatches >= 3)
                {
                    numThreesomes++;
                }
            }
            
            ConsoleUtils.WriteColouredLine($"Got {numThreesomes} 3+ opcode examples", ConsoleColor.Cyan);
        }

        protected override void DoPart2()
        {
            var opcodeMap = new Dictionary<byte, string>();

            while (this._possibleMatches.Any())
            {
                var entry = this._possibleMatches.First(match => match.Value.Count == 1);

                var opcodeStr = entry.Value.Single();
                
                opcodeMap.Add(entry.Key, opcodeStr);

                this._possibleMatches.Remove(entry.Key);

                foreach (var otherMatch in this._possibleMatches)
                {
                    otherMatch.Value.Remove(opcodeStr);
                }
            }

            var program = QuestionLoader.Load(16, true).Split(Environment.NewLine);
            var registers = new List<ushort> {0, 0, 0, 0};

            foreach (var line in program)
            {
                var instructionData = line.Split(' ').Select(byte.Parse).ToList();

                var opInt = instructionData[0];
                var a = instructionData[1];
                var b = instructionData[2];
                var c = instructionData[3];

                var instruction = this._instructions[opcodeMap[opInt]];

                registers[c] = instruction(registers, a, b);
            }

            var colour = ConsoleColor.Cyan;

            if (registers[0] <= 144)
            {
                colour = ConsoleColor.Red;
            }
            
            ConsoleUtils.WriteColouredLine($"Register 0 has value {registers[0]}", colour);
        }
    }
}