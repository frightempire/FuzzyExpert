using System.Collections.Generic;
using CommonLogic;

namespace ProductionRulesParser.Entities
{
    public class StatementCombination
    {
        public StatementCombination(List<UnaryStatement> unaryStatements)
        {
            ExceptionAssert.IsNull(unaryStatements);
            ExceptionAssert.IsEmpty(unaryStatements);

            UnaryStatements = unaryStatements;
        }

        public List<UnaryStatement> UnaryStatements { get; }
    }
}
