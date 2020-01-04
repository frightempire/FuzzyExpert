using FuzzyExpert.Core.UnitTests.TestEntities;
using NUnit.Framework;

namespace FuzzyExpert.Core.UnitTests.Entities
{
    [TestFixture]
    public class MembershipFunctionTests
    {
        private const string LinguisticVariableName = "StubName";
        private StubMembershipFunction _membershipFunction;

        [SetUp]
        public void SetUp()
        {
            _membershipFunction = new StubMembershipFunction(LinguisticVariableName);
        }

        [Test]
        public void Constructor_SetsLinguisticVariableNameProperty()
        {
            // Assert
            Assert.AreEqual(LinguisticVariableName, _membershipFunction.LinguisticVariableName);
        }

        [Test]
        public void Constructor_CreatesEmptyPointsList()
        {
            // Assert
            Assert.IsEmpty(_membershipFunction.PointsList);
        }

        [Test]
        public void LinguisticVariableName_GetterReturnsValue()
        {
            // Act
            string actualLinguisticVariableName = _membershipFunction.LinguisticVariableName;

            // Assert
            Assert.AreEqual(LinguisticVariableName, actualLinguisticVariableName);
        }
    }
}