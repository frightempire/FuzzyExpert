using System;
using System.Collections.Generic;
using LinguisticVariableParser.Entities;
using NUnit.Framework;

namespace LinguisticVariableParser.UnitTests.Entities
{
    [TestFixture]
    public class LinguisticVariableStringsTest
    {
        private const string VariableName = "Water";
        private const string DataOrigin = "Initial";
        private readonly List<MembershipFunctionStrings> _membershipFunctions = new List<MembershipFunctionStrings>
        {
            new MembershipFunctionStrings("Cold", "Trapezoidal", new List<int> {0, 20, 20, 30}),
            new MembershipFunctionStrings("Hot", "Trapezoidal", new List<int> {50, 60, 60, 80})
        };
        private LinguisticVariableStrings _linguisticVariableStrings;

        [SetUp]
        public void SetUp()
        {
            _linguisticVariableStrings = new LinguisticVariableStrings(VariableName, DataOrigin, _membershipFunctions);
        }

        [Test]
        public void Constuctor_ThrowsArgumentNullExceptionIfVariableNameIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                new LinguisticVariableStrings(null, DataOrigin, _membershipFunctions);
            });
        }

        [Test]
        public void Constuctor_ThrowsArgumentNullExceptionIfVariableNameIsEmpty()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                new LinguisticVariableStrings(string.Empty, DataOrigin, _membershipFunctions);
            });
        }

        [Test]
        public void Constuctor_ThrowsArgumentNullExceptionIfDataOriginIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                new LinguisticVariableStrings(VariableName, null, _membershipFunctions);
            });
        }

        [Test]
        public void Constuctor_ThrowsArgumentNullExceptionIfDataOriginIsEmpty()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                new LinguisticVariableStrings(VariableName, string.Empty, _membershipFunctions);
            });
        }

        [Test]
        public void Constuctor_ThrowsArgumentNullExceptionIfMembershipFunctionsIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                new LinguisticVariableStrings(VariableName, DataOrigin, null);
            });
        }

        [Test]
        public void Constuctor_ThrowsArgumentExceptionIfMembershipFunctionsIsEmpty()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
            {
                new LinguisticVariableStrings(VariableName, DataOrigin, new List<MembershipFunctionStrings>());
            });
        }

        [Test]
        public void VariableName_GetterReturnsValue()
        {
            // Act
            string actualVariableName = _linguisticVariableStrings.VariableName;

            // Assert
            Assert.AreEqual(VariableName, actualVariableName);
        }

        [Test]
        public void DataOrigin_GetterReturnsValue()
        {
            // Act
            string actualDataOrigin = _linguisticVariableStrings.DataOrigin;

            // Assert
            Assert.AreEqual(DataOrigin, actualDataOrigin);
        }

        [Test]
        public void MembershipFunctions_GetterReturnsValue()
        {
            // Act
            List<MembershipFunctionStrings> actualMembershipFunctions = _linguisticVariableStrings.MembershipFunctions;

            // Assert
            Assert.AreEqual(_membershipFunctions, actualMembershipFunctions);
        }
    }
}