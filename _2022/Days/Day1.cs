using CodeBits;

namespace _2022.Days;

public class Day1 : Day
{
    private readonly OrderedCollection<int> _topBackpacks = new(); 
    
    public Day1() : base(1)
    {
    }

    protected override void ProcessInputLine(string line)
    {
        var curElfBackpackTotal = 0;

        switch (line?.Length)
        {
            case null:
            case 0:
                this.MaybeAddBackpack(curElfBackpackTotal);
                curElfBackpackTotal = 0;
                break;
            default:
                var calorieCount = int.Parse(line);
                curElfBackpackTotal += calorieCount;
                break;
        }
    }

    public override void SolvePart1()
    {
        this.Part1Solution = this._topBackpacks.Last().ToString();
    }

    private void MaybeAddBackpack(int calorieCount)
    {
        if (this._topBackpacks.Count < 3)
        {
            this._topBackpacks.Add(calorieCount);
        }
        else if (this._topBackpacks.First() < calorieCount)
        {
            this._topBackpacks.RemoveAt(0);
            this._topBackpacks.Add(calorieCount);
        }
    }
    
    public override void SolvePart2()
    {
        this.Part2Solution = this._topBackpacks.Sum().ToString();
    }
}