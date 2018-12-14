using System;
using System.Collections.Generic;
using System.Text;
using _2018.Utils;

namespace _2018.Days
{
    public class Day14 : Day
    {
        private const int NumRecipiesNeeded = 170641;
        private readonly List<int> _recipePattern = new List<int> {1, 7, 0, 6, 4, 1};

        private static bool Cook(int r1, int r2, ICollection<int> recipies)
        {
            var newRecipe = r1 + r2;

            if (newRecipe < 10)
            {
                recipies.Add(newRecipe);
                return false;
            }
            else
            {
                recipies.Add(1);
                recipies.Add(newRecipe - 10);
                return true;
            }
        }
        
        protected override void DoPart1()
        {
            var e1 = 0;
            var e2 = 1;
            var recipies = new List<int> {3, 7};

            while (recipies.Count < NumRecipiesNeeded + 10)
            {
                var r1 = recipies[e1];
                var r2 = recipies[e2];

                Cook(r1, r2, recipies);

                e1 = (e1 + 1 + r1) % recipies.Count;
                e2 = (e2 + 1 + r2) % recipies.Count;
            }

            var finalRecipies = new StringBuilder("Final 10 recipies are ");

            for (var i = NumRecipiesNeeded; i < NumRecipiesNeeded + 10; i++)
            {
                finalRecipies.Append(recipies[i]);
            }
            
            ConsoleUtils.WriteColouredLine(finalRecipies.ToString(), ConsoleColor.Cyan);
        }

        private static (bool atEnd, bool oneBeforeEnd) EndsWith(IList<int> recipies, IList<int> target, bool added10)
        {
            if (recipies.Count < target.Count)
            {
                return (false, false);
            }

            var atEnd = true;

            for (var i = 0; i < target.Count; i++)
            {
                var recipeIndex = recipies.Count - target.Count + i;

                if (recipies[recipeIndex] != target[i])
                {
                    atEnd = false;
                    break;
                }
            }

            var oneBeforeEnd = false;

            if (added10 && recipies.Count >= target.Count + 1)
            {
                oneBeforeEnd = true;
                
                for (var i = 0; i < target.Count; i++)
                {
                    var recipeIndex = recipies.Count - target.Count + i - 1;

                    if (recipies[recipeIndex] != target[i])
                    {
                        oneBeforeEnd = false;
                        break;
                    }
                }
            }

            return (atEnd, oneBeforeEnd);
        }

        protected override void DoPart2()
        {
            var e1 = 0;
            var e2 = 1;
            var recipies = new List<int> {3, 7};
            var added10 = false;

            var (atEnd, oneBeforeEnd) = EndsWith(recipies, this._recipePattern, added10);

            while (!atEnd && !oneBeforeEnd)
            {
                var r1 = recipies[e1];
                var r2 = recipies[e2];

                added10 = Cook(r1, r2, recipies);

                e1 = (e1 + 1 + r1) % recipies.Count;
                e2 = (e2 + 1 + r2) % recipies.Count;
                
                (atEnd, oneBeforeEnd) = EndsWith(recipies, this._recipePattern, added10);
            }

            var numRecipiesBefore = recipies.Count - this._recipePattern.Count;

            if (oneBeforeEnd)
            {
                // We added a 10 in the final loop, but didn't match the 0
                numRecipiesBefore--;
            }
            
            ConsoleUtils.WriteColouredLine($"Num recipies before target pattern is {numRecipiesBefore}", ConsoleColor.Cyan);
        }
    }
}