using System.Collections.Generic;
using CommonLogic;
using ProductionRulesParser.Enums;

namespace ProductionRulesParser.Entities
{
    public class ProductionRule
    {
        public ProductionRule(List<UnaryStatement> ifStatement, List<LogicalOperation> logicalOperationsOrder, UnaryStatement thenStatement)
        {
            ExceptionAssert.IsNull(ifStatement);
            ExceptionAssert.IsNull(logicalOperationsOrder);
            ExceptionAssert.IsNull(thenStatement);

            IfStatement = ifStatement;
            LogicalOperationsOrder = logicalOperationsOrder;
            ThenStatement = thenStatement;
        }

        public List<UnaryStatement> IfStatement { get; }

        public List<LogicalOperation> LogicalOperationsOrder { get; }

        public UnaryStatement ThenStatement { get; }
    }
}
