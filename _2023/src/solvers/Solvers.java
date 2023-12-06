package solvers;

import java.lang.reflect.InvocationTargetException;

public class Solvers {
    public static BaseSolver GetSolver(int day) {
        try {
            Class<?> solverClass = Class.forName("solvers.Day" + day + "Solver");
            var constructor = solverClass.getConstructor();
            return (BaseSolver) constructor.newInstance();
        } catch (ClassNotFoundException e) {
            throw new IndexOutOfBoundsException("Day not supported: " + day);
        } catch (NoSuchMethodException | InstantiationException | IllegalAccessException |
                 InvocationTargetException e) {
            throw new RuntimeException("Failed to load class for day: " + day);
        }
    }
}
