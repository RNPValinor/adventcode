using System;
using System.Collections.Generic;
using _2018.Utils;

namespace _2018.Days
{
    public class Day11 : Day
    {
        private const int SerialNumber = 2187;
        private readonly IDictionary<string, int> _powerCache = new Dictionary<string, int>();
        private readonly IDictionary<int, Dictionary<string, int>> _squareCache = new Dictionary<int, Dictionary<string, int>>();

        private int GetPower(int x, int y)
        {
            var coord = $"{x},{y}";
            
            if (this._powerCache.ContainsKey(coord))
            {
                return this._powerCache[coord];
            }
            
            var rackId = x + 10;

            var power = rackId * y;

            power += SerialNumber;

            power *= rackId;

            power = power < 100 ? 0 : Math.Abs(power / 100 % 10);

            power -= 5;
            
            this._powerCache.Add(coord, power);

            return power;
        }

        private int GetSquarePower(int x, int y, int width)
        {
            var diff = x - y;
            var power = 0;
            var subSquare = $"{x + 1},{y + 1},{width - 1}";

            if (this._squareCache.ContainsKey(diff))
            {
                if (width > 1)
                {
                    // Only load sub-square if there is one
                    power = this._squareCache[diff][subSquare];                    
                }

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
                for (var i = 0; i < width; i++)
                {
                    for (var j = 0; j < width; j++)
                    {
                        power += this.GetPower(x + i, y + j);
                    }
                }

                this._squareCache.Add(diff, new Dictionary<string, int>());
            }

            this._squareCache[diff].Add($"{x},{y},{width}", power);

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

            for (var y = 300; y > 0; y--)
            {
                for (var x = 300; x > 0; x--)
                {
                    for (var width = 1; (width + x <= 301 && width + y <= 301); width++)
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
            }
            
            ConsoleUtils.WriteColouredLine($"Max flexible square power at ({maxX}, {maxY}), width {maxSize}", ConsoleColor.Cyan);
        }
    }
}