using System;
using System.Linq;
using System.Reflection;

namespace _2020.Solvers
{
    public static class SolverManager
    {
        public static ISolver GetSolver(int day)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var type = assembly.GetTypes().FirstOrDefault(t => t.Name == $"Day{day}Solver");

            if (type == null)
            {
                throw new ArgumentOutOfRangeException($"No solution found for day {day}");
            }

            var solver = Activator.CreateInstance(type);

            try
            {
                return (ISolver) solver;
            }
            catch (InvalidCastException)
            {
                throw new ArgumentOutOfRangeException($"No solution found for day {day}");
            }
        }
    }
}