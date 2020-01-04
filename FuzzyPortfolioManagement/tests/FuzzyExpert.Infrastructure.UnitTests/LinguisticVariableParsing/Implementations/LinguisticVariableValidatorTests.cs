using FuzzyExpert.Application.Entities;
using FuzzyExpert.Infrastructure.LinguisticVariableParsing.Implementations;
using FuzzyExpert.Infrastructure.MembershipFunctionParsing.Interfaces;
using NUnit.Framework;
using Rhino.Mocks;

namespace FuzzyExpert.Infrastructure.UnitTests.LinguisticVariableParsing.Implementations
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
            _membershipFunctionValidatorMock
                .Stub(x => x.ValidateMembershipFunctionsPart(Arg<string>.Is.Anything))
                .Return(new ValidationOperationResult());
            _linguisticVariableValidator = new LinguisticVariableValidator(_membershipFunctionValidatorMock);
        }

        [Test]
        public void ValidateLinguisticVariable_ReturnsValidationResultWithErrorIfThereAreWhitespacesInIt()
        {
            // Arrange
            string linguisticVariable = "Water : Initial : [Cold:Trapezoidal:(0,20,20,30)|Hot:Trapezoidal:(50,60,60,80)]";
            string errorMessage = "Linguistic variable string is not valid: haven't been preprocessed";

            // Act
            ValidationOperationResult validationOperationResult = _linguisticVariableValidator.ValidateLinguisticVariable(linguisticVariable);
            
            // Assert
            Assert.AreEqual(false, validationOperationResult.IsSuccess);
            Assert.IsTrue(validationOperationResult.Messages.Contains(errorMessage));
        }

        [Test]
        public void ValidateLinguisticVariable_ReturnsValidationResultWithErrorIfThereIsNotEnoughBrackets()
        {
            // Arrange
            string linguisticVariable = "Water:Initial:[Cold:Trapezoidal:(0,20,20,30)|Hot:Trapezoidal:(50,60,60,80)";
            string errorMessage = "Linguistic variable string is not valid: incorrect brackets for membership functions";

            // Act
            ValidationOperationResult validationOperationResult = _linguisticVariableValidator.ValidateLinguisticVariable(linguisticVariable);

            // Assert
            Assert.AreEqual(false, validationOperationResult.IsSuccess);
            Assert.IsTrue(validationOperationResult.Messages.Contains(errorMessage));
        }

        [Test]
        public void ValidateLinguisticVariable_ReturnsValidationResultWithErrorIfOpeningBracketIsIncorrect()
        {
            // Arrange
            string linguisticVariable = "Water:Initial:]Cold:Trapezoidal:(0,20,20,30)|Hot:Trapezoidal:(50,60,60,80)]";
            string errorMessage = "Linguistic variable string is not valid: incorrect brackets for membership functions";

            // Act
            ValidationOperationResult validationOperationResult = _linguisticVariableValidator.ValidateLinguisticVariable(linguisticVariable);

            // Assert
            Assert.AreEqual(false, validationOperationResult.IsSuccess);
            Assert.IsTrue(validationOperationResult.Messages.Contains(errorMessage));
        }

        [Test]
        public void ValidateLinguisticVariable_ReturnsValidationResultWithErrorIfClosingBracketIsIncorrect()
        {
            // Arrange
            string linguisticVariable = "Water:Initial:[Cold:Trapezoidal:(0,20,20,30)|Hot:Trapezoidal:(50,60,60,80)[";
            string errorMessage = "Linguistic variable string is not valid: incorrect brackets for membership functions";

            // Act
            ValidationOperationResult validationOperationResult = _linguisticVariableValidator.ValidateLinguisticVariable(linguisticVariable);

            // Assert
            Assert.AreEqual(false, validationOperationResult.IsSuccess);
            Assert.IsTrue(validationOperationResult.Messages.Contains(errorMessage));
        }

        [Test]
        public void ValidateLinguisticVariable_ReturnsValidationResultWithErrorIfFirstColumnIsAfterOpeningBracket()
        {
            // Arrange
            string linguisticVariable = "WaterInitial[Cold:Trapezoidal:(0,20,20,30)|Hot:Trapezoidal:(50,60,60,80)]";
            string errorMessage = "Linguistic variable string is not valid: colon delimiters placed incorrectly";

            // Act
            ValidationOperationResult validationOperationResult = _linguisticVariableValidator.ValidateLinguisticVariable(linguisticVariable);

            // Assert
            Assert.AreEqual(false, validationOperationResult.IsSuccess);
            Assert.IsTrue(validationOperationResult.Messages.Contains(errorMessage));
        }

        [Test]
        public void ValidateLinguisticVariable_ReturnsValidationResultWithErrorIfFirstColumnIsAfterClosingBracket()
        {
            // Arrange
            string linguisticVariable = "WaterInitial[ColdTrapezoidal(0,20,20,30)|HotTrapezoidal(50,60,60,80)]:";
            string errorMessage = "Linguistic variable string is not valid: colon delimiters placed incorrectly";

            // Act
            ValidationOperationResult validationOperationResult = _linguisticVariableValidator.ValidateLinguisticVariable(linguisticVariable);

            // Assert
            Assert.AreEqual(false, validationOperationResult.IsSuccess);
            Assert.IsTrue(validationOperationResult.Messages.Contains(errorMessage));
        }
    }
}
