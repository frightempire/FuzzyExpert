using System;
using System.Collections.Generic;
using MembershipFunctionParser.Entities;
using MembershipFunctionParser.Enums;
using MembershipFunctionParser.Implementations;
using NUnit.Framework;

namespace MembershipFunctionParser.UnitTests.Implementations
{
    [TestFixture]
    public class MembershipFunctionValidatorTests
    {
        private MembershipFunctionValidator _membershipFunctionValidator;

        [SetUp]
        public void SetUp()
        {
            _membershipFunctionValidator = new MembershipFunctionValidator();
        }

        [Test]
        public void ValidateMembershipFunctionsPart_ThrowsArgumentExceptionIfThereIsOneMembershipFunction()
        {
            // Arrange
            string membershipFunctionPart = "Cold:Trapezoidal:(0,20,20,30)";
            string exceptionMessage = "Linguistic variable membership functions are not valid: no enough brackets";

            // Act & Assert
            ArgumentException exception = Assert.Throws<ArgumentException>
            (
                () => { _membershipFunctionValidator.ValidateMembershipFunctionsPart(membershipFunctionPart); }
            );
            Assert.AreEqual(exceptionMessage, exception.Message);
        }

        [Test]
        public void ValidateMembershipFunctionsPart_ThrowsArgumentExceptionIfThereAreOddNumberOfBrackets()
        {
            // Arrange
            string membershipFunctionPart = "Cold:Trapezoidal:(0,20,20,30)|Warm:Trapezoidal(40,45,45,50)|Hot:Trapezoidal(50,60,60,80";
            string exceptionMessage = "Linguistic variable membership functions are not valid: odd count of brackets";

            // Act & Assert
            ArgumentException exception = Assert.Throws<ArgumentException>
            (
                () => { _membershipFunctionValidator.ValidateMembershipFunctionsPart(membershipFunctionPart); }
            );
            Assert.AreEqual(exceptionMessage, exception.Message);
        }

        [Test]
        public void ValidateMembershipFunctionsPart_ThrowsArgumentExceptionIfOpeningBracketIsIncorrect()
        {
            // Arrange
            string membershipFunctionPart = "Cold:Trapezoidal:)0,20,20,30)|Hot:Trapezoidal(50,60,60,80)";
            string exceptionMessage = "Linguistic variable membership functions are not valid: first or last brackets are wrong";

            // Act & Assert
            ArgumentException exception = Assert.Throws<ArgumentException>
            (
                () => { _membershipFunctionValidator.ValidateMembershipFunctionsPart(membershipFunctionPart); }
            );
            Assert.AreEqual(exceptionMessage, exception.Message);
        }

        [Test]
        public void ValidateMembershipFunctionsPart_ThrowsArgumentExceptionIfClosingBracketIsIncorrect()
        {
            // Arrange
            string membershipFunctionPart = "Cold:Trapezoidal:(0,20,20,30)|Hot:Trapezoidal(50,60,60,80(";
            string exceptionMessage = "Linguistic variable membership functions are not valid: first or last brackets are wrong";

            // Act & Assert
            ArgumentException exception = Assert.Throws<ArgumentException>
            (
                () => { _membershipFunctionValidator.ValidateMembershipFunctionsPart(membershipFunctionPart); }
            );
            Assert.AreEqual(exceptionMessage, exception.Message);
        }

        [Test]
        public void ValidateMembershipFunctionsPart_ThrowsArgumentExceptionIfBracketsAreMismatching()
        {
            // Arrange
            string membershipFunctionPart = "Cold:Trapezoidal:(0,20,20,30)|Hot:Trapezoidal)50,60,60,80)";
            string exceptionMessage = "Linguistic variable membership functions are not valid: mismatching brackets";

            // Act & Assert
            ArgumentException exception = Assert.Throws<ArgumentException>
            (
                () => { _membershipFunctionValidator.ValidateMembershipFunctionsPart(membershipFunctionPart); }
            );
            Assert.AreEqual(exceptionMessage, exception.Message);
        }

        [Test]
        public void ValidateMembershipFunctionsPart_ThrowsArgumentExceptionIfThereAreNoColuns()
        {
            // Arrange
            string membershipFunctionPart = "ColdTrapezoidal(0,20,20,30)|HotTrapezoidal(50,60,60,80)";
            string exceptionMessage = "Linguistic variable membership functions are not valid: incorrect colun placement";

            // Act & Assert
            ArgumentException exception = Assert.Throws<ArgumentException>
            (
                () => { _membershipFunctionValidator.ValidateMembershipFunctionsPart(membershipFunctionPart); }
            );
            Assert.AreEqual(exceptionMessage, exception.Message);
        }

        [Test]
        public void ValidateMembershipFunctionsPart_ThrowsArgumentExceptionIfThereIsOnlyOneColun()
        {
            // Arrange
            string membershipFunctionPart = "Cold:Trapezoidal(0,20,20,30)|HotTrapezoidal(50,60,60,80)";
            string exceptionMessage = "Linguistic variable membership functions are not valid: incorrect colun placement";

            // Act & Assert
            ArgumentException exception = Assert.Throws<ArgumentException>
            (
                () => { _membershipFunctionValidator.ValidateMembershipFunctionsPart(membershipFunctionPart); }
            );
            Assert.AreEqual(exceptionMessage, exception.Message);
        }

        [Test]
        public void ValidateMembershipFunctionsPart_ThrowsArgumentExceptionIfFirstColonIsAfterBrackets()
        {
            // Arrange
            string membershipFunctionPart = "ColdTrapezoidal(0,20,20,30)|Hot:Trapezoidal:(50,60,60,80)";
            string exceptionMessage = "Linguistic variable membership functions are not valid: incorrect colun placement";

            // Act & Assert
            ArgumentException exception = Assert.Throws<ArgumentException>
            (
                () => { _membershipFunctionValidator.ValidateMembershipFunctionsPart(membershipFunctionPart); }
            );
            Assert.AreEqual(exceptionMessage, exception.Message);
        }

