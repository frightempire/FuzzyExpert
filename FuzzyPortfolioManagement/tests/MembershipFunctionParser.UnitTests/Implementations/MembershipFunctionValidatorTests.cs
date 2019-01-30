using System;
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
        public void ValidateMembershipFunctions_ThrowsArgumentExceptionIfThereIsOneMembershipFunction()
        {
            // Arrange
            string membershipFunctionPart = "Cold:Trapezoidal:(0,20,20,30)";
            string exceptionMessage = "Linguistic variable membership functions are not valid: no enough brackets";

            // Act & Assert
            Assert.Throws<ArgumentException>
            (
                () => { _membershipFunctionValidator.ValidateMembershipFunctions(membershipFunctionPart); },
                exceptionMessage
            );
        }

        [Test]
        public void ValidateMembershipFunctions_ThrowsArgumentExceptionIfThereAreOddNumberOfBrackets()
        {
            // Arrange
            string membershipFunctionPart = "Cold:Trapezoidal:(0,20,20,30)|Hot:Trapezoidal(50,60,60,80";
            string exceptionMessage = "Linguistic variable membership functions are not valid: odd count of brackets";

            // Act & Assert
            Assert.Throws<ArgumentException>
            (
                () => { _membershipFunctionValidator.ValidateMembershipFunctions(membershipFunctionPart); },
                exceptionMessage
            );
        }

        [Test]
        public void ValidateMembershipFunctions_ThrowsArgumentExceptionIfOpeningBracketIsIncorrect()
        {
            // Arrange
            string membershipFunctionPart = "Cold:Trapezoidal:)0,20,20,30)|Hot:Trapezoidal(50,60,60,80)";
            string exceptionMessage = "Linguistic variable membership functions are not valid: first or last brackets are wrong";

            // Act & Assert
            Assert.Throws<ArgumentException>
            (
                () => { _membershipFunctionValidator.ValidateMembershipFunctions(membershipFunctionPart); },
                exceptionMessage
            );
        }

        [Test]
        public void ValidateMembershipFunctions_ThrowsArgumentExceptionIfClosingBracketIsIncorrect()
        {
            // Arrange
            string membershipFunctionPart = "Cold:Trapezoidal:(0,20,20,30)|Hot:Trapezoidal(50,60,60,80(";
            string exceptionMessage = "Linguistic variable membership functions are not valid: first or last brackets are wrong";

            // Act & Assert
            Assert.Throws<ArgumentException>
            (
                () => { _membershipFunctionValidator.ValidateMembershipFunctions(membershipFunctionPart); },
                exceptionMessage
            );
        }

        [Test]
        public void ValidateMembershipFunctions_ThrowsArgumentExceptionIfBracketsAreMismatching()
        {
            // Arrange
            string membershipFunctionPart = "Cold:Trapezoidal:(0,20,20,30)|Hot:Trapezoidal)50,60,60,80)";
            string exceptionMessage = "Linguistic variable membership functions are not valid: mismatching brackets";

            // Act & Assert
            Assert.Throws<ArgumentException>
            (
                () => { _membershipFunctionValidator.ValidateMembershipFunctions(membershipFunctionPart); },
                exceptionMessage
            );
        }

        [Test]
        public void ValidateMembershipFunctions_ThrowsArgumentExceptionIfThereAreNoColuns()
        {
            // Arrange
            string membershipFunctionPart = "ColdTrapezoidal(0,20,20,30)|HotTrapezoidal(50,60,60,80)";
            string exceptionMessage = "Linguistic variable membership functions are not valid: incorrect colun placement";

            // Act & Assert
            Assert.Throws<ArgumentException>
            (
                () => { _membershipFunctionValidator.ValidateMembershipFunctions(membershipFunctionPart); },
                exceptionMessage
            );
        }

        [Test]
        public void ValidateMembershipFunctions_ThrowsArgumentExceptionIfThereIsOnlyOneColun()
        {
            // Arrange
            string membershipFunctionPart = "Cold:Trapezoidal(0,20,20,30)|HotTrapezoidal(50,60,60,80)";
            string exceptionMessage = "Linguistic variable membership functions are not valid: incorrect colun placement";

            // Act & Assert
            Assert.Throws<ArgumentException>
            (
                () => { _membershipFunctionValidator.ValidateMembershipFunctions(membershipFunctionPart); },
                exceptionMessage
            );
        }

        [Test]
        public void ValidateMembershipFunctions_ThrowsArgumentExceptionIfFirstColonIsAfterBrackets()
        {
            // Arrange
            string membershipFunctionPart = "ColdTrapezoidal(0,20,20,30)|Hot:Trapezoidal:(50,60,60,80)";
            string exceptionMessage = "Linguistic variable membership functions are not valid: incorrect colun placement";

            // Act & Assert
            Assert.Throws<ArgumentException>
            (
                () => { _membershipFunctionValidator.ValidateMembershipFunctions(membershipFunctionPart); },
                exceptionMessage
            );
        }

        [Test]
        public void ValidateMembershipFunctions_ThrowsArgumentExceptionIfSecondColonIsAfterBrackets()
        {
            // Arrange
            string membershipFunctionPart = "Cold:Trapezoidal(0,20,20,30)|Hot:Trapezoidal:(50,60,60,80)";
            string exceptionMessage = "Linguistic variable membership functions are not valid: incorrect colun placement";

            // Act & Assert
            Assert.Throws<ArgumentException>
            (
                () => { _membershipFunctionValidator.ValidateMembershipFunctions(membershipFunctionPart); },
                exceptionMessage
            );
        }

        [Test]
        public void ValidateMembershipFunctions_ThrowsArgumentExceptionIfThereIsNothingBeforeFirstColun()
        {
            // Arrange
            string membershipFunctionPart = ":Trapezoidal:(0,20,20,30)|Hot:Trapezoidal:(50,60,60,80)";
            string exceptionMessage = "Linguistic variable membership functions are not valid: incorrect colun placement";

            // Act & Assert
            Assert.Throws<ArgumentException>
            (
                () => { _membershipFunctionValidator.ValidateMembershipFunctions(membershipFunctionPart); },
                exceptionMessage
            );
        }

        [Test]
        public void ValidateMembershipFunctions_ThrowsArgumentExceptionIfThereIsDelimeterBeforeFirstColun()
        {
            // Arrange
            string membershipFunctionPart = "Cold:Trapezoidal:(0,20,20,30)|:Trapezoidal:(50,60,60,80)";
            string exceptionMessage = "Linguistic variable membership functions are not valid: incorrect colun placement";

            // Act & Assert
            Assert.Throws<ArgumentException>
            (
                () => { _membershipFunctionValidator.ValidateMembershipFunctions(membershipFunctionPart); },
                exceptionMessage
            );
        }

        [Test]
        public void ValidateMembershipFunctions_ThrowsArgumentExceptionIfThereIsClosingBracketBeforeFirstColun()
        {
            // Arrange
            string membershipFunctionPart = "Cold:Trapezoidal:(0,20,20,30):Trapezoidal:(50,60,60,80)";
            string exceptionMessage = "Linguistic variable membership functions are not valid: incorrect colun placement";

            // Act & Assert
            Assert.Throws<ArgumentException>
            (
                () => { _membershipFunctionValidator.ValidateMembershipFunctions(membershipFunctionPart); },
                exceptionMessage
            );
        }

        [Test]
        public void ValidateMembershipFunctions_ThrowsArgumentExceptionIfThereIsColunBeforeSecondColun()
        {
            // Arrange
            string membershipFunctionPart = "Cold::(0,20,20,30)|Hot:Trapezoidal:(50,60,60,80)";
            string exceptionMessage = "Linguistic variable membership functions are not valid: incorrect colun placement";

            // Act & Assert
            Assert.Throws<ArgumentException>
            (
                () => { _membershipFunctionValidator.ValidateMembershipFunctions(membershipFunctionPart); },
                exceptionMessage
            );
        }

        [Test]
        public void ValidateMembershipFunctions_ThrowsArgumentExceptionIfThereIsNoOpeningBracketAfterSecondColun()
        {
            // Arrange
            string membershipFunctionPart = "Cold:Trapezoidal:(0,20,20,30)|Hot:Trapezoidal:50,60,60,80)";
            string exceptionMessage = "Linguistic variable membership functions are not valid: incorrect colun placement";

            // Act & Assert
            Assert.Throws<ArgumentException>
            (
                () => { _membershipFunctionValidator.ValidateMembershipFunctions(membershipFunctionPart); },
                exceptionMessage
            );
        }

        [Test]
        public void ValidateMembershipFunctions_ThrowsArgumentExceptionIfThereAreEmptyBrackets()
        {
            // Arrange
            string membershipFunctionPart = "Cold:Trapezoidal:(0,20,20,30)|Hot:Trapezoidal:()";
            string exceptionMessage = "Linguistic variable membership functions are not valid: empty brackets";

            // Act & Assert
            Assert.Throws<ArgumentException>
            (
                () => { _membershipFunctionValidator.ValidateMembershipFunctions(membershipFunctionPart); },
                exceptionMessage
            );
        }

        [Test]
        public void ValidateMembershipFunctions_ThrowsArgumentExceptionIfThereAreNotEnoughDelimeters()
        {
            // Arrange
            string membershipFunctionPart = "Cold:Trapezoidal:(0,20,20,30)Hot:Trapezoidal:(50,60,60,80)";
            string exceptionMessage = "Linguistic variable membership functions are not valid: missing delimiter between functions";

            // Act & Assert
            Assert.Throws<ArgumentException>
            (
                () => { _membershipFunctionValidator.ValidateMembershipFunctions(membershipFunctionPart); },
                exceptionMessage
            );
        }
    }
}
