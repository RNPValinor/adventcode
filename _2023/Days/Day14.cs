using System.Text;

namespace _2023.Days;

public class Day14() : Day(14)
{
    private string _initialState = "";

    private int _rowLength;
    private int _columnLength;

    private int _numColumns;
    private int _numRows;

    protected override void ProcessInputLine(string line)
    {
        // These are the same, but done as 2 variables for sanity purposes.
        this._numColumns = line.Length;
        this._rowLength = line.Length;
        
        // Ditto.
        this._numRows++;
        this._columnLength++;
        
        this._initialState += line;
    }

    protected override void SolvePart1()
    {
        var stateAfterRolling = this.RollNorth(this._initialState);
        
        this.Part1Solution = this.GetLoad(stateAfterRolling).ToString();
    }

    protected override void SolvePart2()
    {
        var currentState = this._initialState;
        var previousStates = new Dictionary<string, int> { { currentState, 0 } };

        const int totalNumCycles = 1000000000;

        for (var i = 1; i <= totalNumCycles; i++)
        {
            currentState = this.DoCycle(currentState);

            if (previousStates.TryGetValue(currentState, out var loopStartIdx))
            {
                // Found our loop!
                var loopLength = i - loopStartIdx;

                var numRemainingCycles = totalNumCycles - i;

                var loopIndex = numRemainingCycles % loopLength;

                var actualIndex = loopIndex + loopStartIdx;

                var finalState = previousStates.Single(s => s.Value == actualIndex);

                this.Part2Solution = this.GetLoad(finalState.Key).ToString();

                return;
            }

            previousStates.Add(currentState, i);
        }
    }

    private int GetLoad(string rocks)
    {
        var load = 0;

        for (var i = 0; i < rocks.Length; i++)
        {
            if (rocks[i] is 'O')
            {
                // The weight is the number of rows in total, minus the number of rows
                // we have gone completely past.
                load += this._numRows - (i / this._rowLength);
            }
        }

        return load;
    }

    private string DoCycle(string current)
    {
        return this.RollEast(this.RollSouth(this.RollWest(this.RollNorth(current))));
    }

    private int ConvertCoordsToStateIndex(int x, int y)
    {
        return y * this._numColumns + x;
    }

    private string RollNorth(string current)
    {
        var next = new StringBuilder(current);

        for (var x = 0; x < this._numColumns; x++)
        {
            var fallPos = 0;
            
            for (var y = 0; y < this._columnLength; y++)
            {
                var stateIdx = this.ConvertCoordsToStateIndex(x, y);

                switch (current[stateIdx])
                {
                    case 'O':
                        next[stateIdx] = '.';

                        var fallIdx = this.ConvertCoordsToStateIndex(x, fallPos++);
                        next[fallIdx] = 'O';
                        break;
                    case '#':
                        fallPos = y + 1;
                        break;
                }
            }
        }

        return next.ToString();
    }
    
    private string RollSouth(string current)
    {
        var next = new StringBuilder(current);

        for (var x = 0; x < this._numColumns; x++)
        {
            var fallPos = this._columnLength - 1;
            
            for (var y = this._columnLength - 1; y >= 0; y--)
            {
                var stateIdx = this.ConvertCoordsToStateIndex(x, y);

                switch (current[stateIdx])
                {
                    case 'O':
                        next[stateIdx] = '.';

                        var fallIdx = this.ConvertCoordsToStateIndex(x, fallPos--);
                        next[fallIdx] = 'O';
                        break;
                    case '#':
                        fallPos = y - 1;
                        break;
                }
            }
        }

        return next.ToString();
    }

    private string RollWest(string current)
    {
        var next = new StringBuilder(current);

        for (var y = 0; y < this._numRows; y++)
        {
            var fallPos = 0;
            
            for (var x = 0; x < this._rowLength; x++)
            {
                var stateIdx = this.ConvertCoordsToStateIndex(x, y);

                switch (current[stateIdx])
                {
                    case 'O':
                        next[stateIdx] = '.';

                        var fallIdx = this.ConvertCoordsToStateIndex(fallPos++, y);
                        next[fallIdx] = 'O';
                        break;
                    case '#':
                        fallPos = x + 1;
                        break;
                }
            }
        }

        return next.ToString();
    }

    private string RollEast(string current)
    {
        var next = new StringBuilder(current);

        for (var y = 0; y < this._numRows; y++)
        {
            var fallPos = this._rowLength - 1;
            
            for (var x = this._rowLength - 1; x >= 0; x--)
            {
                var stateIdx = this.ConvertCoordsToStateIndex(x, y);

                switch (current[stateIdx])
                {
                    case 'O':
                        next[stateIdx] = '.';

                        var fallIdx = this.ConvertCoordsToStateIndex(fallPos--, y);
                        next[fallIdx] = 'O';
                        break;
                    case '#':
                        fallPos = x - 1;
                        break;
                }
            }
        }

        return next.ToString();
    }
}