using System.Text.RegularExpressions;

namespace _2023.Days;

public partial class Day9 : Day
{
    private int _nextInSequenceSum;

    private int _previousInSequenceSum;
    
    public Day9() : base(9)
    {
    }

    protected override void ProcessInputLine(string line)
    {
        var sequence = new List<int>();

        var match = NumberPattern().Match(line);

        while (match.Success) {
            sequence.Add(int.Parse(match.Groups[1].Value));
            match = match.NextMatch();
        }

        var edges = GetPrevAndNextForSequence(sequence);

        this._previousInSequenceSum += edges[0];
        this._nextInSequenceSum += edges[1];
    }
    
    private static int[] GetPrevAndNextForSequence(List<int> sequence) {
        if (sequence[0] == 0 && sequence[^1] == 0) {
            return new[]{0, 0};
        }

        var differenceSequence = GetDifferences(sequence);

        var prevAndNextDiffs = GetPrevAndNextForSequence(differenceSequence);

        var priorNumber = sequence[0] - prevAndNextDiffs[0];
        var nextNumber = sequence[^1] + prevAndNextDiffs[1];

        return new[]{priorNumber, nextNumber};
    }
    
    private static List<int> GetDifferences(IReadOnlyList<int> sequence) {
        var differences = new List<int>(sequence.Count - 1);

        for (var i = 1; i < sequence.Count; i++) {
            differences.Add(sequence[i] - sequence[i - 1]);
        }

        return differences;
    }

    protected override void SolvePart1()
    {
        this.Part1Solution = this._nextInSequenceSum.ToString();
    }

    protected override void SolvePart2()
    {
        this.Part2Solution = this._previousInSequenceSum.ToString();
    }

    [GeneratedRegex("(-?[0-9]+)")]
    private static partial Regex NumberPattern();
}