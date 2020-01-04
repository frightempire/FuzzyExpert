using FuzzyExpert.Core.Entities;

namespace FuzzyExpert.Infrastructure.UnitTests.MembershipFunctionParsing.TestEntities
{
    public class StubMembershipFunction: MembershipFunction
    {
        public StubMembershipFunction(string linguisticVariableName) : base(linguisticVariableName) { }

        public override double MembershipDegree(double value) => 0;
    }
}