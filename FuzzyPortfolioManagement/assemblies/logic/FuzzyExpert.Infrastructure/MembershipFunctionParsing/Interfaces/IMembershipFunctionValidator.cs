using FuzzyExpert.Application.Entities;

namespace FuzzyExpert.Infrastructure.MembershipFunctionParsing.Interfaces
{
    public interface IMembershipFunctionValidator
    {
        ValidationOperationResult ValidateMembershipFunctionsPart(string membershipFunctionsPart);
    }
}