namespace _2023.Utils;

public class Day13Pattern
{
    private readonly IList<string> _columns = new List<string>();
    private readonly IList<string> _rows = new List<string>();

    private int _lastColumnIdx;
    private int _lastRowIdx = -1;

    public void AddRow(string row)
    {
        this._rows.Add(row);
        this._lastColumnIdx = row.Length - 1;
        this._lastRowIdx++;

        for (var i = 0; i < row.Length; i++)
        {
            if (this._columns.Count <= i)
            {
                this._columns.Add(row[i].ToString());
            }
            else
            {
                this._columns[i] += row[i];
            }
        }
    }

    public int GetNumColumnsLeftOfReflection(bool canFlip = false)
    {
        // Working from the left and right, find the largest reflection from one of those sides
        // which extends to one of the edges.
        // Use the fact that it must have an even number of mirrors within the reflection to
        // eliminate some values immediately.
        for (var dx = 0; dx < this._columns.Count - 1; dx++)
        {
            // Check left edge
            if (ReflectsFromStart(this._lastColumnIdx - dx, this._columns, canFlip))
            {
                return (this._lastColumnIdx - dx + 1) / 2;
            }
            
            if (ReflectsFromEnd(dx, this._columns, canFlip))
            {
                var halfOfReflection = (this._lastColumnIdx - dx + 1) / 2;
                return halfOfReflection + dx;
            }
        }
        
        return 0;
    }

    public int GetNumRowsAboveReflection(bool canFlip = false)
    {
        for (var dy = 0; dy < this._rows.Count - 1; dy++)
        {
            // Check left edge
            if (ReflectsFromStart(this._lastRowIdx - dy, this._rows, canFlip))
            {
                return (this._lastRowIdx - dy + 1) / 2;
            }
            
            if (ReflectsFromEnd(dy, this._rows, canFlip))
            {
                var halfOfReflection = (this._lastRowIdx - dy + 1) / 2;
                return halfOfReflection + dy;
            }
        }

        return 0;
    }

    private static bool ReflectsFromStart(int endIdx, IList<string> lines, bool canFlip)
    {
        return endIdx % 2 == 1 && VerifyLineReflection(0, endIdx, lines, canFlip);
    }

    private static bool ReflectsFromEnd(int startIdx, IList<string> lines, bool canFlip)
    {
        var endIdx = lines.Count - 1;

        return (startIdx + endIdx) % 2 == 1 && VerifyLineReflection(startIdx, endIdx, lines, canFlip);
    }

    private static bool VerifyLineReflection(int l, int r, IList<string> lines, bool canFlip)
    {
        while (l < r)
        {
            if (lines[l] != lines[r])
            {
                if (canFlip && CanSingleCellBeFlipped(lines[l], lines[r]))
                {
                    canFlip = false;
                }
                else
                {
                    return false;
                }
            }

            l++;
            r--;
        }

        return canFlip is false && l != r;
    }

    private static bool CanSingleCellBeFlipped(string l1, string l2)
    {
        var foundDifference = false;
        
        for (var i = 0; i < l1.Length; i++)
        {
            if (l1[i] != l2[i])
            {
                if (foundDifference)
                {
                    return false;
                }

                foundDifference = true;
            }
        }

        return foundDifference;
    }
}