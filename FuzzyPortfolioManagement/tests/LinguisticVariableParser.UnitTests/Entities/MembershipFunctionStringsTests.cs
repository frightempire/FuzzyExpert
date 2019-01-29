using System;
using System.Collections.Generic;
using LinguisticVariableParser.Entities;
using NUnit.Framework;

namespace LinguisticVariableParser.UnitTests.Entities
{
    [TestFixture]
    public class MembershipFunctionStringsTests
    {
        private const string MembershipFunctionName = "Cold";
        private const string MembershipFunctionType = "Trapezoidal";
        private readonly List<double> MembershipFunctionValues = new List<double> {0, 20, 20, 30};
        private MembershipFunctionStrings _membershipFunctionStrings;

        [SetUp]
        public void SetUp()
        {
            _membershipFunctionStrings = new MembershipFunctionStrings(
                MembershipFunctionName, MembershipFunctionType, MembershipFunctionValues);
        }

        [Test]
        public void Constructor_ThrowsArgumentNullExceptionIfMembershipFunctionNameIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                new MembershipFunctionStrings(null, MembershipFunctionType, MembershipFunctionValues);
            });
        }

        [Test]
        public void Constructor_ThrowsArgumentNullExceptionIfMembershipFunctionNameIsEmpty()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                new MembershipFunctionStrings(string.Empty, MembershipFunctionType, MembershipFunctionValues);
            });
        }

        [Test]
        public void Constructor_ThrowsArgumentNullExceptionIfMembershipFunctionTypeIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                new MembershipFunctionStrings(MembershipFunctionName, null, MembershipFunctionValues);
            });
        }

        [Test]
        public void Constructor_ThrowsArgumentNullExceptionIfMembershipFunctionTypeIsEmpty()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                new MembershipFunctionStrings(MembershipFunctionName, string.Empty, MembershipFunctionValues);
            });
        }

        [Test]
        public void Constructor_ThrowsArgumentNullExceptionIfMembershipFunctionValuesIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                new MembershipFunctionStrings(MembershipFunctionName, MembershipFunctionType, null);
            });
        }

        [Test]
        public void Constructor_ThrowsArgumentExceptionIfMembershipFunctionValuesIsEmpty()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
            {
                new MembershipFunctionStrings(MembershipFunctionName, MembershipFunctionType, new List<double>());
            });
        }

        [Test]
        public void MembershipFunctionName_GetterReturnsValue()
        {
            // Act
            string actualMembershipFunctionName = _membershipFunctionStrings.MembershipFunctionName;

            // Assert
            Assert.AreEqual(MembershipFunctionName, actualMembershipFunctionName);
        }

        [Test]
        public void MembershipFunctionType_GetterReturnsValue()
        {
            // Act
            string actualMembershipFunctionType = _membershipFunctionStrings.MembershipFunctionType;

            // Assert
            Assert.AreEqual(MembershipFunctionType, actualMembershipFunctionType);
        }

        [Test]
        public void MembershipFunctionValues_GetterReturnsValue()
        {
            // Act
            List<double> actualMembershipFunctionValues = _membershipFunctionStrings.MembershipFunctionValues;

            // Assert
            Assert.AreEqual(MembershipFunctionValues, actualMembershipFunctionValues);
        }
    }
}