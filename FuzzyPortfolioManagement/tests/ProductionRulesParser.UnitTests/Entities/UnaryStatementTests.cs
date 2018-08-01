using NUnit.Framework;
using ProductionRulesParser.Entities;
using ProductionRulesParser.Enums;

namespace ProductionRulesParser.UnitTests.Entities
{
    [TestFixture]
    public class UnaryStatementTests
    {
        private UnaryStatement _unaryStatement;

        [SetUp]
        public void SetUp()
        {
            _unaryStatement = new UnaryStatement();
        }

        [Test]
        public void LeftOperand_SetterWorksProperly()
        {
            // Assert
            string leftOperand = "leftOperand";
            
            // Act
            _unaryStatement.LeftOperand = leftOperand;

            // Arrange
            Assert.AreEqual(leftOperand, _unaryStatement.LeftOperand);
        }

        [Test]
        public void LeftOperand_GetterWorksProperly()
        {
            // Assert
            string leftOperand = "leftOperand";
            _unaryStatement.LeftOperand = leftOperand;

            // Act
            string actualLeftOperand = _unaryStatement.LeftOperand;

            // Arrange
            Assert.AreEqual(leftOperand, actualLeftOperand);
        }

        [Test]
        public void ComparisonOperation_SetterWorksProperly()
        {
            // Assert
            ComparisonOperation comparisonOperation = ComparisonOperation.Equal;

            // Act
            _unaryStatement.ComparisonOperation = comparisonOperation;

            // Arrange
            Assert.AreEqual(comparisonOperation, _unaryStatement.ComparisonOperation);
        }

        [Test]
        public void ComparisonOperation_GetterWorksProperly()
        {
            // Assert
            ComparisonOperation comparisonOperation = ComparisonOperation.Equal;
            _unaryStatement.ComparisonOperation = comparisonOperation;

            // Act
            ComparisonOperation actualComparisonOperation = _unaryStatement.ComparisonOperation;

            // Arrange
            Assert.AreEqual(comparisonOperation, actualComparisonOperation);
        }

        [Test]
        public void RightOperand_SetterWorksProperly()
        {
            // Assert
            string rightOperand = "rightOperand";

            // Act
            _unaryStatement.RightOperand = rightOperand;

            // Arrange
            Assert.AreEqual(rightOperand, _unaryStatement.RightOperand);
        }

        [Test]
        public void RightOperand_GetterWorksProperly()
        {
            // Assert
            string rightOperand = "rightOperand";
            _unaryStatement.RightOperand = rightOperand;

            // Act
            string actualRightOperand = _unaryStatement.RightOperand;

            // Arrange
            Assert.AreEqual(rightOperand, actualRightOperand);
        }
    }
}
