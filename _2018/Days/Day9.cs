using System;
using System.Collections.Generic;
using System.Linq;
using _2018.Utils;

namespace _2018.Days
{
    public class Day9 : Day
    {
        private const int NumPlayers = 412;
        private const int NumMarbles = 71646;
        
        protected override void DoPart1()
        {
            var maxScore = GetMaxScore(NumMarbles);
            
            ConsoleUtils.WriteColouredLine($"Max elf score is {maxScore}", ConsoleColor.Cyan);
        }
        
        protected override void DoPart2()
        {
            var maxScore = GetMaxScore(NumMarbles * 100);
            
            ConsoleUtils.WriteColouredLine($"Max elf score (x100 round) is {maxScore}", ConsoleColor.Cyan);
        }

        private static long GetMaxScore(int numMarbles)
        {
            var playerScores = new long[NumPlayers];
            var board = new LinkedList<int>();

            board.AddFirst(0);
            var currentMarble = board.First;

            for (var i = 1; i <= numMarbles; i++)
            {
                var (score, newMarble) = AddMarble(i, currentMarble, board);

                playerScores[i % NumPlayers] += score;
                currentMarble = newMarble;
            }

            return playerScores.Max();
        }
        
        private static (int score, LinkedListNode<int> newMarble) AddMarble(int value, LinkedListNode<int> currentMarble, LinkedList<int> board)
        {
            if (value % 23 == 0)
            {
                var score = value;
                var marbleToRemove = currentMarble;

                for (var i = 0; i < 7; i++)
                {
                    marbleToRemove = marbleToRemove.Previous ?? board.Last;
                }

                score += marbleToRemove.Value;
                currentMarble = marbleToRemove.Next ?? board.First;

                board.Remove(marbleToRemove);

                return (score, currentMarble);
            }
            else
            {
                var insertAfter = currentMarble.Next ?? board.First;

                board.AddAfter(insertAfter, value);

                return (0, insertAfter.Next);
            }
        }
    }
}