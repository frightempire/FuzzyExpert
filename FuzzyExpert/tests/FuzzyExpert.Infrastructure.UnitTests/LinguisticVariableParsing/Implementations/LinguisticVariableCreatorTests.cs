﻿using System;
using System.Collections.Generic;
using FuzzyExpert.Base.UnitTests;
using FuzzyExpert.Core.Entities;
using FuzzyExpert.Core.Enums;
using FuzzyExpert.Infrastructure.LinguisticVariableParsing.Entities;
using FuzzyExpert.Infrastructure.LinguisticVariableParsing.Implementations;
using FuzzyExpert.Infrastructure.LinguisticVariableParsing.Interfaces;
using FuzzyExpert.Infrastructure.MembershipFunctionParsing.Entities;
using FuzzyExpert.Infrastructure.MembershipFunctionParsing.Interfaces;
using NUnit.Framework;
using Rhino.Mocks;

namespace FuzzyExpert.Infrastructure.UnitTests.LinguisticVariableParsing.Implementations
{
    [TestFixture]
    public class LinguisticVariableCreatorTests
    {
        private IMembershipFunctionCreator _membershipFunctionCreatorMock;
        private ILinguisticVariableParser _linguisticVariableParserMock;
        private LinguisticVariableCreator _linguisticVariableCreator;

        [SetUp]
        public void SetUp()
        {
            _membershipFunctionCreatorMock = MockRepository.GenerateMock<IMembershipFunctionCreator>();
            _linguisticVariableParserMock = MockRepository.GenerateMock<ILinguisticVariableParser>();
            _linguisticVariableCreator = new LinguisticVariableCreator(_membershipFunctionCreatorMock, _linguisticVariableParserMock);
        }

        [Test]
        public void Constructor_ExpectedBehavior()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => { new LinguisticVariableCreator(null, _linguisticVariableParserMock); });
            Assert.Throws<ArgumentNullException>(() => { new LinguisticVariableCreator(_membershipFunctionCreatorMock, null); });
        }

        [Test]
        public void CreateLinguisticVariableEntity_ReturnsCorrectLinguisticVariable()
        {
            // Arrange
            List<double> firstFunctionValues = new List<double> { 0, 20, 20, 30 };
            List<double> secondFunctionValues = new List<double> { 50, 60, 60, 80 };
            List<MembershipFunctionStrings> membershipFunctionStringsList = new List<MembershipFunctionStrings>
            {
                new MembershipFunctionStrings("Cold", "Trapezoidal", firstFunctionValues),
                new MembershipFunctionStrings("Hot", "Trapezoidal", secondFunctionValues)
            };
            string linguisticVariable = "Water:Initial:[Cold:Trapezoidal:(0,20,20,30)|Hot:Trapezoidal:(50,60,60,80)]";
            var linguisticVariableStrings = new LinguisticVariableStrings("Water", "Initial", membershipFunctionStringsList);
            var linguisticVariableStringsList = new List<LinguisticVariableStrings> {linguisticVariableStrings};

            _linguisticVariableParserMock.Expect(x => x.ParseLinguisticVariable(linguisticVariable)).Return(linguisticVariableStringsList);

            var firstMembershipFunction = new TrapezoidalMembershipFunction("Cold", 0, 20, 20, 30);
            var secondMembershipFunction = new TrapezoidalMembershipFunction("Hot", 50, 60, 60, 80);

            _membershipFunctionCreatorMock.Expect(x => x.CreateMembershipFunctionEntity(MembershipFunctionType.Trapezoidal, "Cold", firstFunctionValues))
                .Return(firstMembershipFunction);
            _membershipFunctionCreatorMock.Expect(x => x.CreateMembershipFunctionEntity(MembershipFunctionType.Trapezoidal, "Hot", secondFunctionValues))
                .Return(secondMembershipFunction);

            var expectedLinguisticVariable = new LinguisticVariable(
                "Water",
                new MembershipFunctionList {firstMembershipFunction, secondMembershipFunction},
                true);

            // Act
            var actualLinguisticVariables = _linguisticVariableCreator.CreateLinguisticVariableEntities(linguisticVariable);

            // Assert
            Assert.AreEqual(1, actualLinguisticVariables.Count);
            Assert.IsTrue(ObjectComparer.LinguisticVariablesAreEqual(expectedLinguisticVariable, actualLinguisticVariables[0]));
        }
    }
}