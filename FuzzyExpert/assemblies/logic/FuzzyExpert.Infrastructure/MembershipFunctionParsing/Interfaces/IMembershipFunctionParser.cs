using System.Collections.Generic;
using FuzzyExpert.Infrastructure.MembershipFunctionParsing.Entities;

namespace FuzzyExpert.Infrastructure.MembershipFunctionParsing.Interfaces
{
    public interface IMembershipFunctionParser
    {
        List<MembershipFunctionStrings> ParseMembershipFunctions(string membershipFunctionsPart);
    }
}