using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using FuzzyExpert.Application.Entities;
using FuzzyExpert.Infrastructure.InitialDataProviding.Interfaces;

namespace FuzzyExpert.Infrastructure.InitialDataProviding.Implementations
{
    public class ParsingResultValidator : IParsingResultValidator
    {
        public ValidationOperationResult Validate(List<string[]> parsingResult)
        {
            var validationMessages = new List<string>();

            if (parsingResult.Count == 0)
            {
                validationMessages.Add("No initial data found.");
            }

            foreach (var strings in parsingResult)
            {
                if (strings.Length < 3)
                {
                    validationMessages.Add($"Missing data at {strings[0]}.");
                    continue;
                }
                if (strings.Length > 3)
                {
                    validationMessages.Add($"Too much information at {strings[0]}.");
                    continue;
                }

                if (strings.Length == 3 && !double.TryParse(strings[1], NumberStyles.Any, CultureInfo.InvariantCulture, out var value))
                {
                    validationMessages.Add($"Value for data at {strings[0]} is not numeric.");
                }

                var parsedResult = double.TryParse(strings[2], NumberStyles.Any, CultureInfo.InvariantCulture, out value);
                if (strings.Length == 3 && !parsedResult)
                {
                    validationMessages.Add($"Confidence factor for data at {strings[0]} is not numeric.");
                }

                if (strings.Length == 3 && parsedResult && (value < 0 || value > 1))
                {
                    validationMessages.Add($"Confidence factor for data at {strings[0]} is not in range.");
                }
            }

            return validationMessages.Any() ? ValidationOperationResult.Fail(validationMessages) : ValidationOperationResult.Success();
        }
    }
}