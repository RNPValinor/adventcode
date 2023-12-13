using System.Text;

namespace _2023.Days;

public class Day3 : Day
{
    private List<int> _lastLineSymbols = new();
    private List<PartNumber> _unmatchedLastLineNumbers = new();

    private List<PartNumber> _lastLineNumbers = new();
    private List<PartNumber> _secondLastLineNumbers = new();
    private List<int> _lastLineCogs = new();

    private int _sumOfValidIds;
    private int _sumOfGearRatios;
    
    public Day3() : base(3)
    {
    }

    protected override void ProcessInputLine(string line)
    {
        var currentNumber = new StringBuilder();

        var unmatchedNumbersOnLine = new List<PartNumber>();
        var symbolsOnLine = new List<int>();

        var numbersOnLine = new List<PartNumber>();
        var cogsOnLine = new List<int>();

        for (var x = 0; x < line.Length; x++) {
            var c = line[x];

            if (c is >= '0' and <= '9') {
                // Part of a number;
                currentNumber.Append(c);
            } else {
                if (c != '.') {
                    symbolsOnLine.Add(x);
                    this.CheckIfSymbolIsAdjacentToLastLineNumber(x);
                }

                if (c == '*') {
                    cogsOnLine.Add(x);
                }

                if (currentNumber.Length > 0) {
                    this.ProcessNumber(currentNumber, x, unmatchedNumbersOnLine, symbolsOnLine, numbersOnLine);
                }
            }
        }

        if (currentNumber.Length > 0) {
            this.ProcessNumber(currentNumber, line.Length, unmatchedNumbersOnLine, symbolsOnLine, numbersOnLine);
        }

        // Check cogs on the last line to see if any are actual gears
        this.CheckForCogsOnLastLine(numbersOnLine);

        this._secondLastLineNumbers = this._lastLineNumbers;
        this._lastLineNumbers = numbersOnLine;
        this._lastLineCogs = cogsOnLine;

        this._lastLineSymbols = symbolsOnLine;
        this._unmatchedLastLineNumbers = unmatchedNumbersOnLine;
    }
    
    private void ProcessNumber(StringBuilder numberStr, int x, ICollection<PartNumber> unmatchedNumbersOnLine, List<int> currentLineSymbols, ICollection<PartNumber> numbersOnLine) {
        var number = int.Parse(numberStr.ToString());
        var partNumber = new PartNumber(number, x - numberStr.Length, x - 1);
        numberStr.Remove(0, numberStr.Length);

        if (this.IsNumberAdjacentToSymbolOnLastLine(partNumber)) {
            this._sumOfValidIds += number;
        } else if (currentLineSymbols.Any() && currentLineSymbols[^1] == partNumber.StartX - 1) {
            this._sumOfValidIds += number;
        } else if (currentLineSymbols.Any() && currentLineSymbols[^1] == partNumber.EndX + 1) {
            this._sumOfValidIds += number;
        } else {
            unmatchedNumbersOnLine.Add(partNumber);
        }

        numbersOnLine.Add(partNumber);
    }

    private void CheckIfSymbolIsAdjacentToLastLineNumber(int x) {
        foreach (var partNumber in this._unmatchedLastLineNumbers) {
            if (partNumber.StartX > x + 1) {
                break;
            }

            if (partNumber.EndX >= x - 1) {
                // Overlap
                this._sumOfValidIds += partNumber.Id;
            }
        }
    }

    private bool IsNumberAdjacentToSymbolOnLastLine(PartNumber partNumber) {
        return this._lastLineSymbols
                .Any(x => x >= partNumber.StartX - 1 && x <= partNumber.EndX + 1);
    }

    private void CheckForCogsOnLastLine(List<PartNumber> numbersOnLine) {
        foreach (var cogX in this._lastLineCogs) {
            // Find all adjacent numbers
            var adjacentNumbers = new List<PartNumber>();

            FindXOverlapsInList(this._secondLastLineNumbers, cogX, adjacentNumbers);
            FindXOverlapsInList(this._lastLineNumbers, cogX, adjacentNumbers);
            FindXOverlapsInList(numbersOnLine, cogX, adjacentNumbers);

            if (adjacentNumbers.Count == 2) {
                // Exactly 2 adjacent; it's a gear
                this._sumOfGearRatios += adjacentNumbers[0].Id * adjacentNumbers[1].Id;
            }
        }
    }

    private static void FindXOverlapsInList(List<PartNumber> numbersInLine, int cogX, ICollection<PartNumber> adjacentNumbers) {
        foreach (var partNumber in numbersInLine) {
            if (partNumber.StartX > cogX + 1) {
                break;
            }

            if (partNumber.StartX <= cogX + 1 && partNumber.EndX >= cogX - 1) {
                adjacentNumbers.Add(partNumber);
            }
        }
    }

    protected override void SolvePart1()
    {
        this.Part1Solution = this._sumOfValidIds.ToString();
    }

    protected override void SolvePart2()
    {
        this.Part2Solution = this._sumOfGearRatios.ToString();
    }
    
    private class PartNumber {
        public readonly int Id;
        public readonly int StartX;
        public readonly int EndX;

        public PartNumber(int id, int startX, int endX) {
            this.Id = id;
            this.StartX = startX;
            this.EndX = endX;
        }
    }
}