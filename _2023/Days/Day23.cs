using _2023.Utils;

using ExploredPath = ((int x, int y) pos, System.Collections.Generic.HashSet<(int x, int y)> visited, int upperPathLength);

namespace _2023.Days;

public class Day23() : Day(23)
{
    private char[] _trails = [];

    private int _rowLength;
    private int _columnLength;

    private int _startX;
    
    protected override void ProcessInputLine(string line)
    {
        if (this._trails.Length is 0)
        {
            this._startX = line.IndexOf('.');
        }
        
        this._trails = this._trails.Concat(line.ToCharArray()).ToArray();
        
        this._rowLength = line.Length;
        this._columnLength++;
    }
    
    private int ConvertCoordsToGridIndex((int x, int y) pos)
    {
        return pos.y * this._rowLength + pos.x;
    }

    private bool IsInBounds((int x, int y) pos)
    {
        return pos.x >= 0 && pos.x < this._rowLength && pos.y >= 0 && pos.y < this._columnLength;
    }

    protected override void SolvePart1()
    {
        this.Part1Solution = this.GetMaxPathLength(SlopeType.Slippery).ToString();
    }
    
    protected override void SolvePart2()
    {
        this.Part2Solution = this.GetMaxPathLength(SlopeType.Dry).ToString();
    }

    private int GetMaxPathLength(SlopeType slopeType)
    {
        ExploredPath startPath = ((x: this._startX, y: 0), visited: [], upperPathLength: 0);

        var maxPathLength = 0;
        var activePaths = new HashSet<ExploredPath> { startPath };

        while (activePaths.Count is not 0)
        {
            var nextActivePaths = new HashSet<ExploredPath>();
            
            foreach (var path in activePaths)
            {
                if (path.pos.y == this._columnLength - 1)
                {
                    maxPathLength = Math.Max(maxPathLength, path.visited.Count + path.upperPathLength);
                }
                else
                {
                    nextActivePaths.UnionWith(this.GetNextActivePaths(path, slopeType));
                }
            }

            activePaths = nextActivePaths;
        }

        return maxPathLength;
    }

    private HashSet<ExploredPath> GetNextActivePaths(ExploredPath path, SlopeType slopeType)
    {
        var nextPaths = new HashSet<ExploredPath>();

        var possibleDirections = new HashSet<Directions>
        {
            Directions.North,
            Directions.South,
            Directions.East,
            Directions.West
        };

        foreach (var d in possibleDirections)
        {
            var nextStep = StepInDirection(path.pos, d);

            if (path.visited.Contains(nextStep) || this.IsInBounds(nextStep) is false)
            {
                continue;
            }

            var thingThere = this._trails[this.ConvertCoordsToGridIndex(nextStep)];

            if (thingThere is '.' || (slopeType is SlopeType.Dry && thingThere is not '#'))
            {
                nextPaths.Add((nextStep, [..path.visited, nextStep], path.upperPathLength));
            }
            else if (IsAppropriateSlope(thingThere, d))
            {
                nextStep = StepInDirection(nextStep, d);
                nextPaths.Add((nextStep, [nextStep], path.upperPathLength + path.visited.Count + 1));
            }
        }

        return nextPaths;
    }

    private static (int x, int y) StepInDirection((int x, int y) pos, Directions d)
    {
        return d switch
        {
            Directions.North => pos with { y = pos.y - 1 },
            Directions.South => pos with { y = pos.y + 1 },
            Directions.East => pos with { x = pos.x + 1 },
            Directions.West => pos with { x = pos.x - 1 },
            _ => pos
        };
    }

    private static bool IsAppropriateSlope(char slope, Directions d)
    {
        return d switch
        {
            Directions.North => slope is '^',
            Directions.South => slope is 'v',
            Directions.East => slope is '>',
            Directions.West => slope is '<',
            _ => false
        };
    }

    private enum SlopeType
    {
        Slippery,
        Dry
    }
}