        [Test]
        public void ValidateMembershipFunctionsPart_ThrowsArgumentExceptionIfSecondColonIsAfterBrackets()
        {
            // Arrange
            string membershipFunctionPart = "Cold:Trapezoidal(0,20,20,30)|Hot:Trapezoidal:(50,60,60,80)";
            string exceptionMessage = "Linguistic variable membership functions are not valid: incorrect colun placement";

            // Act & Assert
            ArgumentException exception = Assert.Throws<ArgumentException>
            (
                () => { _membershipFunctionValidator.ValidateMembershipFunctionsPart(membershipFunctionPart); }
            );
            Assert.AreEqual(exceptionMessage, exception.Message);
        }

        [Test]
        public void ValidateMembershipFunctionsPart_ThrowsArgumentExceptionIfThereIsNothingBeforeFirstColun()
        {
            // Arrange
            string membershipFunctionPart = ":Trapezoidal:(0,20,20,30)|Hot:Trapezoidal:(50,60,60,80)";
            string exceptionMessage = "Linguistic variable membership functions are not valid: incorrect colun placement";

            // Act & Assert
            ArgumentException exception = Assert.Throws<ArgumentException>
            (
                () => { _membershipFunctionValidator.ValidateMembershipFunctionsPart(membershipFunctionPart); }
            );
            Assert.AreEqual(exceptionMessage, exception.Message);
        }

        [Test]
        public void ValidateMembershipFunctionsPart_ThrowsArgumentExceptionIfThereIsDelimeterBeforeFirstColun()
        {
            // Arrange
            string membershipFunctionPart = "Cold:Trapezoidal:(0,20,20,30)|:Trapezoidal:(50,60,60,80)";
            string exceptionMessage = "Linguistic variable membership functions are not valid: incorrect colun placement";

            // Act & Assert
            ArgumentException exception = Assert.Throws<ArgumentException>
            (
                () => { _membershipFunctionValidator.ValidateMembershipFunctionsPart(membershipFunctionPart); }
            );
            Assert.AreEqual(exceptionMessage, exception.Message);
        }

        [Test]
        public void ValidateMembershipFunctionsPart_ThrowsArgumentExceptionIfThereIsClosingBracketBeforeFirstColun()
        {
            // Arrange
            string membershipFunctionPart = "Cold:Trapezoidal:(0,20,20,30)|:Trapezoidal:(50,60,60,80)";
            string exceptionMessage = "Linguistic variable membership functions are not valid: incorrect colun placement";

            // Act & Assert
            ArgumentException exception = Assert.Throws<ArgumentException>
            (
                () => { _membershipFunctionValidator.ValidateMembershipFunctionsPart(membershipFunctionPart); }
            );
            Assert.AreEqual(exceptionMessage, exception.Message);
        }

        [Test]
        public void ValidateMembershipFunctionsPart_ThrowsArgumentExceptionIfThereIsColunBeforeSecondColun()
        {
            // Arrange
            string membershipFunctionPart = "Cold::(0,20,20,30)|Hot:Trapezoidal:(50,60,60,80)";
            string exceptionMessage = "Linguistic variable membership functions are not valid: incorrect colun placement";

            // Act & Assert
            ArgumentException exception = Assert.Throws<ArgumentException>
            (
                () => { _membershipFunctionValidator.ValidateMembershipFunctionsPart(membershipFunctionPart); }
            );
            Assert.AreEqual(exceptionMessage, exception.Message);
        }

        [Test]
        public void ValidateMembershipFunctionsPart_ThrowsArgumentExceptionIfThereIsNoOpeningBracketAfterSecondColun()
        {
            // Arrange
            string membershipFunctionPart = "Cold:Trapezoidal:(0,20,20,30)|Hot:Trapezoidal:N(50,60,60,80)";
            string exceptionMessage = "Linguistic variable membership functions are not valid: incorrect colun placement";

            // Act & Assert
            ArgumentException exception = Assert.Throws<ArgumentException>
            (
                () => { _membershipFunctionValidator.ValidateMembershipFunctionsPart(membershipFunctionPart); }
            );
            Assert.AreEqual(exceptionMessage, exception.Message);
        }

        [Test]
        public void ValidateMembershipFunctionsPart_ThrowsArgumentExceptionIfThereAreEmptyBrackets()
        {
            // Arrange
            string membershipFunctionPart = "Cold:Trapezoidal:(0,20,20,30)|Hot:Trapezoidal:()";
            string exceptionMessage = "Linguistic variable membership functions are not valid: empty brackets";

            // Act & Assert
            ArgumentException exception = Assert.Throws<ArgumentException>
            (
                () => { _membershipFunctionValidator.ValidateMembershipFunctionsPart(membershipFunctionPart); }
            );
            Assert.AreEqual(exceptionMessage, exception.Message);
        }

        [Test]
        public void ValidateMembershipFunctionsPartPart_ThrowsArgumentExceptionIfThereAreNotEnoughDelimeters()
        {
            // Arrange
            string membershipFunctionPart = "Cold:Trapezoidal:(0,20,20,30)Hot:Trapezoidal:(50,60,60,80)";
            string exceptionMessage = "Linguistic variable membership functions are not valid: missing delimiter between functions";

            // Act & Assert
            ArgumentException exception = Assert.Throws<ArgumentException>
            (
                () => { _membershipFunctionValidator.ValidateMembershipFunctionsPart(membershipFunctionPart); }
            );
            Assert.AreEqual(exceptionMessage, exception.Message);
        }
    }
}
