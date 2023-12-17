using _2023.Utils;

namespace _2023.Days;

public class Day13() : Day(13)
{
    private readonly List<Day13Pattern> _patterns = [];
    private Day13Pattern? _currentPattern;

    protected override void ProcessInputLine(string line)
    {
        if (string.IsNullOrEmpty(line))
        {
            this._currentPattern = null;
            return;
        }
        
        if (this._currentPattern is null)
        {
            this._currentPattern = new();
            this._patterns.Add(this._currentPattern);
        }
        
        this._currentPattern.AddRow(line);
    }

    protected override void SolvePart1()
    {
        var summary = 0;

        foreach (var pattern in this._patterns)
        {
            var numVertical = pattern.GetNumColumnsLeftOfReflection();

            if (numVertical > 0)
            {
                summary += numVertical;
            }
            else
            {
                summary += pattern.GetNumRowsAboveReflection() * 100;
            }
        }
        
        this.Part1Solution = summary.ToString();
    }

    protected override void SolvePart2()
    {
        var summary = 0;

        foreach (var pattern in this._patterns)
        {
            var numVertical = pattern.GetNumColumnsLeftOfReflection(true);

            if (numVertical > 0)
            {
                summary += numVertical;
            }
            else
            {
                summary += pattern.GetNumRowsAboveReflection(true) * 100;
            }
        }
        
        this.Part2Solution = summary.ToString();
    }
}