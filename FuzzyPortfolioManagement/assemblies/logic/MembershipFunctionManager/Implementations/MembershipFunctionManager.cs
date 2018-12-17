using MembershipFunctionManager.Interfaces;

namespace MembershipFunctionManager.Implementations
{
    public class MembershipFunctionManager: IMembershipFunctionManager
    {
        public MembershipFunctionList MembershipFunctions { get; }
    }
}