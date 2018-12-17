using MembershipFunctionManager.Implementations;

namespace MembershipFunctionManager.Interfaces
{
    public interface IMembershipFunctionManager
    {
        MembershipFunctionList MembershipFunctions { get; }
    }
}
