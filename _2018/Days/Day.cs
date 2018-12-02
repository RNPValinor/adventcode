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

            this.DoPart1();

            watch.Stop();
            
            ConsoleUtils.WriteColouredLine($"Got answer 1 in {watch.Elapsed.TotalMilliseconds}ms", ConsoleColor.DarkCyan);

            watch.Restart();
            
            this.DoPart2();
            
            ConsoleUtils.WriteColouredLine($"Got answer 2 in {watch.Elapsed.TotalMilliseconds}ms", ConsoleColor.DarkCyan);
        }

        protected abstract void DoPart1();
        protected abstract void DoPart2();
    }
}