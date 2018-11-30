using System;
using System.Threading;

namespace _2018.Utils
{
    public static class ConsoleUtils
    {
        private const ConsoleColor DefaultColour = ConsoleColor.White;
        private static readonly SemaphoreSlim WriteSemaphore = new SemaphoreSlim(1, 1);

        public static void WriteColouredLine(string text, ConsoleColor colour)
        {
            WriteSemaphore.Wait();

            Console.ForegroundColor = colour;
            Console.WriteLine(text);
            Console.ForegroundColor = DefaultColour;

            WriteSemaphore.Release();
        }
    }
}