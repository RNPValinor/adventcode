using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using _2018.Utils;

namespace _2018.Days
{
    public class Day10 : Day
    {
        private static HashSet<SkyPoint> GetPoints()
        {
            var data = QuestionLoader.Load(10).Split(Environment.NewLine);

            var points = new HashSet<SkyPoint>();

            foreach (var entry in data)
            {
                var point = entry.Substring(10, 14).Split(',');
                var vel = entry.Substring(36, 6).Split(',');

                var x = int.Parse(point[0].Trim());
                var y = int.Parse(point[1].Trim());

                var dx = int.Parse(vel[0].Trim());
                var dy = int.Parse(vel[1].Trim());

                points.Add(new SkyPoint(x, y, dx, dy));
            }

            return points;
        }

        private static void PrintPoints(HashSet<SkyPoint> points)
        {
            var pointLookup = new Dictionary<int, HashSet<int>>();

            var minX = int.MaxValue;
            var maxX = int.MinValue;
            var minY = int.MaxValue;
            var maxY = int.MinValue;

            foreach (var point in points)
            {
                minX = Math.Min(point.X, minX);
                maxX = Math.Max(point.X, maxX);
                minY = Math.Min(point.Y, minY);
                maxY = Math.Max(point.Y, maxY);

                if (!pointLookup.ContainsKey(point.X))
                {
                    pointLookup.Add(point.X, new HashSet<int>());
                }

                pointLookup[point.X].Add(point.Y);
            }

            for (var y = minY; y <= maxY; y++)
            {
                var line = new StringBuilder();

                for (var x = minX; x <= maxX; x++)
                {
                    if (pointLookup.ContainsKey(x))
                    {
                        line.Append(pointLookup[x].Contains(y) ? '#' : '.');    
                    }
                    else
                    {
                        line.Append('.');
                    }
                }

                ConsoleUtils.WriteColouredLine(line.ToString(), ConsoleColor.Cyan);
            }
        }
        
        protected override void DoPart1()
        {
            var points = GetPoints();

            int oldYDist;
            var newYDist = int.MaxValue;
            // We will do an extra iteration, so start at -1 seconds.
            var numSeconds = -1;

            do
            {
                oldYDist = newYDist;
                
                var minY = int.MaxValue;
                var maxY = int.MinValue;

                foreach (var point in points)
                {
                    point.Pos.X += point.Vel.X;
                    point.Pos.Y += point.Vel.Y;

                    maxY = Math.Max(point.Y, maxY);
                    minY = Math.Min(point.Y, minY);
                }

                newYDist = maxY - minY;

                numSeconds++;
            } while (newYDist <= oldYDist);

            foreach (var point in points)
            {
                point.Pos.X -= point.Vel.X;
                point.Pos.Y -= point.Vel.Y;
            }

            PrintPoints(points);
            
            ConsoleUtils.WriteColouredLine($"Message appears in {numSeconds} seconds", ConsoleColor.Cyan);
        }

        protected override void DoPart2()
        {
            
        }

        private class SkyPoint
        {
            public SkyPoint(int x, int y, int dx, int dy)
            {
                this.Pos = new Point(x, y);
                this.Vel = new Point(dx, dy);
            }
            
            public Point Pos;

            public Point Vel;

            public int X => this.Pos.X;
            public int Y => this.Pos.Y;
        }
    }
}