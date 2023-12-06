package solvers;

public class Solvers {
    public static BaseSolver GetSolver(int day) {
        return switch (day) {
            case 1 -> new Day1Solver();
            case 2 -> new Day2Solver();
            case 3 -> new Day3Solver();
            case 4 -> new Day4Solver();
            case 5 -> new Day5Solver();
            case 6 -> new Day6Solver();
            default -> throw new IndexOutOfBoundsException("Day not supported: " + day);
        };
    }
}
