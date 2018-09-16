using System.Collections.Generic;
using CommonLogic;

namespace ProductionRulesParser.Entities
{
    public class ImplicationRule
    {
        public ImplicationRule(List<UnaryStatement> ifStatement, UnaryStatement thenStatement)
        {
            ExceptionAssert.IsNull(ifStatement);
            ExceptionAssert.IsNull(thenStatement);

            IfStatement = ifStatement;
            ThenStatement = thenStatement;
        }

        public List<UnaryStatement> IfStatement { get; }

        public UnaryStatement ThenStatement { get; }
    }
}
