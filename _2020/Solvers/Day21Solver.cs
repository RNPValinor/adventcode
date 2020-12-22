using System;
using System.Collections.Generic;
using System.Linq;

namespace _2020.Solvers
{
    public class Day21Solver : ISolver
    {
        private readonly HashSet<HashSet<string>> _recipes = new();
        private readonly Dictionary<string, HashSet<HashSet<string>>> _potentialAllergens = new();
        private readonly Dictionary<string, string> _allergens = new();
        private readonly HashSet<string> _nonAllergens = new();
        
        public void Solve(string input)
        {
            var recipes = input.Split(Environment.NewLine);

            foreach (var recipe in recipes)
            {
                this.ProcessRecipe(recipe);
            }

            this.ProcessAllergens();

            var numNonAllergens = this._recipes.Sum(recipe => recipe.Count(i => this._nonAllergens.Contains(i)));

            Console.WriteLine(numNonAllergens);

            var allergens = this._allergens.Keys.ToList();
            allergens.Sort();
            var ingredients = allergens.Select(a => this._allergens[a]);

            Console.WriteLine(string.Join(',', ingredients));
        }

        private void ProcessRecipe(string recipe)
        {
            var parts = recipe.Split(" (contains ");

            var ingredients = parts[0].Split(" ").ToHashSet();
            var allergens = parts[1].Replace(")", "").Split(", ");

            this._recipes.Add(ingredients);
            this._nonAllergens.UnionWith(ingredients);

            foreach (var allergen in allergens)
            {
                if (this._potentialAllergens.TryGetValue(allergen, out var potentials))
                {
                    potentials.Add(ingredients);
                }
                else
                {
                    this._potentialAllergens.Add(allergen, new HashSet<HashSet<string>> { ingredients });
                }
            }
        }

        private void ProcessAllergens()
        {
            var unmatchedAllergensTypes = new HashSet<string>(this._potentialAllergens.Keys);

            while (unmatchedAllergensTypes.Count > 0)
            {
                var stillUnmatchedAllergens = new HashSet<string>();
                
                foreach (var allergenType in unmatchedAllergensTypes)
                {
                    var potentials = this._potentialAllergens[allergenType];

                    HashSet<string> potentialSet = null;

                    foreach (var pot in potentials)
                    {
                        if (potentialSet == null)
                        {
                            potentialSet = new HashSet<string>(pot);
                        }
                        else
                        {
                            potentialSet.IntersectWith(pot);
                        }
                    }

                    if (potentialSet?.Count == 1)
                    {
                        var ingredient = potentialSet.First();
                        this._allergens.Add(allergenType, ingredient);
                        this._potentialAllergens.Remove(allergenType);
                        this._nonAllergens.Remove(ingredient);

                        foreach (var ingredientSet in this._potentialAllergens.Values.SelectMany(ingredientSetSet => ingredientSetSet))
                        {
                            ingredientSet.Remove(ingredient);
                        }
                    }
                    else
                    {
                        stillUnmatchedAllergens.Add(allergenType);
                    }
                }

                unmatchedAllergensTypes = stillUnmatchedAllergens;
            }
        }
    }
}