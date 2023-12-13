namespace _2023.Days;

public abstract class Day
{
    private readonly int _dayNum;
    protected string Part1Solution = "";
    protected string Part2Solution = "";

    protected Day(int dayNum)
    {
        this._dayNum = dayNum;
    }

    private void ProcessInput()
    {
        var fileName = $"Inputs/day{this._dayNum}.txt";
        var filePath = Path.Combine(Environment.CurrentDirectory, fileName);

        if (!File.Exists(filePath)) throw new FileNotFoundException("Failed to find input file", filePath);

        using var stream = new StreamReader(filePath);

        while (!stream.EndOfStream)
        {
            var line = stream.ReadLine();

            if (line != null)
                this.ProcessInputLine(line);
        }
    }

    public Result Solve()
    {
        var start = DateTime.UtcNow;
        this.ProcessInput();
        var stop = DateTime.UtcNow;
        var parseTime = stop - start;

        start = DateTime.UtcNow;
        this.SolvePart1();
        stop = DateTime.UtcNow;
        var part1SolveTime = stop - start;

        start = DateTime.UtcNow;
        this.SolvePart2();
        stop = DateTime.UtcNow;
        var part2SolveTime = stop - start;

        return new(this.Part1Solution, this.Part2Solution, parseTime, part1SolveTime, part2SolveTime);
    }

    protected abstract void ProcessInputLine(string line);

    protected abstract void SolvePart1();

    protected abstract void SolvePart2();
}