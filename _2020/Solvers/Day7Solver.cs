using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace _2020.Solvers
{
    public class Day7Solver : ISolver
    {
        private readonly Regex _innerBagMatch = new Regex("^([0-9]+) ([a-z]+ [a-z]+)");
        private Dictionary<string, HashSet<string>> _bagToContainingBags;
        private Dictionary<string, HashSet<(string bag, int numBags)>> _bagToInnerBags;
        
        public void Solve(string input)
        {
            this.InitialiseBagMap(input);   

            const string startBag = "shiny gold";
            var containingBags = new HashSet<string>();

            this.GetContainingBagsDeep(startBag, containingBags);
            var numBagsWithin = this.GetNumBagsWithinDeep(startBag);

            Console.WriteLine(containingBags.Count);
            Console.WriteLine(numBagsWithin);
        }

        private void InitialiseBagMap(string input)
        {
            this._bagToContainingBags = new Dictionary<string, HashSet<string>>();
            this._bagToInnerBags = new Dictionary<string, HashSet<(string bag, int numBags)>>();

            var lines = input.Split(Environment.NewLine);

            foreach (var line in lines)
            {
                var parts = line.Split(" bags contain ");
                
                var containingBag = parts[0];
                var innerBags = new HashSet<(string bag, int numBags)>();
                var bagsWithin = parts[1].Split(", ");

                foreach (var bagWithin in bagsWithin)
                {
                    var match = this._innerBagMatch.Match(bagWithin);

                    if (!int.TryParse(match.Groups[1].Value, out var numBags))
                    {
                        // Contains no bags.
                        break;
                    }
                    var bagName = match.Groups[2].Value;

                    innerBags.Add((bagName, numBags));

                    if (this._bagToContainingBags.TryGetValue(bagName, out var otherContainingBags))
                    {
                        otherContainingBags.Add(containingBag);
                    }
                    else
                    {
                        this._bagToContainingBags.Add(bagName, new HashSet<string> { containingBag });
                    }
                }

                this._bagToInnerBags.Add(containingBag, innerBags);
            }
        }

        private void GetContainingBagsDeep(string bag, ISet<string> containingBags)
        {
            if (!this._bagToContainingBags.TryGetValue(bag, out var containers))
            {
                // This bag can't be contained!
                return;
            }

            foreach (var containingBag in containers.Where(containingBag => !containingBags.Contains(containingBag)))
            {
                containingBags.Add(containingBag);

                this.GetContainingBagsDeep(containingBag, containingBags);
            }
        }

        private int GetNumBagsWithinDeep(string bag)
        {
            if (!this._bagToInnerBags.TryGetValue(bag, out var innerBags)) return 0;
            
            var numBags = 0;

            foreach (var (innerBag, numInnerBags) in innerBags)
            {
                numBags += numInnerBags;
                numBags += numInnerBags * this.GetNumBagsWithinDeep(innerBag);
            }

            return numBags;
        }
    }
}