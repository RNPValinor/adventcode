package solvers;

import java.awt.*;
import java.util.HashMap;
import java.util.HashSet;

@SuppressWarnings("unused")
public class Day10Solver extends BaseSolver {
    private final HashMap<Point, Point[]> _map = new HashMap<>();
    private final HashMap<Point, PipeType> _pipeMap = new HashMap<>();

    private int _y = 0;
    private int _maxX = 0;

    private final HashSet<Point> _path = new HashSet<>();

    private final HashSet<Point> _oneSide = new HashSet<>();
    private final HashSet<Point> _twoSide = new HashSet<>();

    private Point _startPoint = null;

    public Day10Solver() {
        super(10);
    }

    @Override
    protected void processLine(String line) {
        for (var x = 0; x < line.length(); x++) {
            var curPos = new Point(x, this._y);
            var connectingPoints = new Point[2];
            PipeType pipeType;

            switch (line.charAt(x)) {
                case '|':
                    connectingPoints[0] = new Point(curPos.x, curPos.y - 1);
                    connectingPoints[1] = new Point(curPos.x, curPos.y + 1);
                    pipeType = PipeType.Vertical;
                    break;
                case '-':
                    connectingPoints[0] = new Point(curPos.x - 1, curPos.y);
                    connectingPoints[1] = new Point(curPos.x + 1, curPos.y);
                    pipeType = PipeType.Horizontal;
                    break;
                case 'L':
                    connectingPoints[0] = new Point(curPos.x, curPos.y - 1);
                    connectingPoints[1] = new Point(curPos.x + 1, curPos.y);
                    pipeType = PipeType.NECorner;
                    break;
                case 'J':
                    connectingPoints[0] = new Point(curPos.x, curPos.y - 1);
                    connectingPoints[1] = new Point(curPos.x - 1, curPos.y);
                    pipeType = PipeType.NWCorner;
                    break;
                case '7':
                    connectingPoints[0] = new Point(curPos.x - 1, curPos.y);
                    connectingPoints[1] = new Point(curPos.x, curPos.y + 1);
                    pipeType = PipeType.SWCorner;
                    break;
                case 'F':
                    connectingPoints[0] = new Point(curPos.x + 1, curPos.y);
                    connectingPoints[1] = new Point(curPos.x, curPos.y + 1);
                    pipeType = PipeType.SECorner;
                    break;
                case 'S':
                    // Special case!
                    this._startPoint = curPos;
                    continue;
                case '.':
                    continue;
                default:
                    throw new IllegalArgumentException("Unexpected input character '" + line.charAt(x) + "'");
            }

            this._map.put(curPos, connectingPoints);
            this._pipeMap.put(curPos, pipeType);
        }

        this._y++;
        this._maxX = line.length() - 1;
    }

    @Override
    protected String solvePart1() {
        this.addStartPointToMap();

        var farthestDistance = this.getDistanceToFurthestLoopPoint();

        return String.valueOf(farthestDistance);
    }

