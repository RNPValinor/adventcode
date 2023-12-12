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

        this.ProcessInput();

        Console.WriteLine();
        Console.WriteLine("Part 1:");

        this.SolvePart1();

        Console.WriteLine(this.Part1Solution);
        Console.WriteLine();
        Console.WriteLine("Part 2:");

        this.SolvePart2();

        Console.WriteLine(this.Part2Solution);
    }

    protected abstract void ProcessInputLine(string line);

    protected abstract void SolvePart1();

    protected abstract void SolvePart2();
}