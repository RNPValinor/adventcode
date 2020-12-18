using System;
using System.Collections.Generic;
using System.Linq;
using _2020.Utils;

namespace _2020.Solvers
{
    public class Day18Solver : ISolver
    {
        public void Solve(string input)
        {
            var equations = input.Split(Environment.NewLine).Select(l => l.Split(' ').ToList());

            var (part1, part2) = SolveEquations(equations);

            Console.WriteLine(part1);
            Console.WriteLine(part2);
        }

        private static (ulong part1, ulong part2) SolveEquations(IEnumerable<IList<string>> equations)
        {
            ulong part1 = 0;
            ulong part2 = 0;

            foreach (var equation in equations)
            {
                var (rootNode1, rootNode2) = GenerateEquationTrees(equation);

                part1 += rootNode1.Calculate();
                part2 += rootNode2.Calculate();
            }

            return (part1, part2);
        }

        private static (EquationNode part1, EquationNode part2) GenerateEquationTrees(IList<string> equation)
        {
            var (lhs1, lhs2) = GetNextNode(equation);

            while (equation.Count > 0)
            {
                var op = GetNextOpCode(equation);
                var (rhs1, rhs2) = GetNextNode(equation);

                switch (op)
                {
                    case "*":
                        lhs1 = new MultNode(lhs1, rhs1);
                        lhs2 = new MultNode(lhs2, rhs2);
                        break;
                    case "+":
                        lhs1 = new AddNode(lhs1, rhs1);

                        if (!lhs2.IsBracketNode && lhs2.GetType() == typeof(MultNode))
                        {
                            var multNode = (MultNode) lhs2;
                            multNode.Rhs = new AddNode(multNode.Rhs, rhs2);
                        }
                        else
                        {
                            lhs2 = new AddNode(lhs2, rhs2);
                        }
                        break;
                }
            }

            return (lhs1, lhs2);
        }

        private static (EquationNode part1, EquationNode part2) GetNextNode(IList<string> equation)
        {
            var eqPart = equation.Pop();

            if (eqPart.StartsWith('('))
            {
                var bracketedEquation = new List<string>();
                
                var bracketCount = eqPart.Count(c => c == '(') - eqPart.Count(c => c == ')');
                eqPart = eqPart.Remove(0, 1);

                while (bracketCount > 0)
                {
                    bracketedEquation.Add(eqPart);
                    eqPart = equation.Pop();
                    bracketCount += eqPart.Count(c => c == '(') - eqPart.Count(c => c == ')');
                }

                eqPart = eqPart.Remove(eqPart.Length - 1, 1);
                bracketedEquation.Add(eqPart);
                var (part1, part2) = GenerateEquationTrees(bracketedEquation);

                part1.IsBracketNode = true;
                part2.IsBracketNode = true;

                return (part1, part2);
            }
            else
            {
                var node = new ValueNode(ulong.Parse(eqPart));
                return (node, node);
            }
        }
        
        private static string GetNextOpCode(IList<string> equation)
        {
            var opCode = equation.Pop();

            switch (opCode)
            {
                case "+":
                case "*":
                    return opCode;
                default:
                    throw new Exception($"Invalid opcode: {opCode}");
            }
        }

        #region Equation Nodes
        private abstract class EquationNode
        {
            public bool IsBracketNode { get; set; }
            
            public abstract ulong Calculate();

            public abstract string Print();
        }

        private class MultNode : EquationNode
        {
            public EquationNode Lhs { get; set; }
            public EquationNode Rhs { get; set; }

            public MultNode(EquationNode lhs = null, EquationNode rhs = null)
            {
                this.Lhs = lhs;
                this.Rhs = rhs;
            }
            
            public override ulong Calculate()
            {
                return this.Lhs.Calculate() * this.Rhs.Calculate();
            }

            public override string Print()
            {
                return $"({this.Lhs.Print()}) * ({this.Rhs.Print()})";
            }
        }
        
        private class AddNode : EquationNode
        {
            public EquationNode Lhs { get; set; }
            public EquationNode Rhs { get; set; }

            public AddNode(EquationNode lhs = null, EquationNode rhs = null)
            {
                this.Lhs = lhs;
                this.Rhs = rhs;
            }
            
            public override ulong Calculate()
            {
                return this.Lhs.Calculate() + this.Rhs.Calculate();
            }

            public override string Print()
            {
                return $"({this.Lhs.Print()}) + ({this.Rhs.Print()})";
            }
        }

        private class ValueNode : EquationNode
        {
            private readonly ulong _value;

            public ValueNode(ulong value)
            {
                this._value = value;
            }
            
            public override ulong Calculate()
            {
                return this._value;
            }

            public override string Print()
            {
                return this._value.ToString();
            }
        }
        #endregion
    }
}