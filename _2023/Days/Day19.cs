using System.Text.RegularExpressions;
using _2023.Utils;

using RuleData = (string targetWorkflow, _2023.Utils.Day19Operator op, char category, int value);
using Range = (int min, int max);

namespace _2023.Days;

public partial class Day19() : Day(19)
{
    private const string FirstWorkflowName = "in";
    private static readonly Dictionary<string, Workflow> Workflows = new();
    
    private int _totalAcceptedRatings;
    
    protected override void ProcessInputLine(string line)
    {
        if (line.Length is 0)
        {
            return;
        }
        
        var firstBracket = line.IndexOf('{');

        if (firstBracket is 0)
        {
            // It's a part
            this.ProcessPart(line);
        }
        else
        {
            // It's a workflow
            var workflowName = line[..firstBracket];
            var conditions = line
                .Substring(firstBracket + 1, line.Length - workflowName.Length - 2)
                .Split(',');

            var workflow = new Workflow();

            foreach (var condition in conditions)
            {
                ProcessCondition(condition, workflow);
            }

            Workflows.TryAdd(workflowName, workflow);
        }
    }

    private static void ProcessCondition(string condition, Workflow workflow)
    {
        var conditionParts = condition.Split(':');

        if (conditionParts.Length is 1)
        {
            workflow.AddBaseCase(condition);
        }
        else
        {
            var match = RuleConditionRegex().Match(conditionParts[0]);

            if (match.Success is false)
            {
                throw new ArgumentException($"Failed to parse condition {condition}", nameof(condition));
            }

            var category = match.Groups[1].Value[0];
            var isLessThan = match.Groups[2].Value is "<";
            var value = int.Parse(match.Groups[3].Value);
                    
            workflow.AddComparisonRule(category, isLessThan, value, conditionParts[1]);
        }
    }

    private void ProcessPart(string line)
    {
        var partCategories = line
            .Substring(1, line.Length - 2)
            .Split(",");

        var part = new Dictionary<char, int>();

        foreach (var category in partCategories)
        {
            var categoryName = category[0];
            var categoryValue = int.Parse(category[2..]);

            part.Add(categoryName, categoryValue);
        }

        var startWorkflow = Workflows[FirstWorkflowName];

        if (startWorkflow.IsAccepted(part))
        {
            this._totalAcceptedRatings += part.Values.Sum();
        }
    }
    
    protected override void SolvePart1()
    {
        this.Part1Solution = this._totalAcceptedRatings.ToString();
    }

    protected override void SolvePart2()
    {
        var startWorkflow = Workflows[FirstWorkflowName];

        var acceptableParts = new Dictionary<char, Range>
        {
            { 'x', (min: 1, max: 4000) },
            { 'm', (min: 1, max: 4000) },
            { 'a', (min: 1, max: 4000) },
            { 's', (min: 1, max: 4000) }
        };

        this.Part2Solution = startWorkflow.GetNumAcceptableParts(acceptableParts).ToString();
    }

    private class Workflow
    {
        private readonly List<Func<Dictionary<char, int>, (WorkflowResult result, string nextWorkflow)>> _rules = [];
        private readonly List<RuleData> _ruleData = [];
        
        public void AddComparisonRule(char category, bool isLessThan, int value, string targetWorkflow)
        {
            this._rules.Add(part =>
            {
                var partVal = part[category];

                if ((isLessThan && partVal < value) || (!isLessThan && partVal > value))
                {
                    return targetWorkflow switch
                    {
                        "A" => (WorkflowResult.Accepted, ""),
                        "R" => (WorkflowResult.Rejected, ""),
                        _ => (WorkflowResult.Referral, targetWorkflow)
                    };
                }
                
                return (WorkflowResult.Inconclusive, "");
            });
            
            this._ruleData.Add((targetWorkflow, isLessThan ? Day19Operator.LessThan : Day19Operator.GreaterThan, category, value));
        }

