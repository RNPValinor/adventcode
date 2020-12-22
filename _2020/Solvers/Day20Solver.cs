using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace _2020.Solvers
{
    public class Day20Solver : ISolver
    {
        private readonly Dictionary<int, HashSet<Tile>> _tilesByEdgeId = new();
        
        public void Solve(string input)
        {
            var tiles = input.Split($"{Environment.NewLine}{Environment.NewLine}");

            foreach (var tileStr in tiles)
            {
                if (string.IsNullOrWhiteSpace(tileStr))
                {
                    continue;
                }

                var tile = new Tile(tileStr);

                var tileEdges = tile.GetEdgeIds();
            }
        }

        private class Tile
        {
            public int Id { get; set; }
            public Dictionary<Point, char> Pixels { get; set; } = new();

            private int _maxX;
            private int _maxY;
            
            public Tile(string tileStr)
            {
                var lines = tileStr.Split(Environment.NewLine).ToList();

                var tileId = int.Parse(lines[0].Split(" ")[1].Replace(":", ""));
                lines.RemoveAt(0);

                this._maxY = lines.Count - 1;

                for (var y = 0; y < lines.Count; y++)
                {
                    var line = lines[y];

                    this._maxX = line.Length - 1;

                    for (var x = 0; x < line.Length; x++)
                    {
                        this.Pixels.Add(new Point(x, y), line[x]);
                    }
                }
            }

            public Dictionary<Edge, uint> GetEdgeIds()
            {
                var topEdge = "";
                var bottomEdge = "";
                var rightEdge = "";
                var leftEdge = "";

                for (var x = 0; x <= this._maxX; x++)
                {
                    var topPoint = new Point(x, 0);
                    var bottomPoint = new Point(x, this._maxY);

                    topEdge += this.Pixels[topPoint];
                    bottomEdge += this.Pixels[bottomPoint];
                }

                for (var y = 0; y <= this._maxY; y++)
                {
                    var leftPoint = new Point(0, y);
                    var rightPoint = new Point(this._maxX, y);

                    leftEdge += this.Pixels[leftPoint];
                    rightEdge += this.Pixels[rightPoint];
                }

                var topEdgeId = Convert.ToUInt32(topEdge.Replace('#', '1').Replace('.', '0'), 2);
                var bottomEdgeId = Convert.ToUInt32(bottomEdge.Replace('#', '1').Replace('.', '0'), 2);
                var leftEdgeId = Convert.ToUInt32(leftEdge.Replace('#', '1').Replace('.', '0'), 2);
                var rightEdgeId = Convert.ToUInt32(rightEdge.Replace('#', '1').Replace('.', '0'), 2);

                return new Dictionary<Edge, uint>
                {
                    { Edge.Top, topEdgeId },
                    { Edge.Bottom, bottomEdgeId },
                    { Edge.Left, leftEdgeId },
                    { Edge.Right, rightEdgeId }
                };
            }
        }

        private enum Edge
        {
            Top, Right, Bottom, Left
        }
    }
}