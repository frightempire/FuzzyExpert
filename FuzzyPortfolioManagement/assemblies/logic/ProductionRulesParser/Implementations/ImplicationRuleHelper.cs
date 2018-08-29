using System;
using System.Collections.Generic;
using System.Linq;
using ProductionRulesParser.Entities;
using ProductionRulesParser.Interfaces;

namespace ProductionRulesParser.Implementations
{
    public class ImplicationRuleHelper : IImplicationRuleHelper
    {
        public List<string> GetStatementParts(ref string implicationRuleString)
        {
            List<string> ruleParts = new List<string>();
            List<string> implicationRules = new List<string>();
            string appendingString = string.Empty;

            while (implicationRuleString.Length != 0)
            {
                int iterator = 0;
                char curChar = implicationRuleString[iterator];
                implicationRuleString = implicationRuleString.Remove(iterator, 1);
                switch (curChar)
                {
                    case '(':
                        ruleParts.AddRange(GetStatementParts(ref implicationRuleString));
                        for (int i = 0; i < ruleParts.Count; i++)
                            ruleParts[i] = appendingString + ruleParts[i];
                        appendingString = string.Empty;
                        continue;
                    case ')':
                        if (ruleParts.Any())
                        {                          
                            for (int i = 0; i < ruleParts.Count; i++)
                                ruleParts[i] += appendingString;
                            implicationRules.AddRange(ruleParts);
                        }
                        else if (!string.IsNullOrEmpty(appendingString))
                            implicationRules.Add(appendingString);

                        return implicationRules;
                    case '|':
                        if (ruleParts.Any())
                        {
                            for (int i = 0; i < ruleParts.Count; i++)
                                ruleParts[i] += appendingString;
                            implicationRules.AddRange(ruleParts);
                            ruleParts = new List<string>();
                        }
                        else if (!string.IsNullOrEmpty(appendingString))
                            implicationRules.Add(appendingString);

                        appendingString = string.Empty;
                        continue;
                    default:
                        appendingString += curChar;
                        continue;
                }
            }

            if (ruleParts.Any())
            {
                for (int i = 0; i < ruleParts.Count; i++)
                    ruleParts[i] += appendingString;
                implicationRules.AddRange(ruleParts);
            }
            else if (!string.IsNullOrEmpty(appendingString))
                implicationRules.Add(appendingString);

            return implicationRules;
        }

        public ImplicationRuleStrings ExtractStatementParts(string implicationRule)
        {
            throw new NotFiniteNumberException();
        }

        public void ValidateImplicationRule(string implicationRule)
        {
            if (!implicationRule.StartsWith("IF"))
                throw new ArgumentException("Implication rule string is not valid: no if statement");
            if (!implicationRule.Contains("THEN"))
                throw new ArgumentException("Implication rule string is not valid: no then statement");

            List<char> brackets = implicationRule.Where(character => character == '(' || character == ')').ToList();
            if (brackets.Count == 0)
                throw new ArgumentException("Implication rule string is not valid: no brackets");
            if (brackets.Count % 2 != 0)
                throw new ArgumentException("Implication rule string is not valid: even count of brackets");
            if (brackets[0] != '(' || brackets[brackets.Count - 1] != ')')
                throw new ArgumentException("Implication rule string is not valid: wrong opening or closing bracket");

            int openingBracketsCount = brackets.Count(b => b == '(');
            int closingBracketsCount = brackets.Count(b => b == ')');
            if (openingBracketsCount != closingBracketsCount)
                throw new ArgumentException("Implication rule string is not valid: mismatching brackets");
        }

        public string PreProcessImplicationRule(string implicationRule)
        {
            return implicationRule.Replace(" ", string.Empty);
        }
    }
}