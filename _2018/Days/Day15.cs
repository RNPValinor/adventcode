using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Priority_Queue;
using _2018.Utils;

namespace _2018.Days
{
    public class Day15 : Day
    {
        private readonly HashSet<Point> _emptySquares = new HashSet<Point>();
        private readonly Dictionary<Point, Person> _people = new Dictionary<Point, Person>();

        private void InitialiseMap()
        {
            this._emptySquares.Clear();
            this._people.Clear();

            var lines = QuestionLoader.Load(15).Split(Environment.NewLine);

            for (byte y = 0; y < lines.Length; y++)
            {
                var line = lines[y];

                for (byte x = 0; x < line.Length; x++)
                {
                    var point = new Point(x, y);
                    
                    switch (line[x])
                    {
                        case '.':
                            this._emptySquares.Add(point);
                            break;
                        case 'G':
                            this._people.Add(point, new Goblin());
                            this._emptySquares.Add(point);
                            break;
                        case 'E':
                            this._people.Add(point, new Elf());
                            this._emptySquares.Add(point);
                            break;
                    }
                }
            }
        }

        private Point Move(Person person, Point pos)
        {
            // Start by adding p to visited
            var visited = new HashSet<Point> {pos};

            var nodes = new FastPriorityQueue<VisitedNode>(64);

            var adjacentNodes = GetAdjacentNodes(pos);

            foreach (var node in adjacentNodes)
            {
                node.InitialVel = new Point(node.Pos.X - pos.X, node.Pos.Y - pos.Y);
                nodes.Enqueue(node, node.GetPriority());
                visited.Add(node.Pos);
            }

            while (nodes.Any())
            {
                var nextNodes = new FastPriorityQueue<VisitedNode>(64);

                while (nodes.Any())
                {
                    var node = nodes.Dequeue();

                    visited.Add(node.Pos);

                    if (this._people.TryGetValue(node.Pos, out var enemy) && enemy.GetType() != person.GetType())
                    {
                        // Enemy in this pos - stop searching
                        var newPos = new Point(pos.X, pos.Y);
                        newPos.Offset(node.InitialVel);
                        return newPos;
                    }

                    // Non-empty square - cannot move here, do nothing
                    if (this._people.ContainsKey(node.Pos) || !this._emptySquares.Contains(node.Pos)) continue;
                    
                    // This square is empty - add all adjacent non-visited nodes
                    adjacentNodes = GetAdjacentNodes(node.Pos);

                    foreach (var adjNode in adjacentNodes)
                    {
                        if (!visited.Contains(adjNode.Pos))
                        {
                            adjNode.InitialVel = node.InitialVel;
                            visited.Add(adjNode.Pos);
                            nextNodes.Enqueue(adjNode, adjNode.GetPriority());
                        }
                        else if (nextNodes.Contains(adjNode))
                        {
                            var queueNode = nextNodes.Single(nds => nds.Equals(adjNode));

                            if (adjNode.InitialVel.Y < queueNode.InitialVel.Y)
                            {
                                queueNode.InitialVel = adjNode.InitialVel;
                            }
                            else if (adjNode.InitialVel.Y == queueNode.InitialVel.Y &&
                                     adjNode.InitialVel.X < queueNode.InitialVel.X)
                            {
                                queueNode.InitialVel = adjNode.InitialVel;
                            }
                        }
                    }
                }

                nodes = nextNodes;
            }

            return pos;
        }

        private static HashSet<VisitedNode> GetAdjacentNodes(Point p)
        {
            return new HashSet<VisitedNode>
            {
                new VisitedNode(p.X, p.Y + 1),
                new VisitedNode(p.X, p.Y - 1),
                new VisitedNode(p.X + 1, p.Y),
                new VisitedNode(p.X - 1, p.Y)
            };
        }

        private class VisitedNode : FastPriorityQueueNode
        {
            public VisitedNode(int x, int y)
            {
                this.Pos = new Point(x, y);
            }

            public int GetPriority()
            {
                return this.Pos.Y * 100 + this.Pos.X;
            }
            
            public Point Pos { get; }
            
            public Point InitialVel { get; set; }

            public override bool Equals(object obj)
            {
                return obj.GetType() == typeof(VisitedNode) && this.Pos.Equals(((VisitedNode) obj).Pos);
            }

            public override int GetHashCode()
            {
                return this.Pos.GetHashCode();
            }
        }

