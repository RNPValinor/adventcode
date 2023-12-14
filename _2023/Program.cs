// See https://aka.ms/new-console-template for more information

using _2023.Days;
using _2023.Utils;
using ConsoleTables;
using Figgle;
using ShellProgressBar;

int day;
var benchmark = false;

Console.WriteLine("Running program!");

if (args.Length is 0)
{
    day = DateTime.UtcNow.Day;
}
else
{
    var arg = args.First();

    if (!int.TryParse(arg, out day))
    {
        benchmark = true;
    }
}

if (benchmark)
{
    day = DateTime.UtcNow.Day;

    var table = new ConsoleTable("Day", "Parsing", "Part 1", "Part 2", "Total", "SD");
    const int numRepeats = 10;

    using (var progress = new ProgressBar(day * numRepeats,"Benchmarking"))
    {
        foreach (var d in Enumerable.Range(1, day))
        {
            var results = new HashSet<Result>();

            for (var i = 0; i < numRepeats; i++)
            {
                progress.Tick($"Day {d}, repeat {i}");
                var daySolver = DayFactory.GetDay(d);
                results.Add(daySolver.Solve());
            }

            var meanParseTime = results.Average(r => r.ParseTime.TotalMilliseconds);
            var meanPart1Time = results.Average(r => r.Part1SolveTime.TotalMilliseconds);
            var meanPart2Time = results.Average(r => r.Part2SolveTime.TotalMilliseconds);

            var totalTimes = results
                .Select(r => r.GetTotalTime().TotalMilliseconds)
                .ToList();
                
            var meanTotalTime = totalTimes.Average();
            var standardDeviation = Maths.StandardDeviation(totalTimes);

            table.AddRow(d, $"{meanParseTime:0.####}ms", $"{meanPart1Time:0.####}ms", $"{meanPart2Time:0.####}ms",
                $"{meanTotalTime:0.####}ms", $"{standardDeviation:0.####}");
        }
    }
    
    Console.WriteLine();
    
    table.Write(Format.Alternative);

    Console.WriteLine();
}
else
{
    Console.WriteLine(FiggleFonts.Keyboard.Render($"Advent of Code Day {day}"));
    Console.WriteLine();
    
    var daySolver = DayFactory.GetDay(day);

    var result = daySolver.Solve();

    result.LogToConsole();
}