using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace _2020.Solvers
{
    public class Day5Solver : ISolver
    {
        public void Solve(string input)
        {
            var lines = input.Split(Environment.NewLine);
            var maxIndex = 0;

            var fullSeats = new Dictionary<int, HashSet<int>>();

            foreach (var line in lines)
            {
                decimal minRow = 0;
                decimal maxRow = 127;
                decimal minColumn = 0;
                decimal maxColumn = 7;

                var row = -1;
                var column = -1;
                
                foreach (var c in line)
                {
                    var rowDiff = maxRow - minRow;
                    var colDiff = maxColumn - minColumn;
                    
                    switch (c)
                    {
                        case 'F':
                            if (rowDiff == 1)
                            {
                                row = (int) minRow;
                            }
                            else
                            {
                                maxRow -= Math.Ceiling(rowDiff / 2);
                            }
                            break;
                        case 'B':
                            if (rowDiff == 1)
                            {
                                row = (int) maxRow;
                            }
                            else
                            {
                                minRow += Math.Ceiling(rowDiff / 2);
                            }
                            break;
                        case 'L':
                            if (colDiff == 1)
                            {
                                column = (int) minColumn;
                            }
                            else
                            {
                                maxColumn -= Math.Ceiling(colDiff / 2);
                            }
                            break;
                        case 'R':
                            if (colDiff == 1)
                            {
                                column = (int) maxColumn;
                            }
                            else
                            {
                                minColumn += Math.Ceiling(colDiff / 2);
                            }
                            break;
                        default:
                            throw new ArgumentException($"Invalid character found: {c}");
                    }
                }

                if (row == -1)
                {
                    throw new ArgumentException($"Row could not be determined. Got [{minRow}..{maxRow}] from {line}");
                }

                if (column == -1)
                {
                    throw new ArgumentException($"Column could not be determined. Got [{minColumn}..{maxColumn}] from {line}");
                }

                if (fullSeats.TryGetValue(row, out var fullColumns))
                {
                    fullColumns.Add(column);
                }
                else
                {
                    fullSeats.Add(row, new HashSet<int> { column });
                }

                maxIndex = Math.Max(maxIndex, row * 8 + column);
            }

            var mySeat = new Point();
            HashSet<int> myRowsSeats = null;

            foreach (var (row, columns) in fullSeats)
            {
                if (columns.Count != 7) continue;
                myRowsSeats = columns;
                mySeat.X = row;
                break;
            }

            if (myRowsSeats == null)
            {
                throw new ArgumentException($"Could not find a row with an empty seat.");
            }

            for (; mySeat.Y <= 7; mySeat.Y++)
            {
                if (myRowsSeats.Contains(mySeat.Y)) continue; // This seat is full
                break; // This seat is empty
            }

            Console.WriteLine(maxIndex);
            Console.WriteLine(mySeat.X * 8 + mySeat.Y);
        }
    }
}