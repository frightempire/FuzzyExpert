using System.Collections.Generic;
using CommonLogic;

namespace MembershipFunctionParser.Entities
{
    public class MembershipFunctionStrings
    {
        public MembershipFunctionStrings(
            string membershipFunctionName,
            string membershipFunctionType,
            List<double> membershipFunctionValues)
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

        public List<double> MembershipFunctionValues { get; }
    }
}