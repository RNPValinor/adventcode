using System.Drawing;

namespace _2023.Days;

public class Day11 : Day
{
    private readonly List<Point> _expandedGalaxies = new();
    private readonly List<Point> _superExpandedGalaxies = new();

    private int _numEmptyRows;
    private readonly HashSet<int> _nonEmptyColumns = new();

    private int _numColumns;
    private int _y;
    
    public Day11() : base(11)
    {
    }

    protected override void ProcessInputLine(string line)
    {
        var noGalaxies = true;

        for (var x = 0; x < line.Length; x++)
        {
            if (line[x] is not '#') continue;
            
            noGalaxies = false;
            this._nonEmptyColumns.Add(x);
            this._expandedGalaxies.Add(new(x, this._y + this._numEmptyRows));
            this._superExpandedGalaxies.Add(new(x, this._y + this._numEmptyRows * 999999));
        }

        if (noGalaxies) {
            this._numEmptyRows++;
        }

        this._numColumns = line.Length;
        this._y++;
    }

    protected override void SolvePart1()
    {
        this.DoXExpansion();

        var totalDistances = GetTotalGalaxyDistances(this._expandedGalaxies);

        this.Part1Solution = totalDistances.ToString();
    }
    
    private void DoXExpansion() {
        var cumulativeNumEmptyColumns = new int[this._numColumns];
        cumulativeNumEmptyColumns[0] = 0;

        for (var x = 1; x < this._numColumns; x++) {
            cumulativeNumEmptyColumns[x] = cumulativeNumEmptyColumns[x - 1] + (this._nonEmptyColumns.Contains(x - 1) ? 0 : 1);
        }
        
        for (var i = 0; i < this._expandedGalaxies.Count; i++) {
            var g1 = this._expandedGalaxies[i];
            var g2 = this._superExpandedGalaxies[i];

            var numToExpandBy = cumulativeNumEmptyColumns[g1.X];

            g1.Offset(numToExpandBy, 0);
            this._expandedGalaxies[i] = g1;
            
            g2.Offset(numToExpandBy * 999999, 0);
            this._superExpandedGalaxies[i] = g2;
        }
    }
    
    private static long GetTotalGalaxyDistances(IReadOnlyList<Point> galaxies) {
        var totalDistances = 0L;
        
        for (var i = 0; i < galaxies.Count; i++) {
            var g1 = galaxies[i];

            for (var j = i + 1; j < galaxies.Count; j++) {
                var g2 = galaxies[j];

                totalDistances += GetDistanceBetween(g1, g2);
            }
        }

        return totalDistances;
    }
    
    private static long GetDistanceBetween(Point p1, Point p2)
    {
        var dx = Math.Abs(p1.X - p2.X);
        var dy = Math.Abs(p1.Y - p2.Y);

        var distance = dx + dy;

        return distance;
    }

    protected override void SolvePart2()
    {
        var totalDistances = GetTotalGalaxyDistances(this._superExpandedGalaxies);

        this.Part2Solution = totalDistances.ToString();
    }
}