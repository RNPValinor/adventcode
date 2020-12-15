using System;
using System.Collections.Generic;

namespace _2020.Solvers
{
    public class Day15Solver : ISolver
    {
        public void Solve(string input)
        {
            var seeds = input.Split(',');

            var seenNumbers = new Dictionary<int, int>();
            var lastSeen = 0;

            for (var i = 0; i < seeds.Length; i++)
            {
                if (!int.TryParse(seeds[i], out var seed))
                {
                    throw new ArgumentException($"Non-integer input found: {seeds[i]}");
                }

                if (i == seeds.Length - 1)
                {
                    lastSeen = seed;
                }
                else
                {
                    seenNumbers.Add(seed, i);
                }
            }

            for (var i = seeds.Length; i < 30000000; i++)
            {
                var previousLastSeen = lastSeen;
                
                if (seenNumbers.TryGetValue(lastSeen, out var oldTurn))
                {
                    // Seen the last number before; get the difference
                    lastSeen = i - oldTurn - 1;
                }
                else
                {
                    lastSeen = 0;
                }

                if (seenNumbers.ContainsKey(previousLastSeen))
                {
                    seenNumbers.Remove(previousLastSeen);
                }
                
                seenNumbers.Add(previousLastSeen, i - 1);

                if (i == 2019)
                {
                    Console.WriteLine(lastSeen);
                }
            }

            Console.WriteLine(lastSeen);
        }
    }
}