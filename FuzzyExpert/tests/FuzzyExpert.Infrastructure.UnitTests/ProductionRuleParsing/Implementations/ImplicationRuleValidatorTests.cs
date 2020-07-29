using FuzzyExpert.Infrastructure.ProductionRuleParsing.Implementations;
using FuzzyExpert.Infrastructure.ProductionRuleParsing.Interfaces;
using NUnit.Framework;

namespace FuzzyExpert.Infrastructure.UnitTests.ProductionRuleParsing.Implementations
{
    [TestFixture]
    public class ImplicationRuleValidatorTests
    {
        private IImplicationRuleValidator _implicationRuleValidator;

        [SetUp]
        public void SetUp()
        {
            _implicationRuleValidator = new ImplicationRuleValidator();
        }

        [Test]
        public void ValidateImplicationRule_ReturnValidationResultWithError_IfThereWhitespacesInIt()
        {
            // Arrange
            var implicationRule = "IF (Something > 10) THEN (Anything = 5)";

            // Act
            var validationOperationResult = _implicationRuleValidator.ValidateImplicationRule(implicationRule);

            // Assert
            Assert.IsFalse(validationOperationResult.IsSuccess);
        }

        [Test]
        public void ValidateImplicationRule_ReturnValidationResultWithError_IfImplicationRuleDoesNotStartsWithIf()
        {
            // Arrange
            var implicationRule = "(Something>10)THEN(Anything=5)";

            // Act
            var validationOperationResult = _implicationRuleValidator.ValidateImplicationRule(implicationRule);

            // Assert
            Assert.IsFalse(validationOperationResult.IsSuccess);
        }

        [Test]
        public void ValidateImplicationRule_ReturnValidationResultWithError_IfImplicationRuleDoesNotContainThen()
        {
            // Arrange
            var implicationRule = "IF(Something>10)(Anything=5)";

            // Act
            var validationOperationResult = _implicationRuleValidator.ValidateImplicationRule(implicationRule);

            // Assert
            Assert.IsFalse(validationOperationResult.IsSuccess);
        }

        [Test]
        public void ValidateImplicationRule_ReturnValidationResultWithErrorIfImplicationRuleHasNoBrackets()
        {
            // Arrange
            var implicationRule = "IFSomething>10THENAnything=5";

            // Act
            var validationOperationResult = _implicationRuleValidator.ValidateImplicationRule(implicationRule);

            // Assert
            Assert.IsFalse(validationOperationResult.IsSuccess);
        }

        [Test]
        public void ValidateImplicationRule_ReturnValidationResultWithErrorIfImplicationRuleHasOddCountOfBrackets()
        {
            // Arrange
            var implicationRule = "IF(Something>10)THENAnything=5)";

            // Act
            var validationOperationResult = _implicationRuleValidator.ValidateImplicationRule(implicationRule);

            // Assert
            Assert.IsFalse(validationOperationResult.IsSuccess);
        }

        [Test]
        public void ValidateImplicationRule_ReturnValidationResultWithErrorIfImplicationRuleFirstBracketIsClosing()
        {
            // Arrange
            var implicationRule = "IF)Something>10)THEN(Anything=5)";

            // Act
            var validationOperationResult = _implicationRuleValidator.ValidateImplicationRule(implicationRule);

            // Assert
            Assert.IsFalse(validationOperationResult.IsSuccess);
        }

        [Test]
        public void ValidateImplicationRule_ReturnValidationResultWithErrorIfImplicationRuleLastBracketIsOpening()
        {
            // Arrange
            var implicationRule = "IF(Something>10)THEN(Anything=5(";

            // Act
            var validationOperationResult = _implicationRuleValidator.ValidateImplicationRule(implicationRule);

            // Assert
            Assert.IsFalse(validationOperationResult.IsSuccess);
        }

        [Test]
        public void ValidateImplicationRule_ReturnValidationResultWithoutErrorsIfImplicationRuleIsValid()
        {
            // Arrange
            var implicationRule = "IF(Something>10)THEN(Anything=5)";

            // Act
            var validationOperationResult = _implicationRuleValidator.ValidateImplicationRule(implicationRule);

            // Assert
            Assert.IsTrue(validationOperationResult.IsSuccess);
            Assert.AreEqual(0, validationOperationResult.Messages.Count);
        }
    }
}