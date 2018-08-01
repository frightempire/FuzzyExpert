using ProductionRulesParser.Enums;

namespace ProductionRulesParser.Entities
{
    public class UnaryStatement
    {
        public string LeftOperand { get; set; }

        public ComparisonOperation ComparisonOperation { get; set; }

        public string RightOperand { get; set; }
    }
}