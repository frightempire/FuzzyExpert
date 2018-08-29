using System;
using System.Collections.Generic;
using System.Linq;
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
        public void GetRuleParts_SimpleRuleWithOr()
        {
            // Arrange
            string implicationRule = "A=a|(B=b&C=c)";
            List<string> expectedRuleParts = new List<string>
            {
                "A=a",
                "B=b&C=c"
            };

            // Act
            List<string> actualRuleParts = _implicationRuleHelper.GetStatementParts(ref implicationRule);

            // Assert
            Assert.IsTrue(expectedRuleParts.SequenceEqual(actualRuleParts));
        }

        [Test]
        public void GetRuleParts_SimpleRuleWithAnd()
        {
            // Arrange
            string implicationRule = "(A=a|B=b)&C=c";
            List<string> expectedRuleParts = new List<string>
            {
                "A=a&C=c",
                "B=b&C=c"
            };

            // Act
            List<string> actualRuleParts = _implicationRuleHelper.GetStatementParts(ref implicationRule);

            // Assert
            Assert.IsTrue(expectedRuleParts.SequenceEqual(actualRuleParts));
        }

        [Test]
        public void GetRuleParts_ComplexLettersRuleCase1()
        {
            // Arrange
            string implicationRule = "A=a|B=b|((D=d&(E=e|K=k))|(F=f&G=g))&C=c";
            List<string> expectedRuleParts = new List<string>
            {
                "A=a",
                "B=b",
                "D=d&E=e&C=c",
                "D=d&K=k&C=c",
                "F=f&G=g&C=c"
            };

            // Act
            List<string> actualRuleParts = _implicationRuleHelper.GetStatementParts(ref implicationRule);

            // Assert
            Assert.IsTrue(expectedRuleParts.SequenceEqual(actualRuleParts));
        }

        [Test]
        public void GetRuleParts_ComplexLettersRuleCase2()
        {
            // Arrange
            string implicationRule = "((D=d&(E=e|K=k))|(F=f&G=g))&C=c|A=a|B=b";
            List<string> expectedRuleParts = new List<string>
            {
                "D=d&E=e&C=c",
                "D=d&K=k&C=c",
                "F=f&G=g&C=c",
                "A=a",
                "B=b"
            };

            // Act
            List<string> actualRuleParts = _implicationRuleHelper.GetStatementParts(ref implicationRule);

            // Assert
            Assert.IsTrue(expectedRuleParts.SequenceEqual(actualRuleParts));
        }

        [Test]
        public void GetRuleParts_ComplexNumericalRuleCase1()
        {
            // Arrange
            string implicationRule = "A=5|B=10|((D=7&(E=4|K=555))|(F=45&G=0))&C=2";
            List<string> expectedRuleParts = new List<string>
            {
                "A=5",
                "B=10",
                "D=7&E=4&C=2",
                "D=7&K=555&C=2",
                "F=45&G=0&C=2"
            };

            // Act
            List<string> actualRuleParts = _implicationRuleHelper.GetStatementParts(ref implicationRule);

            // Assert
            Assert.IsTrue(expectedRuleParts.SequenceEqual(actualRuleParts));
        }

        [Test]
        public void GetRuleParts_ComplexNumericalRuleCase2()
        {
            // Arrange
            string implicationRule = "((D=7&(E=4|K=555))|(F=45&G=0))&C=2|A=5|B=10";
            List<string> expectedRuleParts = new List<string>
            {
                "D=7&E=4&C=2",
                "D=7&K=555&C=2",
                "F=45&G=0&C=2",
                "A=5",
                "B=10"
            };

            // Act
            List<string> actualRuleParts = _implicationRuleHelper.GetStatementParts(ref implicationRule);

            // Assert
            Assert.IsTrue(expectedRuleParts.SequenceEqual(actualRuleParts));
        }

        [Test]
        public void GetRuleParts_RuleWithComplexVariables()
        {
            // Arrange
            string implicationRule = "Distance=7&(Enumeration=4|Kinetic=555)|Avarage=5|Business=10";
            List<string> expectedRuleParts = new List<string>
            {
                "Distance=7&Enumeration=4",
                "Distance=7&Kinetic=555",
                "Avarage=5",
                "Business=10"
            };

            // Act
            List<string> actualRuleParts = _implicationRuleHelper.GetStatementParts(ref implicationRule);

            // Assert
            Assert.IsTrue(expectedRuleParts.SequenceEqual(actualRuleParts));
        }

        [Test]
        public void GetRuleParts_RuleWithMultipleBrackets()
        {
            // Arrange
            string implicationRule = "(((Distance=7&(Enumeration=4|Kinetic=555))))|((Avarage=5|Business=10))";
            List<string> expectedRuleParts = new List<string>
            {
                "Distance=7&Enumeration=4",
                "Distance=7&Kinetic=555",
                "Avarage=5",
                "Business=10"
            };

            // Act
            List<string> actualRuleParts = _implicationRuleHelper.GetStatementParts(ref implicationRule);

            // Assert
            Assert.IsTrue(expectedRuleParts.SequenceEqual(actualRuleParts));
        }

        [Test]
        public void GetRuleParts_RuleSurroundedByBrackets()
        {
            // Arrange
            string implicationRule = "(Distance=7&(Enumeration=4|Kinetic=555)|Avarage=5|Business=10)";
            List<string> expectedRuleParts = new List<string>
            {
                "Distance=7&Enumeration=4",
                "Distance=7&Kinetic=555",
                "Avarage=5",
                "Business=10"
            };

            // Act
            List<string> actualRuleParts = _implicationRuleHelper.GetStatementParts(ref implicationRule);

            // Assert
            Assert.IsTrue(expectedRuleParts.SequenceEqual(actualRuleParts));
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
