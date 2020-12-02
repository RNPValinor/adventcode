using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace _2020.Solvers
{
    public class Day2Solver : ISolver
    {
        public void Solve(string input)
        {
            var lines = input.Split(Environment.NewLine);

            var numValidPart1 = 0;
            var numValidPart2 = 0;

            foreach (var line in lines)
            {
                var password = new PasswordWithRequirements(line);

                if (password.IsValidPart1)
                {
                    numValidPart1++;
                }

                if (password.IsValidPart2)
                {
                    numValidPart2++;
                }
            }

            Console.WriteLine(numValidPart1);
            Console.WriteLine(numValidPart2);
        }

        private class PasswordWithRequirements
        {
            private string Password { get; set; }
            
            private char RequiredChar { get; set; }
            
            private int MinOccurrences { get; set; }
            
            private int MaxOccurrences { get; set; }
            
            public bool IsValidPart1
            {
                get
                {
                    var numOfRequiredChar = this.Password.Count(c => c == this.RequiredChar);
                    return numOfRequiredChar >= this.MinOccurrences && numOfRequiredChar <= this.MaxOccurrences;
                }
            }

            public bool IsValidPart2
            {
                get
                {
                    var matchAtFirstIdx = this.Password[this.MinOccurrences - 1] == this.RequiredChar;
                    var matchAtSecondIdx = this.Password[this.MaxOccurrences - 1] == this.RequiredChar;
                    return matchAtFirstIdx ^ matchAtSecondIdx;
                }
            }

            private static readonly Regex LineMatcher = new Regex("^([0-9]+)-([0-9]+) ([a-z]): ([a-z]+)$");

            public PasswordWithRequirements(string inputLine)
            {
                var match = LineMatcher.Match(inputLine);

                if (!match.Success || match.Groups.Count != 5)
                {
                    throw new ArgumentException($"Bad line detected: {inputLine}. Expected 4 matches but got {match.Groups.Count}");
                }

                this.MinOccurrences = int.Parse(match.Groups[1].Value);
                this.MaxOccurrences = int.Parse(match.Groups[2].Value);
                this.RequiredChar = match.Groups[3].Value[0];
                this.Password = match.Groups[4].Value;
            }
        }
    }
}