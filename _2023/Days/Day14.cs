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

    private IEnumerable<Point> RollNorth(IEnumerable<Point> current)
    {
        var next = new HashSet<Point>();
        var nextFallPos = new Dictionary<int, int>();
        
        // North - ascending order of Y
        foreach (var rock in current.OrderBy(r => r.Y))
        {
            var fallPos = nextFallPos.GetValueOrDefault(rock.X, 0);
            
            var obstacleCubes = this._cubicRocks
                .Where(r => r.X == rock.X && r.Y < rock.Y)
                .ToList();

            if (obstacleCubes.Any())
            {
                var nextObstacleCube = obstacleCubes.MaxBy(r => r.Y);
                fallPos = Math.Max(nextObstacleCube.Y + 1, fallPos);
            }
            
            next.Add(rock with { Y = fallPos });
            nextFallPos[rock.X] = fallPos + 1;
        }

        return next;
    }

    private IEnumerable<Point> RollWest(IEnumerable<Point> current)
    {
        var next = new HashSet<Point>();
        var nextFallPos = new Dictionary<int, int>();
        
        // West - ascending order of X
        foreach (var rock in current.OrderBy(r => r.X))
        {
            var fallPos = nextFallPos.GetValueOrDefault(rock.Y, 0);
            
            var obstacleCubes = this._cubicRocks
                .Where(r => r.Y == rock.Y && r.X < rock.X)
                .ToList();

            if (obstacleCubes.Any())
            {
                var nextObstacleCube = obstacleCubes.MaxBy(r => r.X);
                fallPos = Math.Max(nextObstacleCube.X + 1, fallPos);
            }
            
            next.Add(rock with { X = fallPos });
            nextFallPos[rock.Y] = fallPos + 1;
        }

        return next;
    }

    private IEnumerable<Point> RollSouth(IEnumerable<Point> current)
    {
        var next = new HashSet<Point>();
        var nextFallPos = new Dictionary<int, int>();
        
        // South - descending order of X
        foreach (var rock in current.OrderByDescending(r => r.Y))
        {
            var fallPos = nextFallPos.GetValueOrDefault(rock.X, this._maxY);
            
            var obstacleCubes = this._cubicRocks
                .Where(r => r.X == rock.X && r.Y > rock.Y)
                .ToList();

            if (obstacleCubes.Any())
            {
                var nextObstacleCube = obstacleCubes.MinBy(r => r.Y);
                fallPos = Math.Min(nextObstacleCube.Y - 1, fallPos);
            }
            
            next.Add(rock with { Y = fallPos });
            nextFallPos[rock.X] = fallPos - 1;
        }

        return next;
    }

    private IEnumerable<Point> RollEast(IEnumerable<Point> current)
    {
        var next = new HashSet<Point>();
        var nextFallPos = new Dictionary<int, int>();
        
        // North - ascending order of Y
        foreach (var rock in current.OrderByDescending(r => r.X))
        {
            var fallPos = nextFallPos.GetValueOrDefault(rock.Y, this._maxX);
            
            var obstacleCubes = this._cubicRocks
                .Where(r => r.Y == rock.Y && r.X > rock.X)
                .ToList();

            if (obstacleCubes.Any())
            {
                var nextObstacleCube = obstacleCubes.MinBy(r => r.X);
                fallPos = Math.Min(nextObstacleCube.X - 1, fallPos);
            }
            
            next.Add(rock with { X = fallPos });
            nextFallPos[rock.Y] = fallPos - 1;
        }

        return next;
    }
}