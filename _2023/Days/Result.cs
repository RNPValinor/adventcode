namespace _2023.Days;

public class Result
{
    private readonly string _part1Solution;
    private readonly string _part2Solution;
    public readonly TimeSpan ParseTime;
    public readonly TimeSpan Part1SolveTime;
    public readonly TimeSpan Part2SolveTime;

    public Result(string part1Solution, string part2Solution, TimeSpan parseTime, TimeSpan part1SolveTime, TimeSpan part2SolveTime)
    {
        this._part1Solution = part1Solution;
        this._part2Solution = part2Solution;
        this.ParseTime = parseTime;
        this.Part1SolveTime = part1SolveTime;
        this.Part2SolveTime = part2SolveTime;
    }

    public void LogToConsole()
    {
        Console.WriteLine("Part 1:");
        Console.WriteLine(this._part1Solution);
        Console.WriteLine();
        Console.WriteLine("Part 2:");
        Console.WriteLine(this._part2Solution);
    }

    public TimeSpan GetTotalTime()
    {
        return this.ParseTime + this.Part1SolveTime + this.Part2SolveTime;
    }
}