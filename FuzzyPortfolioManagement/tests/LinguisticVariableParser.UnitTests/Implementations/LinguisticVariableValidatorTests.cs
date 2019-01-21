using System;
using LinguisticVariableParser.Implementations;
using NUnit.Framework;

namespace LinguisticVariableParser.UnitTests.Implementations
{
    [TestFixture]
    public class LinguisticVariableValidatorTests
    {
        private LinguisticVariableValidator _linguisticVariableValidator;

        [SetUp]
        public void SetUp()
        {
            _linguisticVariableValidator = new LinguisticVariableValidator();
        }

        [Test]
        public void ValidateLinguisticVariable_LinguisticVariableLevel_ThrowsArgumentExceptionIfThereAreWhitespacesInIt()
        {
            // Arrange
            string linguisticVariable = "Water : Initial : [Cold:Trapezoidal:(0,20,20,30)|Hot:Trapezoidal(50,60,60,80)";
            string exceptionMessage = "Linguistic variable string is not valid: haven't been preprocessed";

            // Act & Assert
            Assert.Throws<ArgumentException>
            (
                () => { _linguisticVariableValidator.ValidateLinguisticVariable(linguisticVariable); },
                exceptionMessage
            );
        }

        [Test]
        public void ValidateLinguisticVariable_LinguisticVariableLevel_ThrowsArgumentExceptionIfThereIsNotEnoughBrackets()
        {
            // Arrange
            string linguisticVariable = "Water:Initial:[Cold:Trapezoidal:(0,20,20,30)|Hot:Trapezoidal(50,60,60,80)";
            string exceptionMessage = "Linguistic variable string is not valid: incorrect brackets for membership functions";

            // Act & Assert
            Assert.Throws<ArgumentException>
            (
                () => { _linguisticVariableValidator.ValidateLinguisticVariable(linguisticVariable); },
                exceptionMessage
            );
        }

        [Test]
        public void ValidateLinguisticVariable_LinguisticVariableLevel_ThrowsArgumentExceptionIfOpeningBracketIsIncorrect()
        {
            // Arrange
            string linguisticVariable = "Water:Initial:]Cold:Trapezoidal:(0,20,20,30)|Hot:Trapezoidal(50,60,60,80)]";
            string exceptionMessage = "Linguistic variable string is not valid: incorrect brackets for membership functions";

            // Act & Assert
            Assert.Throws<ArgumentException>
            (
                () => { _linguisticVariableValidator.ValidateLinguisticVariable(linguisticVariable); },
                exceptionMessage
            );
        }

        [Test]
        public void ValidateLinguisticVariable_LinguisticVariableLevel_ThrowsArgumentExceptionIfClosingBracketIsIncorrect()
        {
            // Arrange
            string linguisticVariable = "Water:Initial:[Cold:Trapezoidal:(0,20,20,30)|Hot:Trapezoidal(50,60,60,80)[";
            string exceptionMessage = "Linguistic variable string is not valid: incorrect brackets for membership functions";

            // Act & Assert
            Assert.Throws<ArgumentException>
            (
                () => { _linguisticVariableValidator.ValidateLinguisticVariable(linguisticVariable); },
                exceptionMessage
            );
        }

        [Test]
        public void ValidateLinguisticVariable_LinguisticVariableLevel_ThrowsArgumentExceptionIfFirstColunIsAfterOpeningBracket()
        {
            // Arrange
            string linguisticVariable = "WaterInitial[Cold:Trapezoidal:(0,20,20,30)|Hot:Trapezoidal(50,60,60,80)]";
            string exceptionMessage = "Linguistic variable string is not valid: colon delimeters placed incorrectly";

            // Act & Assert
            Assert.Throws<ArgumentException>
            (
                () => { _linguisticVariableValidator.ValidateLinguisticVariable(linguisticVariable); },
                exceptionMessage
            );
        }

        [Test]
        public void ValidateLinguisticVariable_LinguisticVariableLevel_ThrowsArgumentExceptionIfFirstColunIsAfterClosingBracket()
        {
            // Arrange
            string linguisticVariable = "WaterInitial[ColdTrapezoidal(0,20,20,30)|HotTrapezoidal(50,60,60,80)]:";
            string exceptionMessage = "Linguistic variable string is not valid: colon delimeters placed incorrectly";

            // Act & Assert
            Assert.Throws<ArgumentException>
            (
                () => { _linguisticVariableValidator.ValidateLinguisticVariable(linguisticVariable); },
                exceptionMessage
            );
        }

        [Test]
        public void ValidateLinguisticVariable_MembershipFunctionsLevel_ThrowsArgumentExceptionIfThereIsOneMembershipFunction()
        {
            // Arrange
            string linguisticVariable = "Water:Initial:[Cold:Trapezoidal:(0,20,20,30)]";
            string exceptionMessage = "Linguistic variable membership functions are not valid: no enough brackets";

            // Act & Assert
            Assert.Throws<ArgumentException>
            (
                () => { _linguisticVariableValidator.ValidateLinguisticVariable(linguisticVariable); },
                exceptionMessage
            );
        }

