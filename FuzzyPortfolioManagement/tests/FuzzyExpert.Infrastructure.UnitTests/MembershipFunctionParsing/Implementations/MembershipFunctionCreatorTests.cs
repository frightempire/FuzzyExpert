using System;
using System.Collections.Generic;
using System.ComponentModel;
using FuzzyExpert.Base.UnitTests;
using FuzzyExpert.Core.Entities;
using FuzzyExpert.Core.Enums;
using FuzzyExpert.Infrastructure.MembershipFunctionParsing.Implementations;
using NUnit.Framework;

namespace FuzzyExpert.Infrastructure.UnitTests.MembershipFunctionParsing.Implementations
{
    [TestFixture]
    public class MembershipFunctionCreatorTests
    {
        private MembershipFunctionCreator _membershipFunctionCreator;

        [SetUp]
        public void SetUp()
        {
            _membershipFunctionCreator = new MembershipFunctionCreator();
        }

        [Test]
        public void CreateMembershipFunctionEntity_ThrowsInvalidEnumArgumentExceptionIfMembershipFunctionTypeIsUnknown()
        {
            // Arrange
            MembershipFunctionType membershipFunctionType = MembershipFunctionType.Triangular;
            string membershipFunctionName = "High";
            List<double> points = new List<double>();

            // Act & Assert
            Assert.Throws<InvalidEnumArgumentException>(() =>
            {
                _membershipFunctionCreator.CreateMembershipFunctionEntity(membershipFunctionType, membershipFunctionName, points);
            });
        }

        [Test]
        public void CreateMembershipFunctionEntity_ThrowsArgumentOutOfRangeExceptionForTrapezoidalFunction()
        {
            // Arrange
            MembershipFunctionType membershipFunctionType = MembershipFunctionType.Trapezoidal;
            string membershipFunctionName = "High";
            List<double> points = new List<double> { 1,2 };
            
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _membershipFunctionCreator.CreateMembershipFunctionEntity(membershipFunctionType, membershipFunctionName, points);
            });
        }

        [Test]
        public void CreateMembershipFunctionEntity_ReturnsCorrectMembershipFunction()
        {
            // Arrange
            MembershipFunctionType membershipFunctionType = MembershipFunctionType.Trapezoidal;
            string membershipFunctionName = "High";
            List<double> points = new List<double> { 1, 2, 3, 5 };

            TrapezoidalMembershipFunction expectedMembershipFunction = new TrapezoidalMembershipFunction(membershipFunctionName,
                points[0], points[1], points[2], points[3]);

            // Act
            TrapezoidalMembershipFunction actualMembershipFunction = (TrapezoidalMembershipFunction)
                _membershipFunctionCreator.CreateMembershipFunctionEntity(membershipFunctionType, membershipFunctionName, points);

            // Assert
            Assert.IsNotNull(actualMembershipFunction);
            Assert.IsTrue(ObjectComparer.MembershipFunctionsAreEqual(expectedMembershipFunction, actualMembershipFunction));
        }
    }
}