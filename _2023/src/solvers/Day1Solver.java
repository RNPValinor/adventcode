package solvers;

import java.util.ArrayList;
import java.util.List;
import java.util.regex.Pattern;

@SuppressWarnings("unused")
public class Day1Solver extends BaseSolver {
    private final List<Integer> _calibrationValuesPart1 = new ArrayList<>();
    private final List<Integer> _calibrationValuesPart2 = new ArrayList<>();

    public Day1Solver() {
        super(1);
    }

    @Override
    protected void ProcessLine(String line) {
        // For part 1
        var intPattern = Pattern.compile("([0-9])");
        var intMatcher = intPattern.matcher(line);

        Integer firstMatch = null, lastMatch = null;

        while (intMatcher.find()) {
            if (firstMatch == null) {
                firstMatch = Integer.parseInt(intMatcher.group(1));
            }

            lastMatch = Integer.parseInt(intMatcher.group(1));
        }

        if (firstMatch == null) {
            System.err.println("Failed to find matches for part 1. Failed line:");
            System.err.println(line);
            throw new ArithmeticException();
        }

        this._calibrationValuesPart1.add(firstMatch * 10 + lastMatch);

        // For part 2
        var intOrStringPattern = Pattern.compile("([0-9]|one|two|three|four|five|six|seven|eight|nine)");
        var intOrStringMatcher = intOrStringPattern.matcher(line);

        firstMatch = null;
        String candidateLastMatch = null;
        var matchIndex = 0;

        while (intOrStringMatcher.find(matchIndex)) {
            if (firstMatch == null) {
                firstMatch = this.ConvertStringOrIntToInteger(intOrStringMatcher.group(1));
            }

            candidateLastMatch = intOrStringMatcher.group(1);

            matchIndex = intOrStringMatcher.start(1) + 1;
        }

        if (firstMatch == null || candidateLastMatch == null) {
            System.err.println("Failed to find matches for part 2. Failed line:");
            System.err.println(line);
            throw new ArithmeticException();
        }

        lastMatch = this.ConvertStringOrIntToInteger(candidateLastMatch);

        this._calibrationValuesPart2.add(firstMatch * 10 + lastMatch);
    }

    private Integer ConvertStringOrIntToInteger(String stringOrInt) {
        return switch (stringOrInt) {
            case "one" -> 1;
            case "two" -> 2;
            case "three" -> 3;
            case "four" -> 4;
            case "five" -> 5;
            case "six" -> 6;
            case "seven" -> 7;
            case "eight" -> 8;
            case "nine" -> 9;
            default -> Integer.parseInt(stringOrInt);
        };
    }

    @Override
    protected String SolvePart1() {
        var sum = 0;

        for (Integer calibrationValue : this._calibrationValuesPart1) {
            sum += calibrationValue;
        }

        return String.valueOf(sum);
    }

    @Override
    protected String SolvePart2() {
        var sum = 0;

        for (Integer calibrationValue : this._calibrationValuesPart2) {
            sum += calibrationValue;
        }

        // > 55255
        if (sum <= 55255) {
            System.err.println("Answer is too low!");
        }

        return String.valueOf(sum);
    }
}
