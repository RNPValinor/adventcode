using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace _2023.Days;

public partial class Day4 : Day
{
    private int _totalPoints;
    
    private int _totalNumScratchcards;
    
    private readonly ConcurrentDictionary<int, int> _scratchcardCopies = new();
    
    public Day4() : base(4)
    {
    }

    protected override void ProcessInputLine(string line)
    {
        int cardId;
        int numberStartIdx;

        var cardIdMatch = CardIdRegex().Match(line);

        if (cardIdMatch.Success) {
            cardId = int.Parse(cardIdMatch.Groups[1].Value);
            numberStartIdx = cardIdMatch.Index + cardIdMatch.Length;
        } else {
            throw new ArgumentException($"Failed to find card ID for line: \"{line}\"");
        }

        var numbers = line[numberStartIdx..].Split(" | ");

        var winningNumbers = numbers[0].Split(" ")
            .Where(n => n.Length > 0)
            .Select(int.Parse)
            .ToHashSet();

        var ourWinningNumbers = numbers[1].Split(" ")
            .Where(n => n.Length > 0)
            .Select(int.Parse)
            .Intersect(winningNumbers)
            .ToHashSet();

        var numWinners = ourWinningNumbers.Count;

        // Part 1
        var numPoints = numWinners == 0 ? 0 : (int) Math.Pow(2, numWinners - 1);
        this._totalPoints += numPoints;

        // Part 2
        var numOfThisScratchcard = 1 + this._scratchcardCopies.GetValueOrDefault(cardId, 0);
        this._totalNumScratchcards += numOfThisScratchcard;

        for (var dId = 1; dId <= numWinners; dId++) {
            var duplicatingCardId = cardId + dId;

            this._scratchcardCopies.AddOrUpdate(duplicatingCardId, numOfThisScratchcard,
                (_, numCards) => numCards + numOfThisScratchcard);
        }
    }

    protected override void SolvePart1()
    {
        this.Part1Solution = this._totalPoints.ToString();
    }

    protected override void SolvePart2()
    {
        this.Part2Solution = this._totalNumScratchcards.ToString();
    }

    [GeneratedRegex("Card +([0-9]+): ")]
    private static partial Regex CardIdRegex();
}