        [Test]
        public void ValidateLinguisticVariable_MembershipFunctionsLevel_ThrowsArgumentExceptionIfThereAreOddNumberOfBrackets()
        {
            // Arrange
            string linguisticVariable = "Water:Initial:[Cold:Trapezoidal:(0,20,20,30)|Hot:Trapezoidal(50,60,60,80]";
            string exceptionMessage = "Linguistic variable membership functions are not valid: odd count of brackets";

            // Act & Assert
            Assert.Throws<ArgumentException>
            (
                () => { _linguisticVariableValidator.ValidateLinguisticVariable(linguisticVariable); },
                exceptionMessage
            );
        }

        [Test]
        public void ValidateLinguisticVariable_MembershipFunctionsLevel_ThrowsArgumentExceptionIfOpeningBracketIsIncorrect()
        {
            // Arrange
            string linguisticVariable = "Water:Initial:[Cold:Trapezoidal:)0,20,20,30)|Hot:Trapezoidal(50,60,60,80)]";
            string exceptionMessage = "Linguistic variable membership functions are not valid: first or last brackets are wrong";

            // Act & Assert
            Assert.Throws<ArgumentException>
            (
                () => { _linguisticVariableValidator.ValidateLinguisticVariable(linguisticVariable); },
                exceptionMessage
            );
        }

        [Test]
        public void ValidateLinguisticVariable_MembershipFunctionsLevel_ThrowsArgumentExceptionIfClosingBracketIsIncorrect()
        {
            // Arrange
            string linguisticVariable = "Water:Initial:[Cold:Trapezoidal:(0,20,20,30)|Hot:Trapezoidal(50,60,60,80(]";
            string exceptionMessage = "Linguistic variable membership functions are not valid: first or last brackets are wrong";

            // Act & Assert
            Assert.Throws<ArgumentException>
            (
                () => { _linguisticVariableValidator.ValidateLinguisticVariable(linguisticVariable); },
                exceptionMessage
            );
        }

        [Test]
        public void ValidateLinguisticVariable_MembershipFunctionsLevel_ThrowsArgumentExceptionIfBracketsAreMismatching()
        {
            // Arrange
            string linguisticVariable = "Water:Initial:[Cold:Trapezoidal:(0,20,20,30)|Hot:Trapezoidal)50,60,60,80)]";
            string exceptionMessage = "Linguistic variable membership functions are not valid: mismatching brackets";

            // Act & Assert
            Assert.Throws<ArgumentException>
            (
                () => { _linguisticVariableValidator.ValidateLinguisticVariable(linguisticVariable); },
                exceptionMessage
            );
        }

        [Test]
        public void ValidateLinguisticVariable_MembershipFunctionsLevel_ThrowsArgumentExceptionIfThereAreNoColuns()
        {
            // Arrange
            string linguisticVariable = "Water:Initial:[ColdTrapezoidal(0,20,20,30)|HotTrapezoidal(50,60,60,80)]";
            string exceptionMessage = "Linguistic variable membership functions are not valid: incorrect colun placement";

            // Act & Assert
            Assert.Throws<ArgumentException>
            (
                () => { _linguisticVariableValidator.ValidateLinguisticVariable(linguisticVariable); },
                exceptionMessage
            );
        }

        [Test]
        public void ValidateLinguisticVariable_MembershipFunctionsLevel_ThrowsArgumentExceptionIfThereIsOnlyOneColun()
        {
            // Arrange
            string linguisticVariable = "Water:Initial:[Cold:Trapezoidal(0,20,20,30)|HotTrapezoidal(50,60,60,80)]";
            string exceptionMessage = "Linguistic variable membership functions are not valid: incorrect colun placement";

            // Act & Assert
            Assert.Throws<ArgumentException>
            (
                () => { _linguisticVariableValidator.ValidateLinguisticVariable(linguisticVariable); },
                exceptionMessage
            );
        }

        [Test]
        public void ValidateLinguisticVariable_MembershipFunctionsLevel_ThrowsArgumentExceptionIfFirstColonIsAfterBrackets()
        {
            // Arrange
            string linguisticVariable = "Water:Initial:[ColdTrapezoidal(0,20,20,30)|Hot:Trapezoidal:(50,60,60,80)]";
            string exceptionMessage = "Linguistic variable membership functions are not valid: incorrect colun placement";

            // Act & Assert
            Assert.Throws<ArgumentException>
            (
                () => { _linguisticVariableValidator.ValidateLinguisticVariable(linguisticVariable); },
                exceptionMessage
            );
        }

