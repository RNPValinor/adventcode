using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using _2018.Utils;

namespace _2018.Days
{
    public class Day18 : Day
    {
        private readonly IDictionary<Point, Acre> _landscape = new Dictionary<Point, Acre>();

        private void LoadLandscape()
        {
            var data = QuestionLoader.Load(18).Split(Environment.NewLine);

            for (var y = 0; y < data.Length; y++)
            {
                var line = data[y];

                for (var x = 0; x < line.Length; x++)
                {
                    var p = new Point(x, y);

                    Acre feature;

                    switch (line[x])
                    {
                        case '.':
                            feature = Acre.Open;
                            break;
                        case '#':
                            feature = Acre.Lumberyard;
                            break;
                        case '|':
                            feature = Acre.Trees;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    this._landscape.Add(p, feature);
                }
            }
        }

        private IDictionary<Point, Acre> MutateLandscape(IDictionary<Point, Acre> oldLandscape)
        {
            var newLandscape = new Dictionary<Point, Acre>();

            foreach (var point in oldLandscape.Keys)
            {
                var surrounding = GetSurroundingPoints(point);

                var surroundingAcres = surrounding.Where(oldLandscape.ContainsKey).Select(p => oldLandscape[p]);

                var nextAcre = oldLandscape[point];

                var numNeighbourTrees = 0;
                var numNeighbourLumberyards = 0;

                foreach (var neighbour in surroundingAcres)
                {
                    if (neighbour == Acre.Lumberyard)
                    {
                        numNeighbourLumberyards++;
                    }
                    else if (neighbour == Acre.Trees)
                    {
                        numNeighbourTrees++;
                    }
                    
                }

                switch (nextAcre)
                {
                    case Acre.Open:
                        if (numNeighbourTrees >= 3)
                        {
                            nextAcre = Acre.Trees;
                        }
                        break;
                    case Acre.Trees:
                        if (numNeighbourLumberyards >= 3)
                        {
                            nextAcre = Acre.Lumberyard;
                        }
                        break;
                    case Acre.Lumberyard:
                        if (numNeighbourLumberyards == 0 || numNeighbourTrees == 0)
                        {
                            nextAcre = Acre.Open;
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                newLandscape.Add(point, nextAcre);
            }

            return newLandscape;
        }

        private static ISet<Point> GetSurroundingPoints(Point p)
        {
            var surrounding = new HashSet<Point>();

            for (var dx = -1; dx <= 1; dx++)
            {
                for (var dy = -1; dy <= 1; dy++)
                {
                    if (!(dx == 0 && dy == 0))
                    {
                        surrounding.Add(new Point(p.X + dx, p.Y + dy));
                    }
                }
            }
            
            return surrounding;
        }
        
        protected override void DoPart1()
        {
            this.LoadLandscape();

            var landscape = this._landscape;

            for (var i = 0; i < 10; i++)
            {
                landscape = this.MutateLandscape(landscape);
            }

            var numTrees = landscape.Values.Count(a => a == Acre.Trees);
            var numLumberyards = landscape.Values.Count(a => a == Acre.Lumberyard);

            var value = numTrees * numLumberyards;
            
            ConsoleUtils.WriteColouredLine($"Got value {value} after 10 mins", ConsoleColor.Cyan);
        }

        private static string ConvertLandscapeToString(IDictionary<Point, Acre> landscape)
        {
            var landscapeString = new StringBuilder();

            for (var y = 0; y < 50; y++)
            {
                var line = new StringBuilder();
                
                for (var x = 0; x < 50; x++)
                {
                    var p = new Point(x, y);
                    var acre = landscape[p];

                    switch (acre)
                    {
                        case Acre.Open:
                            line.Append('.');
                            break;
                        case Acre.Trees:
                            line.Append('|');
                            break;
                        case Acre.Lumberyard:
                            line.Append('#');
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                landscapeString.AppendLine(line.ToString());
            }

            return landscapeString.ToString();
        }

        protected override void DoPart2()
        {
            var seenLandscapes = new Dictionary<string, int>();

            var landscape = this._landscape;
            var i = 0;
            var nextLandscapeString = ConvertLandscapeToString(this._landscape);

            do
            {
                seenLandscapes.Add(nextLandscapeString, i);
                
                landscape = this.MutateLandscape(landscape);

                nextLandscapeString = ConvertLandscapeToString(landscape);

                i++;
            } while (!seenLandscapes.ContainsKey(nextLandscapeString));

            var firstSeen = seenLandscapes[nextLandscapeString];
            
            ConsoleUtils.WriteColouredLine($"Found repeated landscape, first seen at i={firstSeen}, repeated at i={i}", ConsoleColor.Blue);

            var repeatPeriod = i - firstSeen;

            var numToDo = (1000000000 - i) % repeatPeriod;
            
            ConsoleUtils.WriteColouredLine($"Got {numToDo} left to do", ConsoleColor.Blue);

            for (i = 0; i < numToDo; i++)
            {
                landscape = this.MutateLandscape(landscape);
            }
            
            var numTrees = landscape.Values.Count(a => a == Acre.Trees);
            var numLumberyards = landscape.Values.Count(a => a == Acre.Lumberyard);

            var value = numTrees * numLumberyards;
            
            ConsoleUtils.WriteColouredLine($"Got value {value} after 1,000,000,000 minutes", ConsoleColor.Cyan);
        }

        private enum Acre
        {
            Open, Trees, Lumberyard
        }
    }
}