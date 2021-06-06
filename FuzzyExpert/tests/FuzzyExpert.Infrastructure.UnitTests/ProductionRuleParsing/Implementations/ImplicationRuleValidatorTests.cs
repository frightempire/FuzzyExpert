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
        public void ValidateImplicationRule_ReturnValidationResultWithError_IfImplicationRuleDoesNotStartsWithIf()
        {
            // Arrange
            var implicationRule = "(Something>10)THEN(Anything=5)";

            // Act
            var validationOperationResult = _implicationRuleValidator.ValidateImplicationRule(implicationRule);

            // Assert
            Assert.IsFalse(validationOperationResult.Successful);
            Assert.AreEqual(2, validationOperationResult.Messages.Count);
            Assert.AreEqual("No IF statement", validationOperationResult.Messages[0]);
        }

        [Test]
        public void ValidateImplicationRule_ReturnValidationResultWithError_IfImplicationRuleDoesNotContainThen()
        {
            // Arrange
            var implicationRule = "IF(Something>10)(Anything=5)";

            // Act
            var validationOperationResult = _implicationRuleValidator.ValidateImplicationRule(implicationRule);

            // Assert
            Assert.IsFalse(validationOperationResult.Successful);
            Assert.AreEqual(2, validationOperationResult.Messages.Count);
            Assert.AreEqual("No THEN statement", validationOperationResult.Messages[0]);
        }

        [TestCase("IF((Something>10)THEN(Anything=5)")]
        [TestCase("IF()Something>10)THEN(Anything=5)")]
        public void ValidateImplicationRule_ReturnValidationResultWithError_IfIfStatementBracketsDoesNotMatch(string rule)
        {
            // Act
            var validationOperationResult = _implicationRuleValidator.ValidateImplicationRule(rule);

            // Assert
            Assert.IsFalse(validationOperationResult.Successful);
            Assert.AreEqual(2, validationOperationResult.Messages.Count);
            Assert.AreEqual("IF statement parenthesis don't match", validationOperationResult.Messages[0]);
        }

        [TestCase("IF(Something>10)THEN((Anything=5)")]
        [TestCase("IF(Something>10)THEN(Anything=5(")]
        public void ValidateImplicationRule_ReturnValidationResultWithError_IfThenStatementBracketsDoesNotMatch(string rule)
        {
            // Act
            var validationOperationResult = _implicationRuleValidator.ValidateImplicationRule(rule);

            // Assert
            Assert.IsFalse(validationOperationResult.Successful);
            Assert.AreEqual(2, validationOperationResult.Messages.Count);
            Assert.AreEqual("THEN statement parenthesis don't match", validationOperationResult.Messages[0]);
        }

        [Test]
        public void ValidateImplicationRule_ReturnValidationResultWithoutErrorsIfImplicationRuleIsValid()
        {
            // Arrange
            var implicationRule = "IF(Something>10)THEN(Anything=5)";

            // Act
            var validationOperationResult = _implicationRuleValidator.ValidateImplicationRule(implicationRule);

            // Assert
            Assert.IsTrue(validationOperationResult.Successful);
            Assert.IsNull(validationOperationResult.Messages);
        }
    }
}