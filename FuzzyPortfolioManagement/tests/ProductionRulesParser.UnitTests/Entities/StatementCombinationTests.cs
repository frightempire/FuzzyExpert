using System;
using System.Collections.Generic;
using NUnit.Framework;
using ProductionRulesParser.Entities;
using ProductionRulesParser.Enums;

namespace ProductionRulesParser.UnitTests.Entities
{
    [TestFixture]
    public class StatementCombinationTests
    {
        [Test]
        public void Constructor_ThrowsArgumentNullExceptionIfUnaryStatementsIsNull()
        {
            // Arrange
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new StatementCombination(null));
        }

        [Test]
        public void Constructor_ThrowsArgumentExceptionIfUnaryStatementsIsEmpty()
        {
            // Arrange
            List<UnaryStatement> unaryStatements = new List<UnaryStatement>();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new StatementCombination(unaryStatements));
        }

        [Test]
        public void UnaryStatementsGetterReturnsValue()
        {
            // Arrange
            List<UnaryStatement> expectedUnaryStatements = new List<UnaryStatement>
            {
                new UnaryStatement("A", ComparisonOperation.Equal, "10"),
                new UnaryStatement("B", ComparisonOperation.Equal, "20")
            };
            StatementCombination statementCombination = new StatementCombination(expectedUnaryStatements);

            // Act
            List<UnaryStatement> actualUnaryStatements = statementCombination.UnaryStatements;

            //
            Assert.AreEqual(expectedUnaryStatements, actualUnaryStatements);
        }
    }
}
