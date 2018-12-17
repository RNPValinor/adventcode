using System;
using System.Collections.Generic;
using System.Drawing;
using _2018.Utils;

namespace _2018.Days
{
    public class Day17 : Day
    {
        private readonly HashSet<Point> _clay = new HashSet<Point>();
        private int _minY = int.MaxValue;
        private int _maxY = int.MinValue;
        
        private void LoadClay()
        {
            var data = QuestionLoader.Load(17).Split(Environment.NewLine);

            foreach (var line in data)
            {
                var parts = line.Split(", ");
                var range = parts[1].Substring(2).Split("..");
                
                int startX, endX, startY, endY;

                if (parts[0][0] == 'x')
                {
                    startX = int.Parse(parts[0].Substring(2));
                    endX = startX;
                    startY = int.Parse(range[0]);
                    endY = int.Parse(range[1]);
                }
                else
                {
                    startY = int.Parse(parts[0].Substring(2));
                    endY = startY;
                    startX = int.Parse(range[0]);
                    endX = int.Parse(range[1]);
                }

                this.AddLine(startX, endX, startY, endY);
            }
        }

        private void AddLine(int startX, int endX, int startY, int endY)
        {
            for (var x = startX; x <= endX; x++)
            {
                for (var y = startY; y <= endY; y++)
                {
                    this._clay.Add(new Point(x, y));
                }
            }

            if (startY < this._minY)
            {
                this._minY = startY;
            }

            if (endY > this._maxY)
            {
                this._maxY = endY;
            }
        }

        /// <summary>
        /// Settles a single layer of water on the lowest reachable point, given a starting (x, y) co-ordinate
        /// </summary>
        /// <param name="x">X-coordinate of the start of the flow</param>
        /// <param name="y">Y-coordinate of the start of the flow</param>
        /// <param name="settledWater">Set of all currently settled water points</param>
        /// <returns></returns>
        private HashSet<Point> SettleWater(int x, int y, HashSet<Point> settledWater)
        {
            var movingWater = new HashSet<Point>();

            bool FullBelow(Point p) => this._clay.Contains(p) || settledWater.Contains(p);

            while (y <= this._maxY)
            {
                if (FullBelow(new Point(x, y + 1)))
                {
                    // Clay/settled water below, maybe settle
                    
                }
            }

            return movingWater;
        }
        
        protected override void DoPart1()
        {
            this.LoadClay();

            var wateryBits = new HashSet<Point> {new Point(500, 0)};

            var numWaterSquares = 0;

            var gotFlow = false;

            do
            {
                
            } while (gotFlow);
            
            ConsoleUtils.WriteColouredLine($"Got {numWaterSquares} watery bits within bounds", ConsoleColor.Cyan);
        }

        protected override void DoPart2()
        {
            
        }
    }
}