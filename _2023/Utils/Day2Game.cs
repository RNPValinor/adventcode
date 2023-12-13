using System.Text.RegularExpressions;

namespace _2023.Utils;

public partial class Day2Game
{
    private int _id;
    private bool _isValid = true;
    private int _power;

    private int _minRed;
    private int _minGreen;
    private int _minBlue;

    private const int MaxRed = 12;
    private const int MaxGreen = 13;
    private const int MaxBlue = 14;

    public Day2Game(string gameData)
    {
        this.ProcessGameData(gameData);
    }

    private void ProcessGameData(string gameData)
    {
        var parts = gameData.Split(":");

        this._id = int.Parse(parts[0].Split(" ")[1]);

        var game = parts[1].Split(";");

        foreach (var round in game) {
            this.ProcessRoundData(round);
        }

        this._power = this._minRed * this._minGreen * this._minBlue;
    }

    private void ProcessRoundData(string roundData)
    {
        var numRed = 0;
        var numGreen = 0;
        var numBlue = 0;

        var match = BallRegex().Match(roundData);

        while (match.Success)
        {
            var numBalls = int.Parse(match.Groups[1].Value);
            var colour = match.Groups[2].Value;

            switch (colour) {
                case "red":
                    numRed = numBalls;
                    break;
                case "green":
                    numGreen = numBalls;
                    break;
                case "blue":
                    numBlue = numBalls;
                    break;
            }

            match = match.NextMatch();
        }

        // Part 1
        this._isValid &= numRed <= MaxRed && numGreen <= MaxGreen && numBlue <= MaxBlue;

        // Part 2
        this._minRed = Math.Max(this._minRed, numRed);
        this._minGreen = Math.Max(this._minGreen, numGreen);
        this._minBlue = Math.Max(this._minBlue, numBlue);
    }

    public bool IsValid() {
        return this._isValid;
    }

    public int GetId() {
        return this._id;
    }

    public int GetPower() {
        return this._power;
    }

    [GeneratedRegex("([0-9]+) (red|green|blue)")]
    private static partial Regex BallRegex();
}