    private void addStartPointToMap() {
        if (this._map.containsKey(this._startPoint)) {
            return;
        }

        var potentialStartPointTypes = new HashSet<PipeType>();
        potentialStartPointTypes.add(PipeType.Horizontal);
        potentialStartPointTypes.add(PipeType.Vertical);
        potentialStartPointTypes.add(PipeType.NWCorner);
        potentialStartPointTypes.add(PipeType.NECorner);
        potentialStartPointTypes.add(PipeType.SWCorner);
        potentialStartPointTypes.add(PipeType.SECorner);

        var up = new Point(this._startPoint.x, this._startPoint.y - 1);
        var down = new Point(this._startPoint.x, this._startPoint.y + 1);
        var left = new Point(this._startPoint.x - 1, this._startPoint.y);
        var right = new Point(this._startPoint.x + 1, this._startPoint.y);

        if (this._map.containsKey(up)) {
            var upConns = this._map.get(up);

            if (upConns[0].equals(this._startPoint) || upConns[1].equals(this._startPoint)) {
                potentialStartPointTypes.remove(PipeType.Horizontal);
                potentialStartPointTypes.remove(PipeType.SWCorner);
                potentialStartPointTypes.remove(PipeType.SECorner);
            }
        }

        if (this._map.containsKey(down)) {
            var downConns = this._map.get(down);

            if (downConns[0].equals(this._startPoint) || downConns[1].equals(this._startPoint)) {
                potentialStartPointTypes.remove(PipeType.Horizontal);
                potentialStartPointTypes.remove(PipeType.NWCorner);
                potentialStartPointTypes.remove(PipeType.NECorner);
            }
        }

        if (this._map.containsKey(left)) {
            var leftConns = this._map.get(left);

            if (leftConns[0].equals(this._startPoint) || leftConns[1].equals(this._startPoint)) {
                potentialStartPointTypes.remove(PipeType.Vertical);
                potentialStartPointTypes.remove(PipeType.NECorner);
                potentialStartPointTypes.remove(PipeType.SECorner);
            }
        }

        if (this._map.containsKey(right)) {
            var rightConns = this._map.get(right);

            if (rightConns[0].equals(this._startPoint) || rightConns[1].equals(this._startPoint)) {
                potentialStartPointTypes.remove(PipeType.Vertical);
                potentialStartPointTypes.remove(PipeType.NWCorner);
                potentialStartPointTypes.remove(PipeType.SWCorner);
            }
        }

        if (potentialStartPointTypes.size() != 1) {
            throw new RuntimeException("Failed to determine start pos pipe type!");
        }

        var pipeType = potentialStartPointTypes.stream().findFirst().get();

        this._pipeMap.put(this._startPoint, pipeType);

        Point[] connectedNeighbours = switch (pipeType) {
            case NWCorner -> new Point[]{up, left};
            case NECorner -> new Point[]{up, right};
            case SWCorner -> new Point[]{down, left};
            case SECorner -> new Point[]{down, right};
            case Horizontal -> new Point[]{left, right};
            case Vertical -> new Point[]{up, down};
        };

        this._map.put(this._startPoint, connectedNeighbours);
    }

    private int getDistanceToFurthestLoopPoint() {
        var dist = 1;

        this._path.add(this._startPoint);

        Point[] lastExplored = {this._startPoint, this._startPoint};
        var explorers = this._map.get(this._startPoint);

        // Points on each side of the path
        while (!explorers[0].equals(this._startPoint) && !explorers[0].equals(explorers[1])) {
            this.addToSideSets(this._oneSide, this._twoSide, lastExplored[0], explorers[0]);
            this.addToSideSets(this._twoSide, this._oneSide, lastExplored[1], explorers[1]);
            this._path.add(explorers[0]);
            this._path.add(explorers[1]);

            Point[] nextLastExplored = {explorers[0], explorers[1]};

            explorers[0] = this.explore(lastExplored[0], explorers[0]);
            explorers[1] = this.explore(lastExplored[1], explorers[1]);

            lastExplored = nextLastExplored;

            dist++;
        }

        this.addToSideSets(this._oneSide, this._twoSide, lastExplored[0], explorers[0]);
        this.addToSideSets(this._twoSide, this._oneSide, lastExplored[1], explorers[1]);
        this._path.add(explorers[0]);

        return dist;
    }

    private boolean isEdgePoint(Point p) {
        return p.x == 0 || p.x == this._maxX || p.y == 0 || p.y == this._y - 1;
    }

    private Point explore(Point cameFrom, Point nowAt) {
        var pipes = this._map.get(nowAt);

        return !pipes[0].equals(cameFrom) ? pipes[0] : pipes[1];
    }

