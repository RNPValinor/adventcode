package solvers;

import utils.Day2Game;

public class Day2Solver extends BaseSolver {
    private int SumOfValidGameIds = 0;
    private int SumOfGamePowers = 0;

    @SuppressWarnings("unused")
    public Day2Solver() {
        super(2);
    }

    @Override
    protected void processLine(String line) {
        var game = new Day2Game(line);

        if (game.IsValid()) {
            this.SumOfValidGameIds += game.GetId();
        }

        this.SumOfGamePowers += game.GetPower();
    }

    @Override
    protected String solvePart1() {
        return String.valueOf(this.SumOfValidGameIds);
    }

    @Override
    protected String solvePart2() {
        return String.valueOf(this.SumOfGamePowers);
    }
}