        private (Person adjacentEnemy, Point enemyPos) GetAdjacentEnemy(Type personType, Point pos)
        {
            Person adjacentEnemy = null;
            var enemyPos = pos;
            
            var pUp = new Point(pos.X, pos.Y - 1);
            var pLeft = new Point(pos.X - 1, pos.Y);
            var pRight = new Point(pos.X + 1, pos.Y);
            var pDown = new Point(pos.X, pos.Y + 1);

            if (this._people.TryGetValue(pUp, out var upEnemy) && personType != upEnemy.GetType())
            {
                enemyPos = pUp;
                adjacentEnemy = upEnemy;
            }

            if (this._people.TryGetValue(pLeft, out var leftEnemy) && personType != leftEnemy.GetType())
            {
                if (adjacentEnemy == null || leftEnemy.GetHitpoints() < adjacentEnemy.GetHitpoints())
                {
                    enemyPos = pLeft;
                    adjacentEnemy = leftEnemy;
                }
            }
            
            if (this._people.TryGetValue(pRight, out var rightEnemy) && personType != rightEnemy.GetType())
            {
                if (adjacentEnemy == null || rightEnemy.GetHitpoints() < adjacentEnemy.GetHitpoints())
                {
                    enemyPos = pRight;
                    adjacentEnemy = rightEnemy;
                }
            }
            
            if (this._people.TryGetValue(pDown, out var downEnemy) && personType != downEnemy.GetType())
            {
                if (adjacentEnemy == null || downEnemy.GetHitpoints() < adjacentEnemy.GetHitpoints())
                {
                    enemyPos = pDown;
                    adjacentEnemy = downEnemy;
                }
            }

            return (adjacentEnemy, enemyPos);
        }

        private (int result, int elvenLosses) DoBattle()
        {
            this.InitialiseMap();

            var numElves = this._people.Values.Count(person => person.GetType() == typeof(Elf));
            var numGoblins = this._people.Values.Count(person => person.GetType() == typeof(Goblin));
            var numRounds = 0;

            var startingNumElves = numElves;

            while (numElves > 0 && numGoblins > 0)
            {
                var peoplePoints = this._people.Keys.ToList();
                peoplePoints.Sort((pp1, pp2) => pp1.Y == pp2.Y ? pp1.X - pp2.X : pp1.Y - pp2.Y);

                var i = 0;
                
                foreach (var personPos in peoplePoints)
                {
                    i++;
                    
                    if (!this._people.ContainsKey(personPos))
                    {
                        // We died
                        continue;
                    }
                    
                    var person = this._people[personPos];
                    var (enemy, enemyPos) = this.GetAdjacentEnemy(person.GetType(), personPos);

                    if (enemy == null)
                    {
                        // No adjacent enemy - move
                        var newPos = this.Move(person, personPos);

                        if (newPos != personPos)
                        {
                            this._people.Remove(personPos);
                            this._people.Add(newPos, person);

                            (enemy, enemyPos) = this.GetAdjacentEnemy(person.GetType(), newPos);
                        }
                    }

                    if (enemy == null) continue;
                    
                    // Adjacent enemy - attack
                    var killed = enemy.Hit(person.GetAttackPower());

                    // Not dead - nothing else to do
                    if (!killed) continue;
                    
                    // They're dead Dave.
                    this._people.Remove(enemyPos);
                    
                    if (enemy.GetType() == typeof(Elf))
                    {
                        numElves--;
                    }
                    else
                    {
                        numGoblins--;
                    }

                    if (numElves == 0 || numGoblins == 0)
                    {
                        break;
                    }
                }

                if (i == peoplePoints.Count)
                {
                    numRounds++;                    
                }
            }

            var remainingHp = this._people.Values.Sum(p => p.GetHitpoints());
            
//            ConsoleUtils.WriteColouredLine($"Got {numElves} elves, {numGoblins} goblins, total hp {remainingHp} after {numRounds} rounds", ConsoleColor.Magenta);

            return (numRounds * remainingHp, startingNumElves - numElves);
        }
        
        protected override void DoPart1()
        {
            var (result, _) = this.DoBattle();

            var colour = ConsoleColor.Cyan;

            if (result >= 198354)
            {
                colour = ConsoleColor.Red;
            }
            
            ConsoleUtils.WriteColouredLine($"Got result {result}", colour);
        }

        protected override void DoPart2()
        {
            int result, elvenLosses;
            
            do
            {
                (result, elvenLosses) = this.DoBattle();
                
                Elf.ElfAttack++;
            } while (elvenLosses > 0);

            Elf.ElfAttack--;
            
            ConsoleUtils.WriteColouredLine($"No elven losses with attack {Elf.ElfAttack}, result was {result}", ConsoleColor.Cyan);
        }

        private abstract class Person
        {
            protected int Hitpoints;
            protected int AttackPower;

            public int GetHitpoints()
            {
                return this.Hitpoints;
            }

            public int GetAttackPower()
            {
                return this.AttackPower;
            }

            public bool Hit(int attackDamage)
            {
                this.Hitpoints -= attackDamage;

                return this.Hitpoints <= 0;
            }
        }

        private class Elf : Person
        {
            public static int ElfAttack = 3;
            
            public Elf()
            {
                this.Hitpoints = 200;
                this.AttackPower = ElfAttack;
            }
        }

        private class Goblin : Person
        {
            public Goblin()
            {
                this.Hitpoints = 200;
                this.AttackPower = 3;
            }
        }
    }
}