using System;
using System.Collections.Generic;
using _2018.Utils;

namespace _2018.Days
{
    public class Day19 : Day
    {
        private IList<Instruction> _program;
        private byte _ip;

        private void ReadProgram()
        {
            var lines = QuestionLoader.Load(19).Split(Environment.NewLine);

            this._program = new List<Instruction>(lines.Length);

            foreach (var line in lines)
            {
                var data = line.Split(' ');
                var opcode = data[0];
                var a = byte.Parse(data[1]);

                if (opcode == "#ip")
                {
                    this._ip = a;
                }
                else
                {
                    var b = byte.Parse(data[2]);
                    var c = byte.Parse(data[3]);
                    
                    this._program.Add(new Instruction(opcode, a, b, c));
                }
            }
        }

        private int RunToTermination(IList<int> registers, bool printLogs = false)
        {
            while (true)
            {
                if (printLogs)
                {
//                    Console.WriteLine(registers[this._ip]);
                    Console.WriteLine(string.Join(", ", registers));
                }
                
                var instruction = this._program[registers[this._ip]];

                registers[instruction.C] =
                    Day16.Instructions[instruction.Opcode](registers, instruction.A, instruction.B);

                var nextIndex = registers[this._ip] + 1;

                if (nextIndex < 0 || nextIndex >= this._program.Count)
                {
                    break;
                }

                registers[this._ip] = nextIndex;
            }

            return registers[0];
        }

        protected override void DoPart1()
        {
            this.ReadProgram();
            
            var registers = new List<int>(6) { 0, 0, 0, 0, 0, 0 };

            var reg0 = this.RunToTermination(registers);
            
            ConsoleUtils.WriteColouredLine($"Register 0 has value {reg0}", ConsoleColor.Cyan);
        }

        protected override void DoPart2()
        {
            var registers = new List<int>(6) { 0, 1, 10551361, 3, 10551361, 0 };

            var reg0 = this.RunToTermination(registers, true);
            
            ConsoleUtils.WriteColouredLine($"Register 0 has value {reg0}", ConsoleColor.Cyan);
        }

        private class Instruction
        {
            public Instruction(string opcode, byte a, byte b, byte c)
            {
                this.Opcode = opcode;
                this.A = a;
                this.B = b;
                this.C = c;
            }
            
            public string Opcode { get; set; }
            public byte A { get; set; }
            public byte B { get; set; }
            public byte C { get; set; }
        }
    }
}