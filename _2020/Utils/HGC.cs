using System;
using System.Collections.Generic;

namespace _2020.Utils
{
    public class Hgc
    {
        private int _accumulator = 0;
        private int _instrPtr = 0;
        private bool _repairMode = false;
        private readonly IList<string> _instructions;
        private readonly ISet<int> _convertedInstructions;
        
        public Hgc(string instructions)
        {
            this._instructions = instructions.Split(Environment.NewLine);
            this._convertedInstructions = new HashSet<int>();
        }

        public HGCExitCode Run()
        {
            var visitedInstructions = new HashSet<int>();
            var hasConvertedInstruction = false;

            while (this._instrPtr < this._instructions.Count)
            {
                if (visitedInstructions.Contains(this._instrPtr))
                {
                    return HGCExitCode.InfiniteLoop;
                }
                
                visitedInstructions.Add(this._instrPtr);

                if (this._instrPtr < 0)
                {
                    return HGCExitCode.NegativeInstPtr;
                }

                var instruction = this._instructions[this._instrPtr];
                var instParts = instruction.Split(' ');

                if (!int.TryParse(instParts[1], out var instructionValue))
                {
                    return HGCExitCode.BadArgument;
                }

                var instructionCmd = instParts[0];

                if (this._repairMode && !hasConvertedInstruction && !this._convertedInstructions.Contains(this._instrPtr))
                {
                    if (instructionCmd == "jmp" || instructionCmd == "nop")
                    {
                        instructionCmd = instructionCmd == "nop" ? "jmp" : "nop";
                        this._convertedInstructions.Add(this._instrPtr);
                        hasConvertedInstruction = true;
                    }
                }
                
                switch (instructionCmd)
                {
                    case "acc":
                        this._accumulator += instructionValue;
                        break;
                    case "jmp":
                        // Subtract 1 so that we can always add 1 later (for other instructions which
                        // don't change instrPtr).
                        this._instrPtr += instructionValue - 1;
                        break;
                    case "nop":
                        break;
                    default:
                        // Bad instruction!
                        return HGCExitCode.BadInstruction;
                }

                this._instrPtr++;
            }

            return HGCExitCode.Success;
        }

        public void FixAndRun()
        {
            this._repairMode = true;
            HGCExitCode exitCode;

            do
            {
                this._accumulator = 0;
                this._instrPtr = 0;
                exitCode = this.Run();
            } while (exitCode != HGCExitCode.Success);
        }
        
        public int GetAccumulator()
        {
            return this._accumulator;
        }
    }
}