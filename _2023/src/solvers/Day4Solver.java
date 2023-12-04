package solvers;

import java.util.Arrays;
import java.util.HashMap;
import java.util.regex.Pattern;
import java.util.stream.Collectors;

public class Day4Solver extends BaseSolver {
    private int TotalPoints = 0;
    private int TotalNumScratchcards = 0;
    private final HashMap<Integer, Integer> ScratchcardCopies = new HashMap<>();

    public Day4Solver() {
        super(4);
    }

    @Override
    protected void ProcessLine(String line) {
        var cardIdPattern = Pattern.compile("Card +([0-9]+): ");
        var cardIdMatcher = cardIdPattern.matcher(line);
        int cardId;
        int numberStartIdx;

        if (cardIdMatcher.find()) {
            cardId = Integer.parseInt(cardIdMatcher.group(1));
            numberStartIdx = cardIdMatcher.end();
        } else {
            System.err.println("Failed to find card ID for line: \"" + line + "\"");
            return;
        }

        var numbers = line.substring(numberStartIdx).split(" \\| ");

        var winningNumbers = Arrays.stream(numbers[0].split(" "))
                .filter(n -> !n.isEmpty())
                .map(Integer::parseInt)
                .collect(Collectors.toSet());

        var ourNumbers = Arrays.stream(numbers[1].split(" "))
                .filter(n -> !n.isEmpty())
                .map(Integer::parseInt)
                .collect(Collectors.toSet());

        ourNumbers.retainAll(winningNumbers);

        var numWinners = ourNumbers.size();

        // Part 1
        var numPoints = numWinners == 0 ? 0 : (int) Math.pow(2, numWinners - 1);
        this.TotalPoints += numPoints;

        // Part 2
        var numOfThisScratchcard = 1 + this.ScratchcardCopies.getOrDefault(cardId, 0);
        this.TotalNumScratchcards += numOfThisScratchcard;

        for (var dId = 1; dId <= numWinners; dId++) {
            var duplicatingCardId = cardId + dId;

            if (this.ScratchcardCopies.containsKey(duplicatingCardId)) {
                this.ScratchcardCopies.replace(duplicatingCardId, this.ScratchcardCopies.get(duplicatingCardId) + numOfThisScratchcard);
            } else {
                this.ScratchcardCopies.put(duplicatingCardId, numOfThisScratchcard);
            }
        }
    }

    @Override
    protected String SolvePart1() {
        return String.valueOf(this.TotalPoints);
    }

    @Override
    protected String SolvePart2() {
        return String.valueOf(this.TotalNumScratchcards);
    }
}
