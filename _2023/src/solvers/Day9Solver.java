package solvers;

import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

@SuppressWarnings("unused")
public class Day9Solver extends BaseSolver {
    private int _nextInSequenceSum = 0;

    private int _previousInSequenceSum = 0;

    public Day9Solver() {
        super(9);
    }

    @Override
    protected void ProcessLine(String line) {
        var sequence = Arrays.stream(line.split(" "))
                .map(Integer::parseInt)
                .toList();

        var edges = this.getPrevAndNextForSequence(sequence);

        this._previousInSequenceSum += edges[0];
        this._nextInSequenceSum += edges[1];
    }

    private Integer[] getPrevAndNextForSequence(List<Integer> sequence) {
        if (sequence.isEmpty()) {
            throw new IllegalArgumentException("Got an empty sequence when trying to get next");
        } else if (sequence.stream().allMatch(n -> n == 0)) {
            return new Integer[]{0, 0};
        }

        var differenceSequence = this.getDifferences(sequence);

        var prevAndNextDiffs = this.getPrevAndNextForSequence(differenceSequence);

        var priorNumber = sequence.get(0) - prevAndNextDiffs[0];
        var nextNumber = sequence.get(sequence.size() - 1) + prevAndNextDiffs[1];

        return new Integer[]{priorNumber, nextNumber};
    }

    private List<Integer> getDifferences(List<Integer> sequence) {
        var differences = new ArrayList<Integer>(sequence.size() - 1);

        for (var i = 1; i < sequence.size(); i++) {
            differences.add(sequence.get(i) - sequence.get(i - 1));
        }

        return differences;
    }

    @Override
    protected String SolvePart1() {
        return String.valueOf(this._nextInSequenceSum);
    }

    @Override
    protected String SolvePart2() {
        return String.valueOf(this._previousInSequenceSum);
    }
}
