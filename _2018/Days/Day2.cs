using System;
using System.Collections.Generic;
using System.Linq;
using _2018.Utils;

namespace _2018.Days
{
    public class Day2 : Day
    {
        private readonly List<string> _ids;
        
        public Day2()
        {
            this._ids = QuestionLoader.Load(2).Split(Environment.NewLine).ToList();
        }
        
        protected override void DoPart1()
        {
            var numPairs = 0;
            var numTriples = 0;

            foreach (var id in this._ids)
            {
                var frequencies = GetLetterFrequencies(id);
                var hasPair = false;
                var hasTriple = false;

                foreach (var f in frequencies)
                {
                    switch (f)
                    {
                        case 2:
                            hasPair = true;
                            break;
                        case 3:
                            hasTriple = true;
                            break;
                    }
                }

                if (hasPair)
                {
                    numPairs++;
                }

                if (hasTriple)
                {
                    numTriples++;
                }
            }

            var checksum = numPairs * numTriples;
            
            ConsoleUtils.WriteColouredLine($"Checksum is {checksum}", ConsoleColor.Cyan);
        }
        
        protected override void DoPart2()
        {
            while (this._ids.Any())
            {
                // Check each ID to see if it closely matches any other ID, and remove it from the set if it does not.
                var id = this._ids[0];
                this._ids.RemoveAt(0);

                foreach (var entry in this._ids)
                {
                    if (NumDifferentChars(id, entry) == 1)
                    {
                        ConsoleUtils.WriteColouredLine($"Found similar IDs {id} and {entry}", ConsoleColor.Cyan);
                        return;
                    }
                }
            }
        }

        private static int NumDifferentChars(string str1, string str2)
        {
            return str1.Where((t, i) => t != str2[i]).Count();
        }

        private static IEnumerable<int> GetLetterFrequencies(string id)
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

            return frequencies.Values;
        }
    }
}