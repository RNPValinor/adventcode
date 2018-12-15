using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using _2018.Utils;

namespace _2018.Days
{
    public class Day6 : Day
    {
        private readonly int _minX = int.MaxValue;
        private readonly int _maxX = int.MinValue;
        private readonly int _minY = int.MaxValue;
        private readonly int _maxY = int.MinValue;
        private readonly HashSet<Point> _points = new HashSet<Point>();
        
        public Day6()
        {
            var pointData = QuestionLoader.Load(6).Split(Environment.NewLine);

            foreach (var pointStr in pointData)
            {
                var pointArray = pointStr.Split(", ");
                
                var point = new Point(int.Parse(pointArray[0]), int.Parse(pointArray[1]));

                if (point.X < this._minX)
                {
                    this._minX = point.X;
                }

                if (point.X > this._maxX)
                {
                    this._maxX = point.X;
                }

                if (point.Y < this._minY)
                {
                    this._minY = point.Y;
                }

                if (point.Y > this._maxY)
                {
                    this._maxY = point.Y;
                }

                this._points.Add(point);
            }
        }
        
        protected override void DoPart1()
        {
            var areas = this._points.ToDictionary(p => p.ToString(), p => 0);
            var infiniteAreas = new HashSet<string>();

            for (var x = this._minX; x <= this._maxX; x++)
            {
                for (var y = this._minY; y <= this._maxY; y++)
                {
                    var closestPoint = this.GetClosestPoint(x, y);

                    // Has a value so no equidistant points
                    if (closestPoint.HasValue)
                    {
                        var cp = closestPoint.Value.ToString();
                        
                        if (x == this._minX || x == this._maxX || y == this._minY || y == this._maxY)
                        {
                            infiniteAreas.Add(cp);
                        }

                        areas[cp]++;
                    }
                }
            }
            
            var largestArea = int.MinValue;
            
            foreach (var entry in areas)
            {
                if (!infiniteAreas.Contains(entry.Key) && entry.Value > largestArea)
                {
                    largestArea = entry.Value;
                }
            }
            
            ConsoleUtils.WriteColouredLine($"Largest finite area has size {largestArea}", ConsoleColor.Cyan);
        }

        private Point? GetClosestPoint(int x, int y)
        {
            Point? closest = null;
            var distance = int.MaxValue;

            foreach (var point in this._points)
            {
                var newDistance = Math.Abs(point.X - x) + Math.Abs(point.Y - y);

                if (newDistance < distance)
                {
                    closest = point;
                    distance = newDistance;
                }
                else if (newDistance == distance)
                {
                    closest = null;
                }
            }

            return closest;
        }

        protected override void DoPart2()
        {
            var regionSize = 0;

            for (var x = this._minX - 200; x <= this._maxX + 200; x++)
            {
                for (var y = this._minY - 200; y <= this._maxY + 200; y++)
                {
                    if (this.TotalDistanceToPoint(x, y) < 10000)
                    {
                        regionSize++;
                    }
                }
            }
            
            ConsoleUtils.WriteColouredLine($"Got safe region size of {regionSize}", ConsoleColor.Cyan);
        }

        private int TotalDistanceToPoint(int x, int y)
        {
            var distance = 0;

            foreach (var point in this._points)
            {
                distance += Math.Abs(point.X - x) + Math.Abs(point.Y - y);
            }

            return distance;
        }
    }
}