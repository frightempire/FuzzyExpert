using System;
using System.Collections.Generic;
using System.ComponentModel;
using Base.UnitTests;
using LinguisticVariableParser.Entities;
using LinguisticVariableParser.Enums;
using LinguisticVariableParser.Implementations;
using NUnit.Framework;

namespace LinguisticVariableParser.UnitTests.Implementations
{
    [TestFixture]
    public class MembershipFunctionFactoryTests
    {
        private MembershipFunctionFactory _membershipFunctionFactory;

        [SetUp]
        public void SetUp()
        {
            _membershipFunctionFactory = new MembershipFunctionFactory();
        }

        [Test]
        public void CreateMembershipFunction_ThrowsInvalidEnumArgumentExceptionIfMembershipFunctionTypeIsUnknown()
        {
            // Arrange
            MembershipFunctionType membershipFunctionType = MembershipFunctionType.Triangular;
            string linguisticVariableName = "High";
            List<double> points = new List<double>();

            // Act & Assert
            Assert.Throws<InvalidEnumArgumentException>(() =>
            {
                _membershipFunctionFactory.CreateMembershipFunction(membershipFunctionType, linguisticVariableName, points);
            });
        }

        [Test]
        public void CreateMembershipFunction_ThrowsArgumentOutOfRangeExceptionForTrapezoidalFunction()
        {
            // Arrange
            MembershipFunctionType membershipFunctionType = MembershipFunctionType.Trapesoidal;
            string linguisticVariableName = "High";
            List<double> points = new List<double> { 1,2 };
            
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _membershipFunctionFactory.CreateMembershipFunction(membershipFunctionType, linguisticVariableName, points);
            });
        }

        [Test]
        public void CreateMembershipFunction_ReturnsCorrectMembershipFunction()
        {
            // Arrange
            MembershipFunctionType membershipFunctionType = MembershipFunctionType.Trapesoidal;
            string linguisticVariableName = "High";
            List<double> points = new List<double> { 1, 2, 3, 5 };

            TrapezoidalMembershipFunction expectedMembershipFunction = new TrapezoidalMembershipFunction(linguisticVariableName,
                points[0], points[1], points[2], points[3]);

            // Act
            TrapezoidalMembershipFunction actualMembershipFunction = (TrapezoidalMembershipFunction)
                _membershipFunctionFactory.CreateMembershipFunction(membershipFunctionType, linguisticVariableName, points);

            // Assert
            Assert.IsNotNull(actualMembershipFunction);
            Assert.IsTrue(ObjectComparer.MembershipFunctionsAreEqual(expectedMembershipFunction, actualMembershipFunction));
        }
    }
}
