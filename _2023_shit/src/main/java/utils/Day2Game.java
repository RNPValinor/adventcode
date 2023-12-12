package utils;

import java.util.regex.Pattern;

public class Day2Game {
    private int Id;
    private boolean IsValid = true;
    private int Power = 0;

    private int MinRed = 0;
    private int MinGreen = 0;
    private int MinBlue = 0;

    private final int MaxRed = 12;
    private final int MaxGreen = 13;
    private final int MaxBlue = 14;

    public Day2Game(String gameData) {
        this.ProcessGameData(gameData);
    }

    private void ProcessGameData(String gameData) {
        var parts = gameData.split(":");

        this.Id = Integer.parseInt(parts[0].split(" ")[1]);

        var game = parts[1].split(";");

        for (var round : game) {
            this.ProcessRoundData(round);
        }

        this.Power = this.MinRed * this.MinGreen * this.MinBlue;
    }

    private void ProcessRoundData(String roundData) {
        var numRed = 0;
        var numGreen = 0;
        var numBlue = 0;

        var ballPattern = Pattern.compile("([0-9]+) (red|green|blue)");
        var ballMatcher = ballPattern.matcher(roundData);

        while (ballMatcher.find()) {
            var numBalls = Integer.parseInt(ballMatcher.group(1));
            var colour = ballMatcher.group(2);

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
        }

        // Part 1
        this.IsValid &= numRed <= this.MaxRed && numGreen <= this.MaxGreen && numBlue <= this.MaxBlue;

        // Part 2
        this.MinRed = Math.max(this.MinRed, numRed);
        this.MinGreen = Math.max(this.MinGreen, numGreen);
        this.MinBlue = Math.max(this.MinBlue, numBlue);
    }

    public boolean IsValid() {
        return this.IsValid;
    }

    public int GetId() {
        return this.Id;
    }

    public int GetPower() {
        return this.Power;
    }
}


