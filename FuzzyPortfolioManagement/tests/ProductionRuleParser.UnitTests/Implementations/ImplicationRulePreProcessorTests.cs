using System;
using NUnit.Framework;
using ProductionRuleParser.Implementations;
using ProductionRuleParser.Interfaces;

namespace ProductionRuleParser.UnitTests.Implementations
{
    [TestFixture]
    public class ImplicationRulePreProcessorTests
    {
        private IImplicationRulePreProcessor _implicationRulePreProcessor;

        [SetUp]
        public void SetUp()
        {
            _implicationRulePreProcessor = new ImplicationRulePreProcessor();
        }

        [Test]
        public void ValidateImplicationRule_ThrowsArgumentExceptionIfImplicationRuleDoesntStartsWithIf()
        {
            // Arrange
            string implicationRule = "(Something > 10) THEN (Anything = 5)";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => { _implicationRulePreProcessor.ValidateImplicationRule(implicationRule); });
        }

        [Test]
        public void ValidateImplicationRule_ThrowsArgumentExceptionIfImplicationRuleDoesntContainThen()
        {
            // Arrange
            string implicationRule = "IF (Something > 10) (Anything = 5)";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => { _implicationRulePreProcessor.ValidateImplicationRule(implicationRule); });
        }

        [Test]
        public void ValidateImplicationRule_ThrowsArgumentExceptionIfImplicationRuleHasNoBrackets()
        {
            // Arrange
            string implicationRule = "IF Something > 10 THEN Anything = 5";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => { _implicationRulePreProcessor.ValidateImplicationRule(implicationRule); });
        }

        [Test]
        public void ValidateImplicationRule_ThrowsArgumentExceptionIfImplicationRuleHasEvenCountOfBrackets()
        {
            // Arrange
            string implicationRule = "IF (Something > 10) THEN Anything = 5)";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => { _implicationRulePreProcessor.ValidateImplicationRule(implicationRule); });
        }

        [Test]
        public void ValidateImplicationRule_ThrowsArgumentExceptionIfImplicationRuleFirstBracketIsClosing()
        {
            // Arrange
            string implicationRule = "IF )Something > 10) THEN (Anything = 5)";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => { _implicationRulePreProcessor.ValidateImplicationRule(implicationRule); });
        }

        [Test]
        public void ValidateImplicationRule_ThrowsArgumentExceptionIfImplicationRuleLastBracketIsOpening()
        {
            // Arrange
            string implicationRule = "IF (Something > 10) THEN (Anything = 5(";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => { _implicationRulePreProcessor.ValidateImplicationRule(implicationRule); });
        }

        [Test]
        public void ValidateImplicationRule_ThrowsArgumentExceptionIfImplicationRuleHasMismatchingBracketsCount()
        {
            // Arrange
            string implicationRule = "IF (Something > 10) THEN (Anything = 5)))";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => { _implicationRulePreProcessor.ValidateImplicationRule(implicationRule); });
        }

        [Test]
        public void PreProcessImplicationRule_ReturnsImplicationRuleWithoutWhitespaces()
        {
            // Arrange
            string implicationRule = "IF (Something > 10) THEN (Anything = 5)";
            string expectedImplicationRule = "IF(Something>10)THEN(Anything=5)";

            // Act
            string actualImplicationRule = _implicationRulePreProcessor.PreProcessImplicationRule(implicationRule);

            // Assert
            Assert.AreEqual(actualImplicationRule, expectedImplicationRule);
        }
    }
}
