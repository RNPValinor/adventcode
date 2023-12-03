package solvers;

import java.util.ArrayList;
import java.util.List;

public class Day3Solver extends BaseSolver {
    private List<Integer> lastLineSymbols = new ArrayList<>();
    private List<PartNumber> unmatchedLastLineNumbers = new ArrayList<>();

    private List<PartNumber> lastLineNumbers = new ArrayList<>();
    private List<PartNumber> secondLastLineNumbers = new ArrayList<>();
    private List<Integer> lastLineCogs = new ArrayList<>();
    
    private int SumOfValidIds = 0;
    private int SumOfGearRatios = 0;

    public Day3Solver() {
        super(3);
    }

    @Override
    protected void ProcessLine(String line) {
        StringBuilder currentNumber = new StringBuilder();

        var unmatchedNumbersOnLine = new ArrayList<PartNumber>();
        var symbolsOnLine = new ArrayList<Integer>();

        var numbersOnLine = new ArrayList<PartNumber>();
        var cogsOnLine = new ArrayList<Integer>();

        for (var x = 0; x < line.length(); x++) {
            var c = line.charAt(x);

            if (c >= '0' && c <= '9') {
                // Part of a number;
                currentNumber.append(c);
            } else {
                if (c != '.') {
                    symbolsOnLine.add(x);
                    this.CheckIfSymbolIsAdjacentToLastLineNumber(x);
                }

                if (c == '*') {
                    cogsOnLine.add(x);
                }

                if (!currentNumber.isEmpty()) {
                    this.ProcessNumber(currentNumber, x, unmatchedNumbersOnLine, symbolsOnLine, numbersOnLine);
                }
            }
        }

        if (!currentNumber.isEmpty()) {
            this.ProcessNumber(currentNumber, line.length(), unmatchedNumbersOnLine, symbolsOnLine, numbersOnLine);
        }

        // Check cogs on the last line to see if any are actual gears
        this.CheckForCogsOnLastLine(numbersOnLine);

        this.secondLastLineNumbers = this.lastLineNumbers;
        this.lastLineNumbers = numbersOnLine;
        this.lastLineCogs = cogsOnLine;

        this.lastLineSymbols = symbolsOnLine;
        this.unmatchedLastLineNumbers = unmatchedNumbersOnLine;
    }

    private void ProcessNumber(StringBuilder numberStr, int x, List<PartNumber> unmatchedNumbersOnLine, List<Integer> currentLineSymbols, List<PartNumber> numbersOnLine) {
        var number = Integer.parseInt(numberStr.toString());
        var partNumber = new PartNumber(number, x - numberStr.length(), x - 1);
        numberStr.delete(0, numberStr.length());

        if (this.IsNumberAdjacentToSymbolOnLastLine(partNumber)) {
            this.SumOfValidIds += number;
        } else if (!currentLineSymbols.isEmpty() && currentLineSymbols.get(currentLineSymbols.size() - 1) == partNumber.StartX - 1) {
            this.SumOfValidIds += number;
        } else if (!currentLineSymbols.isEmpty() && currentLineSymbols.get(currentLineSymbols.size() - 1) == partNumber.EndX + 1) {
            this.SumOfValidIds += number;
        } else {
            unmatchedNumbersOnLine.add(partNumber);
        }

        numbersOnLine.add(partNumber);
    }

    private void CheckIfSymbolIsAdjacentToLastLineNumber(int x) {
        for (PartNumber partNumber : this.unmatchedLastLineNumbers) {
            if (partNumber.StartX > x + 1) {
                break;
            }

            if (partNumber.EndX >= x - 1) {
                // Overlap
                this.SumOfValidIds += partNumber.Id;
            }
        }
    }

    private boolean IsNumberAdjacentToSymbolOnLastLine(PartNumber partNumber) {
        return this.lastLineSymbols
            .stream()
            .anyMatch(x -> x >= partNumber.StartX - 1 && x <= partNumber.EndX + 1);
    }

    private void CheckForCogsOnLastLine(List<PartNumber> numbersOnLine) {
        for (Integer cogX : this.lastLineCogs) {
            // Find all adjacent numbers
            var adjacentNumbers = new ArrayList<PartNumber>();

            findXOverlapsInList(this.secondLastLineNumbers, cogX, adjacentNumbers);
            findXOverlapsInList(this.lastLineNumbers, cogX, adjacentNumbers);
            findXOverlapsInList(numbersOnLine, cogX, adjacentNumbers);

            if (adjacentNumbers.size() == 2) {
                // Exactly 2 adjacent; it's a gear
                var gearRatio = adjacentNumbers.get(0).Id * adjacentNumbers.get(1).Id;
                this.SumOfGearRatios += gearRatio;
            }
        }
    }

    private void findXOverlapsInList(List<PartNumber> numbersInLine, Integer cogX, ArrayList<PartNumber> adjacentNumbers) {
        for (PartNumber partNumber : numbersInLine) {
            if (partNumber.StartX > cogX + 1) {
                break;
            }

            if (partNumber.StartX <= cogX + 1 && partNumber.EndX >= cogX - 1) {
                adjacentNumbers.add(partNumber);
            }
        }
    }


    @Override
    protected String SolvePart1() {
        return String.valueOf(this.SumOfValidIds);
    }

    @Override
    protected String SolvePart2() {
        return String.valueOf(this.SumOfGearRatios);
    }

    private static class PartNumber {
        public int Id;

        public int StartX;

        public int EndX;

        public PartNumber(int id, int startX, int endX) {
            this.Id = id;
            this.StartX = startX;
            this.EndX = endX;
        }
    }
}
