namespace _2022.Days;

public class Day2 : Day
{
    private readonly IList<Match> _matches = new List<Match>();
    
    public Day2() : base(2)
    {
    }

    protected override void ProcessInputLine(string line)
    {
        this._matches.Add(ParseMatch(line));
    }

    public override void SolvePart1()
    {
        this.Part1Solution = this._matches.Sum(m => m.GetScore()).ToString();
    }

    public override void SolvePart2()
    {
        this.Part2Solution = this._matches.Sum(m => m.GetMark2Score()).ToString();
    }

    private static Match ParseMatch(string match)
    {
        var hands = match.Split(' ');

        return new Match(ParseHand(hands[0]), ParseHand(hands[1]));
    }

    private static Hand ParseHand(string hand)
    {
        switch (hand)
        {
            case "A":
            case "X":
                return Hand.Rock;
            case "B":
            case "Y":
                return Hand.Paper;
            case "C":
            case "Z":
                return Hand.Scissors;
        }

        throw new ArgumentOutOfRangeException(nameof(hand), $"Invalid hand provided {hand}");
    }

    private class Match
    {
        private readonly Hand _player1;
        private readonly Hand _player2;
        
        public Match(Hand player1, Hand player2)
        {
            this._player1 = player1;
            this._player2 = player2;
        }

        public int GetScore()
        {
            var winScore = ((int)this._player2 - (int)this._player1) switch
            {
                -2 => 6,
                -1 => 0,
                0 => 3,
                1 => 6,
                2 => 0,
                _ => throw new IndexOutOfRangeException("Bad math")
            };

            return (int)this._player2 + winScore;
        }

        public int GetMark2Score()
        {
            switch (this._player2)
            {
                case Hand.Rock:
                    // Lose
                    return ((int)this._player1 - 1 + 2) % 3 + 1; 
                case Hand.Paper:
                    // Draw
                    return (int)this._player1 + 3;
                case Hand.Scissors:
                    // Win
                    return ((int)this._player1 - 1 + 1) % 3 + 1 + 6;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private enum Hand
    {
        Rock = 1,
        Paper = 2,
        Scissors = 3
    }
}