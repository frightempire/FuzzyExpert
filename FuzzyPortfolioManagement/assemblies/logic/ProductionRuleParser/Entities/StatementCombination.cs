using System.Collections.Generic;
using CommonLogic;

namespace ProductionRuleParser.Entities
{
    public class StatementCombination
    {
        // List of statement combinations - OR
        // List of unary statements - AND

        public StatementCombination(List<UnaryStatement> unaryStatements)
        {
            ExceptionAssert.IsNull(unaryStatements);
            ExceptionAssert.IsEmpty(unaryStatements);

            UnaryStatements = unaryStatements;
        }

        public List<UnaryStatement> UnaryStatements { get; }
    }
}
