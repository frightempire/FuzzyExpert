using System;
using System.Collections.Generic;
using System.Linq;
using FuzzyExpert.Infrastructure.MembershipFunctionParsing.Entities;

namespace FuzzyExpert.Infrastructure.LinguisticVariableParsing.Entities
{
    public class LinguisticVariableStrings
    {
        public LinguisticVariableStrings(string variableName, string dataOrigin, List<MembershipFunctionStrings> membershipFunctions)
        {
            if (string.IsNullOrWhiteSpace(variableName)) throw new ArgumentNullException(nameof(variableName));
            if (string.IsNullOrWhiteSpace(dataOrigin)) throw new ArgumentNullException(nameof(dataOrigin));
            if (membershipFunctions == null) throw new ArgumentNullException(nameof(membershipFunctions));
            if (!membershipFunctions.Any()) throw new ArgumentNullException(nameof(membershipFunctions));

            VariableName = variableName;
            DataOrigin = dataOrigin;
            MembershipFunctions = membershipFunctions;
        }

        public string VariableName { get; }

        public string DataOrigin { get; }

        public List<MembershipFunctionStrings> MembershipFunctions { get; }
    }
}