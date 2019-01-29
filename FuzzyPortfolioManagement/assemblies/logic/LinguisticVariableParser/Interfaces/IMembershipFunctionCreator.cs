using System.Collections.Generic;
using LinguisticVariableParser.Entities;
using LinguisticVariableParser.Enums;

namespace LinguisticVariableParser.Interfaces
{
    public interface IMembershipFunctionCreator
    {
        MembershipFunction CreateMembershipFunctionEntity(MembershipFunctionType membershipFunctionType, string membershipFunctionName, List<double> points);
    }
}