        public void AddBaseCase(string targetWorkflow)
        {
            this._rules.Add(_ =>
            {
                return targetWorkflow switch
                {
                    "A" => (WorkflowResult.Accepted, ""),
                    "R" => (WorkflowResult.Rejected, ""),
                    _ => (WorkflowResult.Referral, targetWorkflow)
                };
            });
            
            this._ruleData.Add((targetWorkflow, Day19Operator.None, default, default));
        }
        
        public bool IsAccepted(Dictionary<char, int> part)
        {
            foreach (var rule in this._rules)
            {
                var (result, nextWorkflow) = rule(part);

                switch (result)
                {
                    case WorkflowResult.Accepted:
                        return true;
                    case WorkflowResult.Rejected:
                        return false;
                    case WorkflowResult.Referral:
                        return Workflows[nextWorkflow].IsAccepted(part);
                    case WorkflowResult.Inconclusive:
                    default:
                        break;
                }
            }
            
            throw new ApplicationException("Failed to find a successful rule!");
        }

        public long GetNumAcceptableParts(Dictionary<char, Range> possibleParts)
        {
            var numAcceptable = 0L;
            
            foreach (var ruleData in this._ruleData)
            {
                if (ruleData.op is Day19Operator.None)
                {
                    // Base case.
                    numAcceptable += ruleData.targetWorkflow switch
                    {
                        "A" => GetNumPossibilities(possibleParts),
                        "R" => 0,
                        _ => Workflows[ruleData.targetWorkflow].GetNumAcceptableParts(possibleParts)
                    };

                    return numAcceptable;
                }

                var (passingRange, failingRange) = GetPassingAndFailingRangesForRule(possibleParts, ruleData);

                numAcceptable += ProcessPassingRange(passingRange, ruleData, possibleParts);

                if (failingRange.max >= failingRange.min)
                {
                    // Valid range.
                    possibleParts[ruleData.category] = failingRange;
                }
                else
                {
                    // Not possible to fail this condition, so end iteration here
                    return numAcceptable;
                }
            }
            
            return numAcceptable;
        }

        private static (Range passingRange, Range failingRange)
            GetPassingAndFailingRangesForRule(Dictionary<char, Range> possibleParts, RuleData ruleData)
        {
            var curRange = possibleParts[ruleData.category];
            Range passingRange, failingRange;

            if (ruleData.op is Day19Operator.GreaterThan)
            {
                passingRange = (Math.Max(ruleData.value + 1, curRange.min), curRange.max);
                failingRange = (curRange.min, Math.Min(ruleData.value, curRange.max));
            }
            else
            {
                passingRange = (curRange.min, Math.Min(ruleData.value - 1, curRange.max));
                failingRange = (Math.Max(ruleData.value, curRange.min), curRange.max);
            }

            return (passingRange, failingRange);
        }

        private static long GetNumPossibilities(Dictionary<char, Range> parts)
        {
            return parts
                .Values
                .Aggregate(1L, (prev, range) => prev * (range.max - range.min + 1));
        }

        private static long ProcessPassingRange(Range passingRange, RuleData ruleData, Dictionary<char, Range> possibleParts)
        {
            // Check if valid range
            if (passingRange.max < passingRange.min) return 0;
            
            var nextPossibleParts = new Dictionary<char, Range>(possibleParts)
            {
                [ruleData.category] = passingRange
            };

            return ruleData.targetWorkflow switch
            {
                "A" => GetNumPossibilities(nextPossibleParts),
                "R" => 0,
                _ => Workflows[ruleData.targetWorkflow].GetNumAcceptableParts(nextPossibleParts)
            };

        }

        private enum WorkflowResult
        {
            Accepted,
            Rejected,
            Referral,
            Inconclusive
        }

        
    }

    [GeneratedRegex("([xmas])([><])([0-9]+)")]
    private static partial Regex RuleConditionRegex();
}