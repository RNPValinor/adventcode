using System;
using System.Collections.Generic;
using System.Linq;

namespace _2020.Solvers
{
    public class Day10Solver : ISolver
    {
        private readonly IDictionary<int, ulong> _knownCounts = new Dictionary<int, ulong>();
        private List<int> _adapters;
        
        public void Solve(string input)
        {
            this._adapters = input.Split(Environment.NewLine).Select(int.Parse).ToList();
            this._adapters.Sort();

            var lastPort = 0;

            var numOneDiffs = 0;
            var numThreeDiffs = 1;

            foreach (var adapter in this._adapters)
            {
                var diff = adapter - lastPort;

                switch (diff)
                {
                    case 1:
                        numOneDiffs++;
                        break;
                    case 3:
                        numThreeDiffs++;
                        break;
                }

                lastPort = adapter;
            }

            Console.WriteLine(numOneDiffs * numThreeDiffs);
            Console.WriteLine(this.GetNumberOfArrangements(-1));
        }

        private ulong GetNumberOfArrangements(int fromAdapterIdx)
        {
            if (this._knownCounts.TryGetValue(fromAdapterIdx, out var count))
            {
                return count;
            }

            if (fromAdapterIdx == this._adapters.Count - 1)
            {
                // This is the last adapter; only 1 arrangement
                return 1;
            }

            var adapter = fromAdapterIdx == -1 ? 0 : this._adapters[fromAdapterIdx];

            ulong numArrangements = 0;

            // Can be at most 3 different adapters we can connect to
            for (var i = 1; i <= 3; i++)
            {
                var nextIdx = fromAdapterIdx + i;

                if (nextIdx >= this._adapters.Count || this._adapters[nextIdx] - adapter > 3)
                {
                    // Adapter too powerful, or out of range.
                    break;
                }
                else
                {
                    // Valid adapter
                    numArrangements += this.GetNumberOfArrangements(nextIdx);
                }
            }

            this._knownCounts.Add(fromAdapterIdx, numArrangements);

            return numArrangements;
        }
    }
}