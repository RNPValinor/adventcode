using System;
using System.Collections.Generic;
using System.Linq;

namespace _2020.Solvers
{
    public class Day3Solver : ISolver
    {
        private const char Tree = '#';
        
        public void Solve(string input)
        {
            var lines = input.Split(Environment.NewLine);

            var xPoses = new List<int> {0, 0, 0, 0, 0};
            var xChanges = new List<int> {1, 3, 5, 7, 1};
            var yChanges = new List<int> {1, 1, 1, 1, 2};

            var numTrees = new List<int> {0, 0, 0, 0, 0};

            var curY = 0;

            foreach (var line in lines)
            {
                for (var i = 0; i < xPoses.Count; i++)
                {
                    var xPos = xPoses[i];

                    if (curY % yChanges[i] != 0)
                    {
                        continue;
                    }

                    if (xPos >= line.Length)
                    {
                        xPos %= line.Length;
                    }
                    
                    if (line[xPos] == Tree)
                    {
                        numTrees[i]++;
                    }

                    xPoses[i] = xPos + xChanges[i];
                }

                curY++;
            }

            var treeProduct = numTrees.Aggregate(1, (current, treeCount) => current * treeCount);

            Console.WriteLine(numTrees[1]);
            Console.WriteLine(treeProduct);
        }
    }
}