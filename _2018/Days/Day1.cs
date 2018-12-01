using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using _2018.Utils;

namespace _2018.Days
{
    public class Day1 : IDay
    {
        public void Run()
        {
            ConsoleUtils.WriteColouredLine("Running day 1", ConsoleColor.DarkGreen);

            List<int> steps;

            var fileStream = new FileStream("Days/Inputs/Day1.txt", FileMode.Open);

            using (var reader = new StreamReader(fileStream))
            {
                steps = reader.ReadToEnd().Split(Environment.NewLine).Select(int.Parse).ToList();
            }
            
            var seenFrequencies = new HashSet<int>();
            var frequency = 0;
            var i = 0;

            while (!seenFrequencies.Contains(frequency))
            {
                seenFrequencies.Add(frequency);
                frequency += steps[i];
                i = (i + 1) % steps.Count;
            }
            
            ConsoleUtils.WriteColouredLine($"Target frequency is {frequency}", ConsoleColor.Cyan);
        }
    }
}