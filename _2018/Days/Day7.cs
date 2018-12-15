using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using _2018.Utils;

namespace _2018.Days
{
    public class Day7 : Day
    {
        private IDictionary<char, Node> InitialiseNodes()
        {
            var nodes = new Dictionary<char, Node>();

            foreach (var line in QuestionLoader.Load(7).Split(Environment.NewLine))
            {
                var lineData = line.Split(' ');

                var source = lineData[1][0];
                var target = lineData[7][0];

                if (nodes.ContainsKey(target))
                {
                    nodes[target].IncomingNodes.Add(source);
                }
                else
                {
                    var node = new Node(target);
                    node.IncomingNodes.Add(source);
                    
                    nodes.Add(target, node);
                }

                if (!nodes.ContainsKey(source))
                {
                    nodes.Add(source, new Node(source));
                }
            }

            return nodes;
        }
        
        protected override void DoPart1()
        {
            var nodes = this.InitialiseNodes();
            var order = new StringBuilder();

            while (nodes.Count > 0)
            {
                Node nextNode = null;

                foreach (var node in nodes.Values)
                {
                    if (node.IncomingNodes.Count == 0)
                    {
                        if (nextNode == null || node.Id < nextNode.Id)
                        {
                            nextNode = node;                            
                        }
                    }
                }

                if (nextNode == null)
                {
                    ConsoleUtils.WriteColouredLine($"Found no next node!", ConsoleColor.Red);
                    return;
                }

                foreach (var node in nodes.Values)
                {
                    node.IncomingNodes.Remove(nextNode.Id);
                }

                nodes.Remove(nextNode.Id);
                order.Append(nextNode.Id);
            }
            
            ConsoleUtils.WriteColouredLine($"Order is {order}", ConsoleColor.Cyan);
        }

        protected override void DoPart2()
        {
            var nodes = this.InitialiseNodes();
            var time = 0;
            var workingNodes = new List<Node>();

            while (nodes.Count > 0)
            {
                var sourceNodes = new SortedList<char, Node>();

                foreach (var node in nodes.Values)
                {
                    if (node.IncomingNodes.Count == 0)
                    {
                        sourceNodes.Add(node.Id, node);
                    }
                }

                foreach (var entry in sourceNodes)
                {
                    var node = entry.Value;

                    if (!workingNodes.Contains(node))
                    {
                        node.FinishTime = time + 60 + (node.Id + 1 - 'A');
                    
                        workingNodes.Add(node);
                    }
                }
                
                workingNodes.Sort(new NodeSorter());

                // Remove the first node, advance time to when it finishes
                var firstNode = workingNodes.First();

                time = firstNode.FinishTime;
                workingNodes.RemoveAt(0);
                this.RemoveNode(nodes, firstNode.Id);
            }

            var colour = ConsoleColor.Cyan;

            if (time != 1133)
            {
                colour = ConsoleColor.Red;
            }
            
            ConsoleUtils.WriteColouredLine($"Total time taken is {time}", colour);
        }

        private void RemoveNode(IDictionary<char, Node> nodes, char id)
        {
            nodes.Remove(id);

            foreach (var node in nodes.Values)
            {
                node.IncomingNodes.Remove(id);
            }
        }

        private class Node
        {
            public Node(char id)
            {
                this.Id = id;
            }
            
            public readonly char Id;
            public readonly HashSet<char> IncomingNodes = new HashSet<char>();
            public int FinishTime;
        }

        private class NodeSorter : IComparer<Node>
        {
            public int Compare(Node x, Node y)
            {
                return x.FinishTime == y.FinishTime ? x.Id.CompareTo(y.Id) : x.FinishTime.CompareTo(y.FinishTime);
            }
        }
    }
}