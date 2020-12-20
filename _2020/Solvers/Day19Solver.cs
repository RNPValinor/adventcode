using System;
using System.Collections.Generic;
using System.Linq;

namespace _2020.Solvers
{
    public class Day19Solver : ISolver
    {
        private static readonly Dictionary<int, Rule> Rules = new();
        
        public void Solve(string input)
        {
            var isParsingRules = true;
            var numMatches = 0;
            
            foreach (var line in input.Split(Environment.NewLine))
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    isParsingRules = false;
                }
                else if (isParsingRules)
                {
                    ProcessRule(line);
                }
                else
                {
                    numMatches += IsMatch(line) ? 1 : 0;
                }
            }

            Console.WriteLine(numMatches);
        }

        private static void ProcessRule(string ruleStr)
        {
            var parts = ruleStr.Split(": ");

            var ruleId = int.Parse(parts[0]);

            var rule = new Rule(parts[1]);

            Rules.Add(ruleId, rule);
        }

        private static bool IsMatch(string str)
        {
            return Rules[0].GetMatches().Contains(str);
        }

        private class Rule
        {
            private readonly char? _baseRuleChar;
            private readonly HashSet<List<int>> _options = new();

            private HashSet<string> _matches;

            protected Rule()
            {}

            public Rule(string optionsStr)
            {
                if (optionsStr.Contains('"'))
                {
                    this._baseRuleChar = optionsStr[optionsStr.IndexOf('"') + 1];
                }
                else
                {
                    var options = optionsStr.Split(" | ");

                    foreach (var opt in options)
                    {
                        this._options.Add(opt.Split(" ").Select(int.Parse).ToList());
                    }    
                }
            }

            public HashSet<string> GetMatches()
            {
                if (this._matches == null)
                {
                    this.GenerateMatches();
                }

                return this._matches;
            }

            private void GenerateMatches()
            {
                this._matches = new HashSet<string>();
                
                if (this._baseRuleChar.HasValue)
                {
                    this._matches.Add($"{this._baseRuleChar.Value}");
                }
                else
                {
                    foreach (var option in this._options)
                    {
                        switch (option.Count)
                        {
                            case 1:
                                this._matches.UnionWith(Rules[option[0]].GetMatches());
                                break;
                            case 2:
                            {
                                var firstMatches = Rules[option[0]].GetMatches();
                                var secondMatches = Rules[option[1]].GetMatches();

                                foreach (var fm in firstMatches)
                                {
                                    foreach (var sm in secondMatches)
                                    {
                                        this._matches.Add($"{fm}{sm}");
                                    }
                                }
                                break;
                            }
                            default:
                                throw new ArgumentException("Unexpected number of options!");
                        }
                    }
                }
            }
        }

        /*
        private class NewRule8 : Rule
        {
            
            
            public NewRule8(Rule oldRule8)
            {
                
            }
            
            public void GenerateRegexString()
            {
                
                
                Console.WriteLine("Called special func!");
                var rule42 = Rules[42].GetRegexString();
                
                this._regexString = $"({rule42})+";
            }
            
            public new HashSet<string> GetMatches()
            {
                var matchSet = 
            }
        }

        private class NewRule11 : Rule
        {
            public new HashSet<string> GetMatches()
            {
                return null;
            }
        }
        */
    }
}