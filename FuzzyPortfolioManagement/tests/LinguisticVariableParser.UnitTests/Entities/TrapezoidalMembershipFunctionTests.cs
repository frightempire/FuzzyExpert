using System;
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
        public void Constructor_SetsX0Property()
        {
            // Assert
            Assert.AreEqual(X0, _trapezoidalMembershipFunction.X0);
        }

        [Test]
        public void X0_GetterReturnsValue()
        {
            // Act
            double actualX0 = _trapezoidalMembershipFunction.X0;

            // Assert
            Assert.AreEqual(X0, actualX0);
        }

        [Test]
        public void Constructor_SetsX1Property()
        {
            // Assert
            Assert.AreEqual(X1, _trapezoidalMembershipFunction.X1);
        }

        [Test]
        public void X1_GetterReturnsValue()
        {
            // Act
            double actualX1 = _trapezoidalMembershipFunction.X1;

            // Assert
            Assert.AreEqual(X1, actualX1);
        }

        [Test]
        public void Constructor_SetsX2Property()
        {
            // Assert
            Assert.AreEqual(X2, _trapezoidalMembershipFunction.X2);
        }

        [Test]
        public void X2_GetterReturnsValue()
        {
            // Act
            double actualX2 = _trapezoidalMembershipFunction.X2;

            // Assert
            Assert.AreEqual(X2, actualX2);
        }

        [Test]
        public void Constructor_SetsX3Property()
        {
            // Assert
            Assert.AreEqual(X3, _trapezoidalMembershipFunction.X3);
        }

        [Test]
        public void X3_GetterReturnsValue()
        {
            // Act
            double actualX3 = _trapezoidalMembershipFunction.X3;

            // Assert
            Assert.AreEqual(X3, actualX3);
        }
    }
}