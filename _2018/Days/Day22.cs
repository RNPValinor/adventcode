using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Priority_Queue;
using _2018.Utils;

namespace _2018.Days
{
    public class Day22 : Day
    {
        private const int Depth = 8103;
        private readonly Point _targetPosition = new Point(9, 758);
        private readonly Point _furthestPoint = new Point(100, 850);
        private readonly IDictionary<Point, int> _erosionLevels = new Dictionary<Point, int>();
        private readonly IDictionary<Point, TerrainType> _terrain = new Dictionary<Point, TerrainType>();

        private int InitialiseToPoint(Point p)
        {
            this._erosionLevels.Clear();
            this._terrain.Clear();

            var riskLevel = 0;
            
            for (var y = 0; y <= p.Y; y++)
            {
                for (var x = 0; x <= p.X; x++)
                {
                    int geologicalIndex;
                    var curPos = new Point(x, y);
                    
                    if (y == 0)
                    {
                        if (x == 0)
                        {
                            geologicalIndex = 0;
                        }
                        else
                        {
                            geologicalIndex = x * 16807;                            
                        }
                    }
                    else if (x == 0)
                    {
                        geologicalIndex = y * 48271;
                    }
                    else
                    {
                        if (curPos == this._targetPosition)
                        {
                            geologicalIndex = 0;
                        }
                        else
                        {
                            var left = new Point(x - 1, y);
                            var up = new Point(x, y - 1);

                            geologicalIndex = this._erosionLevels[left] * this._erosionLevels[up];
                        }
                    }

                    var erosionLevel = (geologicalIndex + Depth) % 20183;

                    this._erosionLevels.Add(curPos, erosionLevel);
                    this._terrain.Add(curPos, (TerrainType) (erosionLevel % 3));
                    riskLevel += (erosionLevel % 3);
                }
            }

            return riskLevel;
        }

        protected override void DoPart1()
        {
            var riskLevel = this.InitialiseToPoint(this._targetPosition);
            
            ConsoleUtils.WriteColouredLine($"Got risk level of {riskLevel}", ConsoleColor.Cyan);
        }

        private int GetShortestPathToTarget()
        {
            var visitedPoints = new Dictionary<Point, HashSet<EquipmentType>>();
            var consideredPoints = new FastPriorityQueue<PointNode>((this._furthestPoint.X + 1) * (this._furthestPoint.Y + 1) * 3);
            consideredPoints.Enqueue(new PointNode
            {
                P = new Point(0, 0),
                Time = 0,
                Equipment = EquipmentType.Torch
            }, 0);

            while (consideredPoints.Any())
            {
                var consideredPoint = consideredPoints.Dequeue();

                if (!visitedPoints.ContainsKey(consideredPoint.P))
                {
                    visitedPoints.Add(consideredPoint.P, new HashSet<EquipmentType> { consideredPoint.Equipment });
                }
                else
                {
                    if (visitedPoints[consideredPoint.P].Contains(consideredPoint.Equipment))
                    {
                        continue;
                    }
                    
                    visitedPoints[consideredPoint.P].Add(consideredPoint.Equipment);
                }

                if (consideredPoint.P == this._targetPosition)
                {
                    if (consideredPoint.Equipment == EquipmentType.Torch)
                    {
                        return consideredPoint.Time;
                    }

                    var nextPoint = new PointNode
                    {
                        P = consideredPoint.P,
                        Equipment = EquipmentType.Torch,
                        Time = consideredPoint.Time + 7
                    };
                    
                    consideredPoints.Enqueue(nextPoint, nextPoint.Time);
                }

                var neighbours = this.GetNextPoints(consideredPoint, visitedPoints);

                foreach (var neighbour in neighbours)
                {
                    consideredPoints.Enqueue(neighbour, neighbour.Time);
                }
            }

            return int.MaxValue;
        }

        private IEnumerable<PointNode> GetNextPoints(PointNode start, IDictionary<Point, HashSet<EquipmentType>> visited)
        {
            var neighbourPoints = new HashSet<Point>
            {
                new Point(start.P.X + 1, start.P.Y),
                new Point(start.P.X - 1, start.P.Y),
                new Point(start.P.X, start.P.Y + 1),
                new Point(start.P.X, start.P.Y - 1)
            };

            var currentTerrain = this._terrain[start.P];
            var nextPoints = new HashSet<PointNode>();

            foreach (var neighbour in neighbourPoints)
            {
                if (neighbour.X < 0 || neighbour.X > this._furthestPoint.X || neighbour.Y < 0 ||
                    neighbour.Y > this._furthestPoint.Y) continue;
                
                var neighbourTerrain = this._terrain[neighbour];
                var nextPoint = new PointNode
                {
                    Equipment = start.Equipment,
                    P = neighbour,
                    Time = start.Time + 1
                };

                switch (currentTerrain)
                {
                    case TerrainType.Narrow:
                        switch (neighbourTerrain)
                        {
                            case TerrainType.Rocky:
                                nextPoint.Equipment = EquipmentType.Torch;
                                break;
                            case TerrainType.Wet:
                                nextPoint.Equipment = EquipmentType.Neither;
                                break;
                            case TerrainType.Narrow:
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                        break;
                    case TerrainType.Rocky:
                        switch (neighbourTerrain)
                        {
                            case TerrainType.Rocky:
                                break;
                            case TerrainType.Wet:
                                nextPoint.Equipment = EquipmentType.ClimbingGear;
                                break;
                            case TerrainType.Narrow:
                                nextPoint.Equipment = EquipmentType.Torch;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                        break;
                    case TerrainType.Wet:
                        switch (neighbourTerrain)
                        {
                            case TerrainType.Rocky:
                                nextPoint.Equipment = EquipmentType.ClimbingGear;
                                break;
                            case TerrainType.Wet:
                                break;
                            case TerrainType.Narrow:
                                nextPoint.Equipment = EquipmentType.Neither;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (nextPoint.Equipment != start.Equipment)
                {
                    // Changing tools to get to this point, add tool change time
                    nextPoint.Time += 7;
                }

                if (!visited.ContainsKey(nextPoint.P) || !visited[nextPoint.P].Contains(nextPoint.Equipment))
                {
                    // Not visited this point before, or not visited it with this equipment
                    nextPoints.Add(nextPoint);                    
                }
            }

            return nextPoints;
        }

        private class PointNode : FastPriorityQueueNode
        {
            public Point P { get; set; }
            public int Time { get; set; }
            public EquipmentType Equipment { get; set; }
        }

        protected override void DoPart2()
        {
            var initialiseTo = this._furthestPoint;
            this.InitialiseToPoint(initialiseTo);

            var shortestPath = this.GetShortestPathToTarget();
//            int oldShortestPath;
//
//            do
//            {
//                oldShortestPath = shortestPath;
//                initialiseTo = new Point(initialiseTo.X + 50, initialiseTo.Y + 50);
//                this.InitialiseToPoint(initialiseTo);
//                shortestPath = this.GetShortestPathToTarget();
//            } while (shortestPath != oldShortestPath);

            var colour = ConsoleColor.Cyan;

            if (shortestPath >= 1045)
            {
                colour = ConsoleColor.Red;
            }
            
            ConsoleUtils.WriteColouredLine($"Can get to target in {shortestPath} minutes", colour);
        }
    }
}