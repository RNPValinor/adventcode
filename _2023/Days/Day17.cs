using _2023.Utils;

using Crucible = (int x, int y, _2023.Utils.Directions d, int numInD);

namespace _2023.Days;

public class Day17() : Day(17)
{
    private int[] _grid = Array.Empty<int>();
    private readonly HashSet<Crucible> _visited = [];

    private int _rowLength;
    private int _columnLength;

    protected override void ProcessInputLine(string line)
    {
        this._grid = this._grid.Concat(line.Select(c => int.Parse(c.ToString()))).ToArray();

        this._columnLength++;
        this._rowLength = line.Length;
    }
    
    private int ConvertCoordsToGridIndex((int x, int y) pos)
    {
        return pos.y * this._rowLength + pos.x;
    }

    protected override void SolvePart1()
    {
        var bestHeatLoss = this.RollTheCrucibles(CrucibleType.Boring);

        this.Part1Solution = bestHeatLoss.ToString();
    }
    
    protected override void SolvePart2()
    {
        var bestHeatLoss = this.RollTheCrucibles(CrucibleType.Ultra);

        // < 1365
        this.Part2Solution = bestHeatLoss.ToString();
    }

    private int RollTheCrucibles(CrucibleType crucibleType)
    {
        this._visited.Clear();
        
        // The priority of the queue is the total heat. Lowest heat is dequeued first.
        var seekers = new PriorityQueue<Crucible, int>();
        seekers.Enqueue((0, 0, Directions.East, 0), 0);

        if (crucibleType is CrucibleType.Ultra)
        {
            // We can't turn an ultra crucible immediately, so consider going south at the start
            seekers.Enqueue((0, 0, Directions.South, 0), 0);
        }

        var bestHeatLoss = int.MaxValue;

        while (seekers.TryDequeue(out var crucible, out var heatLoss))
        {
            if (crucible.x == this._rowLength - 1 && crucible.y == this._columnLength - 1 && (crucibleType is CrucibleType.Boring || crucible.numInD >= 4))
            {
                bestHeatLoss = Math.Min(bestHeatLoss, heatLoss);
            }
            
            var nextCrucibles = this.GetNextCruciblePossibilities(crucible, heatLoss, crucibleType);

            foreach (var (c, newHeatLoss) in nextCrucibles)
            {
                this._visited.Add(c);
                seekers.Enqueue(c, newHeatLoss);
            }
        }

        return bestHeatLoss;
    }

    private List<(Crucible crucible, int heatLoss)> GetNextCruciblePossibilities(Crucible crucible, int heatLoss, CrucibleType crucibleType)
    {
        var nextDirections = GetNextDirections(crucible.d, crucible.numInD, crucibleType);

        return (
            from d in nextDirections
            let nextPos = MoveInDirection(crucible.x, crucible.y, d)
                where this.IsInBounds(nextPos)
            let blockHeatLoss = this._grid[this.ConvertCoordsToGridIndex(nextPos)]
            let nextHeatLoss = heatLoss + blockHeatLoss
            let nextCrucible = (nextPos.x, nextPos.y, d, d == crucible.d ? crucible.numInD + 1 : 1)
                where !this._visited.Contains(nextCrucible)
            select (nextCrucible, nextHeatLoss)
        ).ToList();
    }

    private static IEnumerable<Directions> GetNextDirections(Directions d, int numInD, CrucibleType crucibleType)
    {
        var nextDirections = new List<Directions>();

        if (numInD >= (crucibleType is CrucibleType.Ultra ? 4 : 0))
        {
            nextDirections = GetPerpendicularDirections(d);
        }

        if (numInD < (crucibleType is CrucibleType.Ultra ? 10 : 3))
        {
            nextDirections.Add(d);
        }

        return nextDirections;
    }

    private static List<Directions> GetPerpendicularDirections(Directions d)
    {
        if (d is Directions.North or Directions.South)
        {
            return
            [
                Directions.East,
                Directions.West
            ];
        }
        else
        {
            return
            [
                Directions.North,
                Directions.South
            ];
        }
    }

    private static (int x, int y) MoveInDirection(int x, int y, Directions d)
    {
        return d switch
        {
            Directions.North => (x, y - 1),
            Directions.South => (x, y + 1),
            Directions.East => (x + 1, y),
            Directions.West => (x - 1, y),
            _ => (x, y)
        };
    }

    private bool IsInBounds((int x, int y) pos)
    {
        return pos.x >= 0 && pos.x < this._rowLength && pos.y >= 0 && pos.y < this._columnLength;
    }

    private enum CrucibleType
    {
        Boring,
        Ultra
    }
}