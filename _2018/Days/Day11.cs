using System;
using System.Collections.Generic;
using _2018.Utils;

namespace _2018.Days
{
    public class Day11 : Day
    {
        private const int SerialNumber = 2187;
        private readonly IDictionary<int, Dictionary<int, int>> _powerCache = new Dictionary<int, Dictionary<int, int>>();
        private readonly IDictionary<int, Dictionary<int, Dictionary<int, int>>> _squareCache = new Dictionary<int, Dictionary<int, Dictionary<int, int>>>();
        private int _numCacheHits;

        private int GetPower(int x, int y)
        {
            if (this._powerCache.ContainsKey(x))
            {
                if (this._powerCache[x].ContainsKey(y))
                {
                    return this._powerCache[x][y];
                }
            }
            
            var rackId = x + 10;

            var power = rackId * y;

            power += SerialNumber;

            power *= rackId;

            power = power < 100 ? 0 : Math.Abs(power / 100 % 10);

            power -= 5;

            if (!this._powerCache.ContainsKey(x))
            {
                this._powerCache.Add(x, new Dictionary<int, int>());
            }
            
            this._powerCache[x].Add(y, power);

            return power;
        }

        private int GetSquarePower(int x, int y, int width)
        {
            int power;

            if (width > 1)
            {
                this._numCacheHits++;
                
                power = this._squareCache[x + 1][y + 1][width - 1];

                for (var i = 0; i < width; i++)
                {
                    power += this.GetPower(x + i, y);
                }

                for (var j = 1; j < width; j++)
                {
                    power += this.GetPower(x, y + j);
                }
            }
            else
            {
                power = this.GetPower(x, y);

                if (!this._squareCache.ContainsKey(x))
                {
                    this._squareCache.Add(x, new Dictionary<int, Dictionary<int, int>> { { y, new Dictionary<int, int>() } });
                }
                else if (!this._squareCache[x].ContainsKey(y))
                {
                    this._squareCache[x].Add(y, new Dictionary<int, int>());
                }
            }

            this._squareCache[x][y].Add(width, power);

            return power;
        }

        protected override void DoPart1()
        {
            var maxPower = int.MinValue;
            var maxX = 0;
            var maxY = 0;
            
            for (var x = 1; x <= 298; x++)
            {
                for (var y = 1; y <= 298; y++)
                {
                    var power = 0;

                    power += this.GetPower(x, y);
                    power += this.GetPower(x + 1, y);
                    power += this.GetPower(x + 2, y);
                    power += this.GetPower(x, y + 1);
                    power += this.GetPower(x + 1, y + 1);
                    power += this.GetPower(x + 2, y + 1);
                    power += this.GetPower(x, y + 2);
                    power += this.GetPower(x + 1, y + 2);
                    power += this.GetPower(x + 2, y + 2);

                    if (power > maxPower)
                    {
                        maxPower = power;
                        maxX = x;
                        maxY = y;
                    }
                }
            }
            
            ConsoleUtils.WriteColouredLine($"Max power at ({maxX}, {maxY})", ConsoleColor.Cyan);
        }

        protected override void DoPart2()
        {
            var maxPower = int.MinValue;
            var maxX = 0;
            var maxY = 0;
            var maxSize = 0;

            for (var width = 1; width <= 300; width++)
            {
                for (var x = 1; x + width - 1 <= 300; x++)
                {
                    for (var y = 1; y + width - 1 <= 300; y++)
                    {
                        var squarePower = this.GetSquarePower(x, y, width);

                        if (squarePower > maxPower)
                        {
                            maxPower = squarePower;
                            maxX = x;
                            maxY = y;
                            maxSize = width;
                        }
                    }
                }
                
                ConsoleUtils.WriteColouredLine($"Done width = {width}, num cache hits = {this._numCacheHits}", ConsoleColor.Magenta);

                this._numCacheHits = 0;
            }
            
            ConsoleUtils.WriteColouredLine($"Max flexible square power at ({maxX}, {maxY}), width {maxSize}", ConsoleColor.Cyan);
        }
    }
}