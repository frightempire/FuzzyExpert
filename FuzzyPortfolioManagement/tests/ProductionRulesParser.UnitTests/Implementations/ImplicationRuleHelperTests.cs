using System;
using System.Collections.Generic;
using Base.UnitTests;
using NUnit.Framework;
using ProductionRulesParser.Implementations;

namespace ProductionRulesParser.UnitTests.Implementations
{
    [TestFixture]
    public class ImplicationRuleHelperTests
    {
        private ImplicationRuleHelper _implicationRuleHelper;

        [SetUp]
        public void SetUp()
        {
            _implicationRuleHelper = new ImplicationRuleHelper();
        }

        [Test]
        public void GetRuleParts_ReturnsCorrectParts()
        {
            // Arrange
            int index = 0;
            string implicationRule = "A=a|(B=b&C=c)";
            List<object> expectedRuleParts = new List<object>
            {
                "A=a",
                new List<object> {"B=b&C=c"}
            };

            // Act
            List<object> actualRuleParts = _implicationRuleHelper.GetRuleParts(implicationRule, ref index);

            // Assert
            Assert.IsTrue(TestHelper.NestedListsAreEqual(actualRuleParts, expectedRuleParts));
        }

        [Test]
        public void ValidateImplicationRule_ThrowsArgumentExceptionIfImplicationRuleDoesntStartsWithIf()
        {
            // Arrange
            string implicationRule = "(Something > 10) THEN (Anything = 5)";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => { _implicationRuleHelper.ValidateImplicationRule(implicationRule); });
        }

        [Test]
        public void ValidateImplicationRule_ThrowsArgumentExceptionIfImplicationRuleDoesntContainThen()
        {
            // Arrange
            string implicationRule = "IF (Something > 10) (Anything = 5)";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => { _implicationRuleHelper.ValidateImplicationRule(implicationRule); });
        }

        [Test]
        public void ValidateImplicationRule_ThrowsArgumentExceptionIfImplicationRuleHasNoBrackets()
        {
            // Arrange
            string implicationRule = "IF Something > 10 THEN Anything = 5";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => { _implicationRuleHelper.ValidateImplicationRule(implicationRule); });
        }

        [Test]
        public void ValidateImplicationRule_ThrowsArgumentExceptionIfImplicationRuleHasEvenCountOfBrackets()
        {
            // Arrange
            string implicationRule = "IF (Something > 10) THEN Anything = 5)";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => { _implicationRuleHelper.ValidateImplicationRule(implicationRule); });
        }

        [Test]
        public void ValidateImplicationRule_ThrowsArgumentExceptionIfImplicationRuleFirstBracketIsClosing()
        {
            // Arrange
            string implicationRule = "IF )Something > 10) THEN (Anything = 5)";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => { _implicationRuleHelper.ValidateImplicationRule(implicationRule); });
        }

        [Test]
        public void ValidateImplicationRule_ThrowsArgumentExceptionIfImplicationRuleLastBracketIsOpening()
        {
            // Arrange
            string implicationRule = "IF (Something > 10) THEN (Anything = 5(";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => { _implicationRuleHelper.ValidateImplicationRule(implicationRule); });
        }

        [Test]
        public void ValidateImplicationRule_ThrowsArgumentExceptionIfImplicationRuleHasMismatchingBracketsCount()
        {
            // Arrange
            string implicationRule = "IF (Something > 10) THEN (Anything = 5)))";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => { _implicationRuleHelper.ValidateImplicationRule(implicationRule); });
        }

        [Test]
        public void PreProcessImplicationRule_ReturnsImplicationRuleWithoutWhitespaces()
        {
            // Arrange
            string implicationRule = "IF (Something > 10) THEN (Anything = 5)";
            string expectedImplicationRule = "IF(Something>10)THEN(Anything=5)";

            // Act
            string actualImplicationRule = _implicationRuleHelper.PreProcessImplicationRule(implicationRule);

            // Assert
            Assert.AreEqual(actualImplicationRule, expectedImplicationRule);
        }
    }
}
