using System;
using System.Collections.Generic;
using System.Linq;
using MinimumEditDistance;
using _2018.Utils;

namespace _2018.Days
{
    public class Day2 : IDay
    {
        public void Run()
        {
            var ids = QuestionLoader.Load(2).Split(Environment.NewLine).ToList();

            var numPairs = 0;
            var numTriples = 0;

            foreach (var id in ids)
            {
                var frequencyLookup = GetLetterFrequencies(id);

                if (frequencyLookup.ContainsKey(2))
                {
                    numPairs++;
                }

                if (frequencyLookup.ContainsKey(3))
                {
                    numTriples++;
                }
            }

            var checksum = numPairs * numTriples;
            
            ConsoleUtils.WriteColouredLine($"Checksum is {checksum}", ConsoleColor.Cyan);

            while (ids.Any())
            {
                // Check each ID to see if it closely matches any other ID, and remove it from the set if it does not.
                var id = ids[0];
                ids.RemoveAt(0);

                foreach (var entry in ids)
                {
                    var distance = Levenshtein.CalculateDistance(id, entry, 1);

                    if (distance == 1)
                    {
                        ConsoleUtils.WriteColouredLine($"Found similar IDs {id} and {entry}", ConsoleColor.Cyan);
                        return;
                    }
                }
            }
        }

        private static IDictionary<int, HashSet<char>> GetLetterFrequencies(string id)
        {
            var frequencies = new Dictionary<char, int>();

            foreach (var c in id)
            {
                if (frequencies.ContainsKey(c))
                {
                    var newFrequency = frequencies[c] + 1;
                    frequencies.Remove(c);
                    frequencies.Add(c, newFrequency);
                }
                else
                {
                    frequencies.Add(c, 1);
                }
            }

            var frequencyLookup = new Dictionary<int, HashSet<char>>();

            foreach (var entry in frequencies)
            {
                if (!frequencyLookup.ContainsKey(entry.Value))
                {
                    frequencyLookup.Add(entry.Value, new HashSet<char> { entry.Key });
                }
                else
                {
                    frequencyLookup[entry.Value].Add(entry.Key);
                }
            }

            return frequencyLookup;
        }
    }
}