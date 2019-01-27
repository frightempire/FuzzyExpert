using System;
using System.Collections.Generic;
using Base.UnitTests;
using LinguisticVariableParser.Entities;
using LinguisticVariableParser.Implementations;
using LinguisticVariableParser.Interfaces;
using NUnit.Framework;
using Rhino.Mocks;

namespace LinguisticVariableParser.UnitTests.Implementations
{
    [TestFixture]
    public class LinguisticVariableParserTests
    {
        private IMembershipFunctionParser _membershipFunctionParserMock;
        private LinguisticVariableParser.Implementations.LinguisticVariableParser _linguisticVariableParser;

        [SetUp]
        public void SetUp()
        {
            _membershipFunctionParserMock = MockRepository.GenerateMock<IMembershipFunctionParser>();
            _linguisticVariableParser = new LinguisticVariableParser.Implementations.LinguisticVariableParser(_membershipFunctionParserMock);
        }

        [Test]
        public void Constructor_ThrowsArgumentNullExceptionIfMembershipFunctionParserIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                new LinguisticVariableParser.Implementations.LinguisticVariableParser(null);
            });
        }

        [Test]
        public void ParseLinguisticVariable_ReturnsCorrectLinguisticVariableString()
        {
            // Arrange
            string linguisticVariable = "Water:Initial:[Cold:Trapezoidal:(0,20,20,30)|Hot:Trapezoidal(50,60,60,80)]";
            List<MembershipFunctionStrings> expectedMembersipFunctioStringsList = new List<MembershipFunctionStrings>
            {
                new MembershipFunctionStrings("Cold", "Trapezoidal", new List<int> {0, 20, 20, 30}),
                new MembershipFunctionStrings("Hot", "Trapezoidal", new List<int> {50, 60, 60, 80})
            };
            LinguisticVariableStrings expectedLinguisticVariableStrings = new LinguisticVariableStrings("Water", "Initial", expectedMembersipFunctioStringsList);

            _membershipFunctionParserMock.Stub(x => x.ParseMembershipFunctions(Arg<string>.Is.Anything))
                .Return(expectedMembersipFunctioStringsList);

            // Act
            LinguisticVariableStrings actualLinguisticVariableStrings = _linguisticVariableParser.ParseLinguisticVariable(linguisticVariable);

            // Assert
            Assert.IsTrue(ObjectComparer.LinguisticVariableStringsAreEqual(expectedLinguisticVariableStrings, actualLinguisticVariableStrings));
        }
    }
}
