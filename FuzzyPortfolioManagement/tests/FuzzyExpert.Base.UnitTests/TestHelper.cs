using System;
using System.Collections.Generic;
using System.Linq;

namespace FuzzyExpert.Base.UnitTests
{
    public static class TestHelper
    {
        public static bool ListsAreSequentiallyEqual<T>(List<T> listToCompare, List<T> listToCompareWith)
        {
            return listToCompare.SequenceEqual(listToCompareWith);
        }

        public static bool NestedListsAreEqual(List<object> listToCompare, List<object> listToCompareWith)
        {
            if (listToCompare.Count != listToCompareWith.Count)
                return false;

            for (int i = 0; i < listToCompare.Count ; i++)
            {
                Type typeToCompare = listToCompare[i].GetType();
                Type typeToCompareWith = listToCompareWith[i].GetType();
                if (typeToCompare != typeToCompareWith)
                    return false;

                if (typeToCompare == typeof(List<object>))
                {
                    if (!NestedListsAreEqual(listToCompare[i] as List<object>,
                        listToCompareWith[i] as List<object>))
                        return false;
                }
                else if (!listToCompare[i].Equals(listToCompareWith[i]))
                    return false;
            }

            return true;
        }
    }
}