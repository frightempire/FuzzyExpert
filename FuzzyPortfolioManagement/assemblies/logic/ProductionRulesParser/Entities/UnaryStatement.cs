using CommonLogic;
using ProductionRulesParser.Enums;

namespace ProductionRulesParser.Entities
{
    public class UnaryStatement
    {
        public UnaryStatement(string leftOperand, ComparisonOperation comparisonOperation, string rightOperand)
        {
            ExceptionAssert.IsEmpty(leftOperand);
            ExceptionAssert.IsEmpty(rightOperand);

            LeftOperand = leftOperand;
            ComparisonOperation = comparisonOperation;
            RightOperand = rightOperand;
        }

        public string LeftOperand { get; }

        public ComparisonOperation ComparisonOperation { get; }

        public string RightOperand { get; }
    }
}