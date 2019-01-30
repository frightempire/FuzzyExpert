using System.Collections.Generic;
using MembershipFunctionParser.Entities;
using MembershipFunctionParser.Enums;

namespace MembershipFunctionParser.Interfaces
{
    public interface IMembershipFunctionCreator
    {
        MembershipFunction CreateMembershipFunctionEntity(MembershipFunctionType membershipFunctionType, string membershipFunctionName, List<double> points);
    }
}