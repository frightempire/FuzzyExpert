using System.Collections.Generic;
using LinguisticVariableParser.Entities;

namespace LinguisticVariableParser.Interfaces
{
    public interface IMembershipFunctionParser
    {
        List<MembershipFunctionStrings> ParseMembershipFunctions(string membershipFunctionsPart);
    }
}
