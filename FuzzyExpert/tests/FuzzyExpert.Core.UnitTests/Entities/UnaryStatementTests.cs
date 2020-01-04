using System;
using FuzzyExpert.Core.Entities;
using FuzzyExpert.Core.Enums;
using FuzzyExpert.Core.Extensions;
using NUnit.Framework;

namespace FuzzyExpert.Core.UnitTests.Entities
{
    [TestFixture]
    public class UnaryStatementTests
    {
        private UnaryStatement _unaryStatement;

        private string _expectedName = "A1";
        private string _expectedLeftOperand = "leftOperand";
        private ComparisonOperation _expectedComparisonOperation = ComparisonOperation.Equal;
        private string _expectedRightOperand = "rightOperand";

        [SetUp]
        public void SetUp()
        {
            _unaryStatement = new UnaryStatement(_expectedLeftOperand, _expectedComparisonOperation, _expectedRightOperand)
            { Name = _expectedName};
        }

        [Test]
        public void Constructor_ThrowsArgumentNullExceptionIfOneOfInputParametersIsIncorrect()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(delegate
            {
                new UnaryStatement(string.Empty, _expectedComparisonOperation, _expectedRightOperand);
            });
            Assert.Throws<ArgumentNullException>(delegate
            {
                new UnaryStatement(null, _expectedComparisonOperation, _expectedRightOperand);
            });
            Assert.Throws<ArgumentNullException>(delegate
            {
                new UnaryStatement(_expectedLeftOperand, _expectedComparisonOperation, string.Empty);
            });
            Assert.Throws<ArgumentNullException>(delegate
            {
                new UnaryStatement(_expectedLeftOperand, _expectedComparisonOperation, null);
            });
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

        [Test]
        public void Name_SetterWorksProperly()
        {
            // Act
            _unaryStatement.Name = _expectedName;

            // Assert
            Assert.AreEqual(_expectedName, _unaryStatement.Name);
        }

        [Test]
        public void Name_GetterWorksProperly()
        {
            // Act
            string actualName = _unaryStatement.Name;

            // Assert
            Assert.AreEqual(_expectedName, actualName);
        }

        [Test]
        public void ToString_ReturnsCorrectRepresentation()
        {
            // Arrange
            string expectedString = $"{_expectedLeftOperand} {_expectedComparisonOperation.GetDescription()} {_expectedRightOperand}";

            // Act
            string actualString = _unaryStatement.ToString();

            // Assert
            Assert.AreEqual(expectedString, actualString);
        }
    }
}