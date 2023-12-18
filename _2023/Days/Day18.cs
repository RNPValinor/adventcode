using System.Globalization;
using _2023.Utils;

namespace _2023.Days;

public class Day18() : Day(18)
{
    private readonly Dictionary<(int x, int y), Directions> _simpleCornerTypes = new();
    private readonly Dictionary<int, SortedSet<int>> _simpleHoleEdges = new();
    private Directions _simpleLastDirection = Directions.None;
    private (int x, int y) _simpleCurPos = (0, 0);
    
    private readonly Dictionary<(int x, int y), Directions> _complexCornerTypes = new();
    private readonly Dictionary<int, SortedSet<int>> _complexHoleEdges = new();
    private Directions _complexLastDirection = Directions.None;
    private (int x, int y) _complexCurPos = (0, 0);

    protected override void ProcessInputLine(string line)
    {
        this.ProcessLineBasic(line);
        this.ProcessLineComplex(line);
    }

    private void ProcessLineBasic(string line)
    {
        var direction = line[0] switch
        {
            'U' => Directions.North,
            'D' => Directions.South,
            'L' => Directions.West,
            'R' => Directions.East,
            _ => throw new ArgumentOutOfRangeException(nameof(line), line, "Unexpected direction")
        };

        var distance = int.Parse(string.Join(null, line[2..].TakeWhile(c => c is >= '0' and <= '9')));

        this._simpleCurPos = DoMove(direction, distance, this._simpleCurPos, this._simpleLastDirection, this._simpleCornerTypes, this._simpleHoleEdges);
        
        this._simpleLastDirection = direction;
    }

    private void ProcessLineComplex(string line)
    {
        var direction = line[^2] switch
        {
            '0' => Directions.East,
            '1' => Directions.South,
            '2' => Directions.West,
            '3' => Directions.North,
            _ => Directions.None
        };

        var distance = int.Parse(line.Substring(line.Length - 7, 5), NumberStyles.HexNumber);

        this._complexCurPos = DoMove(direction, distance, this._complexCurPos, this._complexLastDirection, this._complexCornerTypes, this._complexHoleEdges);

        this._complexLastDirection = direction;
    }

    private static (int x, int y) DoMove(Directions direction, int distance, (int x, int y) curPos, Directions lastDirection, Dictionary<(int x, int y), Directions> cornerTypes, Dictionary<int, SortedSet<int>> holeEdges)
    {
        if (lastDirection is not Directions.None)
        {
            cornerTypes.Add(curPos, GetCornerType(lastDirection, direction));
        }

        int dy;

        if (direction is Directions.North or Directions.South)
        {
            for (dy = 0; dy <= distance; dy++)
            {
                AddPointToPathEdge(curPos with { y = direction is Directions.North ? curPos.y - dy : curPos.y + dy }, holeEdges);
            }
        }

        var dx = direction switch
        {
            Directions.East => distance,
            Directions.West => -distance,
            _ => 0
        };
        
        dy = direction switch
        {
            Directions.South => distance,
            Directions.North => -distance,
            _ => 0
        };

        return (curPos.x + dx, curPos.y + dy);
    }

    private static Directions GetCornerType(Directions d1, Directions d2)
    {
        return d1 switch
        {
            Directions.North => Directions.South,
            Directions.South => Directions.North,
            Directions.East => d2 switch
            {
                Directions.North => Directions.North,
                Directions.South => Directions.South,
                _ => throw new ArgumentException($"Invalid direction combo {d1} and {d2}")
            },
            Directions.West => d2 switch
            {
                Directions.North => Directions.North,
                Directions.South => Directions.South,
                _ => throw new ArgumentException($"Invalid direction combo {d1} and {d2}")
            },
            _ => throw new ArgumentOutOfRangeException(nameof(d1), d1, null)
        };
    }

    private static void AddPointToPathEdge((int x, int y) p, Dictionary<int, SortedSet<int>> holeEdges)
    {
        if (holeEdges.TryGetValue(p.y, out var xes))
        {
            xes.Add(p.x);
        }
        else
        {
            holeEdges[p.y] = [p.x];
        }
    }

    protected override void SolvePart1()
    {
        this._simpleCornerTypes.Add((0, 0), this._simpleLastDirection is Directions.South ? Directions.North : Directions.South);

        var holeSize = GetHoleSize(this._simpleHoleEdges, this._simpleCornerTypes);

        this.Part1Solution = holeSize.ToString();
    }

    protected override void SolvePart2()
    {
        this._complexCornerTypes.Add((0, 0), this._complexLastDirection is Directions.South ? Directions.North : Directions.South);
        
        var holeSize = GetHoleSize(this._complexHoleEdges, this._complexCornerTypes);
        
        this.Part2Solution = holeSize.ToString();
    }
    
    private static long GetHoleSize(Dictionary<int, SortedSet<int>> holeEdges, Dictionary<(int x, int y), Directions> corners)
    {
        return holeEdges.AsParallel().Sum(kvp => GetLineSize(corners, kvp.Value, kvp.Key));
    }

    private static long GetLineSize(Dictionary<(int x, int y), Directions> corners, SortedSet<int> xes, int y)
    {
        var isInside = false;
        var lastEdge = 0;
        var lastCornerType = Directions.None;
        var lineSize = 0L;

        foreach (var x in xes)
        {
            if (corners.TryGetValue((x, y), out var corner))
            {
                if (lastCornerType is Directions.None)
                {
                    if (isInside)
                    {
                        lineSize += x - lastEdge - 1;
                    }
                        
                    lastCornerType = corner;
                }
                else
                {
                    lineSize += x - lastEdge + 1;

                    if (lastCornerType != corner)
                    {
                        isInside = !isInside;
                    }

                    lastCornerType = Directions.None;
                }
            }
            else
            {
                // Not a corner; check to see if we were just inside
                if (isInside)
                {
                    lineSize += x - lastEdge;
                }
                else
                {
                    lineSize++;
                }

                isInside = !isInside;
            }
                
            lastEdge = x;
        }

        return lineSize;
    }
}