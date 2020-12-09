using System;
using System.Collections.Generic;
using System.Linq;

namespace _2020.Solvers
{
    public class Day9Solver : ISolver
    {
        public void Solve(string input)
        {
            var numbers = input.Split(Environment.NewLine).Select(long.Parse).ToList();

            var prevNumbers = new HashSet<long>();
            var prevNumberList = new LinkedList<long>();

            long? targetNumber = null;

            foreach (var number in numbers)
            {
                if (prevNumbers.Count < 25)
                {
                    prevNumbers.Add(number);
                    prevNumberList.AddLast(number);
                }
                else
                {
                    var foundMatchingNumbers = prevNumbers.Any(prevNum =>
                    {
                        var target = number - prevNum;

                        return target != prevNum && prevNumbers.Contains(target);
                    });

                    if (!foundMatchingNumbers)
                    {
                        targetNumber = number;
                        break;
                    }
                    else
                    {
                        var oldestNumber = prevNumberList.First.Value;

                        prevNumbers.Remove(oldestNumber);
                        prevNumberList.RemoveFirst();

                        prevNumbers.Add(number);
                        prevNumberList.AddLast(number);
                    }
                }
            }

            if (targetNumber == null)
            {
                throw new Exception("AAAAAAAAAAAAA");
            }

            long currentTotal = 0;
            var currentSet = new LinkedList<long>();
            var curListIdx = 0;

            while (currentTotal != targetNumber)
            {
                if (numbers[curListIdx] == targetNumber)
                {
                    throw new Exception("Failed to solve part 2");
                }
                
                while (currentTotal > targetNumber)
                {
                    // Too big; remove from the start.
                    currentTotal -= currentSet.First.Value;
                    currentSet.RemoveFirst();
                }

                while (currentTotal < targetNumber)
                {
                    // Too small; add to end
                    var numberToAdd = numbers[curListIdx++];

                    currentTotal += numberToAdd;
                    currentSet.AddLast(numberToAdd);
                }
            }

            Console.WriteLine(targetNumber);
            Console.WriteLine(currentSet.Min() + currentSet.Max());
        }
    }
}