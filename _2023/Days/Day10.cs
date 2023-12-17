using System.Drawing;

namespace _2023.Days;

public class Day10() : Day(10)
{
    private readonly Dictionary<Point, Point[]> _map = new();
    private readonly Dictionary<Point, PipeType> _pipeMap = new();

    private int _y;
    private int _maxX;

    private readonly HashSet<Point> _path = [];

    private readonly HashSet<Point> _oneSide = [];
    private readonly HashSet<Point> _twoSide = [];

    private Point _startPoint = new(-1, -1);

    protected override void ProcessInputLine(string line)
    {
        for (var x = 0; x < line.Length; x++) {
            var curPos = new Point(x, this._y);
            var connectingPoints = new Point[2];
            PipeType pipeType;

            switch (line[x]) {
                case '|':
                    connectingPoints[0] = curPos with { Y = curPos.Y - 1 };
                    connectingPoints[1] = curPos with { Y = curPos.Y + 1 };
                    pipeType = PipeType.Vertical;
                    break;
                case '-':
                    connectingPoints[0] = curPos with { X = curPos.X - 1 };
                    connectingPoints[1] = curPos with { X = curPos.X + 1 };
                    pipeType = PipeType.Horizontal;
                    break;
                case 'L':
                    connectingPoints[0] = curPos with { Y = curPos.Y - 1 };
                    connectingPoints[1] = curPos with { X = curPos.X + 1 };
                    pipeType = PipeType.NeCorner;
                    break;
                case 'J':
                    connectingPoints[0] = curPos with { Y = curPos.Y - 1 };
                    connectingPoints[1] = curPos with { X = curPos.X - 1 };
                    pipeType = PipeType.NwCorner;
                    break;
                case '7':
                    connectingPoints[0] = curPos with { X = curPos.X - 1 };
                    connectingPoints[1] = curPos with { Y = curPos.Y + 1 };
                    pipeType = PipeType.SwCorner;
                    break;
                case 'F':
                    connectingPoints[0] = curPos with { X = curPos.X + 1 };
                    connectingPoints[1] = curPos with { Y = curPos.Y + 1 };
                    pipeType = PipeType.SeCorner;
                    break;
                case 'S':
                    // Special case!
                    this._startPoint = curPos;
                    continue;
                case '.':
                    continue;
                default:
                    throw new ArgumentException("Unexpected input character '" + line[x] + "'");
            }

            this._map.Add(curPos, connectingPoints);
            this._pipeMap.Add(curPos, pipeType);
        }

        this._y++;
        this._maxX = line.Length - 1;
    }

    protected override void SolvePart1()
    {
        this.AddStartPointToMap();

        var farthestDistance = this.GetDistanceToFurthestLoopPoint();

        this.Part1Solution = farthestDistance.ToString();
    }
    
    private void AddStartPointToMap() {
        if (this._map.ContainsKey(this._startPoint)) {
            return;
        }

        var potentialStartPointTypes = new HashSet<PipeType>
        {
            PipeType.Horizontal,
            PipeType.Vertical,
            PipeType.NwCorner,
            PipeType.NeCorner,
            PipeType.SwCorner,
            PipeType.SeCorner
        };

        var up = new Point(this._startPoint.X, this._startPoint.Y - 1);
        var down = new Point(this._startPoint.X, this._startPoint.Y + 1);
        var left = new Point(this._startPoint.X - 1, this._startPoint.Y);
        var right = new Point(this._startPoint.X + 1, this._startPoint.Y);

        if (this._map.TryGetValue(up, out var connections)) {
            if (connections[0] == this._startPoint || connections[1] == this._startPoint) {
                potentialStartPointTypes.Remove(PipeType.Horizontal);
                potentialStartPointTypes.Remove(PipeType.SwCorner);
                potentialStartPointTypes.Remove(PipeType.SeCorner);
            }
        }

        if (this._map.TryGetValue(down, out connections)) {
            if (connections[0] == this._startPoint || connections[1] == this._startPoint) {
                potentialStartPointTypes.Remove(PipeType.Horizontal);
                potentialStartPointTypes.Remove(PipeType.NwCorner);
                potentialStartPointTypes.Remove(PipeType.NeCorner);
            }
        }

        if (this._map.TryGetValue(left, out connections)) {
            if (connections[0] == this._startPoint || connections[1] == this._startPoint) {
                potentialStartPointTypes.Remove(PipeType.Vertical);
                potentialStartPointTypes.Remove(PipeType.NeCorner);
                potentialStartPointTypes.Remove(PipeType.SeCorner);
            }
        }

        if (this._map.TryGetValue(right, out connections)) {
            if (connections[0] == this._startPoint || connections[1] == this._startPoint) {
                potentialStartPointTypes.Remove(PipeType.Vertical);
                potentialStartPointTypes.Remove(PipeType.NwCorner);
                potentialStartPointTypes.Remove(PipeType.SwCorner);
            }
        }

        if (potentialStartPointTypes.Count != 1) {
            throw new ApplicationException("Failed to determine start pos pipe type!");
        }

        var pipeType = potentialStartPointTypes.First();

        this._pipeMap.Add(this._startPoint, pipeType);

        var connectedNeighbours = pipeType switch {
            PipeType.NwCorner => new[]{up, left},
            PipeType.NeCorner => new[]{up, right},
            PipeType.SwCorner => new[]{down, left},
            PipeType.SeCorner => new[]{down, right},
            PipeType.Horizontal => new[]{left, right},
            PipeType.Vertical => new[]{up, down}
        };

        this._map.Add(this._startPoint, connectedNeighbours);
    }
    
