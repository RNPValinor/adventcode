using System.Text.RegularExpressions;

namespace _2023.Days;

public partial class Day1 : Day
{
    private readonly IList<int> _part1CalibrationValues = new List<int>();
    private readonly IList<int> _part2CalibrationValues = new List<int>();
    
    public Day1() : base(1)
    {
    }

    protected override void ProcessInputLine(string line)
    {
        // For part 1
        var matches = IntRegex().Matches(line);

        int firstMatch = -1, lastMatch = -1;

        foreach (var match in matches)
        {
            var matchStr = match?.ToString();
            
            if (matchStr is null)
            {
                break;
            }
            
            if (firstMatch is -1)
            {
                firstMatch = int.Parse(matchStr);
            }

            lastMatch = int.Parse(matchStr);
        }

        this._part1CalibrationValues.Add(firstMatch * 10 + lastMatch);

        // For part 2
        firstMatch = ConvertStringOrIntToInteger(IntOrStringRegex().Match(line).Value);
        lastMatch = ConvertStringOrIntToInteger(LastIntOrStringRegex().Match(line).Value);

        this._part2CalibrationValues.Add(firstMatch * 10 + lastMatch);
    }
    
    private static int ConvertStringOrIntToInteger(string stringOrInt)
    {
        return stringOrInt switch
        {
            "one" => 1,
            "two" => 2,
            "three" => 3,
            "four" => 4,
            "five" => 5,
            "six" => 6,
            "seven" => 7,
            "eight" => 8,
            "nine" => 9,
            _ => int.Parse(stringOrInt)
        };
    }

    protected override void SolvePart1()
    {
        this.Part1Solution = this._part1CalibrationValues.Sum().ToString();
    }

    protected override void SolvePart2()
    {
        this.Part2Solution = this._part2CalibrationValues.Sum().ToString();
    }

    [GeneratedRegex("([0-9])")]
    private static partial Regex IntRegex();
    
    [GeneratedRegex("([0-9]|one|two|three|four|five|six|seven|eight|nine)")]
    private static partial Regex IntOrStringRegex();
    
    [GeneratedRegex("([0-9]|one|two|three|four|five|six|seven|eight|nine)", RegexOptions.RightToLeft)]
    private static partial Regex LastIntOrStringRegex();
}