using System.ComponentModel;

namespace ProductionRulesParser.Enums
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
