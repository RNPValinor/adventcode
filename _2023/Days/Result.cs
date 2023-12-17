namespace _2023.Days;

public class Result(
    string part1Solution,
    string part2Solution,
    TimeSpan parseTime,
    TimeSpan part1SolveTime,
    TimeSpan part2SolveTime)
{
    public readonly TimeSpan ParseTime = parseTime;
    public readonly TimeSpan Part1SolveTime = part1SolveTime;
    public readonly TimeSpan Part2SolveTime = part2SolveTime;

    public void LogToConsole()
    {
        Console.WriteLine("Part 1:");
        Console.WriteLine(part1Solution);
        Console.WriteLine();
        Console.WriteLine("Part 2:");
        Console.WriteLine(part2Solution);
    }

    public TimeSpan GetTotalTime()
    {
        return this.ParseTime + this.Part1SolveTime + this.Part2SolveTime;
    }
}