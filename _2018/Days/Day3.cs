using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using _2018.Utils;

namespace _2018.Days
{
    public class Day3 : Day
    {
        private readonly IList<string> _claims;

        public Day3()
        {
            this._claims = QuestionLoader.Load(3).Split(Environment.NewLine);
        }

        protected override void DoPart1()
        {
            var seenPoints = new Dictionary<string, int>();

            var numDuplicates = this._claims.Sum(claim => AddClaimToSeen(claim, seenPoints));

            ConsoleUtils.WriteColouredLine($"Found {numDuplicates} duplicate points", ConsoleColor.Cyan);
        }

        private static int AddClaimToSeen(string claim, IDictionary<string, int> seen)
        {
            var (x, y, width, height) = ParseClaim(claim);

            var numNewDuplicates = 0;

            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    var coord = $"{x + i},{y + j}";

                    if (seen.ContainsKey(coord))
                    {
                        if (seen[coord] == 1)
                        {
                            numNewDuplicates++;
                        }

                        seen[coord]++;
                    }
                    else
                    {
                        seen.Add(coord, 1);
                    }
                }
            }

            return numNewDuplicates;
        }

        private static (int x, int y, int width, int height) ParseClaim(string claim)
        {
            // Index of the first character of the position
            var firstStartIndex = claim.IndexOf('@') + 2;
            
            var start = claim.Substring(firstStartIndex, claim.IndexOf(':') - firstStartIndex).Split(',');
            
            var x = int.Parse(start[0]);
            var y = int.Parse(start[1]);

            var firstSizeIndex = claim.IndexOf(':') + 2;

            var size = claim.Substring(firstSizeIndex).Split('x');

            var width = int.Parse(size[0]);
            var height = int.Parse(size[1]);

            return (x, y, width, height);
        }

        protected override void DoPart2()
        {
            var canvas = new Dictionary<string, HashSet<int>>();
            var overlappingClaims = new HashSet<int>();
            var claimId = 1;
            
            foreach (var claim in this._claims)
            {
                ProcessClaimV2(claim, claimId, canvas, overlappingClaims);

                claimId++;
            }

            for (var i = 1; i < claimId; i++)
            {
                if (!overlappingClaims.Contains(i))
                {
                    ConsoleUtils.WriteColouredLine($"Found non-overlapping claim with ID {i}", ConsoleColor.Cyan);
                }
            }
        }

        private static void ProcessClaimV2(string claim, int claimId, IDictionary<string, HashSet<int>> canvas, ISet<int> overlappingClaims)
        {
            var (x, y, width, height) = ParseClaim(claim);

            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    var coord = $"{x + i},{y + j}";

                    if (canvas.ContainsKey(coord))
                    {
                        canvas[coord].Add(claimId);
                        
                        foreach (var oldClaim in canvas[coord])
                        {
                            overlappingClaims.Add(oldClaim);
                        }
                    }
                    else
                    {
                        canvas.Add(coord, new HashSet<int> { claimId });
                    }
                }
            }
        }
    }
}