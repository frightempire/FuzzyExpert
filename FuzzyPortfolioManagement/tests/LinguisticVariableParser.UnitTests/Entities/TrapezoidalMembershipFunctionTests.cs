using System;
using System.Collections.Generic;
using LinguisticVariableParser.Entities;
using NUnit.Framework;

namespace LinguisticVariableParser.UnitTests.Entities
{
    [TestFixture]
    public class TrapezoidalMembershipFunctionTests
    {
        private const string LinquisticVariableName = "StubName";
        private const double X0 = 5;
        private const double X1 = 10;
        private const double X2 = 15;
        private const double X3 = 20;
        private TrapezoidalMembershipFunction _trapezoidalMembershipFunction;

        [SetUp]
        public void SetUp()
        {
            _trapezoidalMembershipFunction = new TrapezoidalMembershipFunction(LinquisticVariableName, X0, X1, X2, X3);
        }

        [Test]
        public void Constructor_ThrowsArgumentExceptionIfPointsOrderIsViolatedCase1()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => { new TrapezoidalMembershipFunction(LinquisticVariableName, X1, X0, X2, X3); });
        }

        [Test]
        public void Constructor_ThrowsArgumentExceptionIfPointsOrderIsViolatedCase2()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => { new TrapezoidalMembershipFunction(LinquisticVariableName, X0, X2, X1, X3); });
        }

        [Test]
        public void Constructor_ThrowsArgumentExceptionIfPointsOrderIsViolatedCase3()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => { new TrapezoidalMembershipFunction(LinquisticVariableName, X0, X1, X3, X2); });
        }

        [Test]
        public void Constructor_ThrowsArgumentExceptionIfPointsOrderIsViolatedCase4()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => { new TrapezoidalMembershipFunction(LinquisticVariableName, X3, X1, X2, X0); });
        }

        [Test]
        public void Constructor_PopulatesPointsList()
        {
            // Arrange
            List<double> expectedPointsList = new List<double> {X0, X1, X2, X3};

            // Assert
            Assert.AreEqual(expectedPointsList, _trapezoidalMembershipFunction.PointsList);
        }
    }
}