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

        [SetUp]
        public void SetUp()
        {
            _productionRule = new ProductionRule();
        }

        public void IfStatement_SetterWorksProperly()
        {
            // Arrange
            List<UnaryStatement> unaryStatements = new List<UnaryStatement>
            {
                new UnaryStatement
                {
                    LeftOperand = "LeftOperand",
                    ComparisonOperation = ComparisonOperation.Equal,
                    RightOperand = "RightOperand"
                },
                new UnaryStatement
                {
                    LeftOperand = "OperandLeft",
                    ComparisonOperation = ComparisonOperation.Equal,
                    RightOperand = "OperandRight"
                },
            };

            // Act
            _productionRule.IfStatement = unaryStatements;

            // Assert
            Assert.IsTrue(TestHelper.ListsAreSequencualyEqual(unaryStatements, _productionRule.IfStatement));
        }

        public void IfStatement_GetterWorksProperly()
        {
            // Arrange
            List<UnaryStatement> unaryStatements = new List<UnaryStatement>
            {
                new UnaryStatement
                {
                    LeftOperand = "LeftOperand",
                    ComparisonOperation = ComparisonOperation.Equal,
                    RightOperand = "RightOperand"
                },
                new UnaryStatement
                {
                    LeftOperand = "OperandLeft",
                    ComparisonOperation = ComparisonOperation.Equal,
                    RightOperand = "OperandRight"
                },
            };
            _productionRule.IfStatement = unaryStatements;

            // Act
            List<UnaryStatement> actualUnaryStatements = _productionRule.IfStatement;

            // Assert
            Assert.IsTrue(TestHelper.ListsAreSequencualyEqual(unaryStatements, actualUnaryStatements));
        }

        public void LogicalOperationsOrder_SetterWorksProperly()
        {
            // Arrange
            List<LogicalOperation> logicalOperationsOrder = new List<LogicalOperation>
            {
                LogicalOperation.And, LogicalOperation.And, LogicalOperation.Or
            };

            // Act
            _productionRule.LogicalOperationsOrder = logicalOperationsOrder;

            // Assert
            Assert.IsTrue(TestHelper.ListsAreSequencualyEqual(logicalOperationsOrder, _productionRule.LogicalOperationsOrder));
        }

        public void LogicalOperationsOrder_GetterWorksProperly()
        {
            // Arrange
            List<LogicalOperation> logicalOperationsOrder = new List<LogicalOperation>
            {
                LogicalOperation.And, LogicalOperation.And, LogicalOperation.Or
            };
            _productionRule.LogicalOperationsOrder = logicalOperationsOrder;

            // Act
            List<LogicalOperation> actualLogicalOperationsOrder = _productionRule.LogicalOperationsOrder;

            // Assert
            Assert.IsTrue(TestHelper.ListsAreSequencualyEqual(logicalOperationsOrder, actualLogicalOperationsOrder));
        }

        public void ThenStatement_SetterWorksProperly()
        {
            // Arrange
            UnaryStatement unaryStatement = new UnaryStatement
            {
                LeftOperand = "LeftOperand",
                ComparisonOperation = ComparisonOperation.Equal,
                RightOperand = "RightOperand"
            };

            // Act
            _productionRule.ThenStatement = unaryStatement;

            // Assert
            Assert.AreEqual(unaryStatement, _productionRule.ThenStatement);
        }

        public void ThenStatement_GetterWorksProperly()
        {
            // Arrange
            // Arrange
            UnaryStatement unaryStatement = new UnaryStatement
            {
                LeftOperand = "LeftOperand",
                ComparisonOperation = ComparisonOperation.Equal,
                RightOperand = "RightOperand"
            };
            _productionRule.ThenStatement = unaryStatement;

            // Act
            UnaryStatement actualUnaryStatement = _productionRule.ThenStatement;

            // Assert
            Assert.AreEqual(unaryStatement, actualUnaryStatement);
        }
    }
}
