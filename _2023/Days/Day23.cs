using System.Collections.Immutable;
using _2023.Utils;
using QuikGraph;

using ExploredPath = ((int x, int y) pos, _2023.Utils.Directions dir, int startVertex, int endVertex, int edgeLength);

namespace _2023.Days;

public class Day23() : Day(23)
{
    private readonly ImmutableHashSet<Directions> _possibleDirections =
    [
        Directions.North,
        Directions.South,
        Directions.East,
        Directions.West
    ];
    
    private char[] _trails = [];

    private int _rowLength;
    private int _columnLength;

    private int _startX;

    private int _nextVertexId = 1;
    private int _endVertexId;
    private readonly Dictionary<(int x, int y), int> _vertexIds = new();
    private readonly Dictionary<(int from, int to), int> _edgeCosts = new();
    
    private readonly BidirectionalGraph<int, Edge<int>> _trailGraph = new();

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

    protected override void SolvePart1()
    {
        this.ConstructGraph(SlopeType.Slippery);

        this.Part1Solution = this.GetLongestDirectedPath(0, 0, []).ToString();
    }
    
    protected override void SolvePart2()
    {
        // this.ConstructGraph(SlopeType.Dry);
        //
        // this.Part2Solution = this.GetLongestUndirectedPath(0, 0, []).ToString();
    }

    private void ConstructGraph(SlopeType slopeType)
    {
        this._trailGraph.Clear();
        this._vertexIds.Clear();
        this._edgeCosts.Clear();
        this._endVertexId = 0;
        this._nextVertexId = 1;
        
        this._trailGraph.AddVertex(0);
        this._vertexIds.Add((this._startX, 0), 0);

        ExploredPath startPath = ((this._startX, 1), Directions.South, startVertex: 0, endVertex: 0, edgeLength: 1);

        var activePaths = new HashSet<ExploredPath> { startPath };

        while (activePaths.Count is not 0)
        {
            activePaths = this.DoPathStep(activePaths, slopeType);
        }
    }

    private HashSet<ExploredPath> DoPathStep(HashSet<ExploredPath> paths, SlopeType slopeType)
    {
        var nextPaths = new HashSet<ExploredPath>();
            
        foreach (var path in paths)
        {
            if (path.pos.y == this._columnLength - 1)
            {
                if (this._endVertexId is 0)
                {
                    this._endVertexId = this._nextVertexId++;
                    this._vertexIds.Add(path.pos, this._endVertexId);
                    this._trailGraph.AddVertex(this._endVertexId);
                }

                this.AddPathToGraph(path);
            }
            else
            {
                var newPaths = this.GetNextActivePaths(path)
                    .Where(p => this.IsPossiblePath(p, slopeType))
                    .ToList();

                if (newPaths.Count > 1)
                {
                    if (slopeType is SlopeType.Dry && this._vertexIds.ContainsKey(path.pos))
                    {
                        // Already seen this vertex; don't do any new paths from here. As we've already done them.
                        newPaths.Clear();
                    }
                    
                    this.AddPathToGraph(path);
                }
                    
                nextPaths.UnionWith(newPaths);
            }
        }

        return nextPaths;
    }

    private void AddPathToGraph(ExploredPath path)
    {
        if (this._vertexIds.TryGetValue(path.pos, out var vertexId) is false)
        {
            vertexId = this._nextVertexId++;
            this._vertexIds.Add(path.pos, vertexId);
            this._trailGraph.AddVertex(vertexId);
        }
        
        if (this._trailGraph.ContainsEdge(path.startVertex, vertexId))
        {
            return;
        }
        
        this._edgeCosts.Add((path.startVertex, vertexId), path.edgeLength);
        this._trailGraph.AddEdge(new(path.startVertex, vertexId));
    }
    
    private HashSet<ExploredPath> GetNextActivePaths(ExploredPath path)
    {
        var nextPaths = new HashSet<ExploredPath>();

        foreach (var d in this._possibleDirections)
        {
            // No stepping backwards
            if (IsOppositeDirection(path.dir, d)) continue;
            
            var nextStep = StepInDirection(path.pos, d);

            var thingThere = this._trails[this.ConvertCoordsToGridIndex(nextStep)];
            
            // No stepping into trees
            if (thingThere is '#') continue;
                
            nextPaths.Add(path with { pos = nextStep, dir = d, edgeLength = path.edgeLength + 1 });
        }

        return nextPaths;
    }

    private static bool IsOppositeDirection(Directions d1, Directions d2)
    {
        return d1 switch
        {
            Directions.North => d2 is Directions.South,
            Directions.South => d2 is Directions.North,
            Directions.East => d2 is Directions.West,
            Directions.West => d2 is Directions.East,
            _ => throw new ArgumentOutOfRangeException(nameof(d1), d1, null)
        };
    }

    private static (int x, int y) StepInDirection((int x, int y) pos, Directions dir)
    {
        return dir switch
        {
            Directions.North => pos with { y = pos.y - 1 },
            Directions.South => pos with { y = pos.y + 1 },
            Directions.East => pos with { x = pos.x + 1 },
            Directions.West => pos with { x = pos.x - 1 },
            _ => pos
        };
    }

    private bool IsPossiblePath(ExploredPath path, SlopeType slopeType)
    {
        var thingHere = this._trails[this.ConvertCoordsToGridIndex(path.pos)];

        if (slopeType is SlopeType.Dry || thingHere is '.')
        {
            return true;
        }

        return IsAppropriateSlope(thingHere, path.dir);
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

    private int GetLongestDirectedPath(int cur, int length, HashSet<int> visited)
    {
        if (cur == this._endVertexId)
        {
            return length;
        }

        if (this._trailGraph.TryGetOutEdges(cur, out var edges))
        {
            return edges
                .Where(e => visited.Contains(e.Target) is false)
                .Select(e => this.GetLongestDirectedPath(e.Target, length + this.GetEdgeCost(e),
                    [..visited, e.Target]))
                .Max();
        }

        return 0;
    }

    private int GetEdgeCost(IEdge<int> edge)
    {
        return this._edgeCosts.TryGetValue((edge.Source, edge.Target), out var cost) ? cost : this._edgeCosts[(edge.Target, edge.Source)];
    }

    private int GetLongestUndirectedPath(int cur, int length, HashSet<int> visited)
    {
        if (cur == this._endVertexId)
        {
            return length;
        }

        List<int> lengths = [0];

        if (this._trailGraph.TryGetOutEdges(cur, out var outEdges))
        {
            lengths.AddRange(outEdges
                .Where(e => visited.Contains(e.Target) is false)
                .Select(e => this.GetLongestUndirectedPath(e.Target, length + this.GetEdgeCost(e), [..visited, e.Target])));
        }

        if (this._trailGraph.TryGetInEdges(cur, out var inEdges))
        {
            lengths.AddRange(inEdges
                .Where(e => visited.Contains(e.Source) is false)
                .Select(e => this.GetLongestUndirectedPath(e.Source, length + this.GetEdgeCost(e), [..visited, e.Source])));
        }

        return lengths.Max();
    }

    private enum SlopeType
    {
        Slippery,
        Dry
    }
}