using System.Collections.Generic;
using MembershipFunctionManager.Entities;
using MembershipFunctionManager.Enums;

namespace MembershipFunctionManager.Interfaces
{
    public interface IMembershipFunctionFactory
    {
        MembershipFunction CreateMembershipFunction(MembershipFunctionType membershipFunctionType, string linguisticVariableName, List<double> points);
    }
}