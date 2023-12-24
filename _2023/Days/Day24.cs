using System.Text.RegularExpressions;

namespace _2023.Days;

using Line = (double x, double y, double z, double dx, double dy, double dz, double a, double b, double c, int idx);

public partial class Day24() : Day(24)
{
    private int _lineIndex;
    
    private readonly List<Line> _lines = [];
    private const long MinIntersect = 200000000000000;
    private const long MaxIntersect = 400000000000000;
    
    protected override void ProcessInputLine(string line)
    {
        var match = LineRegex().Match(line);

        if (!match.Success)
        {
            Console.WriteLine($"Failed to parse line: {line}");
            return;
        }
        
        var x = double.Parse(match.Groups[1].Value);
        var y = double.Parse(match.Groups[2].Value);
        var z = double.Parse(match.Groups[3].Value);
            
        var dx = float.Parse(match.Groups[4].Value);
        var dy = float.Parse(match.Groups[5].Value);
        var dz = float.Parse(match.Groups[6].Value);

        var a = dy;
        var b = -dx;
        var c = a * x + b * y;

        this._lines.Add((x, y, z, dx, dy, dz, a, b, c, idx: this._lineIndex++));
    }

    protected override void SolvePart1()
    {
        var numIntersectionsWithinArea = 0;
        
        for (var i = 0; i < this._lines.Count - 1; i++)
        {
            var l1 = this._lines[i];
            
            for (var j = i + 1; j < this._lines.Count; j++)
            {
                var l2 = this._lines[j];

                var delta = l1.a * l2.b - l2.a * l1.b;

                if (delta is 0)
                {
                    // Parallel
                    continue;
                }

                var xIntersect = (l2.b * l1.c - l1.b * l2.c) / delta;
                var yIntersect = (l1.a * l2.c - l2.a * l1.c) / delta;

                if (WithinIntersectBounds(xIntersect, yIntersect)
                    && OccursInFuture(l1.x, l1.dx, xIntersect)
                    && OccursInFuture(l2.x, l2.dx, xIntersect)
                    && OccursInFuture(l1.y, l1.dy, yIntersect)
                    && OccursInFuture(l2.y, l2.dy, yIntersect))
                {
                    numIntersectionsWithinArea++;
                }
            }
        }

        this.Part1Solution = numIntersectionsWithinArea.ToString();
    }

    private static bool WithinIntersectBounds(double xIntersect, double yIntersect)
    {
        return xIntersect is >= MinIntersect and <= MaxIntersect && yIntersect is >= MinIntersect and <= MaxIntersect;
    }

    private static bool OccursInFuture(double p, double v, double intersect)
    {
        // By inspection v is never 0
        return v < 0 ? intersect < p : intersect > p;
    }

    protected override void SolvePart2()
    {
    }

    [GeneratedRegex("^(-?[0-9]+), (-?[0-9]+), (-?[0-9]+) @ (-?[0-9]+), (-?[0-9]+), (-?[0-9]+)$")]
    private static partial Regex LineRegex();
}