using System.Collections.Concurrent;

namespace _2023.Days;

public class Day15 : Day
{
    private string _input = "";
    
    public Day15() : base(15)
    {
    }

    protected override void ProcessInputLine(string line)
    {
        this._input = line;
    }

    protected override void SolvePart1()
    {
        var hashSum = 0;
        var curHash = 0;

        foreach (var c in this._input)
        {
            if (c is ',')
            {
                hashSum += curHash;
                curHash = 0;
            }
            else
            {
                curHash = HashChar(curHash, c);
            }
        }

        hashSum += curHash;

        this.Part1Solution = hashSum.ToString();
    }

    private static int HashChar(int curHash, char c)
    {
        return ((curHash + c) * 17) % 256;
    }

    protected override void SolvePart2()
    {
        var boxes = new ConcurrentDictionary<int, LinkedList<Lens>>();
        
        var lens = "";
        var lensHash = 0;
        var isAdding = false;
        var focalPower = 0;

        var findingFocalPower = false;

        foreach (var c in this._input)
        {
            switch (c)
            {
                case ',':
                    ProcessLens(lens, lensHash, isAdding, focalPower, boxes);
                    lens = "";
                    lensHash = 0;
                    findingFocalPower = false;
                    break;
                case '-':
                    isAdding = false;
                    break;
                case '=':
                    isAdding = true;
                    findingFocalPower = true;
                    break;
                default:
                    if (findingFocalPower)
                    {
                        focalPower = int.Parse(c.ToString());
                    }
                    else
                    {
                        lens += c;
                        lensHash = HashChar(lensHash, c);
                    }
                    break;
            }
        }
        
        ProcessLens(lens, lensHash, isAdding, focalPower, boxes);

        this.Part2Solution = GetCombinedFocusingPower(boxes).ToString();
    }

    private static void ProcessLens(string label, int boxNum, bool isAdding, int focalPower,
        ConcurrentDictionary<int, LinkedList<Lens>> boxes)
    {
        var box = boxes.GetOrAdd(boxNum, _ => new());

        if (isAdding is false)
        {
            var lens = box.FirstOrDefault(l => l.Label == label);

            if (lens is not null)
            {
                box.Remove(lens);
            }
            
            return;
        }

        var existingLens = box.FirstOrDefault(l => l.Label == label);
        
        if (existingLens != null)
        {
            existingLens.FocalPower = focalPower;
        }
        else
        {
            box.AddLast(new Lens(label, focalPower));
        }
    }

    private static int GetCombinedFocusingPower(IDictionary<int, LinkedList<Lens>> boxes)
    {
        var totalFocusingPower = 0;

        foreach (var (boxNum, box) in boxes)
        {
            var lensNode = box.First;
            var lensIdx = 1;

            while (lensNode is not null)
            {
                var lensFocusingPower = (boxNum + 1) * lensIdx++ * lensNode.Value.FocalPower;
                
                totalFocusingPower += lensFocusingPower;
                
                lensNode = lensNode.Next;
            }
        }
        
        return totalFocusingPower;
    }

    private class Lens
    {
        public string Label { get; }
        
        public int FocalPower { get; set; }

        public Lens(string label, int focalPower)
        {
            this.Label = label;
            this.FocalPower = focalPower;
        }
    }
}