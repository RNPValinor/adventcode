package solvers;

import java.util.*;
import java.util.regex.Pattern;

@SuppressWarnings("unused")
public class Day5Solver extends BaseSolver {
    private boolean _hasParsedSeeds = false;
    private final HashSet<Long> _currentValues = new HashSet<>();
    private final HashSet<Long> _nextValues = new HashSet<>();

    private final SortedSet<Range> _currentRanges = new TreeSet<>(Comparator.comparing(r -> r.Start));
    private final HashSet<Range> _nextRanges = new HashSet<>();

    private final Pattern _seedPattern = Pattern.compile("([0-9]+) ([0-9]+)");
    private final Pattern _transformPattern = Pattern.compile("^([0-9]+) ([0-9]+) ([0-9]+)$");

    public Day5Solver() {
        super(5);
    }

    @Override
    protected void processLine(String line) {
        if (!this._hasParsedSeeds) {
            var matcher = this._seedPattern.matcher(line);

            while (matcher.find()) {
                var seedVal = Long.parseLong(matcher.group(1));
                var range = Long.parseLong(matcher.group(2));

                this._currentValues.add(seedVal);
                this._currentValues.add(range);

                this._currentRanges.add(new Range(seedVal, seedVal + range - 1));
            }

            this._hasParsedSeeds = true;

            return;
        }

        if (line.isEmpty()) {
            // Split between the ranges;
            this.PromoteValues();
            return;
        }

        var matcher = this._transformPattern.matcher(line);

        // Skip the text lines; who cares about them.
        if (matcher.find()) {
            var destinationRangeStart = Long.parseLong(matcher.group(1));
            var sourceRangeStart = Long.parseLong(matcher.group(2));
            var rangeLength = Long.parseLong(matcher.group(3));

            this.ProcessRangePart1(destinationRangeStart, sourceRangeStart, rangeLength);
            this.ProcessRangePart2(destinationRangeStart, sourceRangeStart, rangeLength);
        }
    }

    private void ProcessRangePart1(Long destinationRangeStart, Long sourceRangeStart, Long rangeLength) {
        var diff = destinationRangeStart - sourceRangeStart;
        var sourceRangeEnd = sourceRangeStart + rangeLength;

        var changedValues = new HashSet<Long>();

        for (var val : this._currentValues) {
            if (val >= sourceRangeStart && val < sourceRangeEnd) {
                changedValues.add(val);
                this._nextValues.add(val + diff);
            }
        }

        this._currentValues.removeAll(changedValues);
    }

    private void ProcessRangePart2(Long destinationRangeStart, Long sourceRangeStart, Long rangeLength) {
        var diff = destinationRangeStart - sourceRangeStart;
        var sourceRange = new Range(sourceRangeStart, sourceRangeStart + rangeLength - 1);

        var removedRanges = new HashSet<Range>();
        var splitRanges = new HashSet<Range>();

        for (var range : this._currentRanges) {
            if (range.Start > sourceRange.End) {
                // The ranges are in order, won't overlap - if we've gotten to one which starts
                // after the source range then we can stop iterating.
                break;
            } else if (range.Overlaps(sourceRange)) {
                removedRanges.add(range);

                var overlapRange = range.GetOverlap(sourceRange);
                var remainingRangeParts = range.RemoveSubRange(overlapRange);

                this._nextRanges.add(new Range(overlapRange.Start + diff, overlapRange.End + diff));
                splitRanges.addAll(remainingRangeParts);
            }
        }

        this._currentRanges.removeAll(removedRanges);
        this._currentRanges.addAll(splitRanges);
    }

    private void PromoteValues() {
        this._currentValues.addAll(this._nextValues);
        this._nextValues.clear();

        this._currentRanges.addAll(this._nextRanges);
        this._nextRanges.clear();
    }

    @Override
    protected String solvePart1() {
        this.PromoteValues();

        return String.valueOf(Collections.min(this._currentValues));
    }

    @Override
    protected String solvePart2() {
        this.PromoteValues();

        return String.valueOf(this._currentRanges.first().Start);
    }

    private static class Range {
        public final Long Start;
        public final Long End;

        public Range(Long start, Long end) {
            this.Start = start;
            this.End = end;
        }

        public boolean Overlaps(Range range) {
            return this.Start <= range.End && this.End >= range.Start;
        }

        public Range GetOverlap(Range range) {
            if (!this.Overlaps(range)) {
                throw new IllegalArgumentException("This range does not overlap with that range!");
            }

            return new Range(Math.max(this.Start, range.Start), Math.min(this.End, range.End));
        }

        public List<Range> RemoveSubRange(Range subRange) {
            var newSubRanges = new ArrayList<Range>();

            if (this.Start < subRange.Start) {
                newSubRanges.add(new Range(this.Start, subRange.Start - 1));
            }

            if (this.End > subRange.End) {
                newSubRanges.add(new Range(subRange.End + 1, this.End));
            }

            return newSubRanges;
        }

        @Override
        public boolean equals(Object o) {
            if (this == o) return true;
            if (o == null || getClass() != o.getClass()) return false;
            Range range = (Range) o;
            return Objects.equals(Start, range.Start) && Objects.equals(End, range.End);
        }

        @Override
        public int hashCode() {
            return Objects.hash(Start, End);
        }
    }
}
