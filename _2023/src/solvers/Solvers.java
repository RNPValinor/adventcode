package solvers;

public class Solvers
{
    public static BaseSolver GetSolver(int day)
    {
        switch (day)
        {
            case 1:
                return new Day1Solver();
            default:
                throw new IndexOutOfBoundsException("Day not supported: " + day);
        }
    }
}
