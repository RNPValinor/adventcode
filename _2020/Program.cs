using System;
using _2020.Solvers;
using Microsoft.Extensions.CommandLineUtils;

namespace _2020
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var app = new CommandLineApplication(throwOnUnexpectedArg: false)
            {
                Name = "Advent of Code 2020 Problem Solver",
                Description = ".Net Core console application for solving advent of code 2020"
            };

            app.HelpOption("-h|-?|--help");

            var dayOption = app.Option("-d|--day <day>",
                "Day to run. Defaults to today (if today is a day [1-25] December 2020)",
                CommandOptionType.SingleValue);

            app.OnExecute(() =>
            {
                var day = DateTime.Today.Day;
                var needsUserDay = day > 25 || DateTime.Today.Month != 12 || DateTime.Today.Year != 2020;
                
                if (dayOption.HasValue())
                {
                    if (!int.TryParse(dayOption.Value(), out var userDay))
                    {
                        return 1;
                    }

                    day = userDay;
                }
                else if (needsUserDay)
                {
                    return 1;
                }

                if (day > 25)
                {
                    return 1;
                }

                if (app.RemainingArguments.Count == 0)
                {
                    throw new ArgumentException("No input data found");
                }

                var input = app.RemainingArguments[0];
                
                SolverManager.GetSolver(day).Solve(input);

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
