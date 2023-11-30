package solvers;

public class Day1Solver extends BaseSolver {
    public Day1Solver()
    {
        super(1);
    }

    @Override
    protected void ProcessLine(String line) {
        System.out.println("Processed line: " + line);
    }

    @Override
    protected String SolvePart1() {
        return ":D";
    }

    @Override
    protected String SolvePart2() {
        return ":(";
    }
}
