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
            Assert.IsTrue(TestHelper.ListsAreSequencualyEqual(_ifUnaryStatements, _implicationRule.IfStatement));
        }

        [Test]
        public void IfStatement_GetterWorksProperly()
        {
            // Act
            List<StatementCombination> actualUnaryStatements = _implicationRule.IfStatement;

            // Assert
            Assert.IsTrue(TestHelper.ListsAreSequencualyEqual(_ifUnaryStatements, actualUnaryStatements));
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
    }
}
