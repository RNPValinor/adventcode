using Figgle;

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

    public void Solve()
    {
        Console.WriteLine(FiggleFonts.Banner3.Render($"Advent of Code Day {this._dayNum}"));
        Console.WriteLine();
        Console.WriteLine("Processing input...");

        var start = DateTime.UtcNow;
        this.ProcessInput();
        var stop = DateTime.UtcNow;
        var inputProcessTime = stop - start;

        Console.WriteLine();
        Console.WriteLine("Part 1:");

        start = DateTime.UtcNow;
        this.SolvePart1();
        stop = DateTime.UtcNow;
        var part1SolveTime = stop - start;

        Console.WriteLine(this.Part1Solution);
        Console.WriteLine();
        Console.WriteLine("Part 2:");

        start = DateTime.UtcNow;
        this.SolvePart2();
        stop = DateTime.UtcNow;
        var part2SolveTime = stop - start;

        Console.WriteLine(this.Part2Solution);

        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("Timings");
        Console.WriteLine("Input processing: " + GetSensibleTimespanString(inputProcessTime));
        Console.WriteLine("Part 1: " + GetSensibleTimespanString(part1SolveTime));
        Console.WriteLine("Part 2: " + GetSensibleTimespanString(part2SolveTime));
    }

    private static string GetSensibleTimespanString(TimeSpan timeSpan)
    {
        return timeSpan.TotalMilliseconds + "ms";
    }

    protected abstract void ProcessInputLine(string line);

    protected abstract void SolvePart1();

    protected abstract void SolvePart2();
}