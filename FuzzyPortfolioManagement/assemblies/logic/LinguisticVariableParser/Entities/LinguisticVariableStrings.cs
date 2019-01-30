using System.Collections.Generic;
using CommonLogic;
using MembershipFunctionParser.Entities;

namespace LinguisticVariableParser.Entities
{
    public class LinguisticVariableStrings
    {
        public LinguisticVariableStrings(string variableName, string dataOrigin, List<MembershipFunctionStrings> membershipFunctions)
        {
            ExceptionAssert.IsEmpty(variableName);
            ExceptionAssert.IsEmpty(dataOrigin);
            ExceptionAssert.IsNull(membershipFunctions);
            ExceptionAssert.IsEmpty(membershipFunctions);

            VariableName = variableName;
            DataOrigin = dataOrigin;
            MembershipFunctions = membershipFunctions;
        }

        public string VariableName { get; }

        public string DataOrigin { get; }

        public List<MembershipFunctionStrings> MembershipFunctions { get; }
    }
}