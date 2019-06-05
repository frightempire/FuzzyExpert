using System;
using System.Collections.Generic;
using CommonLogic.Implementations;
using KnowledgeManager.Helpers;
using NUnit.Framework;
using ProductionRuleParser.Entities;
using ProductionRuleParser.Enums;

namespace KnowledgeManager.UnitTests.Helpers
{
    [TestFixture]
    public class NameSupervisorTests
    {
        private UniqueNameProvider _nameProvider;
        private NameSupervisor _nameSupervisor;

        [SetUp]
        public void SetUp()
        {
            _nameProvider = new UniqueNameProvider();
            _nameSupervisor = new NameSupervisor(_nameProvider);
        }

        [Test]
        public void Constructor_ThrowsArgumentNullExceptionIfOneOfInputParametersIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => { new NameSupervisor(null); });
        }

        [Test]
        public void AssignNames_WorksProperly()
        {
            // Arrange
            var first = new UnaryStatement("A", ComparisonOperation.Equal, "5");
            var second = new UnaryStatement("C", ComparisonOperation.Equal, "6");
            var third = new UnaryStatement("X", ComparisonOperation.Equal, "7");
            var fourth = new UnaryStatement("B", ComparisonOperation.Equal, "10");
            var fifth = new UnaryStatement("C", ComparisonOperation.Equal, "6");
            var sixth = new UnaryStatement("X", ComparisonOperation.Equal, "7");
            List<UnaryStatement> unaryStatements = new List<UnaryStatement>
            {
                first, second, third, fourth, fifth, sixth
            };

            // Act
            _nameSupervisor.AssignNames(unaryStatements);

            // Assert
            Assert.AreEqual("A1", first.Name);
            Assert.AreEqual("A2", second.Name);
            Assert.AreEqual("A3", third.Name);
            Assert.AreEqual("A4", fourth.Name);
            Assert.AreEqual("A2", fifth.Name);
            Assert.AreEqual("A3", sixth.Name);
        }
    }
}