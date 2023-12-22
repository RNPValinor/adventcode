namespace _2023.Days;

public class Day22() : Day(22)
{
    private int _id;
    private readonly HashSet<Brick> _bricks = [];
    private readonly Dictionary<(int x, int y), HashSet<Brick>> _bricksByXyPos = new();
    
    protected override void ProcessInputLine(string line)
    {
        var parts = line.Split('~');

        var startPos = ConvertStringToPos(parts[0]);
        var endPos = ConvertStringToPos(parts[1]);

        var brick = new Brick(startPos, endPos, this._id++);

        var coords = brick.GetAllColumns();

        foreach (var coord in coords)
        {
            if (this._bricksByXyPos.TryGetValue(coord, out var bricks))
            {
                bricks.Add(brick);
            }
            else
            {
                this._bricksByXyPos.Add(coord, [brick]);
            }
        }

        this._bricks.Add(brick);
    }

    private static (int x, int y, int z) ConvertStringToPos(string strPos)
    {
        var posList = strPos.Split(',').Select(int.Parse).ToList();
        return (x: posList[0], y: posList[1], z: posList[2]);
    }

    protected override void SolvePart1()
    {
        // Lower all the bricks
        this.LowerAllTheBricks();

        this.Part1Solution = this.GetNumDisintegrationTargets().ToString();
    }

    private void LowerAllTheBricks()
    {
        foreach (var brick in this._bricks.OrderBy(b => b.GetMinZ()))
        {
            // Drop the bricks from lowest to highest. That way we already know the exact fall position for each brick
            var fallZ = brick
                .GetAllColumns()
                .Select(c => this.GetHighestBrickInColumnBelowZ(c, brick.GetMinZ()))
                .Max();

            brick.FallTo(fallZ);
        }
    }

    private int GetHighestBrickInColumnBelowZ((int x, int y) column, int z)
    {
        return this._bricksByXyPos[column]
            .Select(b => b.GetMaxZ() + 1)
            .Where(brickZ => brickZ <= z)
            .Append(1)
            .Max();
    }

    private int GetNumDisintegrationTargets()
    {
        return this._bricks
            .Select(this.GetUniquelySupportedBricks)
            // Count all the bricks which have no uniquely supported bricks
            .Count(bricksOnlySupportedByThisBrick => bricksOnlySupportedByThisBrick.Count is 0);
    }

    private List<Brick> GetUniquelySupportedBricks(Brick brick)
    {
        return this.GetUniquelySupportedBricks(brick, []);
        
    }

    private List<Brick> GetUniquelySupportedBricks(Brick brick, HashSet<Brick> disintegratedBricks)
    {
        return brick.GetAllColumns()
            // Get the bricks that are resting on this one
            .Select(c => this._bricksByXyPos[c].SingleOrDefault(b => b.GetMinZ() == brick.GetMaxZ() + 1))
            .Where(b => b is not null)
            .Cast<Brick>()
            // Only have a brick which is resting longways on this once
            .Distinct()
            // Return just the bricks which are only supported by this brick
            .Where(b => this.GetNumSupportingBricks(b, disintegratedBricks) == 1)
            .ToList();
    }

    private int GetNumSupportingBricks(Brick brick, HashSet<Brick> disintegratedBricks)
    {
        return brick.GetAllColumns()
            .Select(c => this._bricksByXyPos[c].SingleOrDefault(b => b.GetMaxZ() == brick.GetMinZ() - 1))
            .Where(b => b is not null)
            .Cast<Brick>()
            .Where(b => disintegratedBricks.Contains(b) is false)
            .Distinct()
            .Count();
    }

    protected override void SolvePart2()
    {
        this.Part2Solution = this._bricks.Sum(this.GetNumBricksWhichFall).ToString();
    }

    private int GetNumBricksWhichFall(Brick brick)
    {
        var disintegratedBricks = new HashSet<Brick>();
        
        this.AddFallBricksToSet(brick, disintegratedBricks);

        return disintegratedBricks.Count;
    }

    private void AddFallBricksToSet(Brick brick, HashSet<Brick> ignoredBricks)
    {
        foreach (var b in this.GetUniquelySupportedBricks(brick, ignoredBricks))
        {
            this.AddFallBricksToSet(b, ignoredBricks);
            ignoredBricks.Add(b);
        }
    }

    private class Brick
    {
        private int _minZ;
        private int _maxZ;

        private readonly int _id;

        private readonly HashSet<(int x, int y)> _cubes = [];

        public Brick((int x, int y, int z) start, (int x, int y, int z) end, int id)
        {
            this._id = id;
            this._minZ = Math.Min(start.z, end.z);
            this._maxZ = Math.Max(start.z, end.z);

            if (this._minZ == this._maxZ)
            {
                // Not a vertical brick; add all the cubes
                if (start.x == end.x)
                {
                    var startY = Math.Min(start.y, end.y);
                    var endY = Math.Max(start.y, end.y);
                    
                    // Moves in y
                    for (var y = startY; y <= endY; y++)
                    {
                        this._cubes.Add((start.x, y));
                    }
                }
                else
                {
                    // Moves in x
                    var startX = Math.Min(start.x, end.x);
                    var endX = Math.Max(start.x, end.x);
                    
                    for (var x = startX; x <= endX; x++)
                    {
                        this._cubes.Add((x, start.y));
                    }
                }
            }
            else
            {
                this._cubes.Add((start.x, start.y));
            }
        }

        public HashSet<(int x, int y)> GetAllColumns()
        {
            return this._cubes;
        }

        public int GetMinZ()
        {
            return this._minZ;
        }

        public int GetMaxZ()
        {
            return this._maxZ;
        }

        public void FallTo(int fallPos)
        {
            var dz = this._minZ - fallPos;

            this._minZ -= dz;
            this._maxZ -= dz;
        }

        public int GetId()
        {
            return this._id;
        }

        public override string ToString()
        {
            var minX = this._cubes.Select(c => c.x).Min();
            var maxX = this._cubes.Select(c => c.x).Max();

            var minY = this._cubes.Select(c => c.y).Min();
            var maxY = this._cubes.Select(c => c.y).Max();
            
            return $"{this._id}: ({minX}, {minY}, {this._minZ}) -> ({maxX}, {maxY}, {this._maxZ})";
        }
    }
}