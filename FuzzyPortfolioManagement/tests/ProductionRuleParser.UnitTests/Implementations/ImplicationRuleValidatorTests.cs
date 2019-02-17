﻿using CommonLogic.Entities;
using NUnit.Framework;
using ProductionRuleParser.Implementations;
using ProductionRuleParser.Interfaces;

namespace ProductionRuleParser.UnitTests.Implementations
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
        public void ValidateImplicationRule_ReturnValidationResultWithErrorIfThereWhitespacesInIt()
        {
            // Arrange
            string implicationRule = "IF (Something > 10) THEN (Anything = 5)";
            string errorMessage = "Implication rule string is not valid: haven't been preprocessed";

            // Act
            ValidationOperationResult validationOperationResult =
                _implicationRuleValidator.ValidateImplicationRule(implicationRule);

            // Assert
            Assert.AreEqual(false, validationOperationResult.IsSuccess);
            Assert.IsTrue(validationOperationResult.GetMessages().Contains(errorMessage));
        }

        [Test]
        public void ValidateImplicationRule_ReturnValidationResultWithErrorIfImplicationRuleDoesntStartsWithIf()
        {
            // Arrange
            string implicationRule = "(Something>10)THEN(Anything=5)";
            string errorMessage = "Implication rule string is not valid: no if statement";

            // Act
            ValidationOperationResult validationOperationResult =
                _implicationRuleValidator.ValidateImplicationRule(implicationRule);

            // Assert
            Assert.AreEqual(false, validationOperationResult.IsSuccess);
            Assert.IsTrue(validationOperationResult.GetMessages().Contains(errorMessage));
        }

        [Test]
        public void ValidateImplicationRule_ReturnValidationResultWithErrorIfImplicationRuleDoesntContainThen()
        {
            // Arrange
            string implicationRule = "IF(Something>10)(Anything=5)";
            string errorMessage = "Implication rule string is not valid: no then statement";

            // Act
            ValidationOperationResult validationOperationResult =
                _implicationRuleValidator.ValidateImplicationRule(implicationRule);

            // Assert
            Assert.AreEqual(false, validationOperationResult.IsSuccess);
            Assert.IsTrue(validationOperationResult.GetMessages().Contains(errorMessage));
        }

        [Test]
        public void ValidateImplicationRule_ReturnValidationResultWithErrorIfImplicationRuleHasNoBrackets()
        {
            // Arrange
            string implicationRule = "IFSomething>10THENAnything=5";
            string errorMessage = "Implication rule string is not valid: no brackets";

            // Act
            ValidationOperationResult validationOperationResult =
                _implicationRuleValidator.ValidateImplicationRule(implicationRule);

            // Assert
            Assert.AreEqual(false, validationOperationResult.IsSuccess);
            Assert.IsTrue(validationOperationResult.GetMessages().Contains(errorMessage));
        }

        [Test]
        public void ValidateImplicationRule_ReturnValidationResultWithErrorIfImplicationRuleHasOddCountOfBrackets()
        {
            // Arrange
            string implicationRule = "IF(Something>10)THENAnything=5)";
            string errorMessage = "Implication rule string is not valid: odd count of brackets";

            // Act
            ValidationOperationResult validationOperationResult =
                _implicationRuleValidator.ValidateImplicationRule(implicationRule);

            // Assert
            Assert.AreEqual(false, validationOperationResult.IsSuccess);
            Assert.IsTrue(validationOperationResult.GetMessages().Contains(errorMessage));
        }

        [Test]
        public void ValidateImplicationRule_ReturnValidationResultWithErrorIfImplicationRuleFirstBracketIsClosing()
        {
            // Arrange
            string implicationRule = "IF)Something>10)THEN(Anything=5)";
            string errorMessage = "Implication rule string is not valid: wrong opening or closing bracket";

            // Act
            ValidationOperationResult validationOperationResult =
                _implicationRuleValidator.ValidateImplicationRule(implicationRule);

            // Assert
            Assert.AreEqual(false, validationOperationResult.IsSuccess);
            Assert.IsTrue(validationOperationResult.GetMessages().Contains(errorMessage));
        }

        [Test]
        public void ValidateImplicationRule_ReturnValidationResultWithErrorIfImplicationRuleLastBracketIsOpening()
        {
            // Arrange
            string implicationRule = "IF(Something>10)THEN(Anything=5(";
            string errorMessage = "Implication rule string is not valid: wrong opening or closing bracket";

            // Act
            ValidationOperationResult validationOperationResult =
                _implicationRuleValidator.ValidateImplicationRule(implicationRule);

            // Assert
            Assert.AreEqual(false, validationOperationResult.IsSuccess);
            Assert.IsTrue(validationOperationResult.GetMessages().Contains(errorMessage));
        }

        [Test]
        public void ValidateImplicationRule_ReturnValidationResultWithErrorIfImplicationRuleHasMismatchingBracketsCount()
        {
            // Arrange
            string implicationRule = "IF(Something>10)THEN(Anything=5)))";
            string errorMessage = "Implication rule string is not valid: mismatching brackets";

            // Act
            ValidationOperationResult validationOperationResult =
                _implicationRuleValidator.ValidateImplicationRule(implicationRule);

            // Assert
            Assert.AreEqual(false, validationOperationResult.IsSuccess);
            Assert.IsTrue(validationOperationResult.GetMessages().Contains(errorMessage));
        }

        [Test]
        public void ValidateImplicationRule_ReturnValidationResultWithoutErrorsIfImplicationRuleIsValid()
        {
            // Arrange
            string implicationRule = "IF(Something>10)THEN(Anything=5)";

            // Act
            ValidationOperationResult validationOperationResult =
                _implicationRuleValidator.ValidateImplicationRule(implicationRule);

            // Assert
            Assert.AreEqual(true, validationOperationResult.IsSuccess);
            Assert.AreEqual(0, validationOperationResult.GetMessages().Count);
        }
    }
}
