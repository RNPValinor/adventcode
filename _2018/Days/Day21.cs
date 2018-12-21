using System;
using System.Collections.Generic;
using _2018.Utils;

namespace _2018.Days
{
    public class Day21 : Day
    {
        private IList<Instruction> _program;
        private byte _ip;

        private void ReadProgram()
        {
            var lines = QuestionLoader.Load(21).Split(Environment.NewLine);

            this._program = new List<Instruction>(lines.Length);

            foreach (var line in lines)
            {
                var data = line.Split(' ');
                var opcode = data[0];
                var a = int.Parse(data[1]);

                if (opcode == "#ip")
                {
                    this._ip = (byte) a;
                }
                else
                {
                    var b = int.Parse(data[2]);
                    var c = int.Parse(data[3]);
                    
                    this._program.Add(new Instruction(opcode, a, b, c));
                }
            }
        }
        
        private int RunToTermination(IList<int> registers, bool fewestInstructions = true)
        {
            var numInstructions = 0;
            var seenR1Values = new Dictionary<int, int>();
            
            while (true)
            {
                numInstructions++;
                var instruction = this._program[registers[this._ip]];

                if (registers[this._ip] == 28)
                {
                    if (fewestInstructions)
                    {
                        return registers[1];
                    }

                    if (seenR1Values.ContainsKey(registers[1]))
                    {
                        break;
                    }

                    seenR1Values.Add(registers[1], numInstructions);
                }

                registers[instruction.C] =
                    Day16.Instructions[instruction.Opcode](registers, instruction.A, instruction.B);

                var nextIndex = registers[this._ip] + 1;

                if (nextIndex < 0 || nextIndex >= this._program.Count)
                {
                    break;
                }

                registers[this._ip] = nextIndex;
            }

            var maxNumInstructions = 0;
            var maxR1Value = 0;

            foreach (var entry in seenR1Values)
            {
                if (entry.Value <= maxNumInstructions) continue;
                
                maxR1Value = entry.Key;
                maxNumInstructions = entry.Value;
            }

            return maxR1Value;
        }
        
        protected override void DoPart1()
        {
            this.ReadProgram();
            
            var registers = new List<int> {0, 0, 0, 0, 0, 0};

            var firstR1Value = this.RunToTermination(registers);
            
            ConsoleUtils.WriteColouredLine($"Got first r1 value of {firstR1Value}", ConsoleColor.Magenta);
        }

        protected override void DoPart2()
        {
            var registers = new List<int> {0, 0, 0, 0, 0, 0};

            var lastR1Value = this.RunToTermination(registers, false);
            
            ConsoleUtils.WriteColouredLine($"Got last r1 value of {lastR1Value}", ConsoleColor.Magenta);
        }
        
        private class Instruction
        {
            public Instruction(string opcode, int a, int b, int c)
            {
                this.Opcode = opcode;
                this.A = a;
                this.B = b;
                this.C = c;
            }
            
            public string Opcode { get; set; }
            public int A { get; set; }
            public int B { get; set; }
            public int C { get; set; }
        }
    }
}