    private int GetDistanceToFurthestLoopPoint() {
        var dist = 1;

        this._path.Add(this._startPoint);

        Point[] lastExplored = {this._startPoint, this._startPoint};
        var explorers = this._map[this._startPoint];

        // Points on each side of the path
        while (explorers[0] != this._startPoint && explorers[0] != explorers[1]) {
            this.AddToSideSets(this._oneSide, this._twoSide, lastExplored[0], explorers[0]);
            this.AddToSideSets(this._twoSide, this._oneSide, lastExplored[1], explorers[1]);
            this._path.Add(explorers[0]);
            this._path.Add(explorers[1]);

            Point[] nextLastExplored = {explorers[0], explorers[1]};

            explorers[0] = this.Explore(lastExplored[0], explorers[0]);
            explorers[1] = this.Explore(lastExplored[1], explorers[1]);

            lastExplored = nextLastExplored;

            dist++;
        }

        this.AddToSideSets(this._oneSide, this._twoSide, lastExplored[0], explorers[0]);
        this.AddToSideSets(this._twoSide, this._oneSide, lastExplored[1], explorers[1]);
        this._path.Add(explorers[0]);

        return dist;
    }

    private Point Explore(Point cameFrom, Point nowAt) {
        var pipes = this._map[nowAt];

        return pipes[0] != cameFrom ? pipes[0] : pipes[1];
    }

    private void AddToSideSets(ISet<Point> leftSet, ISet<Point> rightSet, Point prevPoint, Point curPoint)
    {
        var dx = curPoint.X - prevPoint.X;
        var dy = curPoint.Y - prevPoint.Y;

        var prevPointType = this._pipeMap[prevPoint];
        var north = prevPoint with { Y = prevPoint.Y - 1 };
        var south = prevPoint with { Y = prevPoint.Y + 1 };
        var east = prevPoint with { X = prevPoint.X + 1 };
        var west = prevPoint with { X = prevPoint.X - 1 };

        switch (prevPointType) {
            case PipeType.NwCorner:
                if (dx == -1) {
                    leftSet.Add(east);
                    leftSet.Add(south);
                } else {
                    rightSet.Add(east);
                    rightSet.Add(south);
                }

                break;
            case PipeType.NeCorner:
                if (dx == 1) {
                    rightSet.Add(west);
                    rightSet.Add(south);
                } else {
                    leftSet.Add(west);
                    leftSet.Add(south);
                }

                break;
            case PipeType.SwCorner:
                if (dx == -1) {
                    rightSet.Add(north);
                    rightSet.Add(east);
                } else {
                    leftSet.Add(north);
                    leftSet.Add(east);
                }

                break;
            case PipeType.SeCorner:
                if (dx == 1) {
                    leftSet.Add(north);
                    leftSet.Add(west);
                } else {
                    rightSet.Add(north);
                    rightSet.Add(west);
                }

                break;
            case PipeType.Horizontal:
                if (dx == 1) {
                    leftSet.Add(north);
                    rightSet.Add(south);
                } else {
                    leftSet.Add(south);
                    rightSet.Add(north);
                }

                break;
            case PipeType.Vertical:
                if (dy == 1) {
                    leftSet.Add(east);
                    rightSet.Add(west);
                } else {
                    leftSet.Add(west);
                    rightSet.Add(east);
                }

                break;
        }
    }

    protected override void SolvePart2()
    {
        var numEnclosedTiles = this.GetNumEnclosedLoopTiles();

        this.Part2Solution = numEnclosedTiles.ToString();
    }
    
    private int GetNumEnclosedLoopTiles() {
        var innerLoop = this._oneSide.Any(this.IsEdgePoint) ? this._twoSide : this._oneSide;
        innerLoop.RemoveWhere(this._path.Contains);

        this.AddInternalTiles(innerLoop);

        return innerLoop.Count;
    }
    
    private bool IsEdgePoint(Point p) {
        return p.X == 0 || p.X == this._maxX || p.Y == 0 || p.Y == this._y - 1;
    }

    private void AddInternalTiles(HashSet<Point> pointSet) {
        var innerPoints = new HashSet<Point>(pointSet);

        do {
            pointSet.UnionWith(innerPoints);
            var nextInnerPoints = new HashSet<Point>();

            foreach (var adjacentPoints in innerPoints.Select(point => this.GetNonPathAdjacentPoints(point, pointSet)))
            {
                nextInnerPoints.UnionWith(adjacentPoints);
            }

            innerPoints = nextInnerPoints;
        } while (innerPoints.Any());
    }

    private HashSet<Point> GetNonPathAdjacentPoints(Point p, IReadOnlySet<Point> ignoredPoints) {
        var adjacentPoints = new HashSet<Point>();

        var north = p with { Y = p.Y - 1 };
        var south = p with { Y = p.Y + 1 };
        var east = p with { X = p.X + 1 };
        var west = p with { X = p.X - 1 };

        if (!this._path.Contains(north) && !ignoredPoints.Contains(north)) {
            adjacentPoints.Add(north);
        }

        if (!this._path.Contains(south) && !ignoredPoints.Contains(south)) {
            adjacentPoints.Add(south);
        }

        if (!this._path.Contains(east) && !ignoredPoints.Contains(east)) {
            adjacentPoints.Add(east);
        }

        if (!this._path.Contains(west) && !ignoredPoints.Contains(west)) {
            adjacentPoints.Add(west);
        }

        return adjacentPoints;
    }

    private enum PipeType {
        NwCorner,
        NeCorner,
        SwCorner,
        SeCorner,
        Horizontal,
        Vertical
    }
}