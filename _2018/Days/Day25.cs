using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using _2018.Utils;

namespace _2018.Days
{
    public class Day25 : Day
    {
        private readonly HashSet<List<Vector4>> _constellations = new HashSet<List<Vector4>>();

        private readonly Func<Vector4, Vector4, int> _getDistance = (s1, s2) => (int) (Math.Abs(s1.W - s2.W) +
                                                                                       Math.Abs(s1.X - s2.X) +
                                                                                       Math.Abs(s1.Y - s2.Y) +
                                                                                       Math.Abs(s1.Z - s2.Z));
        
        private void LoadConstellations()
        {
            var points = QuestionLoader.Load(25).Split(Environment.NewLine);

            foreach (var point in points)
            {
                var coordinates = point.Split(',').Select(float.Parse).ToList();
                
                var star = new Vector4(coordinates[0], coordinates[1], coordinates[2], coordinates[3]);

                var otherConstellations = this.LoadNeighbouringConstellations(star);

                if (otherConstellations.Any())
                {
                    this._constellations.RemoveWhere(otherConstellations.Contains);

                    var newConstellation = new List<Vector4>();

                    foreach (var constellation in otherConstellations)
                    {
                        newConstellation.AddRange(constellation);
                    }

                    newConstellation.Add(star);

                    this._constellations.Add(newConstellation);
                }
                else
                {
                    var constellation = new List<Vector4> {star};
                    
                    this._constellations.Add(constellation);
                }
            }
        }

        private HashSet<List<Vector4>> LoadNeighbouringConstellations(Vector4 star)
        {
            var neighbouringConstellations = new HashSet<List<Vector4>>();

            foreach (var constellation in this._constellations)
            {
                foreach (var constStar in constellation)
                {
                    if (this._getDistance(constStar, star) <= 3)
                    {
                        neighbouringConstellations.Add(constellation);
                        break;
                    }
                }
            }

            return neighbouringConstellations;
        }
        
        protected override void DoPart1()
        {
            this.LoadConstellations();
            
            ConsoleUtils.WriteColouredLine($"Found {this._constellations.Count} constellations", ConsoleColor.Cyan);
        }

        protected override void DoPart2()
        {
        }
    }
}