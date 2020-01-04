using System.ComponentModel;

namespace FuzzyExpert.Core.Enums
{
    public enum ComparisonOperation
    {
        [Description("<")]
        Less,

        [Description("<=")]
        LessOrEqual,

        [Description(">")]
        Greater,

        [Description(">=")]
        GreaterOrEqual,

        [Description("=")]
        Equal,

        [Description("!=")]
        NotEqual
    }
}