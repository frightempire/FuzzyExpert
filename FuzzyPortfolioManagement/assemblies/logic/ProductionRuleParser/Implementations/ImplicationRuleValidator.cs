using System;
using System.Collections.Generic;
using System.Linq;
using ProductionRuleParser.Interfaces;

namespace ProductionRuleParser.Implementations
{
    public class ImplicationRuleValidator : IImplicationRuleValidator
    {
        public void ValidateImplicationRule(string implicationRule)
        {
            if (implicationRule.Contains(" "))
                throw new ArgumentException("Implication rule string is not valid: haven't been preprocessed");
            if (!implicationRule.StartsWith("IF"))
                throw new ArgumentException("Implication rule string is not valid: no if statement");
            if (!implicationRule.Contains("THEN"))
                throw new ArgumentException("Implication rule string is not valid: no then statement");

            List<char> brackets = implicationRule.Where(character => character == '(' || character == ')').ToList();
            if (brackets.Count == 0)
                throw new ArgumentException("Implication rule string is not valid: no brackets");
            if (brackets.Count % 2 != 0)
                throw new ArgumentException("Implication rule string is not valid: odd count of brackets");
            if (brackets[0] != '(' || brackets[brackets.Count - 1] != ')')
                throw new ArgumentException("Implication rule string is not valid: wrong opening or closing bracket");

            int openingBracketsCount = brackets.Count(b => b == '(');
            int closingBracketsCount = brackets.Count(b => b == ')');
            if (openingBracketsCount != closingBracketsCount)
                throw new ArgumentException("Implication rule string is not valid: mismatching brackets");
        }
    }
}