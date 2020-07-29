using System.Text.RegularExpressions;
using FuzzyExpert.Application.Entities;
using FuzzyExpert.Infrastructure.LinguisticVariableParsing.Interfaces;

namespace FuzzyExpert.Infrastructure.LinguisticVariableParsing.Implementations
{
    public class LinguisticVariableValidator : ILinguisticVariableValidator
    {
        public ValidationOperationResult ValidateLinguisticVariables(string linguisticVariable)
        {
            var validationOperationResult = new ValidationOperationResult();
            var regexPattern = @"\[\w+(,\w+)*\]:\w+:\[\w+:\w+:\(\d+(\s*,\s*\d+)*\){1}(\|\w+:\w+:\(\d+(\s*,\s*\d+)*\))*\]";
            if (!Regex.IsMatch(linguisticVariable, regexPattern))
            {
                validationOperationResult.AddMessage($"Linguistic variable string is not valid. Format example : {FormatExample}");
            }
            return validationOperationResult;
        }

        private string FormatExample => "[WaterTemperature,AirTemperature]:Initial:" +
                                        "[Cold:Trapezoidal:(0,20,20,30)|Hot:Trapezoidal:(50,60,60,80)]";
    }
}