using System;
using System.Collections.Generic;
using System.Linq;
using _2018.Utils;

namespace _2018.Days
{
    public class Day16 : Day
    {
        private readonly IDictionary<byte, HashSet<string>> _possibleMatches = new Dictionary<byte, HashSet<string>>();
        
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

                var instructions = GenerateInstructions(
                    byte.Parse(instData[1]),
                    byte.Parse(instData[2]),
                    byte.Parse(instData[3]));

                var numMatches = 0;
                var currentMatches = new HashSet<string>();

                foreach (var instruction in instructions)
                {
                    var registers = new List<ushort>(initialRegisters);

                    instruction.Run(registers);

                    if (!registers.SequenceEqual(targetRegisters)) continue;

                    numMatches++;
                    currentMatches.Add(instruction.Opcode);
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

        private static IEnumerable<Instruction> GenerateInstructions(byte a, byte b, byte c)
        {
            return new HashSet<Instruction>
            {
                new Add(a, b, c, false),
                new Add(a, b, c, true),
                new Mul(a, b, c, false),
                new Mul(a, b, c, true),
                new Ban(a, b, c, false),
                new Ban(a, b, c, true),
                new Bor(a, b, c, false),
                new Bor(a, b, c, true),
                new Set(a, b, c, false),
                new Set(a, b, c, true),
                new Gti(a, b, c),
                new Gtr(a, b, c, false),
                new Gtr(a, b, c, true),
                new Eqi(a, b, c),
                new Eqr(a, b, c, false),
                new Eqr(a, b, c, true)
            };
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

                var opcode = opcodeMap[instructionData[0]];

                var instruction =
                    this.GetInstruction(opcode, instructionData[1], instructionData[2], instructionData[3]);
                
                instruction.Run(registers);
            }

            var colour = ConsoleColor.Cyan;

            if (registers[0] <= 144)
            {
                colour = ConsoleColor.Red;
            }
            
            ConsoleUtils.WriteColouredLine($"Register 0 has value {registers[0]}", colour);
        }

        private Instruction GetInstruction(string opcode, byte a, byte b, byte c)
        {
            switch (opcode)
            {
                case "addi":
                    return new Add(a, b, c, true);
                case "addr":
                    return new Add(a, b, c, false);
                case "muli":
                    return new Mul(a, b, c, true);
                case "mulr":
                    return new Mul(a, b, c, false);
                case "bani":
                    return new Ban(a, b, c, true);
                case "banr":
                    return new Ban(a, b, c, false);
                case "bori":
                    return new Bor(a, b, c, true);
                case "borr":
                    return new Bor(a, b, c, false);
                case "seti":
                    return new Set(a, b, c, true);
                case "setr":
                    return new Set(a, b, c, false);
                case "gtir":
                    return new Gti(a, b, c);
                case "gtri":
                    return new Gtr(a, b, c, true);
                case "gtrr":
                    return new Gtr(a, b, c, false);
                case "eqir":
                    return new Eqi(a, b, c);
                case "eqri":
                    return new Eqr(a, b, c, true);
                case "eqrr":
                    return new Eqr(a, b, c, false);
                default:
                    throw new ArgumentOutOfRangeException(opcode);
            }
        }

        private abstract class Instruction
        {
            protected readonly byte A;
            protected readonly byte B;
            protected readonly byte C;
            protected readonly bool Immediate;
            protected Func<ushort, ushort, int> Op;
            public readonly string Opcode;

            protected Instruction(byte a, byte b, byte c, bool immediate, string opcode)
            {
                this.A = a;
                this.B = b;
                this.C = c;
                this.Immediate = immediate;
                this.Opcode = opcode + (immediate ? "i" : "r");
            }

            public virtual void Run(IList<ushort> registers)
            {
                registers[this.C] = (ushort) this.Op(registers[this.A], this.Immediate ? this.B : registers[this.B]);
            }
        }

        private class Add : Instruction
        {
            public Add(byte a, byte b, byte c, bool immediate) : base(a, b, c, immediate, "add")
            {
                this.Op = (x, y) => x + y;
            }
        }

        private class Mul : Instruction
        {
            public Mul(byte a, byte b, byte c, bool immediate) : base(a, b, c, immediate, "mul")
            {
                this.Op = (x, y) => x * y;
            }
        }

        private class Ban : Instruction
        {
            public Ban(byte a, byte b, byte c, bool immediate) : base(a, b, c, immediate, "ban")
            {
                this.Op = (x, y) => x & y;
            }
        }
        
        private class Bor : Instruction
        {
            public Bor(byte a, byte b, byte c, bool immediate) : base(a, b, c, immediate, "bor")
            {
                this.Op = (x, y) => x | y;
            }
        }

        private class Set : Instruction
        {
            public Set(byte a, byte b, byte c, bool immediate) : base(a, b, c, immediate, "set")
            {
            }

            public override void Run(IList<ushort> registers)
            {
                registers[this.C] = this.Immediate ? this.A : registers[this.A];
            }
        }

        private class Gti : Instruction
        {
            public Gti(byte a, byte b, byte c) : base(a, b, c, false, "gti")
            {
            }
            
            public override void Run(IList<ushort> registers)
            {
                registers[this.C] = (ushort) (this.A > registers[this.B] ? 1 : 0);
            }
        }

        private class Gtr : Instruction
        {
            public Gtr(byte a, byte b, byte c, bool immediate) : base(a, b, c, immediate, "gtr")
            {
                this.Op = (x, y) => x > y ? 1 : 0;
            }
        }

        private class Eqi : Instruction
        {
            public Eqi(byte a, byte b, byte c) : base(a, b, c, false, "eqi")
            {
            }
            
            public override void Run(IList<ushort> registers)
            {
                registers[this.C] = (ushort) (this.A == registers[this.B] ? 1 : 0);
            }
        }

        private class Eqr : Instruction
        {
            public Eqr(byte a, byte b, byte c, bool immediate) : base(a, b, c, immediate, "eqr")
            {
                this.Op = (x, y) => x == y ? 1 : 0;
            }
        }
    }
}