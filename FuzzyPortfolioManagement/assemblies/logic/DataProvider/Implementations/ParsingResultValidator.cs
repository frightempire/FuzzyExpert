using System.Collections.Generic;
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
                if (strings.Length < 2) result.AddMessage($"Missing data at {strings[0]}.");
                if (strings.Length > 2) result.AddMessage($"Too much information at {strings[0]}.");

                double value;
                if (strings.Length == 2 && !double.TryParse(strings[1], out value))
                    result.AddMessage($"Value for data at {strings[0]} is not numeric.");
            }
            return result;
        }
    }
}