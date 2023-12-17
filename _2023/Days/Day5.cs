using System.Text.RegularExpressions;

namespace _2023.Days;

public partial class Day5() : Day(5)
{
    private bool _hasParsedSeeds;
    private readonly HashSet<long> _currentValues = [];
    private readonly HashSet<long> _nextValues = [];

    private readonly SortedSet<Range> _currentRanges = [];
    private readonly List<Range> _nextRanges = [];

    protected override void ProcessInputLine(string line)
    {
        if (!this._hasParsedSeeds)
        {
            var seedMatch = SeedPattern().Match(line);

            while (seedMatch.Success) {
                var seedVal = long.Parse(seedMatch.Groups[1].Value);
                var range = long.Parse(seedMatch.Groups[2].Value);

                this._currentValues.Add(seedVal);
                this._currentValues.Add(range);

                this._currentRanges.Add(new(seedVal, seedVal + range - 1));

                seedMatch = seedMatch.NextMatch();
            }

            this._hasParsedSeeds = true;

            return;
        }

        if (string.IsNullOrWhiteSpace(line)) {
            // Split between the ranges;
            this.PromoteValues();
            return;
        }

        var transformMatch = TransformPattern().Match(line);

        if (transformMatch.Success is false)
        {
            return;
        }

        // Skip the text lines; who cares about them.
        var destinationRangeStart = long.Parse(transformMatch.Groups[1].Value);
        var sourceRangeStart = long.Parse(transformMatch.Groups[2].Value);
        var rangeLength = long.Parse(transformMatch.Groups[3].Value);

        this.ProcessRangePart1(destinationRangeStart, sourceRangeStart, rangeLength);
        this.ProcessRangePart2(destinationRangeStart, sourceRangeStart, rangeLength);
    }
    
    private void PromoteValues() {
        this._currentValues.UnionWith(this._nextValues);
        this._nextValues.Clear();

        this._currentRanges.UnionWith(this._nextRanges);
        this._nextRanges.Clear();
    }
    
    private void ProcessRangePart1(long destinationRangeStart, long sourceRangeStart, long rangeLength) {
        var diff = destinationRangeStart - sourceRangeStart;
        var sourceRangeEnd = sourceRangeStart + rangeLength;

        var changedValues = new HashSet<long>();

        foreach (var val in this._currentValues.Where(val => val >= sourceRangeStart && val < sourceRangeEnd))
        {
            changedValues.Add(val);
            this._nextValues.Add(val + diff);
        }

        foreach (var changed in changedValues)
        {
            this._currentValues.Remove(changed);
        }
    }

    private void ProcessRangePart2(long destinationRangeStart, long sourceRangeStart, long rangeLength) {
        var diff = destinationRangeStart - sourceRangeStart;
        var sourceRange = new Range(sourceRangeStart, sourceRangeStart + rangeLength - 1);

        var removedRanges = new HashSet<Range>();
        var splitRanges = new HashSet<Range>();

        foreach (var range in this._currentRanges.TakeWhile(range => range.Start <= sourceRange.End).Where(range => range.Overlaps(sourceRange)))
        {
            removedRanges.Add(range);

            var overlapRange = range.GetOverlap(sourceRange);
            var remainingRangeParts = range.RemoveSubRange(overlapRange);

            this._nextRanges.Add(new(overlapRange.Start + diff, overlapRange.End + diff));
            splitRanges.UnionWith(remainingRangeParts);
        }

        foreach (var removed in removedRanges)
        {
            this._currentRanges.Remove(removed);
        }

        this._currentRanges.UnionWith(splitRanges);
    }

    protected override void SolvePart1()
    {
        this.PromoteValues();

        this.Part1Solution = this._currentValues.Min().ToString();
    }

    protected override void SolvePart2()
    {
        this.PromoteValues();

        this.Part2Solution = this._currentRanges.First().Start.ToString();
    }
    
    private class Range(long start, long end) : IComparable<Range>
    {
        public readonly long Start = start;
        public readonly long End = end;

        public bool Overlaps(Range range) {
            return this.Start <= range.End && this.End >= range.Start;
        }

        public Range GetOverlap(Range range) {
            if (!this.Overlaps(range)) {
                throw new ArgumentException("This range does not overlap with that range!");
            }

            return new(Math.Max(this.Start, range.Start), Math.Min(this.End, range.End));
        }

        public IEnumerable<Range> RemoveSubRange(Range subRange) {
            var newSubRanges = new List<Range>();

            if (this.Start < subRange.Start) {
                newSubRanges.Add(new(this.Start, subRange.Start - 1));
            }

            if (this.End > subRange.End) {
                newSubRanges.Add(new(subRange.End + 1, this.End));
            }

            return newSubRanges;
        }
        
        protected bool Equals(Range other)
        {
            return this.Start == other.Start && this.End == other.End;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return this.Equals((Range)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.Start, this.End);
        }

        public int CompareTo(Range? other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return this.Start.CompareTo(other.Start);
        }
    }

    [GeneratedRegex("([0-9]+) ([0-9]+)")]
    private static partial Regex SeedPattern();
    [GeneratedRegex("^([0-9]+) ([0-9]+) ([0-9]+)$")]
    private static partial Regex TransformPattern();
}