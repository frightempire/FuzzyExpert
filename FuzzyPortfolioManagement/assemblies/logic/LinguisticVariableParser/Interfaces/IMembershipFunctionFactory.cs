using System.Collections.Generic;
using LinguisticVariableParser.Entities;
using LinguisticVariableParser.Enums;

namespace LinguisticVariableParser.Interfaces
{
    public interface IMembershipFunctionFactory
    {
        MembershipFunction CreateMembershipFunction(MembershipFunctionType membershipFunctionType, string linguisticVariableName, List<double> points);
    }
}