using FuzzyExpert.Application.Entities;
using FuzzyExpert.Infrastructure.MembershipFunctionParsing.Implementations;
using NUnit.Framework;

namespace FuzzyExpert.Infrastructure.UnitTests.MembershipFunctionParsing.Implementations
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
        public void ValidateMembershipFunctionsPart_ReturnsValidationResultWithErrorIfThereIsOneMembershipFunction()
        {
            // Arrange
            string membershipFunctionPart = "Cold:Trapezoidal:(0,20,20,30)";
            string errorMessage = "Linguistic variable membership functions are not valid: no enough brackets";

            // Act
            ValidationOperationResult validationOperationResult =
                _membershipFunctionValidator.ValidateMembershipFunctionsPart(membershipFunctionPart);

            // Assert
            Assert.AreEqual(false, validationOperationResult.IsSuccess);
            Assert.IsTrue(validationOperationResult.Messages.Contains(errorMessage));
        }

        [Test]
        public void ValidateMembershipFunctionsPart_ReturnsValidationResultWithErrorIfThereAreOddNumberOfBrackets()
        {
            // Arrange
            string membershipFunctionPart = "Cold:Trapezoidal:(0,20,20,30)|Warm:Trapezoidal:(40,45,45,50)|Hot:Trapezoidal:(50,60,60,80";
            string errorMessage = "Linguistic variable membership functions are not valid: odd count of brackets";

            // Act
            ValidationOperationResult validationOperationResult =
                _membershipFunctionValidator.ValidateMembershipFunctionsPart(membershipFunctionPart);

            // Assert
            Assert.AreEqual(false, validationOperationResult.IsSuccess);
            Assert.IsTrue(validationOperationResult.Messages.Contains(errorMessage));
        }

        [Test]
        public void ValidateMembershipFunctionsPart_ReturnsValidationResultWithErrorIfOpeningBracketIsIncorrect()
        {
            // Arrange
            string membershipFunctionPart = "Cold:Trapezoidal:)0,20,20,30)|Hot:Trapezoidal:(50,60,60,80)";
            string errorMessage = "Linguistic variable membership functions are not valid: first or last brackets are wrong";

            // Act
            ValidationOperationResult validationOperationResult =
                _membershipFunctionValidator.ValidateMembershipFunctionsPart(membershipFunctionPart);

            // Assert
            Assert.AreEqual(false, validationOperationResult.IsSuccess);
            Assert.IsTrue(validationOperationResult.Messages.Contains(errorMessage));
        }

        [Test]
        public void ValidateMembershipFunctionsPart_ReturnsValidationResultWithErrorIfClosingBracketIsIncorrect()
        {
            // Arrange
            string membershipFunctionPart = "Cold:Trapezoidal:(0,20,20,30)|Hot:Trapezoidal:(50,60,60,80(";
            string errorMessage = "Linguistic variable membership functions are not valid: first or last brackets are wrong";

            // Act
            ValidationOperationResult validationOperationResult =
                _membershipFunctionValidator.ValidateMembershipFunctionsPart(membershipFunctionPart);

            // Assert
            Assert.AreEqual(false, validationOperationResult.IsSuccess);
            Assert.IsTrue(validationOperationResult.Messages.Contains(errorMessage));
        }

        [Test]
        public void ValidateMembershipFunctionsPart_ReturnsValidationResultWithErrorIfBracketsAreMismatching()
        {
            // Arrange
            string membershipFunctionPart = "Cold:Trapezoidal:(0,20,20,30)|Hot:Trapezoidal:)50,60,60,80)";
            string errorMessage = "Linguistic variable membership functions are not valid: mismatching brackets";

            // Act
            ValidationOperationResult validationOperationResult =
                _membershipFunctionValidator.ValidateMembershipFunctionsPart(membershipFunctionPart);

            // Assert
            Assert.AreEqual(false, validationOperationResult.IsSuccess);
            Assert.IsTrue(validationOperationResult.Messages.Contains(errorMessage));
        }

        [Test]
        public void ValidateMembershipFunctionsPart_ReturnsValidationResultWithErrorIfThereAreNoColuns()
        {
            // Arrange
            string membershipFunctionPart = "ColdTrapezoidal(0,20,20,30)|HotTrapezoidal(50,60,60,80)";
            string errorMessage = "Linguistic variable membership functions are not valid: incorrect colun placement";

            // Act
            ValidationOperationResult validationOperationResult =
                _membershipFunctionValidator.ValidateMembershipFunctionsPart(membershipFunctionPart);

            // Assert
            Assert.AreEqual(false, validationOperationResult.IsSuccess);
            Assert.IsTrue(validationOperationResult.Messages.Contains(errorMessage));
        }

        [Test]
        public void ValidateMembershipFunctionsPart_ReturnsValidationResultWithErrorIfThereIsOnlyOneColun()
        {
            // Arrange
            string membershipFunctionPart = "Cold:Trapezoidal(0,20,20,30)|HotTrapezoidal(50,60,60,80)";
            string errorMessage = "Linguistic variable membership functions are not valid: incorrect colun placement";

            // Act
            ValidationOperationResult validationOperationResult =
                _membershipFunctionValidator.ValidateMembershipFunctionsPart(membershipFunctionPart);

            // Assert
            Assert.AreEqual(false, validationOperationResult.IsSuccess);
            Assert.IsTrue(validationOperationResult.Messages.Contains(errorMessage));
        }

        [Test]
        public void ValidateMembershipFunctionsPart_ReturnsValidationResultWithErrorIfFirstColonIsAfterBrackets()
        {
            // Arrange
            string membershipFunctionPart = "ColdTrapezoidal(0,20,20,30)|Hot:Trapezoidal:(50,60,60,80)";
            string errorMessage = "Linguistic variable membership functions are not valid: incorrect colun placement";

            // Act
            ValidationOperationResult validationOperationResult =
                _membershipFunctionValidator.ValidateMembershipFunctionsPart(membershipFunctionPart);

            // Assert
            Assert.AreEqual(false, validationOperationResult.IsSuccess);
            Assert.IsTrue(validationOperationResult.Messages.Contains(errorMessage));
        }

        [Test]
        public void ValidateMembershipFunctionsPart_ReturnsValidationResultWithErrorIfSecondColonIsAfterBrackets()
        {
            // Arrange
            string membershipFunctionPart = "Cold:Trapezoidal(0,20,20,30)|Hot:Trapezoidal:(50,60,60,80)";
            string errorMessage = "Linguistic variable membership functions are not valid: incorrect colun placement";

            // Act
            ValidationOperationResult validationOperationResult =
                _membershipFunctionValidator.ValidateMembershipFunctionsPart(membershipFunctionPart);

            // Assert
            Assert.AreEqual(false, validationOperationResult.IsSuccess);
            Assert.IsTrue(validationOperationResult.Messages.Contains(errorMessage));
        }

        [Test]
        public void ValidateMembershipFunctionsPart_ReturnsValidationResultWithErrorIfThereIsNothingBeforeFirstColun()
        {
            // Arrange
            string membershipFunctionPart = ":Trapezoidal:(0,20,20,30)|Hot:Trapezoidal:(50,60,60,80)";
            string errorMessage = "Linguistic variable membership functions are not valid: incorrect colun placement";

            // Act
            ValidationOperationResult validationOperationResult =
                _membershipFunctionValidator.ValidateMembershipFunctionsPart(membershipFunctionPart);

            // Assert
            Assert.AreEqual(false, validationOperationResult.IsSuccess);
            Assert.IsTrue(validationOperationResult.Messages.Contains(errorMessage));
        }

        [Test]
        public void ValidateMembershipFunctionsPart_ReturnsValidationResultWithErrorIfThereIsDelimiterBeforeFirstColun()
        {
            // Arrange
            string membershipFunctionPart = "Cold:Trapezoidal:(0,20,20,30)|:Trapezoidal:(50,60,60,80)";
            string errorMessage = "Linguistic variable membership functions are not valid: incorrect colun placement";

            // Act
            ValidationOperationResult validationOperationResult =
                _membershipFunctionValidator.ValidateMembershipFunctionsPart(membershipFunctionPart);

            // Assert
            Assert.AreEqual(false, validationOperationResult.IsSuccess);
            Assert.IsTrue(validationOperationResult.Messages.Contains(errorMessage));
        }

        [Test]
        public void ValidateMembershipFunctionsPart_ReturnsValidationResultWithErrorIfThereIsClosingBracketBeforeFirstColun()
        {
            // Arrange
            string membershipFunctionPart = "Cold:Trapezoidal:(0,20,20,30)|:Trapezoidal:(50,60,60,80)";
            string errorMessage = "Linguistic variable membership functions are not valid: incorrect colun placement";

            // Act
            ValidationOperationResult validationOperationResult =
                _membershipFunctionValidator.ValidateMembershipFunctionsPart(membershipFunctionPart);

            // Assert
            Assert.AreEqual(false, validationOperationResult.IsSuccess);
            Assert.IsTrue(validationOperationResult.Messages.Contains(errorMessage));
        }

        [Test]
        public void ValidateMembershipFunctionsPart_ReturnsValidationResultWithErrorIfThereIsColunBeforeSecondColun()
        {
            // Arrange
            string membershipFunctionPart = "Cold::(0,20,20,30)|Hot:Trapezoidal:(50,60,60,80)";
            string errorMessage = "Linguistic variable membership functions are not valid: incorrect colun placement";

            // Act
            ValidationOperationResult validationOperationResult =
                _membershipFunctionValidator.ValidateMembershipFunctionsPart(membershipFunctionPart);

            // Assert
            Assert.AreEqual(false, validationOperationResult.IsSuccess);
            Assert.IsTrue(validationOperationResult.Messages.Contains(errorMessage));
        }

        [Test]
        public void ValidateMembershipFunctionsPart_ReturnsValidationResultWithErrorIfThereIsNoOpeningBracketAfterSecondColun()
        {
            // Arrange
            string membershipFunctionPart = "Cold:Trapezoidal:(0,20,20,30)|Hot:Trapezoidal:N(50,60,60,80)";
            string errorMessage = "Linguistic variable membership functions are not valid: incorrect colun placement";

            // Act
            ValidationOperationResult validationOperationResult =
                _membershipFunctionValidator.ValidateMembershipFunctionsPart(membershipFunctionPart);

            // Assert
            Assert.AreEqual(false, validationOperationResult.IsSuccess);
            Assert.IsTrue(validationOperationResult.Messages.Contains(errorMessage));
        }

        [Test]
        public void ValidateMembershipFunctionsPart_ReturnsValidationResultWithErrorIfThereAreEmptyBrackets()
        {
            // Arrange
            string membershipFunctionPart = "Cold:Trapezoidal:(0,20,20,30)|Hot:Trapezoidal:()";
            string errorMessage = "Linguistic variable membership functions are not valid: empty brackets";

            // Act
            ValidationOperationResult validationOperationResult =
                _membershipFunctionValidator.ValidateMembershipFunctionsPart(membershipFunctionPart);

            // Assert
            Assert.AreEqual(false, validationOperationResult.IsSuccess);
            Assert.IsTrue(validationOperationResult.Messages.Contains(errorMessage));
        }

        [Test]
        public void ValidateMembershipFunctionsPartPart_ReturnsValidationResultWithErrorIfThereAreNotEnoughDelimiters()
        {
            // Arrange
            string membershipFunctionPart = "Cold:Trapezoidal:(0,20,20,30)Hot:Trapezoidal:(50,60,60,80)";
            string errorMessage = "Linguistic variable membership functions are not valid: missing delimiter between functions";

            // Act
            ValidationOperationResult validationOperationResult =
                _membershipFunctionValidator.ValidateMembershipFunctionsPart(membershipFunctionPart);

            // Assert
            Assert.AreEqual(false, validationOperationResult.IsSuccess);
            Assert.IsTrue(validationOperationResult.Messages.Contains(errorMessage));
        }

        [Test]
        public void ValidateMembershipFunctionsPartPart_ReturnsValidationResultWithoutErrorsIfMembershipFunctionPartIsValid()
        {
            // Arrange
            string membershipFunctionPart = "Cold:Trapezoidal:(0,20,20,30)|Warm:Trapezoidal:(40,45,45,50)|Hot:Trapezoidal:(50,60,60,80)";

            // Act
            ValidationOperationResult validationOperationResult =
                _membershipFunctionValidator.ValidateMembershipFunctionsPart(membershipFunctionPart);

            // Assert
            Assert.AreEqual(true, validationOperationResult.IsSuccess);
            Assert.AreEqual(0, validationOperationResult.Messages.Count);
        }
    }
}
