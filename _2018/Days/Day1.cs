using System;
using System.Collections.Generic;
using System.Linq;
using _2018.Utils;

namespace _2018.Days
{
    public class Day1 : Day
    {
        private readonly List<int> _steps;
        
        public Day1()
        {
            this._steps = QuestionLoader.Load(1).Split(Environment.NewLine).Select(int.Parse).ToList();
        }
        
        protected override void DoPart1()
        {
            var frequency = 0;

            foreach (var t in this._steps)
            {
                frequency += t;
            }
            
            ConsoleUtils.WriteColouredLine($"Target frequency (part 1) is {frequency}", ConsoleColor.Cyan);
        }
        
        protected override void DoPart2()
        {
            var seenFrequencies = new HashSet<int>();
            var frequency = 0;
            var i = 0;

            while (!seenFrequencies.Contains(frequency))
            {
                seenFrequencies.Add(frequency);
                frequency += this._steps[i];
                i = (i + 1) % this._steps.Count;
            }
            
            ConsoleUtils.WriteColouredLine($"Target frequency (part 2) is {frequency}", ConsoleColor.Cyan);
        }
    }
}