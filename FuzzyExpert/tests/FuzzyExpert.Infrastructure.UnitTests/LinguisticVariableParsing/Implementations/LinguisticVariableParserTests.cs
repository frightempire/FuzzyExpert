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
        private FuzzyExpert.Infrastructure.LinguisticVariableParsing.Implementations.LinguisticVariableParser _linguisticVariableParser;

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
        public void ParseLinguisticVariable_ReturnsCorrectLinguisticVariableString()
        {
            // Arrange
            string linguisticVariable = "Water:Initial:[Cold:Trapezoidal:(0,20,20,30)|Hot:Trapezoidal(50,60,60,80)]";
            List<MembershipFunctionStrings> expectedMembershipFunctionStringsList = new List<MembershipFunctionStrings>
            {
                new MembershipFunctionStrings("Cold", "Trapezoidal", new List<double> {0, 20, 20, 30}),
                new MembershipFunctionStrings("Hot", "Trapezoidal", new List<double> {50, 60, 60, 80})
            };
            LinguisticVariableStrings expectedLinguisticVariableStrings = new LinguisticVariableStrings("Water", "Initial", expectedMembershipFunctionStringsList);

            _membershipFunctionParserMock.Stub(x => x.ParseMembershipFunctions(Arg<string>.Is.Anything))
                .Return(expectedMembershipFunctionStringsList);

            // Act
            LinguisticVariableStrings actualLinguisticVariableStrings = _linguisticVariableParser.ParseLinguisticVariable(linguisticVariable);

            // Assert
            Assert.IsTrue(ObjectComparer.LinguisticVariableStringsAreEqual(expectedLinguisticVariableStrings, actualLinguisticVariableStrings));
        }
    }
}