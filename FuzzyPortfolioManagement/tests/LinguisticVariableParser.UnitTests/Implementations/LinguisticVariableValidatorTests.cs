using System;
using LinguisticVariableParser.Implementations;
using MembershipFunctionParser.Interfaces;
using NUnit.Framework;
using Rhino.Mocks;

namespace LinguisticVariableParser.UnitTests.Implementations
{
    [TestFixture]
    public class LinguisticVariableValidatorTests
    {
        private IMembershipFunctionValidator _membershipFunctionValidatorMock;
        private LinguisticVariableValidator _linguisticVariableValidator;

        [SetUp]
        public void SetUp()
        {
            _membershipFunctionValidatorMock = MockRepository.GenerateMock<IMembershipFunctionValidator>();
            _linguisticVariableValidator = new LinguisticVariableValidator(_membershipFunctionValidatorMock);
        }

        [Test]
        public void ValidateLinguisticVariable_ThrowsArgumentExceptionIfThereAreWhitespacesInIt()
        {
            // Arrange
            string linguisticVariable = "Water : Initial : [Cold:Trapezoidal:(0,20,20,30)|Hot:Trapezoidal:(50,60,60,80)]";
            string exceptionMessage = "Linguistic variable string is not valid: haven't been preprocessed";

            // Act & Assert
            ArgumentException exception = Assert.Throws<ArgumentException>
            (
                () => { _linguisticVariableValidator.ValidateLinguisticVariable(linguisticVariable); }
            );
            Assert.AreEqual(exceptionMessage, exception.Message);
        }

        [Test]
        public void ValidateLinguisticVariable_ThrowsArgumentExceptionIfThereIsNotEnoughBrackets()
        {
            // Arrange
            string linguisticVariable = "Water:Initial:[Cold:Trapezoidal:(0,20,20,30)|Hot:Trapezoidal:(50,60,60,80)";
            string exceptionMessage = "Linguistic variable string is not valid: incorrect brackets for membership functions";

            // Act & Assert
            ArgumentException exception = Assert.Throws<ArgumentException>
            (
                () => { _linguisticVariableValidator.ValidateLinguisticVariable(linguisticVariable); }
            );
            Assert.AreEqual(exceptionMessage, exception.Message);
        }

        [Test]
        public void ValidateLinguisticVariable_ThrowsArgumentExceptionIfOpeningBracketIsIncorrect()
        {
            // Arrange
            string linguisticVariable = "Water:Initial:]Cold:Trapezoidal:(0,20,20,30)|Hot:Trapezoidal:(50,60,60,80)]";
            string exceptionMessage = "Linguistic variable string is not valid: incorrect brackets for membership functions";

            // Act & Assert
            ArgumentException exception = Assert.Throws<ArgumentException>
            (
                () => { _linguisticVariableValidator.ValidateLinguisticVariable(linguisticVariable); }
            );
            Assert.AreEqual(exceptionMessage, exception.Message);
        }

        [Test]
        public void ValidateLinguisticVariable_ThrowsArgumentExceptionIfClosingBracketIsIncorrect()
        {
            // Arrange
            string linguisticVariable = "Water:Initial:[Cold:Trapezoidal:(0,20,20,30)|Hot:Trapezoidal:(50,60,60,80)[";
            string exceptionMessage = "Linguistic variable string is not valid: incorrect brackets for membership functions";

            // Act & Assert
            ArgumentException exception = Assert.Throws<ArgumentException>
            (
                () => { _linguisticVariableValidator.ValidateLinguisticVariable(linguisticVariable); }
            );
            Assert.AreEqual(exceptionMessage, exception.Message);
        }

        [Test]
        public void ValidateLinguisticVariable_ThrowsArgumentExceptionIfFirstColunIsAfterOpeningBracket()
        {
            // Arrange
            string linguisticVariable = "WaterInitial[Cold:Trapezoidal:(0,20,20,30)|Hot:Trapezoidal:(50,60,60,80)]";
            string exceptionMessage = "Linguistic variable string is not valid: colon delimeters placed incorrectly";

            // Act & Assert
            ArgumentException exception = Assert.Throws<ArgumentException>
            (
                () => { _linguisticVariableValidator.ValidateLinguisticVariable(linguisticVariable); }
            );
            Assert.AreEqual(exceptionMessage, exception.Message);
        }

        [Test]
        public void ValidateLinguisticVariable_ThrowsArgumentExceptionIfFirstColunIsAfterClosingBracket()
        {
            // Arrange
            string linguisticVariable = "WaterInitial[ColdTrapezoidal(0,20,20,30)|HotTrapezoidal(50,60,60,80)]:";
            string exceptionMessage = "Linguistic variable string is not valid: colon delimeters placed incorrectly";

            // Act & Assert
            ArgumentException exception = Assert.Throws<ArgumentException>
            (
                () => { _linguisticVariableValidator.ValidateLinguisticVariable(linguisticVariable); }
            );
            Assert.AreEqual(exceptionMessage, exception.Message);
        }
    }
}
