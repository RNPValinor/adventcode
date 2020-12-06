using System;
using System.Collections.Generic;
using System.Linq;

namespace _2020.Solvers
{
    public class Day6Solver : ISolver
    {
        public void Solve(string input)
        {
            var groups = input.Split($"{Environment.NewLine}{Environment.NewLine}");

            var numYeses = 0;
            var realNumYeses = 0;

            foreach (var group in groups)
            {
                var answers = new HashSet<char>();
                var realAnswers = new Dictionary<char, int>();

                var people = group.Split(Environment.NewLine);

                foreach (var person in people)
                {
                    foreach (var answer in person)
                    {
                        answers.Add(answer);

                        if (realAnswers.TryGetValue(answer, out var numSeen))
                        {
                            realAnswers.Remove(answer);
                            realAnswers.Add(answer, numSeen + 1);
                        }
                        else
                        {
                            realAnswers.Add(answer, 1);
                        }
                    }
                }

                numYeses += answers.Count;
                realNumYeses += realAnswers.Values.Count(c => c == people.Length);
            }

            Console.WriteLine(numYeses);
            Console.WriteLine(realNumYeses);
        }
    }
}