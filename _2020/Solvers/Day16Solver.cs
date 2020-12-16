using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace _2020.Solvers
{
    public class Day16Solver : ISolver
    {
        private int _lowMin = int.MaxValue;
        private int _lowMax;
        private int _highMin = int.MaxValue;
        private int _highMax;

        private readonly IDictionary<string, ((int, int), (int, int))> _constraints =
            new Dictionary<string, ((int, int), (int, int))>(); 

        private readonly Regex _constraintRegex = new Regex("^([a-z ]+): ([0-9]+)-([0-9]+) or ([0-9]+)-([0-9]+)$");
        
        public void Solve(string input)
        {
            var parts = input.Split($"{Environment.NewLine}{Environment.NewLine}");

            var constraints = parts[0];
            var myTicket = parts[1].Split(Environment.NewLine)[1].Split(",").Select(int.Parse).ToList();
            var otherTickets = parts[2].Split(Environment.NewLine).ToList();

            // First line is the intro text
            otherTickets.RemoveAt(0);

            this.ProcessConstraints(constraints);

            var (validTickets, errorRate) = this.ProcessTickets(otherTickets);

            Console.WriteLine(errorRate);

            validTickets.Add(myTicket);

            var orderedConstraints = this.CalculateConstraintOrder(validTickets);

            ulong ticketChecksum = 1;

            for (var i = 0; i < orderedConstraints.Count; i++)
            {
                if (orderedConstraints[i].StartsWith("departure"))
                {
                    ticketChecksum *= (ulong) myTicket[i];
                }
            }

            Console.WriteLine(ticketChecksum);
        }

        private void ProcessConstraints(string constraints)
        {
            foreach (var constraint in constraints.Split(Environment.NewLine))
            {
                var match = _constraintRegex.Match(constraint);

                if (!match.Success)
                {
                    throw new ArgumentException($"Bad constraint: {constraint}");
                }
                
                var groups = match.Groups;

                var constraintName = groups[1].Value;
                var lowMin = int.Parse(groups[2].Value);
                var lowMax = int.Parse(groups[3].Value);
                var highMin = int.Parse(groups[4].Value);
                var highMax = int.Parse(groups[5].Value);
                
                this._constraints.Add(constraintName, ((lowMin, lowMax), (highMin, highMax)));

                this._lowMin = Math.Min(this._lowMin, lowMin);
                this._lowMax = Math.Max(this._lowMax, lowMax);
                this._highMin = Math.Min(this._highMin, highMin);
                this._highMax = Math.Max(this._highMax, highMax);
            }
        }

        private (HashSet<IList<int>> validTickets, int errorRate)  ProcessTickets(IEnumerable<string> tickets)
        {
            var validTickets = new HashSet<IList<int>>();
            var errorRate = 0;

            foreach (var ticket in tickets)
            {
                var fields = ticket.Split(",").Select(int.Parse).ToList();
                var ticketErrorRate = fields
                    .Where(fv => fv < this._lowMin || fv > this._highMax)
                    .Sum();

                if (ticketErrorRate == 0)
                {
                    validTickets.Add(fields);
                }
                else
                {
                    errorRate += ticketErrorRate;
                }
            }

            return (validTickets, errorRate);
        }

        private IList<string> CalculateConstraintOrder(IReadOnlyCollection<IList<int>> tickets)
        {
            var potentialConstraints = new List<HashSet<string>>();

            for (var i = 0; i < tickets.First().Count; i++)
            {
                // Initialise every field to have every potential constraint
                potentialConstraints.Add(new HashSet<string> (this._constraints.Keys));
            }

            foreach (var ticket in tickets)
            {
                for (var i = 0; i < ticket.Count; i++)
                {
                    var fieldValue = ticket[i];

                    var fieldConstraints = potentialConstraints[i].Where(c =>
                    {
                        var ((lowMin, lowMax), (highMin, highMax)) = this._constraints[c];

                        return (fieldValue >= lowMin && fieldValue <= lowMax) ||
                               (fieldValue >= highMin && fieldValue <= highMax);
                    }).ToHashSet();

                    if (fieldConstraints.Count == 1)
                    {
                        // We know the constraint for this field; it can't be on any others
                        foreach (var constraints in potentialConstraints)
                        {
                            constraints.Remove(fieldConstraints.Single());
                        }
                    }

                    potentialConstraints[i] = fieldConstraints;
                }
                
                if (potentialConstraints.TrueForAll(c => c.Count == 1))
                {
                    break;
                }
            }

            while (!potentialConstraints.TrueForAll(c => c.Count == 1))
            {
                foreach (var fieldConstraints in potentialConstraints)
                {
                    if (fieldConstraints.Count == 1)
                    {
                        foreach (var otherConstraints in potentialConstraints.Where(otherConstraints => otherConstraints != fieldConstraints))
                        {
                            otherConstraints.Remove(fieldConstraints.Single());
                        }
                    }
                }
            }

            return potentialConstraints.Select(set => set.Single()).ToList();
        }
    }
}