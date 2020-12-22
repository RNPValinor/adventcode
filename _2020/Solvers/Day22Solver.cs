using System;
using System.Collections.Generic;
using System.Linq;

namespace _2020.Solvers
{
    public class Day22Solver : ISolver
    {
        public void Solve(string input)
        {
            var handStrs = input.Split($"{Environment.NewLine}{Environment.NewLine}");

            var hand1 = ProcessHandStr(handStrs[0]).ToList();
            var hand2 = ProcessHandStr(handStrs[1]).ToList();

            var part1WinningScore = PlayCombat(new Queue<int>(hand1), new Queue<int>(hand2));

            Console.WriteLine(part1WinningScore);

            var recursiveHand1 = new Queue<int>(hand1);
            var recursiveHand2 = new Queue<int>(hand2);

            PlayRecursiveCombat(recursiveHand1, recursiveHand2);

            var part2WinningScore = GetScore(recursiveHand1);

            Console.WriteLine(part2WinningScore);
        }

        private static long PlayCombat(Queue<int> hand1, Queue<int> hand2)
        {
            while (hand1.Count > 0 && hand2.Count > 0)
            {
                var hand1Card = hand1.Dequeue();
                var hand2Card = hand2.Dequeue();

                if (hand1Card > hand2Card)
                {
                    hand1.Enqueue(hand1Card);
                    hand1.Enqueue(hand2Card);
                }
                else
                {
                    hand2.Enqueue(hand2Card);
                    hand2.Enqueue(hand1Card);
                }
            }

            return hand1.Count > 0 ? GetScore(hand1) : GetScore(hand2);
        }

        private static string HandsToString(IEnumerable<int> hand1, IEnumerable<int> hand2)
        {
            return $"[{string.Join(',', hand1)}][{string.Join(',', hand2)}]";
        }

        private static Player PlayRecursiveCombat(Queue<int> hand1, Queue<int> hand2)
        {
            var seenHands = new HashSet<string>();

            while (hand1.Count > 0 && hand2.Count > 0)
            {
                var handId = HandsToString(hand1, hand2);

                if (!seenHands.Add(handId))
                {
                    return Player.One;
                }
                
                var hand1Card = hand1.Dequeue();
                var hand2Card = hand2.Dequeue();

                if (hand1.Count >= hand1Card && hand2.Count >= hand2Card)
                {
                    var newHand1 = new Queue<int>(hand1.Take(hand1Card));
                    var newHand2 = new Queue<int>(hand2.Take(hand2Card));

                    var recursiveWinner = PlayRecursiveCombat(newHand1, newHand2);
                    
                    if (recursiveWinner == Player.One)
                    {
                        hand1.Enqueue(hand1Card);
                        hand1.Enqueue(hand2Card);
                    }
                    else
                    {
                        hand2.Enqueue(hand2Card);
                        hand2.Enqueue(hand1Card);
                    }
                }
                else
                {
                    if (hand1Card > hand2Card)
                    {
                        hand1.Enqueue(hand1Card);
                        hand1.Enqueue(hand2Card);
                    }
                    else
                    {
                        hand2.Enqueue(hand2Card);
                        hand2.Enqueue(hand1Card);
                    }
                }
            }

            var winner = hand1.Count > 0 ? Player.One : Player.Two;

            return winner;
        }

        private static IEnumerable<int> ProcessHandStr(string handStr)
        {
            var hand = new LinkedList<int>();

            foreach (var line in handStr.Split(Environment.NewLine))
            {
                if (int.TryParse(line, out var card))
                {
                    hand.AddLast(card);
                }
            }

            return hand;
        }

        private static long GetScore(IReadOnlyCollection<int> hand)
        {
            var multiplier = hand.Count;

            return hand.Aggregate<int, long>(0, (current, card) => current + card * multiplier--);
        }

        private enum Player
        {
            One, Two
        }
    }
}