    private void addToSideSets(HashSet<Point> leftSet, HashSet<Point> rightSet, Point prevPoint, Point curPoint) {
        var dx = curPoint.x - prevPoint.x;
        var dy = curPoint.y - prevPoint.y;

        var prevPointType = this._pipeMap.get(prevPoint);
        var north = new Point(prevPoint.x, prevPoint.y - 1);
        var south = new Point(prevPoint.x, prevPoint.y + 1);
        var east = new Point(prevPoint.x + 1, prevPoint.y);
        var west = new Point(prevPoint.x - 1, prevPoint.y);

        switch (prevPointType) {
            case NWCorner -> {
                if (dx == -1) {
                    leftSet.add(east);
                    leftSet.add(south);
                } else {
                    rightSet.add(east);
                    rightSet.add(south);
                }
            }
            case NECorner -> {
                if (dx == 1) {
                    rightSet.add(west);
                    rightSet.add(south);
                } else {
                    leftSet.add(west);
                    leftSet.add(south);
                }
            }
            case SWCorner -> {
                if (dx == -1) {
                    rightSet.add(north);
                    rightSet.add(east);
                } else {
                    leftSet.add(north);
                    leftSet.add(east);
                }
            }
            case SECorner -> {
                if (dx == 1) {
                    leftSet.add(north);
                    leftSet.add(west);
                } else {
                    rightSet.add(north);
                    rightSet.add(west);
                }
            }
            case Horizontal -> {
                if (dx == 1) {
                    leftSet.add(north);
                    rightSet.add(south);
                } else {
                    leftSet.add(south);
                    rightSet.add(north);
                }
            }
            case Vertical -> {
                if (dy == 1) {
                    leftSet.add(east);
                    rightSet.add(west);
                } else {
                    leftSet.add(west);
                    rightSet.add(east);
                }
            }
        }
    }

    @Override
    protected String solvePart2() {
        var numEnclosedTiles = this.getNumEnclosedLoopTiles();

        return String.valueOf(numEnclosedTiles);
    }

    private int getNumEnclosedLoopTiles() {
        HashSet<Point> innerLoop;

        if (this._oneSide.stream().anyMatch(this::isEdgePoint)) {
            innerLoop = this._twoSide;
        } else {
            innerLoop = this._oneSide;
        }

        innerLoop.removeIf(this._path::contains);

        this.addInternalTiles(innerLoop);

        return innerLoop.size();
    }

    private void addInternalTiles(HashSet<Point> pointSet) {
        var innerPoints = new HashSet<>(pointSet);

        do {
            pointSet.addAll(innerPoints);
            var nextInnerPoints = new HashSet<Point>();

            for (var point : innerPoints) {
                var adjacentPoints = this.getNonPathAdjacentPoints(point, pointSet);
                nextInnerPoints.addAll(adjacentPoints);
            }

            innerPoints = nextInnerPoints;
        } while (!innerPoints.isEmpty());
    }

    private HashSet<Point> getNonPathAdjacentPoints(Point p, HashSet<Point> ignoredPoints) {
        var adjacentPoints = new HashSet<Point>();

        var north = new Point(p.x, p.y - 1);
        var south = new Point(p.x, p.y + 1);
        var east = new Point(p.x + 1, p.y);
        var west = new Point(p.x - 1, p.y);

        if (!this._path.contains(north) && !ignoredPoints.contains(north)) {
            adjacentPoints.add(north);
        }

        if (!this._path.contains(south) && !ignoredPoints.contains(south)) {
            adjacentPoints.add(south);
        }

        if (!this._path.contains(east) && !ignoredPoints.contains(east)) {
            adjacentPoints.add(east);
        }

        if (!this._path.contains(west) && !ignoredPoints.contains(west)) {
            adjacentPoints.add(west);
        }

        return adjacentPoints;
    }

    public enum PipeType {
        NWCorner,
        NECorner,
        SWCorner,
        SECorner,
        Horizontal,
        Vertical
    }
}
