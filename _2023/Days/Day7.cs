using System.Collections.Concurrent;

namespace _2023.Days;

public class Day7 : Day
{
    private readonly HashSet<Hand> _hands = new();
    
    public Day7() : base(7)
    {
    }

    protected override void ProcessInputLine(string line)
    {
        var cards = line[..5];
        var wager = int.Parse(line[6..]);

        var hand = new Hand(cards, wager);
        this._hands.Add(hand);
    }

    protected override void SolvePart1()
    {
        this.Part1Solution = this.GetTotalWinnings(false).ToString();
    }

    protected override void SolvePart2()
    {
        this.Part2Solution = this.GetTotalWinnings(true).ToString();
    }

    private int GetTotalWinnings(bool usingJokers)
    {
        var sortedHands = this._hands.Order(new HandComparator(usingJokers));

        var totalWinnings = 0;
        var rank = 1;

        foreach (var hand in sortedHands) {
            totalWinnings += rank * hand.GetWager();
            rank++;
        }

        return totalWinnings;
    }
    
    private class Hand {
        private readonly int _wager;
        public readonly string Cards;
        public readonly HandType HandType;
        public readonly HandType JokerHandType;

        public Hand(string cards, int wager) {
            this.Cards = cards;
            this._wager = wager;

            var cardTotals = this.GetCardTotals();

            this.HandType = GetHandType(cardTotals, false);
            this.JokerHandType = GetHandType(cardTotals, true);
        }

        private IDictionary<char, int> GetCardTotals() {
            var cardTotals = new ConcurrentDictionary<char, int>();

            foreach (var card in this.Cards)
            {
                cardTotals.AddOrUpdate(card, 1, (_, numExisting) => numExisting + 1);
            }

            return cardTotals;
        }

        private static HandType GetHandType(IDictionary<char, int> cardTotals, bool withJokers)
        {
            var numJokers = 0;
            
            if (withJokers)
            {
                cardTotals.Remove('J', out numJokers);
            }
            
            var cardCounts = cardTotals.Values.ToList();

            return cardCounts.Count switch
            {
                5 => HandType.HighCard,
                4 => HandType.Pair,
                3 => cardCounts.Any(c => c + numJokers == 3) ? HandType.ThreeOfAKind : HandType.TwoPair,
                2 => cardCounts.Any(c => c + numJokers == 4) ? HandType.FourOfAKind : HandType.FullHouse,
                _ => HandType.FiveOfAKind
            };
        }

        public int GetWager() {
            return this._wager;
        }
    }

    private class HandComparator : IComparer<Hand> {
        private readonly Dictionary<char, int> _cardValues = new();

        private readonly bool _usingJokers;

        public HandComparator(bool usingJokers) {
            this._usingJokers = usingJokers;
            this.InitCardValues();
        }

        private void InitCardValues() {
            this._cardValues.Add('2', 2);
            this._cardValues.Add('3', 3);
            this._cardValues.Add('4', 4);
            this._cardValues.Add('5', 5);
            this._cardValues.Add('6', 6);
            this._cardValues.Add('7', 7);
            this._cardValues.Add('8', 8);
            this._cardValues.Add('9', 9);
            this._cardValues.Add('T', 10);
            this._cardValues.Add('J', this._usingJokers ? 1 : 11);
            this._cardValues.Add('Q', 12);
            this._cardValues.Add('K', 13);
            this._cardValues.Add('A', 14);
        }

        public int Compare(Hand? o1, Hand? o2)
        {
            if (o1 is null) return -1;
            if (o2 is null) return 1;
            if (o1 == o2) return 0;
            
            var o1HandType = this._usingJokers ? o1.JokerHandType : o1.HandType;
            var o2HandType = this._usingJokers ? o2.JokerHandType : o2.HandType;

            // If this._hand is lesser than otherHand._hand, -1
            if (o1HandType < o2HandType) {
                return -1;
            } else if (o1HandType > o2HandType) {
                return 1;
            } else {
                return this.CompareCardsTo(o1, o2);
            }
        }

        private int CompareCardsTo(Hand o1, Hand o2) {
            for (var i = 0; i < o1.Cards.Length; i++) {
                var thisCardValue = this._cardValues[o1.Cards[i]];
                var otherCardValue = this._cardValues[o2.Cards[i]];

                if (thisCardValue != otherCardValue) {
                    return thisCardValue < otherCardValue ? -1 : 1;
                }
            }

            return 0;
        }
    }

    private enum HandType {
        HighCard,
        Pair,
        TwoPair,
        ThreeOfAKind,
        FullHouse,
        FourOfAKind,
        FiveOfAKind
    }
}