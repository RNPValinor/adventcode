using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace _2020.Solvers
{
    public class Day4Solver : ISolver
    {
        private static readonly Regex FieldRegex = new Regex("([a-z][a-z][a-z]):([a-z0-9#]+)");
        private static readonly HashSet<string> RequiredFields = new HashSet<string>
        {
            "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid"
        };

        public void Solve(string input)
        {
            var passports = input.Split($"{Environment.NewLine}{Environment.NewLine}");

            var numValidV1 = 0;
            var numValidV2 = 0;

            foreach (var passport in passports)
            {
                var (validV1, validV2) = IsPassportStringValid(passport);

                numValidV1 += validV1 ? 1 : 0;
                numValidV2 += validV2 ? 1 : 0;
            }

            Console.WriteLine(numValidV1);
            Console.WriteLine(numValidV2);
        }

        private static (bool v1, bool v2) IsPassportStringValid(string passportString)
        {
            var matches = FieldRegex.Matches(passportString).ToList();

            if (matches.Count < 7)
            {
                return (false, false);
            }

            var numRequiredFields = 0;
            var numRequiredAndValidFields = 0;

            foreach (var match in matches)
            {
                var fieldId = match.Groups[1].Value;

                if (!RequiredFields.Contains(fieldId))
                {
                    continue;
                }

                numRequiredFields++;
                
                var fieldValue = match.Groups[2].Value;

                if (IsFieldValid(fieldId, fieldValue))
                {
                    numRequiredAndValidFields++;
                }
            }

            return (numRequiredFields == 7, numRequiredAndValidFields == 7);
        }

        private static readonly Regex HgtRegex = new Regex("^([0-9]+)(in|cm)$");
        private static readonly Regex HclRegex = new Regex("^#[0-9a-f][0-9a-f][0-9a-f][0-9a-f][0-9a-f][0-9a-f]$");
        private static readonly HashSet<string> ValidEyeColours = new HashSet<string>
        {
            "amb", "blu", "brn", "gry", "grn", "hzl", "oth"
        };

        private static bool IsFieldValid(string fieldId, string fieldValue)
        {
            switch (fieldId)
            {
                case "byr":
                    if (int.TryParse(fieldValue, out var birthYear))
                    {
                        return birthYear >= 1920 && birthYear <= 2002;
                    }
                    break;
                case "iyr":
                    if (int.TryParse(fieldValue, out var issueYear))
                    {
                        return issueYear >= 2010 && issueYear <= 2020;
                    }
                    break;
                case "eyr":
                    if (int.TryParse(fieldValue, out var expiryYear))
                    {
                        return expiryYear >= 2020 && expiryYear <= 2030;
                    }
                    break;
                case "hgt":
                    var matchGroups = HgtRegex.Match(fieldValue).Groups;

                    if (matchGroups.Count != 3)
                    {
                        return false;
                    }

                    var height = int.Parse(matchGroups[1].Value);

                    if (matchGroups[2].Value == "cm")
                    {
                        return height >= 150 && height <= 193;
                    }
                    else
                    {
                        return height >= 59 && height <= 76;
                    }
                case "hcl":
                    return HclRegex.IsMatch(fieldValue);
                case "ecl":
                    return ValidEyeColours.Contains(fieldValue);
                case "pid":
                    return fieldValue.Length == 9 && int.TryParse(fieldValue, out _);
            }

            return false;
        }
    }
}