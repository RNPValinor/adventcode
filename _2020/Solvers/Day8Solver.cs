using System;
using System.Text;
using _2020.Utils;

namespace _2020.Solvers
{
    public class Day8Solver : ISolver
    {
        public void Solve(string input)
        {
            var console = new Hgc(input);

            var exitCode = console.Run();

            switch (exitCode)
            {
                case HGCExitCode.Success:
                    Console.WriteLine("Failure: program succeeded");
                    break;
                case HGCExitCode.InfiniteLoop:
                    Console.WriteLine(console.GetAccumulator());
                    break;
                case HGCExitCode.BadInstruction:
                    Console.WriteLine("Failure: unsupported instruction");
                    break;
                case HGCExitCode.BadArgument:
                    Console.WriteLine("Failure: non-integer argument");
                    break;
                case HGCExitCode.NegativeInstPtr:
                    Console.WriteLine("Failure: instruction pointer is negative");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            console.FixAndRun();

            Console.WriteLine(console.GetAccumulator());
        }
    }
}