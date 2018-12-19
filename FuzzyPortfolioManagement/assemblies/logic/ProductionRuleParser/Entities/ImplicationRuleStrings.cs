using CommonLogic;

namespace ProductionRuleParser.Entities
{
    public class ImplicationRuleStrings
    {
        public ImplicationRuleStrings(string ifStatement, string thenStatement)
        {
            ExceptionAssert.IsEmpty(ifStatement);
            ExceptionAssert.IsEmpty(thenStatement);

            IfStatement = ifStatement;
            ThenStatement = thenStatement;
        }

        public string IfStatement { get; }

        public string ThenStatement { get; }
    }
}
