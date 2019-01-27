using System.Collections.Generic;
using CommonLogic;

namespace LinguisticVariableParser.Entities
{
    public class MembershipFunctionStrings
    {
        public MembershipFunctionStrings(
            string membershipFunctionName,
            string membershipFunctionType,
            List<int> membershipFunctionValues)
        {
            ExceptionAssert.IsEmpty(membershipFunctionName);
            ExceptionAssert.IsEmpty(membershipFunctionType);
            ExceptionAssert.IsNull(membershipFunctionValues);
            ExceptionAssert.IsEmpty(membershipFunctionValues);

            MembershipFunctionName = membershipFunctionName;
            MembershipFunctionType = membershipFunctionType;
            MembershipFunctionValues = membershipFunctionValues;
        }

        public string MembershipFunctionName { get; }

        public string MembershipFunctionType { get; }

        public List<int> MembershipFunctionValues { get; }
    }
}