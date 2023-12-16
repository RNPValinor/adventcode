namespace _2023.Days;

public class Day16 : Day
{
    private string _grid = "";

    private int _rowLength;
    private int _columnLength;
    
    public Day16() : base(16)
    {
    }

    protected override void ProcessInputLine(string line)
    {
        this._grid += line;
        this._rowLength = line.Length;
        this._columnLength++;
    }

    private int ConvertCoordsToGridIndex(int x, int y)
    {
        return y * this._rowLength + x;
    }

    protected override void SolvePart1()
    {
        var numVisited = this.GetNumVisitedPointsFromStartBeam((-1, 0, Directions.East));

        this.Part1Solution = numVisited.ToString();
    }

    private int GetNumVisitedPointsFromStartBeam((int x, int y, Directions d) startBeam)
    {
        var beams = new Queue<(int x, int y, Directions d)>();
        beams.Enqueue((startBeam.x, startBeam.y, startBeam.d));

        var visited = new Dictionary<(int x, int y), Directions>();

        while (beams.TryDequeue(out var beam))
        {
            if (visited.TryGetValue((beam.x, beam.y), out var directions))
            {
                if (directions.HasFlag(beam.d))
                {
                    continue;
                }

                visited[(beam.x, beam.y)] |= beam.d;
            }
            else
            {
                visited.Add((beam.x, beam.y), beam.d);
            }

            var (nextX, nextY) = GetNextPoint(beam);

            if (this.IsInBounds(nextX, nextY) is false)
            {
                continue;
            }

            this.EnqueueNextBeams(nextX, nextY, beam.d, beams);
        }

        // Don't count the start beam as visited, as it was out of bounds.
        return visited.Count - 1;
    }

    private void EnqueueNextBeams(int x, int y, Directions d, Queue<(int x, int y, Directions d)> beams)
    {
        var tile = this._grid[this.ConvertCoordsToGridIndex(x, y)];
        Directions nextD;

        switch (tile)
        {
            case '/':
                nextD = d switch
                {
                    Directions.East => Directions.North,
                    Directions.North => Directions.East,
                    Directions.South => Directions.West,
                    Directions.West => Directions.South,
                    _ => throw new ArgumentOutOfRangeException(nameof(d), d, null)
                };
                
                beams.Enqueue((x, y, nextD));
                break;
            case '\\':
                nextD = d switch
                {
                    Directions.East => Directions.South,
                    Directions.North => Directions.West,
                    Directions.South => Directions.East,
                    Directions.West => Directions.North,
                    _ => throw new ArgumentOutOfRangeException(nameof(d), d, null)
                };
                
                beams.Enqueue((x, y, nextD));
                break;
            case '|':
                if (d is Directions.East or Directions.West)
                {
                    beams.Enqueue((x, y, Directions.North));
                    beams.Enqueue((x, y, Directions.South));
                }
                else
                {
                    beams.Enqueue((x, y, d));
                }
                break;
            case '-':
                if (d is Directions.North or Directions.South)
                {
                    beams.Enqueue((x, y, Directions.East));
                    beams.Enqueue((x, y, Directions.West));
                }
                else
                {
                    beams.Enqueue((x, y, d));
                }
                break;
            default:
                beams.Enqueue((x, y, d));
                break;
        }
    }

    private static (int x, int y) GetNextPoint((int x, int y, Directions d) beam)
    {
        return beam.d switch {
            Directions.North => (beam.x, beam.y - 1),
            Directions.South => (beam.x, beam.y + 1),
            Directions.East => (beam.x + 1, beam.y),
            Directions.West => (beam.x - 1, beam.y),
            _ => (0, 0)
        };
    }

    private bool IsInBounds(int x, int y)
    {
        return x >= 0 && x < this._rowLength && y >= 0 && y < this._columnLength;
    }

    protected override void SolvePart2()
    {
        var startBeams = new HashSet<(int x, int y, Directions d)>();

        for (var x = 0; x < this._rowLength; x++)
        {
            startBeams.Add((x, -1, Directions.South));
            startBeams.Add((x, this._columnLength, Directions.North));
        }

        for (var y = 0; y < this._columnLength; y++)
        {
            startBeams.Add((-1, y, Directions.East));
            startBeams.Add((this._rowLength, y, Directions.West));
        }

        var energyValues = new List<int>();
        
        Parallel.ForEach(startBeams, beam =>
        {
            energyValues.Add(this.GetNumVisitedPointsFromStartBeam(beam));
        });
        
        this.Part2Solution = energyValues.Max().ToString();
    }

    [Flags]
    private enum Directions
    {
        North = 1,
        South = 2,
        East = 4,
        West = 8
    }
}