namespace _2023.Days;

public class Day21() : Day(21)
{
    private char[] _garden = [];

    private int _rowLength;
    private int _columnLength;

    private (int x, int y) _startPos;
    
    protected override void ProcessInputLine(string line)
    {
        this._garden = this._garden.Concat(line.ToCharArray()).ToArray();

        var startIndex = line.IndexOf('S');

        if (startIndex is not -1)
        {
            this._startPos = (x: startIndex, y: this._columnLength);
        }
        
        this._rowLength = line.Length;
        this._columnLength++;
    }
    
    private int ConvertCoordsToGridIndex((int x, int y) pos)
    {
        var x = pos.x % this._rowLength;
        var y = pos.y % this._rowLength;

        if (x < 0)
        {
            x += this._rowLength;
        }

        if (y < 0)
        {
            y += this._columnLength;
        }
        
        return y * this._rowLength + x;
    }

    protected override void SolvePart1()
    {
        var currentPossiblePositions = new HashSet<(int x, int y)> { this._startPos };

        for (var step = 1; step <= 64; step++)
        {
            var nextPossiblePositions = new HashSet<(int x, int y)>();

            foreach (var pos in currentPossiblePositions)
            {
                nextPossiblePositions.UnionWith(this.GetValidAdjacentPoints(pos));
            }

            currentPossiblePositions = nextPossiblePositions;
        }

        this.Part1Solution = currentPossiblePositions.Count.ToString();
    }

    private HashSet<(int x, int y)> GetValidAdjacentPoints((int x, int y) pos)
    {
        var adjacentPoints = new HashSet<(int x, int y)>
        {
            pos with { y = pos.y - 1 },
            pos with { y = pos.y + 1 },
            pos with { x = pos.x - 1 },
            pos with { x = pos.x + 1 }
        };

        return adjacentPoints.Where(this.CanMoveTo).ToHashSet();
    }

    private bool CanMoveTo((int x, int y) pos)
    {
        return this._garden[this.ConvertCoordsToGridIndex(pos)] != '#';
    }

    protected override void SolvePart2()
    {
        if (this._rowLength != this._columnLength)
        {
            throw new ArgumentException("Need this to be square");
        }
        
        var grids = 26501365 / this._rowLength;
        var rem = 26501365 % this._rowLength;

        var sequence = new List<int>();
        var walkedOn = new HashSet<(int x, int y)> { this._startPos };
        var steps = 0;

        for (var n = 0; n < 3; n++)
        {
            for (; steps < n * this._rowLength + rem; steps++)
            {
                walkedOn = walkedOn
                    .SelectMany(this.GetValidAdjacentPoints)
                    .ToHashSet();
            }
            
            sequence.Add(walkedOn.Count);
        }
        
        var c = sequence[0];
        var aPlusB = sequence[1] - c;
        var fourAPlusTwoB = sequence[2] - c;
        var twoA = fourAPlusTwoB - (2 * aPlusB);
        var a = twoA / 2;
        var b = aPlusB - a;

        this.Part2Solution = F(grids).ToString();
        return;

        long F(long n)
        {
            return a * (n * n) + b * n + c;
        }
    }
}