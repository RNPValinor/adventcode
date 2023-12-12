package solvers;

import utils.NonogramSolver;

import java.util.Arrays;

@SuppressWarnings("unused")
public class Day12Solver extends BaseSolver {
    private int _totalNumCombinations = 0;

    public Day12Solver() {
        super(12);
    }

    @Override
    protected void processLine(String line) {
        var parts = line.split(" ");

        var knownBlocks = parts[0];
        var blockSizesStrs = parts[1].split(",");
        var blockSizes = new int[blockSizesStrs.length];

        for (var i = 0; i < blockSizesStrs.length; i++) {
            blockSizes[i] = Integer.parseInt(blockSizesStrs[i]);
        }

        this._totalNumCombinations += NonogramSolver.getNumCombinations(knownBlocks, blockSizes);
    }

    @Override
    protected String solvePart1() {
        return String.valueOf(this._totalNumCombinations);
    }

    @Override
    protected String solvePart2() {
        return null;
    }
}
