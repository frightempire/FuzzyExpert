using System;
using Base.UnitTests;
using LinguisticVariableParser.Entities;
using MembershipFunctionParser.Entities;
using MembershipFunctionParser.Implementations;
using NUnit.Framework;

namespace LinguisticVariableParser.UnitTests.Entities
{
    [TestFixture]
    public class LinguisticVariableTests
    {
        private const string VariableName = "Capital";
        private MembershipFunctionList _membershipFunctions;
        private const bool IsInitialData = true;
        private LinguisticVariable _linguisticVariable;

        [SetUp]
        public void SetUp()
        {
            PrepareMembershipFunctions();
            _linguisticVariable = new LinguisticVariable(VariableName, _membershipFunctions, IsInitialData);
        }

        private void PrepareMembershipFunctions()
        {
            _membershipFunctions = new MembershipFunctionList
            {
                new TrapezoidalMembershipFunction("Low", 0, 1, 2, 3),
                new TrapezoidalMembershipFunction("Middle", 4, 5, 6, 7),
                new TrapezoidalMembershipFunction("High", 10, 11, 12, 13)
            };
        }

        [Test]
        public void Constructor_ThrowsArgumentNullExceptionIfVariableNameIsEmpty()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                new LinguisticVariable(string.Empty, _membershipFunctions, IsInitialData);
            });
        }

        [Test]
        public void Constructor_ThrowsArgumentNullExceptionIfVariableNameIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                new LinguisticVariable(null, _membershipFunctions, IsInitialData);
            });
        }

        [Test]
        public void Constructor_ThrowsArgumentNullExceptionIfMembershipFunctionListIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                new LinguisticVariable(VariableName, null, IsInitialData);
            });
        }

        [Test]
        public void Constructor_ThrowsArgumentExceptionIfMembershipFunctionListIsEmpty()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
            {
                new LinguisticVariable(VariableName, new MembershipFunctionList(), IsInitialData);
            });
        }

        [Test]
        public void VariableName_GetterReturnsValue()
        {
            // Act
            string actualVariableName = _linguisticVariable.VariableName;

            // Assert
            Assert.AreEqual(VariableName, actualVariableName);
        }

        [Test]
        public void IsInitialData_GetterReturnsValue()
        {
            // Act
            bool actualIsInitialData = _linguisticVariable.IsInitialData;

            // Assert
            Assert.AreEqual(IsInitialData, actualIsInitialData);
        }

        [Test]
        public void MembershipFunctionList_GetterReturnsValue()
        {
            // Act
            MembershipFunctionList membershipFunctionList = _linguisticVariable.MembershipFunctionList;

            // Assert
            Assert.IsTrue(ObjectComparer.MembershipFunctionListsAreEqual(_membershipFunctions, membershipFunctionList));
        }

        [Test]
        public void MinValue_ReturnsCorrectValue()
        {
            // Arrange
            double expectedMinValue = 0;

            // Act
            double actualMinValue = _linguisticVariable.MinValue();

            // Assert
            Assert.AreEqual(expectedMinValue, actualMinValue);
        }

        [Test]
        public void MaxValue_ReturnsCorrectValue()
        {
            // Arrange
            double expectedMaxValue = 13;

            // Act
            double actualMaxValue = _linguisticVariable.MaxValue();

            // Assert
            Assert.AreEqual(expectedMaxValue, actualMaxValue);
        }

        [Test]
        public void ValueRange_ReturnsCorrectValue()
        {
            // Arrange
            double expectedValueRange = 13;

            // Act
            double actualValueRange = _linguisticVariable.ValueRange();

            // Assert
            Assert.AreEqual(expectedValueRange, actualValueRange);
        }
    }
}