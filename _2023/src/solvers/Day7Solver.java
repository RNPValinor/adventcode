package solvers;

import java.util.*;

@SuppressWarnings("unused")
public class Day7Solver extends BaseSolver {
    private final Set<Hand> _hands = new HashSet<>();

    public Day7Solver() {
        super(7);
    }

    @Override
    protected void processLine(String line) {
        var cards = line.substring(0, 5);
        var wager = Integer.parseInt(line.substring(6));

        var hand = new Hand(cards, wager);
        this._hands.add(hand);
    }

    @Override
    protected String solvePart1() {
        var sortedHands = this._hands.stream().sorted(new HandComparator(false)).toList();

        var totalWinnings = 0;
        var rank = 1;

        for (var hand : sortedHands) {
            totalWinnings += rank * hand.getWager();
            rank++;
        }

        return String.valueOf(totalWinnings);
    }

    @Override
    protected String solvePart2() {
        var sortedHands = this._hands.stream().sorted(new HandComparator(true)).toList();

        var totalWinnings = 0;
        var rank = 1;

        for (var hand : sortedHands) {
            totalWinnings += rank * hand.getWager();
            rank++;
        }

        return String.valueOf(totalWinnings);
    }

    private static class Hand {
        private final int _wager;
        private final String _cards;
        private final HandType _handType;
        private final HandType _jokerHandType;

        public Hand(String cards, int wager) {
            this._cards = cards;
            this._wager = wager;

            var cardTotals = this.getCardTotals();

            this._handType = this.getHandType(cardTotals, false);
            this._jokerHandType = this.getHandType(cardTotals, true);
        }

        private HashMap<Character, Integer> getCardTotals() {
            var cardTotals = new HashMap<Character, Integer>();

            for (var i = 0; i < this._cards.length(); i++) {
                var card = this._cards.charAt(i);
                cardTotals.merge(card, 1, Integer::sum);
            }

            return cardTotals;
        }

        private HandType getHandType(HashMap<Character, Integer> cardTotals, boolean withJokers) {
            var numJokers = (withJokers && cardTotals.containsKey('J')) ? cardTotals.remove('J') : 0;
            var cardCounts = cardTotals.values();

            if (cardCounts.size() == 5) {
                // All single cards
                return HandType.HIGHCARD;
            }

            if (cardCounts.size() == 4) {
                return HandType.PAIR;
            }

            if (cardCounts.size() == 3) {
                if (cardCounts.stream().anyMatch(c -> (c + numJokers) == 3)) {
                    return HandType.THREEOFAKIND;
                } else {
                    return HandType.TWOPAIR;
                }
            }

            if (cardCounts.size() == 2) {
                if (cardCounts.stream().anyMatch(c -> (c + numJokers) == 4)) {
                    return HandType.FOUROFAKIND;
                } else {
                    return HandType.FULLHOUSE;
                }
            }

            return HandType.FIVEOFAKIND;
        }

        public int getWager() {
            return this._wager;
        }
    }

    private static class HandComparator implements Comparator<Hand> {
        private final HashMap<Character, Integer> _cardValues = new HashMap<>();

        private final boolean _usingJokers;

        public HandComparator(boolean usingJokers) {
            this._usingJokers = usingJokers;
            this.initCardValues();
        }

        private void initCardValues() {
            this._cardValues.put('2', 2);
            this._cardValues.put('3', 3);
            this._cardValues.put('4', 4);
            this._cardValues.put('5', 5);
            this._cardValues.put('6', 6);
            this._cardValues.put('7', 7);
            this._cardValues.put('8', 8);
            this._cardValues.put('9', 9);
            this._cardValues.put('T', 10);
            this._cardValues.put('J', this._usingJokers ? 1 : 11);
            this._cardValues.put('Q', 12);
            this._cardValues.put('K', 13);
            this._cardValues.put('A', 14);
        }

        @Override
        public int compare(Hand o1, Hand o2) {
            var o1HandType = this._usingJokers ? o1._jokerHandType : o1._handType;
            var o2HandType = this._usingJokers ? o2._jokerHandType : o2._handType;

            // If this._hand is lesser than otherHand._hand, -1
            if (o1HandType.ordinal() < o2HandType.ordinal()) {
                return -1;
            } else if (o1HandType.ordinal() > o2HandType.ordinal()) {
                return 1;
            } else {
                return this.compareCardsTo(o1, o2);
            }
        }

        private int compareCardsTo(Hand o1, Hand o2) {
            for (var i = 0; i < o1._cards.length(); i++) {
                var thisCardValue = this._cardValues.get(o1._cards.charAt(i));
                var otherCardValue = this._cardValues.get(o2._cards.charAt(i));

                if (!thisCardValue.equals(otherCardValue)) {
                    return thisCardValue < otherCardValue ? -1 : 1;
                }
            }

            return 0;
        }

        @Override
        public boolean equals(Object obj) {
            return false;
        }
    }

    private enum HandType {
        HIGHCARD,
        PAIR,
        TWOPAIR,
        THREEOFAKIND,
        FULLHOUSE,
        FOUROFAKIND,
        FIVEOFAKIND
    }
}
