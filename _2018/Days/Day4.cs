using System;
using System.Collections.Generic;
using System.Linq;
using _2018.Utils;

namespace _2018.Days
{
    public class Day4 : Day
    {
        private readonly IList<string> _logs;
        private readonly IDictionary<int, Guard> _guards;

        public Day4()
        {
            this._logs = QuestionLoader.Load(4).Split(Environment.NewLine).OrderBy(l => l).ToList();
            this._guards = new Dictionary<int, Guard>();
        }
        
        protected override void DoPart1()
        {
            var timeAsleep = 0;
            Guard guard = null;
            
            foreach (var log in this._logs)
            {
                var date = DateTime.Parse(log.Substring(log.IndexOf('[') + 1, 16));
                var logData = log.Substring(log.IndexOf(']') + 2);

                switch (logData)
                {
                    case "wakes up":
                        while (timeAsleep < date.Minute)
                        {
                            guard.MinutesAsleep[timeAsleep++]++;
                        }

                        break;
                    case "falls asleep":
                        timeAsleep = date.Minute;
                        break;
                    default:
                        var parts = logData.Split(' ');
                        var id = int.Parse(parts[1].Substring(1));

                        if (this._guards.ContainsKey(id))
                        {
                            guard = this._guards[id];
                        }
                        else
                        {
                            guard = new Guard
                            {
                                Id = id
                            };
                            
                            this._guards.Add(id, guard);
                        }

                        break;
                }
            }

            var maxGuard = guard;

            foreach (var entry in this._guards)
            {
                if (entry.Value.TotalTimeAsleep > maxGuard.TotalTimeAsleep)
                {
                    maxGuard = entry.Value;
                }
            }

            var asleepMinute = maxGuard.MinutesAsleep.IndexOf(maxGuard.MaxTimeAsleepOnMinute);
            
            ConsoleUtils.WriteColouredLine($"Guard {maxGuard.Id} asleep the most, at time {asleepMinute}", ConsoleColor.Cyan);
        }

        protected override void DoPart2()
        {
            Guard maxGuard = null;

            foreach (var entry in this._guards)
            {
                if (maxGuard == null)
                {
                    maxGuard = entry.Value;
                }
                else
                {
                    if (entry.Value.MaxTimeAsleepOnMinute > maxGuard.MaxTimeAsleepOnMinute)
                    {
                        maxGuard = entry.Value;
                    }
                }
            }

            var asleepMinute = maxGuard.MinutesAsleep.IndexOf(maxGuard.MaxTimeAsleepOnMinute);
            
            ConsoleUtils.WriteColouredLine($"Guard {maxGuard.Id} asleep for the same minute most, at time {asleepMinute}", ConsoleColor.Cyan);
        }

        private class Guard
        {
            public int Id { get; set; }

            public IList<int> MinutesAsleep { get; } = new int[60];

            public int TotalTimeAsleep => this.MinutesAsleep.Sum();

            public int MaxTimeAsleepOnMinute => this.MinutesAsleep.Max();
        }
    }
}