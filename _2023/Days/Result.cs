namespace _2023.Days;

public class Result
{
    private readonly string _part1Solution;
    private readonly string _part2Solution;
    private readonly TimeSpan _parseTime;
    private readonly TimeSpan _part1SolveTime;
    private readonly TimeSpan _part2SolveTime;

    public Result(string part1Solution, string part2Solution, TimeSpan parseTime, TimeSpan part1SolveTime, TimeSpan part2SolveTime)
    {
        this._part1Solution = part1Solution;
        this._part2Solution = part2Solution;
        this._parseTime = parseTime;
        this._part1SolveTime = part1SolveTime;
        this._part2SolveTime = part2SolveTime;
    }

    public void LogToConsole()
    {
        Console.WriteLine("Part 1:");
        Console.WriteLine(this._part1Solution);
        Console.WriteLine();
        Console.WriteLine("Part 2:");
        Console.WriteLine(this._part2Solution);
    }

    public string GetParseTime()
    {
        return GetSensibleTimespanString(this._parseTime);
    }

    public string GetPart1SolveTime()
    {
        return GetSensibleTimespanString(this._part1SolveTime);
    }
    
    public string GetPart2SolveTime()
    {
        return GetSensibleTimespanString(this._part2SolveTime);
    }

    public string GetTotalSolveTime()
    {
        var totalTime = this._parseTime + this._part1SolveTime + this._part2SolveTime;

        return GetSensibleTimespanString(totalTime);
    }
    
    private static string GetSensibleTimespanString(TimeSpan timeSpan)
    {
        return timeSpan.TotalMilliseconds + "ms";
    }
}