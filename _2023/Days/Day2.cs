using _2023.Utils;

namespace _2023.Days;

public class Day2() : Day(2)
{
    private readonly HashSet<Day2Game> _games = [];

    protected override void ProcessInputLine(string line)
    {
        this._games.Add(new(line));
    }

    protected override void SolvePart1()
    {
        this.Part1Solution = this._games.Sum(g => g.IsValid() ? g.GetId() : 0).ToString();
    }

    protected override void SolvePart2()
    {
        this.Part2Solution = this._games.Sum(g => g.GetPower()).ToString();
    }
}