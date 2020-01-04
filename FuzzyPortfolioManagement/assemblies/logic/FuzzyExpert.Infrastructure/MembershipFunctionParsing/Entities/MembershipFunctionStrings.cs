using System;
using System.Collections.Generic;
using System.Linq;

namespace FuzzyExpert.Infrastructure.MembershipFunctionParsing.Entities
{
    public class MembershipFunctionStrings
    {
        public MembershipFunctionStrings(
            string membershipFunctionName,
            string membershipFunctionType,
            List<double> membershipFunctionValues)
        {
            if (string.IsNullOrWhiteSpace(membershipFunctionName)) throw new ArgumentNullException(nameof(membershipFunctionName));
            if (string.IsNullOrWhiteSpace(membershipFunctionType)) throw new ArgumentNullException(nameof(membershipFunctionType));
            if (membershipFunctionValues == null) throw new ArgumentNullException(nameof(membershipFunctionValues));
            if (!membershipFunctionValues.Any()) throw new ArgumentNullException(nameof(membershipFunctionValues));

            MembershipFunctionName = membershipFunctionName;
            MembershipFunctionType = membershipFunctionType;
            MembershipFunctionValues = membershipFunctionValues;
        }

        public string MembershipFunctionName { get; }

        public string MembershipFunctionType { get; }

        public List<double> MembershipFunctionValues { get; }
    }
}