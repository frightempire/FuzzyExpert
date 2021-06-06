using System;
using System.Collections.Generic;
using FuzzyExpert.Base.UnitTests;
using FuzzyExpert.Core.Entities;
using FuzzyExpert.Core.Enums;
using NUnit.Framework;

namespace FuzzyExpert.Core.UnitTests.Entities
{
    [TestFixture]
    public class ImplicationRuleTests
    {
        private ImplicationRule _implicationRule;

        private readonly List<StatementCombination> _ifUnaryStatements = new List<StatementCombination>
        {
            new StatementCombination(new List<UnaryStatement>
            {
                new UnaryStatement("LeftOperand", ComparisonOperation.Equal, "RightOperand"),
                new UnaryStatement("OperandLeft", ComparisonOperation.Equal, "OperandRight")
            })           
        };

        private readonly List<LogicalOperation> _logicalOperationsOrder = new List<LogicalOperation>
        {
            LogicalOperation.And, LogicalOperation.And, LogicalOperation.Or
        };

        private readonly StatementCombination _thenUnaryStatement = new StatementCombination(new List<UnaryStatement>
        {
            new UnaryStatement("LeftOperand", ComparisonOperation.Equal, "RightOperand")
        });

        [SetUp]
        public void SetUp()
        {
            _implicationRule = new ImplicationRule(_ifUnaryStatements, _thenUnaryStatement);
        }

        [Test]
        public void Constructor_ThrowsArgumentNullExceptionIfIfStatementIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(delegate
            {
                new ImplicationRule(null, _thenUnaryStatement);
            });
        }

        [Test]
        public void Constructor_ThrowsArgumentNullExceptionIfThenStatementIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(delegate
            {
                new ImplicationRule(_ifUnaryStatements, null);
            });
        }

        [Test]
        public void IfStatement_ConstructorSetsProperly()
        {
            // Assert
            Assert.IsTrue(TestHelper.ListsAreSequentiallyEqual(_ifUnaryStatements, _implicationRule.IfStatement));
        }

        [Test]
        public void IfStatement_GetterWorksProperly()
        {
            // Act
            List<StatementCombination> actualUnaryStatements = _implicationRule.IfStatement;

            // Assert
            Assert.IsTrue(TestHelper.ListsAreSequentiallyEqual(_ifUnaryStatements, actualUnaryStatements));
        }

        [Test]
        public void ThenStatement_ConstructorSetsProperly()
        {
            // Assert
            Assert.AreEqual(_thenUnaryStatement, _implicationRule.ThenStatement);
        }

        [Test]
        public void ThenStatement_GetterWorksProperly()
        {
            // Act
            StatementCombination actualUnaryStatement = _implicationRule.ThenStatement;

            // Assert
            Assert.AreEqual(_thenUnaryStatement, actualUnaryStatement);
        }

        [Test]
        public void ToString_ReturnsCorrectStringRepresentaionSingleUnaryCase()
        {
            // Arrange
            string expectedStringRepresentation = "IF (C != 2) THEN (X = 10)";
            ImplicationRule implicationRule = new ImplicationRule(
                new List<StatementCombination>
                {
                    new StatementCombination(new List<UnaryStatement>
                    {
                        new UnaryStatement("C", ComparisonOperation.NotEqual, "2")
                    })
                },
                new StatementCombination(new List<UnaryStatement>
                {
                    new UnaryStatement("X", ComparisonOperation.Equal, "10")
                }));

            // Act
            string actualStringRepresentation = implicationRule.ToString();

            // Assert
            Assert.AreEqual(expectedStringRepresentation, actualStringRepresentation);
        }

        [Test]
        public void ToString_ReturnsCorrectStringRepresentaionSimpleCase()
        {
            // Arrange
            string expectedStringRepresentation = "IF (B != 1 & C != 2) THEN (X = 10)";
            ImplicationRule implicationRule = new ImplicationRule(
                new List<StatementCombination>
                {
                    new StatementCombination(new List<UnaryStatement>
                    {
                        new UnaryStatement("B", ComparisonOperation.NotEqual, "1"),
                        new UnaryStatement("C", ComparisonOperation.NotEqual, "2")
                    })
                },
                new StatementCombination(new List<UnaryStatement>
                {
                    new UnaryStatement("X", ComparisonOperation.Equal, "10")
                }));

            // Act
            string actualStringRepresentation = implicationRule.ToString();

            // Assert
            Assert.AreEqual(expectedStringRepresentation, actualStringRepresentation);
        }

        [Test]
        public void ToString_ReturnsCorrectStringRepresentaionComplexCase()
        {
            // Arrange
            string expectedStringRepresentation = "IF ((B != 1 & C != 2) | D >= 5) THEN (X = 10 & Y = 7)";
            ImplicationRule implicationRule = new ImplicationRule(
                new List<StatementCombination>
                {
                    new StatementCombination(new List<UnaryStatement>
                    {
                        new UnaryStatement("B", ComparisonOperation.NotEqual, "1"),
                        new UnaryStatement("C", ComparisonOperation.NotEqual, "2")
                    }),
                    new StatementCombination(new List<UnaryStatement>
                    {
                        new UnaryStatement("D", ComparisonOperation.GreaterOrEqual, "5")
                    })
                },
                new StatementCombination(new List<UnaryStatement>
                {
                    new UnaryStatement("X", ComparisonOperation.Equal, "10"),
                    new UnaryStatement("Y", ComparisonOperation.Equal, "7")
                }));

            // Act
            string actualStringRepresentation = implicationRule.ToString();

            // Assert
            Assert.AreEqual(expectedStringRepresentation, actualStringRepresentation);
        }

        [Test]
        public void ToString_ReturnsCorrectStringRepresentaionReversedComplexCase()
        {
            // Arrange
            string expectedStringRepresentation = "IF (D >= 5 | (B != 1 & C != 2)) THEN (X = 10 & Y = 7)";
            ImplicationRule implicationRule = new ImplicationRule(
                new List<StatementCombination>
                {
                    new StatementCombination(new List<UnaryStatement>
                    {
                        new UnaryStatement("D", ComparisonOperation.GreaterOrEqual, "5")
                    }),
                    new StatementCombination(new List<UnaryStatement>
                    {
                        new UnaryStatement("B", ComparisonOperation.NotEqual, "1"),
                        new UnaryStatement("C", ComparisonOperation.NotEqual, "2")
                    })
                },
                new StatementCombination(new List<UnaryStatement>
                {
                    new UnaryStatement("X", ComparisonOperation.Equal, "10"),
                    new UnaryStatement("Y", ComparisonOperation.Equal, "7")
                }));

            // Act
            string actualStringRepresentation = implicationRule.ToString();

            // Assert
            Assert.AreEqual(expectedStringRepresentation, actualStringRepresentation);
        }
    }
}