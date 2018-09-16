using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using ProductionRulesParser.Entities;
using ProductionRulesParser.Enums;
using ProductionRulesParser.Implementations;

namespace ProductionRulesParser.UnitTests.Implementations
{
    [TestFixture]
    public class ImplicationRuleParserTests
    {
        private ImplicationRuleParser _implicationRuleParser;

        [SetUp]
        public void SetUp()
        {
            _implicationRuleParser = new ImplicationRuleParser();
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
            List<string> actualRuleParts = _implicationRuleParser.GetStatementParts(ref implicationRule);

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
            List<string> actualRuleParts = _implicationRuleParser.GetStatementParts(ref implicationRule);

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
            List<string> actualRuleParts = _implicationRuleParser.GetStatementParts(ref implicationRule);

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
            List<string> actualRuleParts = _implicationRuleParser.GetStatementParts(ref implicationRule);

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
            List<string> actualRuleParts = _implicationRuleParser.GetStatementParts(ref implicationRule);

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
            List<string> actualRuleParts = _implicationRuleParser.GetStatementParts(ref implicationRule);

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
            List<string> actualRuleParts = _implicationRuleParser.GetStatementParts(ref implicationRule);

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
            List<string> actualRuleParts = _implicationRuleParser.GetStatementParts(ref implicationRule);

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
            List<string> actualRuleParts = _implicationRuleParser.GetStatementParts(ref implicationRule);

            // Assert
            Assert.IsTrue(expectedRuleParts.SequenceEqual(actualRuleParts));
        }

        [Test]
        public void ExtractStatementParts_ThrowsArgumentExceptionIfImplicationRuleIsInvalid()
        {
            // Arrange
            string implicationRule = "IF(Something>10)IF(Anything=5)";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _implicationRuleParser.ExtractStatementParts(implicationRule));
        }

        [Test]
        public void ExtractStatementParts_ReturnsCorrectRuleStringsSimpleRule()
        {
            // Arrange
            string expectedIfStatement = "(Something>10)";
            string expectedThenStatement = "(Anything=5)";
            string implicationRule = "IF" + expectedIfStatement + "THEN" + expectedThenStatement;
            ImplicationRuleStrings expectedImplicationRuleStrings = new ImplicationRuleStrings(expectedIfStatement, expectedThenStatement);

            // Act
            ImplicationRuleStrings actualImplicationRuleStrings = _implicationRuleParser.ExtractStatementParts(implicationRule);

            // Assert
            Assert.IsTrue(ImplicationRuleStringsAreEqual(expectedImplicationRuleStrings, actualImplicationRuleStrings));
        }

        [Test]
        public void ExtractStatementParts_ReturnsCorrectRuleStringsComplexRule()
        {
            // Arrange
            string expectedIfStatement = "((Something>10)&(SomethingElse<9)|ThatOne=7)";
            string expectedThenStatement = "(Anything=5)";
            string implicationRule = "IF" + expectedIfStatement + "THEN" + expectedThenStatement;
            ImplicationRuleStrings expectedImplicationRuleStrings = new ImplicationRuleStrings(expectedIfStatement, expectedThenStatement);

            // Act
            ImplicationRuleStrings actualImplicationRuleStrings = _implicationRuleParser.ExtractStatementParts(implicationRule);

            // Assert
            Assert.IsTrue(ImplicationRuleStringsAreEqual(expectedImplicationRuleStrings, actualImplicationRuleStrings));
        }

        [Test]
        public void ParseUnaryStatement_ReturnsStatementWithLess()
        {
            // Arrange
            string statement = "Avarage<10";
            UnaryStatement expectedUnaryStatement = new UnaryStatement("Avarage", ComparisonOperation.Less, "10");

            // Act
            UnaryStatement actualUnaryStatement = _implicationRuleParser.ParseUnaryStatement(statement);

            // Assert
            Assert.IsTrue(UnaryStatementsAreEqual(expectedUnaryStatement, actualUnaryStatement));
        }

