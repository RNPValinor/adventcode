using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using _2018.Utils;

namespace _2018.Days
{
    public class Day24 : Day
    {
        private readonly HashSet<Army> _immuneSystem = new HashSet<Army>();
        private readonly HashSet<Army> _infection = new HashSet<Army>();

        private void LoadArmies()
        {
            this._immuneSystem.Clear();
            this._infection.Clear();
            
            var questionData = QuestionLoader.Load(24).Split(Environment.NewLine + Environment.NewLine);
            var regexWithImmunity = new Regex("^([0-9]+) units each with ([0-9]+) hit points ((.*) )with an attack that does ([0-9]+) ([a-z]+) damage at initiative ([0-9]+)$");
            var regexWithoutImmunity = new Regex("^([0-9]+) units each with ([0-9]+) hit points with an attack that does ([0-9]+) ([a-z]+) damage at initiative ([0-9]+)$");

            foreach (var armyData in questionData)
            {
                var armies = armyData.Split(Environment.NewLine).ToList();

                var armySet = armies[0] == "Immune System:" ? this._immuneSystem : this._infection;
                
                armies.RemoveAt(0);
                var armyNum = 1;

                foreach (var armyLine in armies)
                {
                    var matches = regexWithImmunity.Match(armyLine);

                    if (matches.Length == 0)
                    {
                        matches = regexWithoutImmunity.Match(armyLine);
                    }

                    var army = new Army
                    {
                        NumUnits = int.Parse(matches.Groups[1].Value),
                        HitPoints = int.Parse(matches.Groups[2].Value),
                        ArmyNum = armyNum++,
                        IsInfection = armySet == this._infection
                    };

                    if (matches.Groups.Count == 8)
                    {
                        var immunityData = matches.Groups[3].Value;

                        immunityData = immunityData.Substring(1, immunityData.Length - 3);

                        var immunityParts = immunityData.Split("; ").ToList();

                        foreach (var immunityPart in immunityParts)
                        {
                            var immunities = immunityPart.Split(' ').ToList();
                            var immunitySet = immunities[0] == "immune" ? army.Immunities : army.Weaknesses;

                            immunities.RemoveAt(0);
                            immunities.RemoveAt(0);

                            foreach (var immunity in immunities)
                            {
                                immunitySet.Add(immunity.EndsWith(',')
                                    ? Enum.Parse<AttackType>(
                                        StringUtils.UpperFirst(immunity.Substring(0, immunity.Length - 1)))
                                    : Enum.Parse<AttackType>(StringUtils.UpperFirst(immunity)));
                            }
                        }
                        
                        army.AttackPower = int.Parse(matches.Groups[5].Value);
                        army.AttackType = Enum.Parse<AttackType>(StringUtils.UpperFirst(matches.Groups[6].Value));
                        army.Initiative = int.Parse(matches.Groups[7].Value);
                    }
                    else
                    {
                        army.AttackPower = int.Parse(matches.Groups[3].Value);
                        army.AttackType = Enum.Parse<AttackType>(StringUtils.UpperFirst(matches.Groups[4].Value));
                        army.Initiative = int.Parse(matches.Groups[5].Value);
                    }

                    armySet.Add(army);
                }
            }
        }

        private Army SelectTarget(Army attacker, ICollection<Army> selectedTargets)
        {
            var defenders = this._immuneSystem;

            if (defenders.Contains(attacker))
            {
                defenders = this._infection;
            }

            Army target = null;
            var maxDamage = 0;

            foreach (var defender in defenders)
            {
                if (selectedTargets.Contains(defender)) continue;
                
                var damage = GetDamage(attacker, defender);
                
                if (damage > maxDamage)
                {
                    target = defender;
                    maxDamage = damage;
                }
                else if (damage == maxDamage && target != null)
                {
                    if (defender.EffectivePower > target.EffectivePower)
                    {
                        target = defender;
                    }
                    else if (defender.EffectivePower == target.EffectivePower &&
                             defender.Initiative > target.Initiative)
                    {
                        target = defender;
                    }
                }
            }

            return target;
        }

