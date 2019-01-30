using MembershipFunctionParser.Entities;

namespace MembershipFunctionParser.UnitTests.TestEntities
{
    public class StubMembershipFunction: MembershipFunction
    {
        public StubMembershipFunction(string linguisticVariableName) : base(linguisticVariableName)
        { }
    }
}