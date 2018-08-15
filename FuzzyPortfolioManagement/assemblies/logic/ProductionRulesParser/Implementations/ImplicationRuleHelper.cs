using System;
using System.Collections.Generic;
using System.Linq;
using ProductionRulesParser.Interfaces;

namespace ProductionRulesParser.Implementations
{
    public class ImplicationRuleHelper : IImplicationRuleHelper
    {
        public List<object> GetRuleParts(string implicationRuleString, ref int index)
        {
            List<object> ruleParts = new List<object>();
            string appendingString = string.Empty;

            for (int iterator = 0; iterator < implicationRuleString.Length; iterator++)
            {
                switch (implicationRuleString[iterator])
                {
                    case '(':
                        index++;
                        ruleParts.Add(GetRuleParts(implicationRuleString.Substring(index), ref index));
                        iterator = index;
                        continue;
                    case ')':
                        if (!string.IsNullOrEmpty(appendingString))
                            ruleParts.Add(appendingString);

                        index++;
                        return ruleParts;
                    case '|':
                        if (!string.IsNullOrEmpty(appendingString))
                            ruleParts.Add(appendingString);

                        appendingString = string.Empty;
                        index++;
                        continue;
                    default:
                        appendingString += implicationRuleString[iterator];
                        index++;
                        continue;
                }
            }

            if (!string.IsNullOrEmpty(appendingString))
                ruleParts.Add(appendingString);

            return ruleParts;
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