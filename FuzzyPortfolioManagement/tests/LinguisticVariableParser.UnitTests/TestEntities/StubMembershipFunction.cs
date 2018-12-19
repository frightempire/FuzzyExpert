using LinguisticVariableParser.Entities;

namespace LinguisticVariableParser.UnitTests.TestEntities
{
    public class StubMembershipFunction: MembershipFunction
    {
        public StubMembershipFunction(string linguisticVariableName) : base(linguisticVariableName)
        { }
    }
}