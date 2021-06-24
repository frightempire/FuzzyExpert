using System.Collections.Generic;
using System.Text.RegularExpressions;
using FuzzyExpert.Application.Entities;
using FuzzyExpert.Infrastructure.LinguisticVariableParsing.Interfaces;

namespace FuzzyExpert.Infrastructure.LinguisticVariableParsing.Implementations
{
    public class LinguisticVariableValidator : ILinguisticVariableValidator
    {
        public ValidationOperationResult ValidateLinguisticVariables(string linguisticVariable)
        {
            var regexPattern = @"\[\w+(,\w+)*\]:\w+:\[\w+:\w+:\(\d+(\s*,\s*\d+)*\){1}(\|\w+:\w+:\(\d+(\s*,\s*\d+)*\))*\]";
            return Regex.IsMatch(linguisticVariable, regexPattern) ? 
                ValidationOperationResult.Success() : 
                ValidationOperationResult.Fail(new List<string> { "Linguistic variable is not valid" });
        }
    }
}