        private static int GetDamage(Army attacker, Army defender)
        {
            var attackPower = attacker.EffectivePower;

            if (defender.Weaknesses.Contains(attacker.AttackType))
            {
                attackPower *= 2;
            }
            else if (defender.Immunities.Contains(attacker.AttackType))
            {
                attackPower = 0;
            }

            return attackPower;
        }

        private void Fight(bool log = false)
        {
            var roundNum = 0;

            while (this._infection.Any() && this._immuneSystem.Any())
            {
                if (log)
                    ConsoleUtils.WriteColouredLine($"Running round {++roundNum}", ConsoleColor.Green);
                
                // Target phase
                var armies = new List<Army>(this._infection);
                armies.AddRange(this._immuneSystem);
                
                armies.Sort((a, b) => a.EffectivePower == b.EffectivePower ? b.Initiative - a.Initiative : b.EffectivePower - a.EffectivePower);

                var targeting = new Dictionary<Army, Army>();
                var selectedTargets = new HashSet<Army>();

                foreach (var army in armies)
                {
                    var target = this.SelectTarget(army, selectedTargets);

                    if (target != null)
                    {
                        selectedTargets.Add(target);
                        targeting.Add(army, target);
                    }
                }
                
                // Attack phase

                var attackers = targeting.Keys;
                var orderedAttackers = attackers.ToList();
                orderedAttackers.Sort((a, b) => b.Initiative - a.Initiative);
                var totalUnitsKilled = 0;

                foreach (var attacker in orderedAttackers)
                {
                    if (attacker.IsDead) continue;
                    
                    var defender = targeting[attacker];
                    var damage = GetDamage(attacker, defender);
                    var oldNumUnits = defender.NumUnits;

                    if (defender.Attack(damage))
                    {
                        this._infection.Remove(defender);
                        this._immuneSystem.Remove(defender);
                    }

                    var numUnitsKilled = oldNumUnits - Math.Max(0, defender.NumUnits);

                    totalUnitsKilled += numUnitsKilled;

                    if (log)
                        ConsoleUtils.WriteColouredLine($"{(attacker.IsInfection ? "Infection" : "Immune System")} group {attacker.ArmyNum} attacks {(defender.IsInfection ? "Infection" : "Immune System")} group {defender.ArmyNum}, killing {numUnitsKilled} units ({damage} damage)", ConsoleColor.Red);
                }

                if (totalUnitsKilled == 0)
                {
                    ConsoleUtils.WriteColouredLine("Stalemate", ConsoleColor.DarkBlue);
                    return;
                }
            }
        }
        
        protected override void DoPart1()
        {
            this.LoadArmies();
            this.Fight();

            ConsoleUtils.WriteColouredLine(
                this._infection.Any()
                    ? $"Infection won with {this._infection.Sum(a => a.NumUnits)} units remaining"
                    : $"Immune system won with {this._immuneSystem.Sum(a => a.NumUnits)} units remaining", ConsoleColor.Cyan);
        }

        protected override void DoPart2()
        {
            var boost = 0;

            do
            {
                this.LoadArmies();

                foreach (var army in this._immuneSystem)
                {
                    army.AttackPower += boost;
                }

                this.Fight();

                boost += 1;
            } while (!this._immuneSystem.Any() || this._infection.Any());
            
            ConsoleUtils.WriteColouredLine($"Immune system has {this._immuneSystem.Sum(a => a.NumUnits)} units remaining", ConsoleColor.Cyan);
        }

        private class Army
        {
            public int ArmyNum { get; set; }
            
            public bool IsInfection { get; set; }
            
            public int NumUnits { get; set; }

            public int HitPoints { private get; set; }
            
            public AttackType AttackType { get; set; }

            public int AttackPower { get; set; }

            public int Initiative { get; set; }

            public HashSet<AttackType> Immunities { get; } = new HashSet<AttackType>();

            public HashSet<AttackType> Weaknesses { get; } = new HashSet<AttackType>();
            
            public int EffectivePower => this.NumUnits * this.AttackPower;

            public bool IsDead => this.NumUnits <= 0;

            public bool Attack(int damage)
            {
                var unitsToKill = (int)Math.Floor((double) damage / this.HitPoints);

                this.NumUnits -= unitsToKill;

                return this.NumUnits <= 0;
            }
        }
    }
}