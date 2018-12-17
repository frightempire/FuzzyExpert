using MembershipFunctionManager.Entities;

namespace MembershipFuctionManager.UnitTests.TestEntities
{
    public class StubMembershipFunction: MembershipFunction
    {
        public StubMembershipFunction(string linguisticVariableName) : base(linguisticVariableName)
        { }
    }
}