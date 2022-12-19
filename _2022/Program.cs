// See https://aka.ms/new-console-template for more information

using _2022.Days;

int day;

Console.WriteLine("Running program!");

if (args.Length is 0)
{
    day = DateTime.UtcNow.Day;
}
else
{
    if (!int.TryParse(args.First(), out day))
        throw new ArgumentException($"Expected first argument to be a number, but got {args.First()}");
}

var daySolver = DayFactory.GetDay(day);

daySolver.Solve();