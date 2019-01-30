using System.Collections.Generic;
using MembershipFunctionParser.Entities;

namespace MembershipFunctionParser.Interfaces
{
    public interface IMembershipFunctionParser
    {
        List<MembershipFunctionStrings> ParseMembershipFunctions(string membershipFunctionsPart);
    }
}
