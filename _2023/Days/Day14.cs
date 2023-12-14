using System.Collections.Immutable;
using System.Drawing;

namespace _2023.Days;

public class Day14 : Day
{
    private readonly HashSet<Point> _sphericalRocks = new();
    private readonly HashSet<Point> _cubicRocks = new();
    
    private readonly Dictionary<int, int> _nextFallPositionPerColumn = new();
    private int _y;

    private int _numRocks;
    private int _load;

    private int _maxX;
    private int _maxY;
    
    public Day14() : base(14)
    {
    }

    protected override void ProcessInputLine(string line)
    {
        this._load += this._numRocks;
        this._maxX = line.Length - 1;
        
        for (var x = 0; x < line.Length; x++)
        {
            switch (line[x])
            {
                case '#':
                    this._cubicRocks.Add(new(x, this._y));
                    this._nextFallPositionPerColumn.Remove(x);
                    this._nextFallPositionPerColumn.Add(x, this._y + 1);
                    break;
                case 'O':
                    this._sphericalRocks.Add(new(x, this._y));
                    this._numRocks++;

                    var fallPosition = this._nextFallPositionPerColumn.GetValueOrDefault(x, 0);

                    this._load += (this._y - fallPosition) + 1;

                    this._nextFallPositionPerColumn.Remove(x);
                    this._nextFallPositionPerColumn.Add(x, fallPosition + 1);
                    
                    break;
            }
        }
        
        this._y++;
    }

    protected override void SolvePart1()
    {
        this.Part1Solution = this._load.ToString();
    }

    protected override void SolvePart2()
    {
        this._maxY = this._y - 1;

        var currentState = this._sphericalRocks.ToImmutableHashSet();
        var previousStates = new HashSet<(ImmutableHashSet<Point> rocks, int idx)> { (currentState, 0) };

        const int totalNumCycles = 1000000000;

        for (var i = 1; i <= totalNumCycles; i++)
        {
            currentState = this.DoCycle(currentState);

            if (previousStates.Any(s => s.rocks.SetEquals(currentState)))
            {
                // Found our loop!
                var (_, loopStartIdx) = previousStates.Single(s => s.rocks.SetEquals(currentState));

                var loopLength = i - loopStartIdx;

                var numRemainingCycles = totalNumCycles - i;

                var loopIndex = numRemainingCycles % loopLength;

                var actualIndex = loopIndex + loopStartIdx;

                var finalState = previousStates.Single(s => s.idx == actualIndex);

                // < 98983
                this.Part2Solution = this.GetLoad(finalState.rocks).ToString();

                return;
            }

            previousStates.Add((currentState, i));
        }
    }

    private int GetLoad(IEnumerable<Point> rocks)
    {
        return rocks.Sum(rock => this._maxY - rock.Y + 1);
    }

    private ImmutableHashSet<Point> DoCycle(ImmutableHashSet<Point> current)
    {
        return this.RollEast(this.RollSouth(this.RollWest(this.RollNorth(current)))).ToImmutableHashSet();
    }

    private HashSet<Point> RollNorth(IReadOnlySet<Point> current)
    {
        var next = new HashSet<Point>();

        for (var x = 0; x <= this._maxX; x++)
        {
            var fallPos = 0;
            
            for (var y = 0; y <= this._maxY; y++)
            {
                var curPos = new Point(x, y);
                
                if (current.Contains(curPos))
                {
                    next.Add(new(x, fallPos++));
                }
                else if (this._cubicRocks.Contains(curPos))
                {
                    fallPos = y + 1;
                }
            }
        }

        return next;
    }
    
    private HashSet<Point> RollSouth(IReadOnlySet<Point> current)
    {
        var next = new HashSet<Point>();

        for (var x = 0; x <= this._maxX; x++)
        {
            var fallPos = this._maxY;
            
            for (var y = this._maxY; y >= 0; y--)
            {
                var curPos = new Point(x, y);
                
                if (current.Contains(curPos))
                {
                    next.Add(new(x, fallPos--));
                }
                else if (this._cubicRocks.Contains(curPos))
                {
                    fallPos = y - 1;
                }
            }
        }

        return next;
    }

    private HashSet<Point> RollWest(IReadOnlySet<Point> current)
    {
        var next = new HashSet<Point>();

        for (var y = 0; y <= this._maxY; y++)
        {
            var fallPos = 0;
            
            for (var x = 0; x <= this._maxX; x++)
            {
                var curPos = new Point(x, y);
                
                if (current.Contains(curPos))
                {
                    next.Add(new(fallPos++, y));
                }
                else if (this._cubicRocks.Contains(curPos))
                {
                    fallPos = x + 1;
                }
            }
        }

        return next;
    }

    private HashSet<Point> RollEast(IReadOnlySet<Point> current)
    {
        var next = new HashSet<Point>();

        for (var y = 0; y <= this._maxY; y++)
        {
            var fallPos = this._maxX;
            
            for (var x = this._maxX; x >= 0; x--)
            {
                var curPos = new Point(x, y);
                
                if (current.Contains(curPos))
                {
                    next.Add(new(fallPos--, y));
                }
                else if (this._cubicRocks.Contains(curPos))
                {
                    fallPos = x - 1;
                }
            }
        }

        return next;
    }
}