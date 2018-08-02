using System;
using System.Collections.Generic;
using Base.UnitTests;
using NUnit.Framework;
using ProductionRulesParser.Entities;
using ProductionRulesParser.Enums;

namespace ProductionRulesParser.UnitTests.Entities
{
    [TestFixture]
    public class ProductionRuleTests
    {
        private ProductionRule _productionRule;

        private readonly List<UnaryStatement> _ifUnaryStatements = new List<UnaryStatement>
        {
            new UnaryStatement("LeftOperand", ComparisonOperation.Equal, "RightOperand"),
            new UnaryStatement("OperandLeft", ComparisonOperation.Equal, "OperandRight")
        };

        private readonly List<LogicalOperation> _logicalOperationsOrder = new List<LogicalOperation>
        {
            LogicalOperation.And, LogicalOperation.And, LogicalOperation.Or
        };

        private readonly UnaryStatement _thenUnaryStatement = new UnaryStatement("LeftOperand", ComparisonOperation.Equal, "RightOperand");

        [SetUp]
        public void SetUp()
        {
            _productionRule = new ProductionRule(_ifUnaryStatements, _logicalOperationsOrder, _thenUnaryStatement);
        }

        [Test]
        public void Constructor_ThrowsArgumentNullExceptionIfIfStatementIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(delegate
            {
                new ProductionRule(null, _logicalOperationsOrder, _thenUnaryStatement);
            });
        }

        [Test]
        public void Constructor_ThrowsArgumentNullExceptionIfLogicalOperationsOrderIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(delegate
            {
                new ProductionRule(_ifUnaryStatements, null, _thenUnaryStatement);
            });
        }

        [Test]
        public void Constructor_ThrowsArgumentNullExceptionIfThenStatementIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(delegate
            {
                new ProductionRule(_ifUnaryStatements, _logicalOperationsOrder, null);
            });
        }

        [Test]
        public void IfStatement_ConstructorSetsProperly()
        {
            // Assert
            Assert.IsTrue(TestHelper.ListsAreSequencualyEqual(_ifUnaryStatements, _productionRule.IfStatement));
        }

        [Test]
        public void IfStatement_GetterWorksProperly()
        {
            // Act
            List<UnaryStatement> actualUnaryStatements = _productionRule.IfStatement;

            // Assert
            Assert.IsTrue(TestHelper.ListsAreSequencualyEqual(_ifUnaryStatements, actualUnaryStatements));
        }

        [Test]
        public void LogicalOperationsOrder_ConstructorSetsProperly()
        {
            // Assert
            Assert.IsTrue(TestHelper.ListsAreSequencualyEqual(_logicalOperationsOrder, _productionRule.LogicalOperationsOrder));
        }

        [Test]
        public void LogicalOperationsOrder_GetterWorksProperly()
        {
            // Act
            List<LogicalOperation> actualLogicalOperationsOrder = _productionRule.LogicalOperationsOrder;

            // Assert
            Assert.IsTrue(TestHelper.ListsAreSequencualyEqual(_logicalOperationsOrder, actualLogicalOperationsOrder));
        }

        [Test]
        public void ThenStatement_ConstructorSetsProperly()
        {
            // Assert
            Assert.AreEqual(_thenUnaryStatement, _productionRule.ThenStatement);
        }

        [Test]
        public void ThenStatement_GetterWorksProperly()
        {
            // Act
            UnaryStatement actualUnaryStatement = _productionRule.ThenStatement;

            // Assert
            Assert.AreEqual(_thenUnaryStatement, actualUnaryStatement);
        }
    }
}
