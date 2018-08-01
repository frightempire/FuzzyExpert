using System.Collections.Generic;
using System.Linq;

namespace Base.UnitTests
{
    public static class TestHelper
    {
        public static bool ListsAreSequencualyEqual<T>(List<T> listToCompare, List<T> listToCompareWith)
        {
            return listToCompare.SequenceEqual(listToCompareWith);
        }
    }
}