        [Test]
        public void ValidateLinguisticVariable_MembershipFunctionsLevel_ThrowsArgumentExceptionIfSecondColonIsAfterBrackets()
        {
            // Arrange
            string linguisticVariable = "Water:Initial:[Cold:Trapezoidal(0,20,20,30)|Hot:Trapezoidal:(50,60,60,80)]";
            string exceptionMessage = "Linguistic variable membership functions are not valid: incorrect colun placement";

            // Act & Assert
            Assert.Throws<ArgumentException>
            (
                () => { _linguisticVariableValidator.ValidateLinguisticVariable(linguisticVariable); },
                exceptionMessage
            );
        }

        [Test]
        public void ValidateLinguisticVariable_MembershipFunctionsLevel_ThrowsArgumentExceptionIfThereIsNothingBeforeFirstColun()
        {
            // Arrange
            string linguisticVariable = "Water:Initial:[:Trapezoidal:(0,20,20,30)|Hot:Trapezoidal:(50,60,60,80)]";
            string exceptionMessage = "Linguistic variable membership functions are not valid: incorrect colun placement";

            // Act & Assert
            Assert.Throws<ArgumentException>
            (
                () => { _linguisticVariableValidator.ValidateLinguisticVariable(linguisticVariable); },
                exceptionMessage
            );
        }

        [Test]
        public void ValidateLinguisticVariable_MembershipFunctionsLevel_ThrowsArgumentExceptionIfThereIsDelimeterBeforeFirstColun()
        {
            // Arrange
            string linguisticVariable = "Water:Initial:[Cold:Trapezoidal:(0,20,20,30)|:Trapezoidal:(50,60,60,80)]";
            string exceptionMessage = "Linguistic variable membership functions are not valid: incorrect colun placement";

            // Act & Assert
            Assert.Throws<ArgumentException>
            (
                () => { _linguisticVariableValidator.ValidateLinguisticVariable(linguisticVariable); },
                exceptionMessage
            );
        }

        [Test]
        public void ValidateLinguisticVariable_MembershipFunctionsLevel_ThrowsArgumentExceptionIfThereIsClosingBracketBeforeFirstColun()
        {
            // Arrange
            string linguisticVariable = "Water:Initial:[Cold:Trapezoidal:(0,20,20,30):Trapezoidal:(50,60,60,80)]";
            string exceptionMessage = "Linguistic variable membership functions are not valid: incorrect colun placement";

            // Act & Assert
            Assert.Throws<ArgumentException>
            (
                () => { _linguisticVariableValidator.ValidateLinguisticVariable(linguisticVariable); },
                exceptionMessage
            );
        }

        [Test]
        public void ValidateLinguisticVariable_MembershipFunctionsLevel_ThrowsArgumentExceptionIfThereIsColunBeforeSecondColun()
        {
            // Arrange
            string linguisticVariable = "Water:Initial:[Cold::(0,20,20,30)|Hot:Trapezoidal:(50,60,60,80)]";
            string exceptionMessage = "Linguistic variable membership functions are not valid: incorrect colun placement";

            // Act & Assert
            Assert.Throws<ArgumentException>
            (
                () => { _linguisticVariableValidator.ValidateLinguisticVariable(linguisticVariable); },
                exceptionMessage
            );
        }

        [Test]
        public void ValidateLinguisticVariable_MembershipFunctionsLevel_ThrowsArgumentExceptionIfThereIsNoOpeningBracketAfterSecondColun()
        {
            // Arrange
            string linguisticVariable = "Water:Initial:[Cold:Trapezoidal:(0,20,20,30)|Hot:Trapezoidal:50,60,60,80)]";
            string exceptionMessage = "Linguistic variable membership functions are not valid: incorrect colun placement";

            // Act & Assert
            Assert.Throws<ArgumentException>
            (
                () => { _linguisticVariableValidator.ValidateLinguisticVariable(linguisticVariable); },
                exceptionMessage
            );
        }

        [Test]
        public void ValidateLinguisticVariable_MembershipFunctionsLevel_ThrowsArgumentExceptionIfThereAreEmptyBrackets()
        {
            // Arrange
            string linguisticVariable = "Water:Initial:[Cold:Trapezoidal:(0,20,20,30)|Hot:Trapezoidal:()]";
            string exceptionMessage = "Linguistic variable membership functions are not valid: empty brackets";

            // Act & Assert
            Assert.Throws<ArgumentException>
            (
                () => { _linguisticVariableValidator.ValidateLinguisticVariable(linguisticVariable); },
                exceptionMessage
            );
        }

        [Test]
        public void ValidateLinguisticVariable_MembershipFunctionsLevel_ThrowsArgumentExceptionIfThereAreNotEnoughDelimeters()
        {
            // Arrange
            string linguisticVariable = "Water:Initial:[Cold:Trapezoidal:(0,20,20,30)Hot:Trapezoidal:(50,60,60,80)]";
            string exceptionMessage = "Linguistic variable membership functions are not valid: missing delimiter between functions";

            // Act & Assert
            Assert.Throws<ArgumentException>
            (
                () => { _linguisticVariableValidator.ValidateLinguisticVariable(linguisticVariable); },
                exceptionMessage
            );
        }
    }
}
