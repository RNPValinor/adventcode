using System.Globalization;

namespace _2023.Days;

public class Day18() : Day(18)
{
    private readonly List<(long x, long y)> _simpleVertices = [(0, 0)];
    private (long x, long y) _simpleCurPos = (0, 0);

    private readonly List<(long x, long y)> _complexVertices = [(0, 0)];
    private (long x, long y) _complexCurPos = (0, 0);

    protected override void ProcessInputLine(string line)
    {
        this.ProcessLineBasic(line);
        this.ProcessLineComplex(line);
    }

    private void ProcessLineBasic(string line)
    {
        var distance = int.Parse(string.Join(null, line[2..].TakeWhile(c => c is >= '0' and <= '9')));
        
        this._simpleCurPos = line[0] switch
        {
            'D' => this._simpleCurPos with { y = this._simpleCurPos.y + distance },
            'U' => this._simpleCurPos with { y = this._simpleCurPos.y - distance },
            'R' => this._simpleCurPos with { x = this._simpleCurPos.x + distance },
            'L' => this._simpleCurPos with { x = this._simpleCurPos.x - distance },
            _ => throw new ArgumentOutOfRangeException(nameof(line), line, "Unexpected direction")
        };
        
        this._simpleVertices.Add(this._simpleCurPos);
    }

    private void ProcessLineComplex(string line)
    {
        var distance = int.Parse(line.Substring(line.Length - 7, 5), NumberStyles.HexNumber);

        this._complexCurPos = line[^2] switch
        {
            '0' => this._complexCurPos with { x = this._complexCurPos.x + distance },
            '2' => this._complexCurPos with { x = this._complexCurPos.x - distance },
            '1' => this._complexCurPos with { y = this._complexCurPos.y + distance },
            '3' => this._complexCurPos with { y = this._complexCurPos.y - distance },
            _ => throw new ArgumentException($"Invalid line {line}", nameof(line))
        };
        
        this._complexVertices.Add(this._complexCurPos);
    }

    protected override void SolvePart1()
    {
        this.Part1Solution = GetShapeArea(this._simpleVertices).ToString();
    }

    protected override void SolvePart2()
    {
        this.Part2Solution = GetShapeArea(this._complexVertices).ToString();
    }

    private static long GetShapeArea(IReadOnlyList<(long x, long y)> vertices)
    {
        var perimeter = 0L;
        var area = 0L;

        for (var i = 0; i < vertices.Count - 1; i++)
        {
            var pCur = vertices[i];
            var pNext = vertices[i + 1];
            
            perimeter += Math.Abs(pNext.x - pCur.x) + Math.Abs(pNext.y - pCur.y);

            area += Determinant(pCur, pNext);
        }

        area = Math.Abs(area);

        return (area + perimeter) / 2 + 1;
    }

    private static long Determinant((long x, long y) p1, (long x, long y) p2)
    {
        return p1.x * p2.y - p2.x * p1.y;
    }
}