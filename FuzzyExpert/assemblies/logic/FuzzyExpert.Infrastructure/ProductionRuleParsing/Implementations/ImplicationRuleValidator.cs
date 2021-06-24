using System;
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
            var validationMessages = new List<string>();
            if (string.IsNullOrWhiteSpace(implicationRule))
            {
                validationMessages.Add("Implication rule is empty");
                return ValidationOperationResult.Fail(validationMessages);
            }

            var indexOfIfDelimiter = implicationRule.IndexOf("IF(", StringComparison.Ordinal);
            if (indexOfIfDelimiter != 0)
            {
                validationMessages.Add("No IF statement");
            }

            var indexOfThenDelimiter = implicationRule.IndexOf(")THEN(", StringComparison.Ordinal);
            if (indexOfThenDelimiter == -1)
            {
                validationMessages.Add("No THEN statement");
            }

            if (validationMessages.Any())
            {
                return ValidationOperationResult.Fail(validationMessages);
            }

            var ifStatement = implicationRule.Substring(2, indexOfThenDelimiter - 1);
            var thenStatement = implicationRule.Substring(indexOfThenDelimiter + 5);

            if (ParenthesisMismatched(ifStatement))
            {
                validationMessages.Add("IF statement parenthesis don't match");
            }

            if (ParenthesisMismatched(thenStatement))
            {
                validationMessages.Add("THEN statement parenthesis don't match");
            }

            return !validationMessages.Any() ? 
                ValidationOperationResult.Success() : 
                ValidationOperationResult.Fail(validationMessages);
        }

        private bool ParenthesisMismatched(string ifStatement)
        {
            var parenthesisStack = new Stack<char>();
            foreach (var element in ifStatement)
            {
                switch (element)
                {
                    case '(':
                        parenthesisStack.Push(element);
                        break;
                    case ')':
                        if (parenthesisStack.Any())
                        {
                            parenthesisStack.Pop();
                        }
                        else
                        {
                            parenthesisStack.Push(element);
                        }

                        break;
                }
            }

            return parenthesisStack.Any();
        }
    }
}