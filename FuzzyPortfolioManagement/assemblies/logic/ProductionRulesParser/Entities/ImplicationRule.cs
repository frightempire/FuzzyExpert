using System.Collections.Generic;
using CommonLogic;

namespace ProductionRulesParser.Entities
{
    public class ImplicationRule
    {
        public ImplicationRule(List<StatementCombination> ifStatement, StatementCombination thenStatement)
        {
            ExceptionAssert.IsNull(ifStatement);
            ExceptionAssert.IsNull(thenStatement);

            IfStatement = ifStatement;
            ThenStatement = thenStatement;
        }

        public List<StatementCombination> IfStatement { get; }

        public StatementCombination ThenStatement { get; }
    }
}
