using CommonLogic;

namespace MembershipFunctionManager.Entities
{
    public abstract class MembershipFunction
    {
        protected MembershipFunction(string linguisticVariableName)
        {
            ExceptionAssert.IsEmpty(linguisticVariableName);

            LinguisticVariableName = linguisticVariableName;
        }

        public string LinguisticVariableName { get; }
    }
}
