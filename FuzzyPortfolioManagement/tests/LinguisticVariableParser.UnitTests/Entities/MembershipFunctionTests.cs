using LinguisticVariableParser.UnitTests.TestEntities;
using NUnit.Framework;

namespace LinguisticVariableParser.UnitTests.Entities
{
    [TestFixture]
    public class MembershipFunctionTests
    {
        private const string LinquisticVariableName = "StubName";
        private StubMembershipFunction _membershipFunction;

        [SetUp]
        public void SetUp()
        {
            _membershipFunction = new StubMembershipFunction(LinquisticVariableName);
        }

        [Test]
        public void Constructor_SetsLinguisticVariableNameProperty()
        {
            // Assert
            Assert.AreEqual(LinquisticVariableName, _membershipFunction.LinguisticVariableName);
        }

        [Test]
        public void LinguisticVariableName_GetterReturnsValue()
        {
            // Act
            string actualLinquisticVariableName = _membershipFunction.LinguisticVariableName;

            // Assert
            Assert.AreEqual(LinquisticVariableName, actualLinquisticVariableName);
        }
    }
}