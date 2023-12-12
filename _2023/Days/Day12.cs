using System.Collections.Immutable;

using Cache = System.Collections.Generic.Dictionary<(string, System.Collections.Immutable.ImmutableStack<int>), long>;

namespace _2023.Days;

public class Day12 : Day
{
    private readonly List<(string pattern, string numString)> _rows = new();
    
    public Day12() : base(12)
    {
    }

    protected override void ProcessInputLine(string line)
    {
        var parts = line.Split(" ");
        
        this._rows.Add((parts[0], parts[1]));
    }

    protected override void SolvePart1()
    {
        this.Part1Solution = this.Solve(1).ToString();
    }

    protected override void SolvePart2()
    {
        this.Part2Solution = this.Solve(5).ToString();
    }

    private long Solve(int repeat)
    {
        var numCombinations = 0L;

        foreach (var (pattern, groupString) in this._rows)
        {
            var longPattern = Unfold(pattern, '?', repeat);
            var longGroupString = Unfold(groupString, ',', repeat);

            var groups = longGroupString.Split(',').Select(int.Parse);

            numCombinations += Compute(longPattern, ImmutableStack.CreateRange(groups.Reverse()), new());
        }

        return numCombinations;
    }

    private static string Unfold(string st, char join, int unfold)
    {
        return string.Join(join, Enumerable.Repeat(st, unfold));
    }

    private static long Compute(string pattern, ImmutableStack<int> groups, Cache cache)
    {
        if (!cache.ContainsKey((pattern, groups)))
        {
            cache.Add((pattern, groups), Calculate(pattern, groups, cache));
        }

        return cache[(pattern, groups)];
    }

    private static long Calculate(string pattern, ImmutableStack<int> groups, Cache cache)
    {
        return pattern.FirstOrDefault() switch
        {
            '.' => ProcessDot(pattern, groups, cache),
            '?' => ProcessQuestion(pattern, groups, cache),
            '#' => ProcessHash(pattern, groups, cache),
            _ => ProcessEnd(groups)
        };
    }

    private static long ProcessEnd(ImmutableStack<int> groups)
    {
        return groups.Any() ? 0 : 1;
    }

    private static long ProcessDot(string pattern, ImmutableStack<int> groups, Cache cache)
    {
        return Compute(pattern[1..], groups, cache);
    }

    private static long ProcessQuestion(string pattern, ImmutableStack<int> groups, Cache cache)
    {
        return Compute("." + pattern[1..], groups, cache) + Compute("#" + pattern[1..], groups, cache);
    }

    private static long ProcessHash(string pattern, ImmutableStack<int> groups, Cache cache)
    {
        if (!groups.Any())
        {
            return 0;
        }

        var groupSize = groups.Peek();
        groups = groups.Pop();
        
        if (pattern.Length < groupSize)
        {
            return 0;
        }
        else if (pattern[..groupSize].Any(c => c != '#' && c != '?'))
        {
            return 0;
        }
        else if (pattern.Length == groupSize)
        {
            return Compute("", groups, cache);
        }
        else if (pattern[groupSize] == '#')
        {
            return 0;
        }
        else
        {
            return Compute(pattern[(groupSize + 1)..], groups, cache);
        }
    }
}