using System.ComponentModel;

namespace Base.UnitTests.TestEntitites
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
