// See https://aka.ms/new-console-template for more information

using _2023.Days;
using ConsoleTables;
using Figgle;

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
        switch (arg)
        {
            case "a":
            case "all":
                benchmark = true;
                break;
            default:
                throw new ArgumentException($"Unsupported argument: {arg}");
        }
    }
}

if (benchmark)
{
    day = DateTime.UtcNow.Day;

    var table = new ConsoleTable("Day", "Parsing", "Part 1", "Part 2", "Total");

    foreach (var d in Enumerable.Range(1, day))
    {
        var daySolver = DayFactory.GetDay(d);
        var result = daySolver.Solve();

        table.AddRow(d, result.GetParseTime(), result.GetPart1SolveTime(), result.GetPart2SolveTime(),
            result.GetTotalSolveTime());
    }
    
    table.Write(Format.Alternative);
}
else
{
    var daySolver = DayFactory.GetDay(day);

    var result = daySolver.Solve();

    Console.WriteLine(FiggleFonts.Keyboard.Render($"Advent of Code Day {day}"));
    Console.WriteLine();

    result.LogToConsole();
}