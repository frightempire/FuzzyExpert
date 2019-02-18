using System.Collections.Generic;

namespace CommonLogic.Extensions
{
    public static class ListExtensions
    {
        public static List<string> AppendToEachString(this List<string> sourceStrings, string appendix)
        {
            List<string> resultStrings = new List<string>();
            sourceStrings.ForEach(s => resultStrings.Add($"{appendix} : {s}"));
            return resultStrings;
        }
    }
}
