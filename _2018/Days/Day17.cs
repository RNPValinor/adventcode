using System;
using System.Collections.Generic;
using System.Drawing;
using AnimatedGif;
using _2018.Utils;

namespace _2018.Days
{
    public class Day17 : Day
    {
        private readonly HashSet<Point> _clay = new HashSet<Point>();
        private readonly HashSet<Point> _settledWater = new HashSet<Point>();
        private int _minY = int.MaxValue;
        private int _maxY = int.MinValue;
        private int _minX = int.MaxValue;
        private int _maxX = int.MinValue;
        
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

            if (startX < this._minX)
            {
                this._minX = startX;
            }

            if (endX> this._maxX)
            {
                this._maxX = endX;
            }
        }

        /// <summary>
        /// Settles a single layer of water on the lowest reachable point, given a starting (x, y) co-ordinate
        /// </summary>
        /// <param name="x">X-coordinate of the start of the flow</param>
        /// <param name="y">Y-coordinate of the start of the flow</param>
        /// <param name="visitedPoints">Set of already considered points for this iteration</param>
        /// <returns></returns>
        private HashSet<Point> SettleWater(int x, int y, ISet<Point> visitedPoints)
        {
            var movingWater = new HashSet<Point>();

            while (y <= this._maxY)
            {
                var currentPoint = new Point(x, y);

                if (visitedPoints.Contains(currentPoint))
                {
                    return movingWater;
                }
                
                visitedPoints.Add(currentPoint);
                
                if (this.FullBelow(currentPoint, this._settledWater))
                {
                    // Clay/settled water below, maybe settle
                    var (fallLeft, leftPoint) = this.GetFallPoint(currentPoint, -1);
                    var (fallRight, rightPoint) = this.GetFallPoint(currentPoint, 1);

                    if (!fallLeft && !fallRight)
                    {
                        // Can fill at this level - do so and return
                        for (var i = leftPoint.X; i <= rightPoint.X; i++)
                        {
                            this._settledWater.Add(new Point(i, currentPoint.Y));
                        }
                    }
                    else
                    {
                        for (var newX = leftPoint.X + (fallLeft ? 1 : 0); newX <= rightPoint.X - (fallRight ? 1 : 0); newX++)
                        {
                            movingWater.Add(new Point(newX, currentPoint.Y));
                        }
                        
                        if (fallLeft)
                        {
                            var moving = this.SettleWater(leftPoint.X, leftPoint.Y, visitedPoints);

                            foreach (var movingPoint in moving)
                            {
                                movingWater.Add(movingPoint);
                            }
                        }

                        if (fallRight)
                        {
                            var moving = this.SettleWater(rightPoint.X, rightPoint.Y, visitedPoints);

                            foreach (var movingPoint in moving)
                            {
                                movingWater.Add(movingPoint);
                            }
                        }
                    }
                    
                    return movingWater;
                }
                else
                {
                    if (y >= this._minY)
                    {
                        movingWater.Add(new Point(x, y));
                    }
                    
                    y++;
                }
            }

            return movingWater;
        }

        private bool FullBelow(Point p, ICollection<Point> settledWater)
        {
            var belowPoint = new Point(p.X, p.Y + 1);
            
            return this._clay.Contains(belowPoint) || settledWater.Contains(belowPoint);
        }

        private (bool hasFall, Point fallPoint) GetFallPoint(Point start, int xOffset)
        {
            var prevPoint = start;
            var currentPoint = start;

            while (!this._clay.Contains(currentPoint))
            {
                if (!this.FullBelow(currentPoint, this._settledWater))
                {
                    return (true, currentPoint);
                }

                prevPoint = currentPoint;
                currentPoint = new Point(currentPoint.X + xOffset, currentPoint.Y);
            }

            return (false, prevPoint);
        }

        private Image GenerateImage(HashSet<Point> flowingWater)
        {
            var image = new Bitmap(this._maxX - this._minX + 2, this._maxY + 1);

            for (var x = 0; x < image.Width; x++)
            {
                for (var y = 0; y <= this._maxY; y++)
                {
                    var colour = Color.SandyBrown;
                    var p = new Point(x + this._minX, y);

                    if (this._clay.Contains(p))
                    {
                        colour = Color.SaddleBrown;
                    }
                    else if (this._settledWater.Contains(p))
                    {
                        colour = Color.Blue;
                    }
                    else if (flowingWater.Contains(p))
                    {
                        colour = Color.Aqua;
                    }
                    
                    image.SetPixel(x, y, colour);
                }
            }

            return image;
        }

        protected override void DoPart1()
        {
            this.LoadClay();

            int oldSettledSize;
            var flowingWater = new HashSet<Point>();
            var numIterations = 0;
            
            var images = new List<Image>
            {
                this.GenerateImage(flowingWater)
            };

            do
            {
                oldSettledSize = this._settledWater.Count;
                
                flowingWater = this.SettleWater(500, 0, new HashSet<Point>());
                images.Add(this.GenerateImage(flowingWater));
                
                numIterations++;
            } while (this._settledWater.Count > oldSettledSize);

            ConsoleUtils.WriteColouredLine($"Got {this._settledWater.Count} settled water and {flowingWater.Count} flowing water, after {numIterations} loops", ConsoleColor.Cyan);
            
            using (var gif = AnimatedGif.AnimatedGif.Create("water.gif", 33))
            {
                foreach (var image in images)
                {
                    gif.AddFrame(image, delay: -1, quality: GifQuality.Bit8);
                }
            }
        }

        protected override void DoPart2()
        {
            ConsoleUtils.WriteColouredLine($"Got {this._settledWater.Count} settled water", ConsoleColor.Cyan);
        }
    }
}