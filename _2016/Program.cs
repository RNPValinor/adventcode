using System;

namespace _2016
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Valinor's 2016 Advent of Code answers!");

            Console.WriteLine("Choose a day to run:");

            var day = Console.ReadLine();

            Console.WriteLine("And choose which parts to run: [1],[2],[b]oth");

            var parts = Console.ReadLine();

            var manager = new AnswerManager(day, parts);

            manager.Run();
        }
    }
}
