using System.Text;
using System.Text.RegularExpressions;

namespace _2023.Days;

public partial class Day6 : Day
{
    private readonly Queue<int> _times = new();

    private long _bigTime;

    private int _recordBeatingMult = 1;
    private int _numberOfBigWins = -1;
    
    public Day6() : base(6)
    {
    }

    protected override void ProcessInputLine(string line)
    {
        var numberMatch = NumberRegex().Match(line);

        var bigNumberBuilder = new StringBuilder();

        if (this._times.Any() is false) {
            // Times
            while (numberMatch.Success) {
                var group = numberMatch.Groups[1].Value;
                this._times.Enqueue(int.Parse(group));
                bigNumberBuilder.Append(group);
                numberMatch = numberMatch.NextMatch();
            }

            this._bigTime = long.Parse(bigNumberBuilder.ToString());
        } else {
            // Records
            while (numberMatch.Success) {
                var group = numberMatch.Groups[1].Value;

                bigNumberBuilder.Append(group);

                var record = int.Parse(group);
                var time = this._times.Dequeue();

                this._recordBeatingMult *= FindWinningWays(time, record);

                numberMatch = numberMatch.NextMatch();
            }

            var bigRecord = long.Parse(bigNumberBuilder.ToString());

            this._numberOfBigWins = FindWinningWays(this._bigTime, bigRecord);
        }
    }
    
    private static int FindWinningWays(long totalTime, long record) {
        // Find all solutions of t for (totalTime - t) * t > record
        // t^2 - t*totalTime + record = 0
        // t = totalTime +- Sqrt(-totalTime ^ 2 - 4 * 1 * record) / 2

        var sqrt = Math.Sqrt(Math.Pow(-totalTime, 2) - 4 * record);

        var isPerfectSquare = (Math.Floor(sqrt) - sqrt) == 0;

        var minTime = (int) Math.Ceiling((totalTime - sqrt) / 2);
        var maxTime = (int) Math.Floor((totalTime + sqrt) / 2);

        if (isPerfectSquare) {
            // In this case the start and end times only match
            // the record, rather than beating it.
            minTime++;
            maxTime--;
        }

        return maxTime - minTime + 1;
    }

    protected override void SolvePart1()
    {
        this.Part1Solution = this._recordBeatingMult.ToString();
    }

    protected override void SolvePart2()
    {
        this.Part2Solution = this._numberOfBigWins.ToString();
    }

    [GeneratedRegex("([0-9]+)")]
    private static partial Regex NumberRegex();
}