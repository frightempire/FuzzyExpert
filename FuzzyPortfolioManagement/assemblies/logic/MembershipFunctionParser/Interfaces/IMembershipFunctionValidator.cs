using CommonLogic.Entities;

namespace MembershipFunctionParser.Interfaces
{
    public interface IMembershipFunctionValidator
    {
        ValidationOperationResult ValidateMembershipFunctionsPart(string membershipFunctionsPart);
    }
}
