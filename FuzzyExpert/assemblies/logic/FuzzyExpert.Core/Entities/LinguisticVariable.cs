using System;
using System.Linq;

namespace FuzzyExpert.Core.Entities
{
    public class LinguisticVariable
    {
        public LinguisticVariable(string variableName, MembershipFunctionList membershipFunctionList, bool isInitialData)
        {
            if (string.IsNullOrWhiteSpace(variableName)) throw new ArgumentNullException(nameof(variableName));
            if (membershipFunctionList == null) throw new ArgumentNullException(nameof(membershipFunctionList));
            if (!membershipFunctionList.Any()) throw new ArgumentNullException(nameof(membershipFunctionList));

            VariableName = variableName;
            MembershipFunctionList = membershipFunctionList;
            IsInitialData = isInitialData;
        }

        public string VariableName { get; }

        public MembershipFunctionList MembershipFunctionList { get; }

        public bool IsInitialData { get; }
    }
}