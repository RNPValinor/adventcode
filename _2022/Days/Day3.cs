namespace _2022.Days;

public class Day3 : Day
{
    private readonly List<Backpack> _backpacks = new();
    
    public Day3() : base(3)
    {
    }

    protected override void ProcessInputLine(string line)
    {
        this._backpacks.Add(new Backpack(line));
    }

    public override void SolvePart1()
    {
        Console.WriteLine($"a: {(int)'a'}");
        Console.WriteLine($"A: {(int)'A'}");
        var prioritySum = this._backpacks.Sum(backpack => this.ConvertItemToPriority(backpack.CommonItem));

        this.Part1Solution = prioritySum.ToString();
    }

    public override void SolvePart2()
    {
        var prioritySum = 0;

        for (var i = 0; i < this._backpacks.Count; i += 3)
        {
            var elf1 = this._backpacks[i];
            var elf2 = this._backpacks[i + 1];
            var elf3 = this._backpacks[i + 2];

            var commonItem = elf1.AllItems.Intersect(elf2.AllItems).Intersect(elf3.AllItems).Single();

            prioritySum += this.ConvertItemToPriority(commonItem);
        }

        this.Part2Solution = prioritySum.ToString();
    }

    private int ConvertItemToPriority(char item)
    {
        if (item <= 'Z')
        {
            // Uppercase
            return item - 'A' + 27;
        }
        else
        {
            // Lowercase
            return item - 'a' + 1;
        }
    }

    private class Backpack
    {
        public readonly HashSet<char> AllItems = new();
        public readonly char CommonItem;

        public Backpack(string contents)
        {
            var halfContentsLength = contents.Length / 2;
            var compartment1 = new HashSet<char>();
            var compartment2 = new HashSet<char>();
            
            for (var i = 0; i < halfContentsLength; i++)
            {
                var item1 = contents[i];
                compartment1.Add(item1);

                var item2 = contents[i + halfContentsLength];
                compartment2.Add(item2);

                if (compartment1.Contains(item2))
                {
                    this.CommonItem = item2;
                }
                else if (compartment2.Contains(item1))
                {
                    this.CommonItem = item1;
                }

                this.AllItems.Add(item1);
                this.AllItems.Add(item2);
            }
        }
    }
}