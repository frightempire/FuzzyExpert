using System.Text;
using CommonLogic;
using CommonLogic.Extensions;
using ProductionRuleParser.Enums;

namespace ProductionRuleParser.Entities
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

        public string Name { get; set; }

        public override string ToString()
        {
            return $"{LeftOperand} {ComparisonOperation.GetDescription()} {RightOperand}";
        }
    }
}