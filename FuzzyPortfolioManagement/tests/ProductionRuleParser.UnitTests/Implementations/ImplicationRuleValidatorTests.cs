using System;
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
        public void ValidateImplicationRule_ThrowsArgumentExceptionIfThereWhitespacesInIt()
        {
            // Arrange
            string implicationRule = "IF (Something > 10) THEN (Anything = 5)";
            string exceptionMessage = "Implication rule string is not valid: haven't been preprocessed";

            // Act & Assert
            ArgumentException exception = Assert.Throws<ArgumentException>
            (
                () => { _implicationRuleValidator.ValidateImplicationRule(implicationRule); }
            );
            Assert.AreEqual(exceptionMessage, exception.Message);
        }

        [Test]
        public void ValidateImplicationRule_ThrowsArgumentExceptionIfImplicationRuleDoesntStartsWithIf()
        {
            // Arrange
            string implicationRule = "(Something>10)THEN(Anything=5)";
            string exceptionMessage = "Implication rule string is not valid: no if statement";

            // Act & Assert
            ArgumentException exception = Assert.Throws<ArgumentException>
            (
                () => { _implicationRuleValidator.ValidateImplicationRule(implicationRule); }
            );
            Assert.AreEqual(exceptionMessage, exception.Message);
        }

        [Test]
        public void ValidateImplicationRule_ThrowsArgumentExceptionIfImplicationRuleDoesntContainThen()
        {
            // Arrange
            string implicationRule = "IF(Something>10)(Anything=5)";
            string exceptionMessage = "Implication rule string is not valid: no then statement";

            // Act & Assert
            ArgumentException exception = Assert.Throws<ArgumentException>
            (
                () => { _implicationRuleValidator.ValidateImplicationRule(implicationRule); }
            );
            Assert.AreEqual(exceptionMessage, exception.Message);
        }

        [Test]
        public void ValidateImplicationRule_ThrowsArgumentExceptionIfImplicationRuleHasNoBrackets()
        {
            // Arrange
            string implicationRule = "IFSomething>10THENAnything=5";
            string exceptionMessage = "Implication rule string is not valid: no brackets";

            // Act & Assert
            ArgumentException exception = Assert.Throws<ArgumentException>
            (
                () => { _implicationRuleValidator.ValidateImplicationRule(implicationRule); }
            );
            Assert.AreEqual(exceptionMessage, exception.Message);
        }

        [Test]
        public void ValidateImplicationRule_ThrowsArgumentExceptionIfImplicationRuleHasOddCountOfBrackets()
        {
            // Arrange
            string implicationRule = "IF(Something>10)THENAnything=5)";
            string exceptionMessage = "Implication rule string is not valid: odd count of brackets";

            // Act & Assert
            ArgumentException exception = Assert.Throws<ArgumentException>
            (
                () => { _implicationRuleValidator.ValidateImplicationRule(implicationRule); }
            );
            Assert.AreEqual(exceptionMessage, exception.Message);
        }

        [Test]
        public void ValidateImplicationRule_ThrowsArgumentExceptionIfImplicationRuleFirstBracketIsClosing()
        {
            // Arrange
            string implicationRule = "IF)Something>10)THEN(Anything=5)";
            string exceptionMessage = "Implication rule string is not valid: wrong opening or closing bracket";

            // Act & Assert
            ArgumentException exception = Assert.Throws<ArgumentException>
            (
                () => { _implicationRuleValidator.ValidateImplicationRule(implicationRule); }
            );
            Assert.AreEqual(exceptionMessage, exception.Message);
        }

        [Test]
        public void ValidateImplicationRule_ThrowsArgumentExceptionIfImplicationRuleLastBracketIsOpening()
        {
            // Arrange
            string implicationRule = "IF(Something>10)THEN(Anything=5(";
            string exceptionMessage = "Implication rule string is not valid: wrong opening or closing bracket";

            // Act & Assert
            ArgumentException exception = Assert.Throws<ArgumentException>
            (
                () => { _implicationRuleValidator.ValidateImplicationRule(implicationRule); }
            );
            Assert.AreEqual(exceptionMessage, exception.Message);
        }

        [Test]
        public void ValidateImplicationRule_ThrowsArgumentExceptionIfImplicationRuleHasMismatchingBracketsCount()
        {
            // Arrange
            string implicationRule = "IF(Something>10)THEN(Anything=5)))";
            string exceptionMessage = "Implication rule string is not valid: mismatching brackets";

            // Act & Assert
            ArgumentException exception = Assert.Throws<ArgumentException>
            (
                () => { _implicationRuleValidator.ValidateImplicationRule(implicationRule); }
            );
            Assert.AreEqual(exceptionMessage, exception.Message);
        }
    }
}
