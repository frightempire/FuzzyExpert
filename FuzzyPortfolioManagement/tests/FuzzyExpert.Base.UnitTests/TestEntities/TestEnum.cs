using System.ComponentModel;

namespace FuzzyExpert.Base.UnitTests.TestEntities
{
    public enum TestEnum
    {
        [Description("Nothing")]
        Zero = 0,

        [Description("Something")]
        First = 1,

        Second = 2,
        Third = 3
    }
}