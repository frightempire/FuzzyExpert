using System.Collections.Generic;
using System.Linq;

namespace FuzzyExpert.Core.Extensions
{
    public static class ListExtensions
    {
        public static List<string> AppendToEachString(this List<string> sourceStrings, string appendix)
        {
            List<string> resultStrings = new List<string>();
            sourceStrings.ForEach(s => resultStrings.Add($"{appendix} : {s}"));
            return resultStrings;
        }

        public static bool Intersect(this List<string> firstList, List<string> secondList)
        {
            var intersection = from firstListEntry in firstList
                join secondListEntry in secondList
                on firstListEntry equals secondListEntry
                select firstListEntry;
            int intersectCount = intersection.Count();
            return intersectCount != 0;
        }
    }
}