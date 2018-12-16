using System;
using System.Collections.Generic;
using System.Linq;
using _2018.Utils;

namespace _2018.Days
{
    public class Day12 : Day
    {
        private const string Input = ".#..##..#.....######.....#....####.##.#.#...#...##.#...###..####.##.##.####..######......#..##.##.##";

        private readonly Bst<bool> _transforms = new Bst<bool>();

        public Day12()
        {
            this.InitialiseTransforms();
        }

        private void InitialiseTransforms()
        {
            var transformData = QuestionLoader.Load(12).Split(Environment.NewLine);

            foreach (var transform in transformData)
            {
                var node = this._transforms.Root;

                var transformParts = transform.Split(' ');
                
                foreach (var plant in transformParts[0])
                {
                    node = plant == '#' ? node.GetRight() : node.GetLeft();
                }

                node.Value = transformParts[2] != ".";
            }
        }

        private bool GetTransform(IEnumerable<bool> pattern)
        {
            var node = this._transforms.Root;

            node = pattern.Aggregate(node, (current, p) => p ? current.GetRight() : current.GetLeft());

            return node.Value;
        }
        
        protected override void DoPart1()
        {
            var currentGen = Input.Select(plant => plant == '#').ToList();
            var nextGen = new List<bool>();
            
            for (var i = 0; i < 20; i++)
            {
                for (var j = -2; j < currentGen.Count + 2; j++)
                {
                    var pattern = new List<bool>();

                    for (var k = j - 2; k <= j + 2; k++)
                    {
                        if (k < 0 || k >= currentGen.Count)
                        {
                            pattern.Add(false);
                        }
                        else
                        {
                            pattern.Add(currentGen[k]);
                        }
                    }
                    
                    nextGen.Add(this.GetTransform(pattern));
                }

                currentGen = nextGen;
                nextGen = new List<bool>();
            }
            
            // Having run 20 iterations and added 2 plants onto the front each time, we know that the 0-pot
            // it at index 40
            var potSum = currentGen.Select((plant, index) => plant ? (index - 40) : 0).Sum();
            
            ConsoleUtils.WriteColouredLine($"Resulting pot sum is {potSum}", ConsoleColor.Cyan);
        }

        protected override void DoPart2()
        {
            var currentGen = Input.Select(plant => plant == '#').ToList();
            var nextGen = new List<bool>();

            int potDiff;

            var newPotSum = currentGen.Select((plant, index) => plant ? index : 0).Sum();
            var newPotDiff = 0;
            
            var i = 0;

            do
            {
                var potSum = newPotSum;
                potDiff = newPotDiff;
                
                for (var j = -2; j < currentGen.Count + 2; j++)
                {
                    var pattern = new List<bool>();

                    for (var k = j - 2; k <= j + 2; k++)
                    {
                        if (k < 0 || k >= currentGen.Count)
                        {
                            pattern.Add(false);
                        }
                        else
                        {
                            pattern.Add(currentGen[k]);
                        }
                    }

                    nextGen.Add(this.GetTransform(pattern));
                }

                currentGen = nextGen;
                nextGen = new List<bool>();

                i++;
                newPotSum = currentGen.Select((plant, index) => plant ? (index - (2 * i)) : 0).Sum();
                newPotDiff = newPotSum - potSum;
            } while (newPotDiff != potDiff);

            ConsoleUtils.WriteColouredLine($"Got stable increase of (+{potDiff}) after {i} generations, potSum is {newPotSum}", ConsoleColor.Green);

            var endPotSum = newPotSum + (50000000000 - i) * potDiff;
            
            var colour = ConsoleColor.Cyan;

            if (endPotSum >= 4050000001041 || endPotSum <= 2025000000520 || endPotSum == 4050000000960)
            {
                colour = ConsoleColor.Red;
            }
            
            ConsoleUtils.WriteColouredLine($"End pot sum is {endPotSum}", colour);
        }
    }
}