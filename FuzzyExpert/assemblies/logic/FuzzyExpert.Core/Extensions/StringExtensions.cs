using System.Collections.Generic;
using System.Text;

namespace FuzzyExpert.Core.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveUnwantedCharacters(this string stringToProcess, List<char> charactersToRemove)
        {
            StringBuilder resultString = new StringBuilder();
            resultString.Append(stringToProcess);

            foreach (char character in charactersToRemove)
            {
                resultString.Replace($"{character}", string.Empty);
            }

            return resultString.ToString();
        }
    }
}