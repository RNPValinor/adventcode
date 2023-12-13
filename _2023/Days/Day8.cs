using _2023.Utils;

namespace _2023.Days;

public class Day8 : Day
{
    private readonly Dictionary<string, string[]> _map = new();
    
    private readonly List<int> _instructions = new();
    
    public Day8() : base(8)
    {
    }

    protected override void ProcessInputLine(string line)
    {
        if (this._instructions.Any() is false)
        {
            foreach (var t in line)
            {
                switch (t) {
                    case 'L':
                        this._instructions.Add(0);
                        break;
                    case 'R':
                        this._instructions.Add(1);
                        break;
                }
            }
        } else if (string.IsNullOrWhiteSpace(line) is false) {
            var node = line[0..3];
            var left = line[7..10];
            var right = line[12..15];

            this._map.Add(node, new[]{left, right});
        }
    }

    protected override void SolvePart1()
    {
        var instructionIndex = 0;
        var curPos = "AAA";
        var numSteps = 0;

        while (curPos != "ZZZ") {
            var instruction = this._instructions[instructionIndex];

            curPos = this._map[curPos][instruction];
            numSteps++;

            instructionIndex = (instructionIndex + 1) % this._instructions.Count;
        }

        this.Part1Solution = numSteps.ToString();
    }

    protected override void SolvePart2()
    {
        var numSteps = 1L;

        var nodes = this._map.Keys.Where(k => k[2] is 'A').ToList();

        var stepIdx = 0;

        while (nodes.Any()) {
            var instruction = this._instructions[stepIdx++ % this._instructions.Count];

            for (var i = 0; i < nodes.Count; i++) {
                nodes[i] = this._map[nodes[i]][instruction];
            }

            var nextNodes = new List<string>(nodes.Count);

            foreach (var node in nodes) {
                if (node[2] == 'Z') {
                    numSteps = Maths.Lcm(numSteps, stepIdx);
                } else {
                    nextNodes.Add(node);
                }
            }

            nodes = nextNodes;
        }

        this.Part2Solution = numSteps.ToString();
    }
}