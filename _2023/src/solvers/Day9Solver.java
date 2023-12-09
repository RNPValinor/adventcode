package solvers;

import java.util.ArrayList;
import java.util.LinkedList;
import java.util.List;
import java.util.regex.Pattern;

@SuppressWarnings("unused")
public class Day9Solver extends BaseSolver {
    private static final Pattern _numberPattern = Pattern.compile("(-?[0-9]+)");

    private int _nextInSequenceSum = 0;

    private int _previousInSequenceSum = 0;

    public Day9Solver() {
        super(9);
    }

    @Override
    protected void ProcessLine(String line) {
        var sequence = new LinkedList<Integer>();

        var matcher = _numberPattern.matcher(line);

        while (matcher.find()) {
            sequence.add(Integer.parseInt(matcher.group(1)));
        }

        var edges = this.getPrevAndNextForSequence(sequence);

        this._previousInSequenceSum += edges[0];
        this._nextInSequenceSum += edges[1];
    }

    private Integer[] getPrevAndNextForSequence(List<Integer> sequence) {
        if (sequence.get(0) == 0 && sequence.get(sequence.size() - 1) == 0) {
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
