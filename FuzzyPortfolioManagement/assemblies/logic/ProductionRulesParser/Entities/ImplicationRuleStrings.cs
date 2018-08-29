using CommonLogic;

namespace ProductionRulesParser.Entities
{
    public class ImplicationRuleStrings
    {
        public ImplicationRuleStrings(string ifStatement, string thenStatement)
        {
            ExceptionAssert.IsNull(ifStatement);
            ExceptionAssert.IsEmpty(ifStatement);
            ExceptionAssert.IsNull(thenStatement);
            ExceptionAssert.IsEmpty(thenStatement);

            IfStatement = ifStatement;
            ThenStatement = thenStatement;
        }

        public string IfStatement { get; }

        public string ThenStatement { get; }
    }
}
