using System.Collections.Generic;
using FuzzyExpert.Core.Entities;
using FuzzyExpert.Core.Enums;

namespace FuzzyExpert.Infrastructure.MembershipFunctionParsing.Interfaces
{
    public interface IMembershipFunctionCreator
    {
        MembershipFunction CreateMembershipFunctionEntity(MembershipFunctionType membershipFunctionType, string membershipFunctionName, List<double> points);
    }
}