using System.Collections.Generic;
using System.Linq;
using FuzzyExpert.Application.Entities;
using FuzzyExpert.Infrastructure.ProductionRuleParsing.Interfaces;

namespace FuzzyExpert.Infrastructure.ProductionRuleParsing.Implementations
{
    public class ImplicationRuleValidator : IImplicationRuleValidator
    {
        public ValidationOperationResult ValidateImplicationRule(string implicationRule)
        {
            ValidationOperationResult validationOperationResult = new ValidationOperationResult();

            if (implicationRule.Contains(" "))
                validationOperationResult.AddMessage("Implication rule string is not valid: haven't been preprocessed");
            if (!implicationRule.StartsWith("IF"))
                validationOperationResult.AddMessage("Implication rule string is not valid: no if statement");
            if (!implicationRule.Contains("THEN"))
                validationOperationResult.AddMessage("Implication rule string is not valid: no then statement");

            List<char> brackets = implicationRule.Where(character => character == '(' || character == ')').ToList();
            if (brackets.Count == 0)
            {
                validationOperationResult.AddMessage("Implication rule string is not valid: no brackets");
            }
            else
            {
                if (brackets.Count % 2 != 0)
                    validationOperationResult.AddMessage("Implication rule string is not valid: odd count of brackets");
                if (brackets[0] != '(' || brackets[brackets.Count - 1] != ')')
                    validationOperationResult.AddMessage("Implication rule string is not valid: wrong opening or closing bracket");

                int openingBracketsCount = brackets.Count(b => b == '(');
                int closingBracketsCount = brackets.Count(b => b == ')');
                if (openingBracketsCount != closingBracketsCount)
                    validationOperationResult.AddMessage("Implication rule string is not valid: mismatching brackets");
            }

            return validationOperationResult;
        }
    }
}