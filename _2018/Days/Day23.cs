using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using _2018.Utils;

namespace _2018.Days
{
    public class Day23 : Day
    {
        private readonly IDictionary<Vector3, int> _nanobots = new Dictionary<Vector3, int>();

        private (Vector3 bot, int radius) LoadNanobots()
        {
            var input = QuestionLoader.Load(23).Split(Environment.NewLine);
            var maxRadius = int.MinValue;
            var maxBot = new Vector3();

            foreach (var line in input)
            {
                var parts = line.Split(", ");

                var posData = parts[0].Substring(parts[0].IndexOf('<') + 1, parts[0].Length - 6).Split(',').Select(int.Parse).ToList();

                var point = new Vector3(posData[0], posData[1], posData[2]);
                var radius = int.Parse(parts[1].Substring(2));

                this._nanobots.Add(point, radius);

                if (radius > maxRadius)
                {
                    maxRadius = radius;
                    maxBot = point;
                }
            }

            return (maxBot, maxRadius);
        }
        
        protected override void DoPart1()
        {
            var (maxBot, maxRadius) = this.LoadNanobots();

            var botsInRange = (from bot in this._nanobots.Keys select Math.Abs(bot.X - maxBot.X) + Math.Abs(bot.Y - maxBot.Y) + Math.Abs(bot.Z - maxBot.Z)).Count(distance => distance <= maxRadius);

            ConsoleUtils.WriteColouredLine($"Got {botsInRange} bots in range", ConsoleColor.Cyan);
        }

        protected override void DoPart2()
        {
            var intersections = new Dictionary<Vector3, HashSet<Vector3>>();

            foreach (var bot in this._nanobots.Keys)
            {
                intersections.Add(bot, new HashSet<Vector3>());
                var radius = this._nanobots[bot];

                foreach (var entry in this._nanobots)
                {
                    var otherPos = entry.Key;

                    var distance = Math.Abs(bot.X - otherPos.X) + Math.Abs(bot.Y - otherPos.Y) +
                                   Math.Abs(bot.Z - otherPos.Z);

                    if (distance <= radius + entry.Value)
                    {
                        intersections[bot].Add(otherPos);
                    }
                }
            }

            var mutualIntersections = new HashSet<Vector3>();

            foreach (var entry in intersections)
            {
                var pointSet = entry.Value;
                var mutualPoints = new HashSet<Vector3>(pointSet);

                foreach (var point in pointSet)
                {
                    mutualPoints.IntersectWith(intersections[point]);
                }

                if (mutualPoints.Count > mutualIntersections.Count)
                {
                    mutualIntersections = mutualPoints;
                }
            }
            
            ConsoleUtils.WriteColouredLine($"Got {mutualIntersections.Count} mutual intersections", ConsoleColor.Blue);
            
            
        }
    }
}