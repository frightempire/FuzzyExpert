using System.Collections.Generic;
using System.Globalization;
using CommonLogic.Entities;
using DataProvider.Interfaces;

namespace DataProvider.Implementations
{
    public class ParsingResultValidator : IParsingResultValidator
    {
        public ValidationOperationResult Validate(List<string[]> parsignResult)
        {
            ValidationOperationResult result = new ValidationOperationResult();
            if (parsignResult.Count == 0) result.AddMessage("No initial data found.");

            foreach (string[] strings in parsignResult)
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

                double value;
                if (strings.Length == 3 && !double.TryParse(strings[1], NumberStyles.Any, CultureInfo.InvariantCulture, out value))
                    result.AddMessage($"Value for data at {strings[0]} is not numeric.");

                var parsingResult = double.TryParse(strings[2], NumberStyles.Any, CultureInfo.InvariantCulture, out value);
                if (strings.Length == 3 && !parsingResult)
                    result.AddMessage($"Confidence factor for data at {strings[0]} is not numeric.");
                if (strings.Length == 3 && parsingResult && (value < 0 || value > 1))
                    result.AddMessage($"Confidence factor for data at {strings[0]} is not in range.");
            }
            return result;
        }
    }
}