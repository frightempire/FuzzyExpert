using System.Text.RegularExpressions;
using FuzzyExpert.Application.Entities;
using FuzzyExpert.Infrastructure.ProductionRuleParsing.Interfaces;

namespace FuzzyExpert.Infrastructure.ProductionRuleParsing.Implementations
{
    public class ImplicationRuleValidator : IImplicationRuleValidator
    {
        public ValidationOperationResult ValidateImplicationRule(string implicationRule)
        {
            var validationOperationResult = new ValidationOperationResult();
            var regexPattern = @"IF\(.+\)THEN\(.+\)";
            if (!Regex.IsMatch(implicationRule, regexPattern))
            {
                validationOperationResult.AddMessage($"Implication rule string is not valid. Format example : {FormatExample}");
            }
            return validationOperationResult;
        }

        private string FormatExample => "IF (Pressure = HIGH & Danger = HIGH) THEN (Evacuate = TRUE)";
    }
}