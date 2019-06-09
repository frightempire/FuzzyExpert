using System;
using System.Collections.Generic;
using MembershipFunctionParser.Entities;
using NUnit.Framework;

namespace MembershipFunctionParser.UnitTests.Entities
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
        public void Constructor_ThrowsArgumentExceptionIfPointsOrderIsViolated()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => { new TrapezoidalMembershipFunction(LinquisticVariableName, X1, X0, X2, X3); });
            Assert.Throws<ArgumentException>(() => { new TrapezoidalMembershipFunction(LinquisticVariableName, X0, X2, X1, X3); });
            Assert.Throws<ArgumentException>(() => { new TrapezoidalMembershipFunction(LinquisticVariableName, X0, X1, X3, X2); });
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

        [Test]
        public void MembershipDegree_ReturnsNotBelongingDegree()
        {
            // Arrange
            double inputValue = 4;
            double expectedDegree = 0;

            // Act
            double actualDegree = _trapezoidalMembershipFunction.MembershipDegree(inputValue);

            // Assert
            Assert.AreEqual(expectedDegree, actualDegree);
        }

        [Test]
        public void MembershipDegree_ReturnsLowDegree()
        {
            // Arrange
            double inputValue = 6;
            double expectedDegree = 0.2;

            // Act
            double actualDegree = _trapezoidalMembershipFunction.MembershipDegree(inputValue);

            // Assert
            Assert.AreEqual(expectedDegree, actualDegree);
        }

        [Test]
        public void MembershipDegree_ReturnsHighDegree()
        {
            // Arrange
            double inputValue = 17;
            double expectedDegree = 0.6;

            // Act
            double actualDegree = _trapezoidalMembershipFunction.MembershipDegree(inputValue);

            // Assert
            Assert.AreEqual(expectedDegree, actualDegree);
        }

        [Test]
        public void MembershipDegree_ReturnsMaxDegree()
        {
            // Arrange
            double inputValue = 12;
            double expectedDegree = 1;

            // Act
            double actualDegree = _trapezoidalMembershipFunction.MembershipDegree(inputValue);

            // Assert
            Assert.AreEqual(expectedDegree, actualDegree);
        }
    }
}