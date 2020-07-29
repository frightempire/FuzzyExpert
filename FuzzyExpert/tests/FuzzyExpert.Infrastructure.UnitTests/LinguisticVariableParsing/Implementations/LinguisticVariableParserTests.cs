using System;
using System.Collections.Generic;
using FuzzyExpert.Base.UnitTests;
using FuzzyExpert.Infrastructure.LinguisticVariableParsing.Entities;
using FuzzyExpert.Infrastructure.MembershipFunctionParsing.Entities;
using FuzzyExpert.Infrastructure.MembershipFunctionParsing.Interfaces;
using NUnit.Framework;
using Rhino.Mocks;

namespace FuzzyExpert.Infrastructure.UnitTests.LinguisticVariableParsing.Implementations
{
    [TestFixture]
    public class LinguisticVariableParserTests
    {
        private IMembershipFunctionParser _membershipFunctionParserMock;
        private Infrastructure.LinguisticVariableParsing.Implementations.LinguisticVariableParser _linguisticVariableParser;

        [SetUp]
        public void SetUp()
        {
            _membershipFunctionParserMock = MockRepository.GenerateMock<IMembershipFunctionParser>();
            _linguisticVariableParser = new Infrastructure.LinguisticVariableParsing.Implementations.LinguisticVariableParser(_membershipFunctionParserMock);
        }

        [Test]
        public void Constructor_ThrowsArgumentNullExceptionIfMembershipFunctionParserIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                new Infrastructure.LinguisticVariableParsing.Implementations.LinguisticVariableParser(null);
            });
        }

        [Test]
        public void ParseLinguisticVariable_ReturnsCorrectLinguisticVariableString_WithOneVariable()
        {
            // Arrange
            var membershipFunction = "Cold:Trapezoidal:(0,20,20,30)|Hot:Trapezoidal(50,60,60,80)";
            var linguisticVariable = $"[Water]:Initial:[{membershipFunction}]";
            var expectedMembershipFunctionStringsList = new List<MembershipFunctionStrings>
            {
                new MembershipFunctionStrings("Cold", "Trapezoidal", new List<double> {0, 20, 20, 30}),
                new MembershipFunctionStrings("Hot", "Trapezoidal", new List<double> {50, 60, 60, 80})
            };
            var expectedLinguisticVariableStrings = new LinguisticVariableStrings("Water", "Initial", expectedMembershipFunctionStringsList);

            _membershipFunctionParserMock.Stub(x => x.ParseMembershipFunctions(membershipFunction)).Return(expectedMembershipFunctionStringsList);

            // Act
            var actualLinguisticVariableStringsList = _linguisticVariableParser.ParseLinguisticVariable(linguisticVariable);

            // Assert
            Assert.AreEqual(1, actualLinguisticVariableStringsList.Count);
            Assert.IsTrue(ObjectComparer.LinguisticVariableStringsAreEqual(expectedLinguisticVariableStrings, actualLinguisticVariableStringsList[0]));
        }

        [Test]
        public void ParseLinguisticVariable_ReturnsCorrectLinguisticVariableString_WithTwoVariables()
        {
            // Arrange
            var membershipFunction = "Cold:Trapezoidal:(0,20,20,30)|Hot:Trapezoidal(50,60,60,80)";
            var linguisticVariable = $"[Water,NotWater]:Initial:[{membershipFunction}]";
            var expectedMembershipFunctionStringsList = new List<MembershipFunctionStrings>
            {
                new MembershipFunctionStrings("Cold", "Trapezoidal", new List<double> {0, 20, 20, 30}),
                new MembershipFunctionStrings("Hot", "Trapezoidal", new List<double> {50, 60, 60, 80})
            };
            var firstExpectedLinguisticVariableStrings = new LinguisticVariableStrings("Water", "Initial", expectedMembershipFunctionStringsList);
            var secondExpectedLinguisticVariableStrings = new LinguisticVariableStrings("NotWater", "Initial", expectedMembershipFunctionStringsList);

            _membershipFunctionParserMock.Stub(x => x.ParseMembershipFunctions(membershipFunction)).Return(expectedMembershipFunctionStringsList);

            // Act
            var actualLinguisticVariableStringsList = _linguisticVariableParser.ParseLinguisticVariable(linguisticVariable);

            // Assert
            Assert.AreEqual(2, actualLinguisticVariableStringsList.Count);
            Assert.IsTrue(ObjectComparer.LinguisticVariableStringsAreEqual(firstExpectedLinguisticVariableStrings, actualLinguisticVariableStringsList[0]));
            Assert.IsTrue(ObjectComparer.LinguisticVariableStringsAreEqual(secondExpectedLinguisticVariableStrings, actualLinguisticVariableStringsList[1]));
        }
    }
}