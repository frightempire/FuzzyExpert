using System;
using System.Collections.Generic;
using Base.UnitTests;
using NUnit.Framework;
using ProductionRulesParser.Entities;
using ProductionRulesParser.Enums;

namespace ProductionRulesParser.UnitTests.Entities
{
    [TestFixture]
    public class ImplicationRuleTests
    {
        private ImplicationRule _implicationRule;

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
            _implicationRule = new ImplicationRule(_ifUnaryStatements, _logicalOperationsOrder, _thenUnaryStatement);
        }

        [Test]
        public void Constructor_ThrowsArgumentNullExceptionIfIfStatementIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(delegate
            {
                new ImplicationRule(null, _logicalOperationsOrder, _thenUnaryStatement);
            });
        }

        [Test]
        public void Constructor_ThrowsArgumentNullExceptionIfLogicalOperationsOrderIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(delegate
            {
                new ImplicationRule(_ifUnaryStatements, null, _thenUnaryStatement);
            });
        }

        [Test]
        public void Constructor_ThrowsArgumentNullExceptionIfThenStatementIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(delegate
            {
                new ImplicationRule(_ifUnaryStatements, _logicalOperationsOrder, null);
            });
        }

        [Test]
        public void IfStatement_ConstructorSetsProperly()
        {
            // Assert
            Assert.IsTrue(TestHelper.ListsAreSequencualyEqual(_ifUnaryStatements, _implicationRule.IfStatement));
        }

        [Test]
        public void IfStatement_GetterWorksProperly()
        {
            // Act
            List<UnaryStatement> actualUnaryStatements = _implicationRule.IfStatement;

            // Assert
            Assert.IsTrue(TestHelper.ListsAreSequencualyEqual(_ifUnaryStatements, actualUnaryStatements));
        }

        [Test]
        public void LogicalOperationsOrder_ConstructorSetsProperly()
        {
            // Assert
            Assert.IsTrue(TestHelper.ListsAreSequencualyEqual(_logicalOperationsOrder, _implicationRule.LogicalOperationsOrder));
        }

        [Test]
        public void LogicalOperationsOrder_GetterWorksProperly()
        {
            // Act
            List<LogicalOperation> actualLogicalOperationsOrder = _implicationRule.LogicalOperationsOrder;

            // Assert
            Assert.IsTrue(TestHelper.ListsAreSequencualyEqual(_logicalOperationsOrder, actualLogicalOperationsOrder));
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
            UnaryStatement actualUnaryStatement = _implicationRule.ThenStatement;

            // Assert
            Assert.AreEqual(_thenUnaryStatement, actualUnaryStatement);
        }
    }
}
