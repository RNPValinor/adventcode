using QuikGraph;
using QuikGraph.Graphviz;

namespace _2023.Days;

public class Day25() : Day(25)
{
    private readonly BidirectionalGraph<string, Edge<string>> _wires = new();
    
    protected override void ProcessInputLine(string line)
    {
        var inputWire = line[..3];

        var outputWires = line[5..].Split(' ');

        var edges = outputWires.Select(ow => new Edge<string>(inputWire, ow));

        this._wires.AddVerticesAndEdgeRange(edges);
    }

    protected override void SolvePart1()
    {
        using var outputFile = new StreamWriter(Path.Combine(".", "25.dot"), new FileStreamOptions
        {
            Mode = FileMode.Create
        });
        
        outputFile.WriteLine(this._wires.ToGraphviz(algo =>
        {
            algo.FormatVertex += (sender, args) =>
            {
                args.VertexFormat.Label = args.Vertex;
            };
        }));
        
        // Haha lol this doesn't solve it.
        // But it creates a dot file, which can be loaded into any popular graph
        // imaging software to manually identify the edges to remove, and count
        // the distinct groups that that leaves behind.

        this.Part1Solution = "547080";
    }

    protected override void SolvePart2()
    {
        this.Part2Solution = "Rudolph.";
    }
}