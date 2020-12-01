using System;
using System.Collections.Generic;

namespace _2020.Solvers
{
    public class Day1Solver : ISolver
    {
        public void Solve(string input)
        {
            var orderedNumbers = new SortedList<int, int>();

            foreach (var item in input.Split(Environment.NewLine))
            {
                if (!int.TryParse(item, out var numberItem))
                {
                    throw new ArgumentException($"Failed to parse {item}");
                }

                orderedNumbers.Add(numberItem, numberItem);
            }
            
            // Part 1 & 2
            var answer1 = 0;
            var answer2 = 0;

            var theRealList = orderedNumbers.Values;

            for (int i = 0, len = theRealList.Count; i < len; i++)
            {
                for (var j = i + 1; j < len; j++)
                {
                    var p1Total = theRealList[i] + theRealList[j];

                    if (p1Total == 2020)
                    {
                        answer1 = theRealList[i] * theRealList[j];
                    }
                    else if (p1Total > 2020)
                    {
                        break;
                    }
                    
                    for (var k = j + 1; k < len; k++)
                    {
                        var p2Total = p1Total + theRealList[k];

                        if (p2Total == 2020)
                        {
                            answer2 = theRealList[i] * theRealList[j] * theRealList[k];
                        }
                        else if (p2Total > 2020)
                        {
                            break;
                        }
                    }
                }
                
                if (answer1 != 0 && answer2 != 0)
                {
                    break;
                }
            }

            Console.WriteLine($"{answer1} {answer2}");
        }
    }
}