using System;
using System.Collections.Generic;

namespace _2020.Solvers
{
    public class Day13Solver : ISolver
    {
        public void Solve(string input)
        {
            var lines = input.Split(Environment.NewLine);

            var departureTime = int.Parse(lines[0]);

            var currentWait = int.MaxValue;
            var bestBusId = 0;
            var offset = 0;

            var buses = new List<Bus>();

            foreach (var bus in lines[1].Split(","))
            {
                if (bus == "x")
                {
                    offset++;
                    continue;
                }

                var busId = int.Parse(bus);
                
                buses.Add(new Bus
                {
                    Id = busId,
                    Offset = offset
                });

                offset++;

                var missedByTime = departureTime % busId;

                if (missedByTime == 0)
                {
                    currentWait = 0;
                    bestBusId = busId;
                    break;
                }

                var waitingTime = busId - missedByTime;

                if (waitingTime < currentWait)
                {
                    currentWait = waitingTime;
                    bestBusId = busId;
                }
            }

            Console.WriteLine(bestBusId * currentWait);

            long t = 0;
            long increment = buses[0].Id;
            buses.RemoveAt(0);

            foreach (var bus in buses)
            {
                long remainder;

                do
                {
                    t += increment;
                    remainder = (t + bus.Offset) % bus.Id;
                } while (remainder != 0);

                increment *= bus.Id;
            }

            Console.WriteLine(t);
        }

        private class Bus
        {
            public int Id { get; set; }
            public int Offset { get; set; }
        }
    }
}