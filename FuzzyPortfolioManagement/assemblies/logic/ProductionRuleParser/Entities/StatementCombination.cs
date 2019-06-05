using System.Collections.Generic;
using CommonLogic;

namespace ProductionRuleParser.Entities
{
    public class StatementCombination
    {
        // List of statement combinations - divided by OR
        public StatementCombination(List<UnaryStatement> unaryStatements)
        {
            ExceptionAssert.IsNull(unaryStatements);
            ExceptionAssert.IsEmpty(unaryStatements);

            UnaryStatements = unaryStatements;
        }

        // List of unary statements - divided by AND
        public List<UnaryStatement> UnaryStatements { get; }
    }
}
