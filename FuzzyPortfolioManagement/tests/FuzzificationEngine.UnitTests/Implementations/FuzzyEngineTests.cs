using FuzzificationEngine.Implementaions;
using LinguisticVariableParser.Entities;
using MembershipFunctionParser.Entities;
using MembershipFunctionParser.Implementations;
using NUnit.Framework;

namespace FuzzificationEngine.UnitTests.Implementations
{
    [TestFixture]
    public class FuzzyEngineTests
    {
        private FuzzyEngine _fuzzyEngine;

        [SetUp]
        public void SetUp()
        {
            _fuzzyEngine = new FuzzyEngine();
        }

        [Test]
        public void Fuzzify_ReturnsMostAppropriateMembershipFunction_FirstFunctionCase()
        {
            // Arrange
            double inputValue = 22;
            string expectedMembershipFunctionName = "Cold";
            LinguisticVariable variable = PrepareLinguisticVariable();

            // Act
            MembershipFunction function = _fuzzyEngine.Fuzzify(variable, inputValue);

            // Assert
            Assert.AreEqual(expectedMembershipFunctionName, function.LinguisticVariableName);
        }

        [Test]
        public void Fuzzify_ReturnsMostAppropriateMembershipFunction_SecondFunctionCase()
        {
            // Arrange
            double inputValue = 43;
            string expectedMembershipFunctionName = "Warm";
            LinguisticVariable variable = PrepareLinguisticVariable();

            // Act
            MembershipFunction function = _fuzzyEngine.Fuzzify(variable, inputValue);

            // Assert
            Assert.AreEqual(expectedMembershipFunctionName, function.LinguisticVariableName);
        }

        [Test]
        public void Fuzzify_ReturnsMostAppropriateMembershipFunction_ContradictionCase()
        {
            // Arrange
            double inputValue = 29;
            string expectedMembershipFunctionName = "Cold";
            LinguisticVariable variable = PrepareLinguisticVariable();

            // Act
            MembershipFunction function = _fuzzyEngine.Fuzzify(variable, inputValue);

            // Assert
            Assert.AreEqual(expectedMembershipFunctionName, function.LinguisticVariableName);
        }

        private LinguisticVariable PrepareLinguisticVariable()
        {
            MembershipFunctionList functionList = new MembershipFunctionList
            {
                new TrapezoidalMembershipFunction("Cold", 0, 20, 25, 30),
                new TrapezoidalMembershipFunction("Warm", 28, 40, 45, 50)
            };
            return new LinguisticVariable("Temperature", functionList, isInitialData: true);
        }
    }
}