        [Test]
        public void ParseUnaryStatement_ReturnsStatementWithGreater()
        {
            // Arrange
            string statement = "Avarage>10";
            UnaryStatement expectedUnaryStatement = new UnaryStatement("Avarage", ComparisonOperation.Greater, "10");

            // Act
            UnaryStatement actualUnaryStatement = _implicationRuleParser.ParseUnaryStatement(statement);

            // Assert
            Assert.IsTrue(UnaryStatementsAreEqual(expectedUnaryStatement, actualUnaryStatement));
        }

        [Test]
        public void ParseUnaryStatement_ReturnsStatementWithLessOrEqual()
        {
            // Arrange
            string statement = "Avarage<=10";
            UnaryStatement expectedUnaryStatement = new UnaryStatement("Avarage", ComparisonOperation.LessOrEqual, "10");

            // Act
            UnaryStatement actualUnaryStatement = _implicationRuleParser.ParseUnaryStatement(statement);

            // Assert
            Assert.IsTrue(UnaryStatementsAreEqual(expectedUnaryStatement, actualUnaryStatement));
        }

        [Test]
        public void ParseUnaryStatement_ReturnsStatementWithGreaterOrEqual()
        {
            // Arrange
            string statement = "Avarage>=10";
            UnaryStatement expectedUnaryStatement = new UnaryStatement("Avarage", ComparisonOperation.GreaterOrEqual, "10");

            // Act
            UnaryStatement actualUnaryStatement = _implicationRuleParser.ParseUnaryStatement(statement);

            // Assert
            Assert.IsTrue(UnaryStatementsAreEqual(expectedUnaryStatement, actualUnaryStatement));
        }

        [Test]
        public void ParseUnaryStatement_ReturnsStatementWithEqual()
        {
            // Arrange
            string statement = "Avarage=10";
            UnaryStatement expectedUnaryStatement = new UnaryStatement("Avarage", ComparisonOperation.Equal, "10");

            // Act
            UnaryStatement actualUnaryStatement = _implicationRuleParser.ParseUnaryStatement(statement);

            // Assert
            Assert.IsTrue(UnaryStatementsAreEqual(expectedUnaryStatement, actualUnaryStatement));
        }

        [Test]
        public void ParseUnaryStatement_ReturnsStatementWithNotEqual()
        {
            // Arrange
            string statement = "Avarage!=10";
            UnaryStatement expectedUnaryStatement = new UnaryStatement("Avarage", ComparisonOperation.NotEqual, "10");

            // Act
            UnaryStatement actualUnaryStatement = _implicationRuleParser.ParseUnaryStatement(statement);

            // Assert
            Assert.IsTrue(UnaryStatementsAreEqual(expectedUnaryStatement, actualUnaryStatement));
        }

        [Test]
        public void ParseUnaryStatement_ThrowsArgumentExceptionIfItDoesntContainAnyComparisonOperations()
        {
            // Arrange
            string statement = "Avarage";

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _implicationRuleParser.ParseUnaryStatement(statement));
        }

        private bool ImplicationRuleStringsAreEqual(
            ImplicationRuleStrings implicationRuleStringsToCompare,
            ImplicationRuleStrings implicationRuleStringsToCompareWith)
        {
            return implicationRuleStringsToCompare.IfStatement == implicationRuleStringsToCompareWith.IfStatement &&
                   implicationRuleStringsToCompare.ThenStatement == implicationRuleStringsToCompareWith.ThenStatement;
        }

        private bool UnaryStatementsAreEqual(
            UnaryStatement unaryStatementToCompare,
            UnaryStatement unaryStatementToCompareWith)
        {
            return unaryStatementToCompare.LeftOperand == unaryStatementToCompareWith.LeftOperand &&
                   unaryStatementToCompare.ComparisonOperation == unaryStatementToCompareWith.ComparisonOperation &&
                   unaryStatementToCompare.RightOperand == unaryStatementToCompareWith.RightOperand;
        }
    }
}
