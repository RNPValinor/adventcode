using System;
using Microsoft.Extensions.CommandLineUtils;
using _2018.Days;
using _2018.Utils;

namespace _2018
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var app = new CommandLineApplication
            {
                Name = "Solidatus Licence Generator",
                Description = ".NET Core console app for generating licence codes"
            };

            app.HelpOption("-h|-?|--help");

            var day = app.Option("-d|--day <day>",
                "Day of code to run (defaults to today)",
                CommandOptionType.SingleValue);

            app.OnExecute(() =>
            {
                var dayToRun = DateTime.UtcNow.Day;

                if (day.HasValue())
                {
                    if (int.TryParse(day.Value(), out var userDay))
                    {
                        // This check only works in December :P
                        if (userDay > 25 || userDay < 1)
                        {
                            ConsoleUtils.WriteColouredLine($"Specified day {userDay} must be between 1 and {dayToRun}", ConsoleColor.Red);
                            return 1;
                        }

                        dayToRun = userDay;
                    }
                    else
                    {
                        ConsoleUtils.WriteColouredLine($"Day to run must be an integer", ConsoleColor.Red);
                        return 1;
                    }
                }

                IDay dayAnswer;

                try
                {
                    var dayType = Type.GetType("_2018.Days.Day" + dayToRun);

                    dayAnswer = (IDay) Activator.CreateInstance(dayType);
                }
                catch (Exception)
                {
                    ConsoleUtils.WriteColouredLine($"Answer not found for day {dayToRun}", ConsoleColor.Yellow);
                    app.ShowHint();
                    return 0;
                }

                dayAnswer.Run();

                return 0;
            });

            try
            {
                app.Execute(args);
            }
            catch (CommandParsingException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
