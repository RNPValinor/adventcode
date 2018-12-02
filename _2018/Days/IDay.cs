using System;
using System.Diagnostics;
using _2018.Utils;

namespace _2018.Days
{
    public abstract class Day
    {
        public void Run()
        {
            var watch = Stopwatch.StartNew();
            
            this.DoSolution();
            
            ConsoleUtils.WriteColouredLine($"Got answer in {watch.Elapsed.TotalMilliseconds}ms", ConsoleColor.DarkCyan);
        }

        protected abstract void DoSolution();
    }
}