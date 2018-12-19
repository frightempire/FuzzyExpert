using System;
using NUnit.Framework;
using ProductionRuleParser.Entities;
using ProductionRuleParser.Enums;

namespace ProductionRuleParser.UnitTests.Entities
{
    [TestFixture]
    public class UnaryStatementTests
    {
        private UnaryStatement _unaryStatement;

        private string _expectedLeftOperand = "leftOperand";
        private ComparisonOperation _expectedComparisonOperation = ComparisonOperation.Equal;
        private string _expectedRightOperand = "rightOperand";

        [SetUp]
        public void SetUp()
        {
            _unaryStatement = new UnaryStatement(_expectedLeftOperand, _expectedComparisonOperation, _expectedRightOperand);
        }

        [Test]
        public void Constructor_ThrowsArgumentNullExceptionIfLeftOperandIsEmpty()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(delegate
            {
                new UnaryStatement(string.Empty, _expectedComparisonOperation, _expectedRightOperand);
            });
        }

        [Test]
        public void Constructor_ThrowsArgumentNullExceptionIfLeftOperandIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(delegate
            {
                new UnaryStatement(null, _expectedComparisonOperation, _expectedRightOperand);
            });
        }

        [Test]
        public void Constructor_ThrowsArgumentNullExceptionIfRightOperandIsEmpty()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(delegate
            {
                new UnaryStatement(_expectedLeftOperand, _expectedComparisonOperation, string.Empty);
            });
        }

        [Test]
        public void Constructor_ThrowsArgumentNullExceptionIfRightOperandIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(delegate
            {
                new UnaryStatement(_expectedLeftOperand, _expectedComparisonOperation, null);
            });
        }

        [Test]
        public void Constructor_ThrowsArgumentNullExceptionIfIfStatementIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(delegate
            {
                new UnaryStatement(null, _expectedComparisonOperation, _expectedRightOperand);
            });
        }

        [Test]
        public void LeftOperand_ConstructorSetsProperly()
        {
            // Assert
            Assert.AreEqual(_expectedLeftOperand, _unaryStatement.LeftOperand);
        }

        [Test]
        public void LeftOperand_GetterWorksProperly()
        {
            // Act
            string actualLeftOperand = _unaryStatement.LeftOperand;

            // Assert
            Assert.AreEqual(_expectedLeftOperand, actualLeftOperand);
        }

        [Test]
        public void ComparisonOperation_ConstructorSetsProperly()
        {
            // Assert
            Assert.AreEqual(_expectedComparisonOperation, _unaryStatement.ComparisonOperation);
        }

        [Test]
        public void ComparisonOperation_GetterWorksProperly()
        {
            // Act
            ComparisonOperation actualComparisonOperation = _unaryStatement.ComparisonOperation;

            // Assert
            Assert.AreEqual(_expectedComparisonOperation, actualComparisonOperation);
        }

        [Test]
        public void RightOperand_ConstructorSetsProperly()
        {
            // Assert
            Assert.AreEqual(_expectedRightOperand, _unaryStatement.RightOperand);
        }

        [Test]
        public void RightOperand_GetterWorksProperly()
        {
            // Act
            string actualRightOperand = _unaryStatement.RightOperand;

            // Assert
            Assert.AreEqual(_expectedRightOperand, actualRightOperand);
        }
    }
}
