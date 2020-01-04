using System.Collections.Generic;
using System.Globalization;
using FuzzyExpert.Application.Entities;
using FuzzyExpert.Infrastructure.InitialDataProviding.Interfaces;

namespace FuzzyExpert.Infrastructure.InitialDataProviding.Implementations
{
    public class ParsingResultValidator : IParsingResultValidator
    {
        public ValidationOperationResult Validate(List<string[]> parsingResult)
        {
            ValidationOperationResult result = new ValidationOperationResult();
            if (parsingResult.Count == 0) result.AddMessage("No initial data found.");

            foreach (string[] strings in parsingResult)
            {
                if (strings.Length < 3)
                {
                    result.AddMessage($"Missing data at {strings[0]}.");
                    continue;
                }
                if (strings.Length > 3)
                {
                    result.AddMessage($"Too much information at {strings[0]}.");
                    continue;
                }

                if (strings.Length == 3 && !double.TryParse(strings[1], NumberStyles.Any, CultureInfo.InvariantCulture, out var value))
                    result.AddMessage($"Value for data at {strings[0]} is not numeric.");

                var parsedResult = double.TryParse(strings[2], NumberStyles.Any, CultureInfo.InvariantCulture, out value);
                if (strings.Length == 3 && !parsedResult)
                    result.AddMessage($"Confidence factor for data at {strings[0]} is not numeric.");
                if (strings.Length == 3 && parsedResult && (value < 0 || value > 1))
                    result.AddMessage($"Confidence factor for data at {strings[0]} is not in range.");
            }
            return result;
        }
    }
}