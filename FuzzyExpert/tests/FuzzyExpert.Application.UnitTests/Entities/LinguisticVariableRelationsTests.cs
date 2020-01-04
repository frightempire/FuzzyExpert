using System.Collections.Generic;
using FuzzyExpert.Application.Entities;
using NUnit.Framework;

namespace FuzzyExpert.Application.UnitTests.Entities
{
    [TestFixture]
    public class LinguisticVariableRelationsTests
    {
        private int _variableNumber = 1;
        private readonly List<string> _statementNames = new List<string> {"A1", "X2"};
        private LinguisticVariableRelations _relations;

        [SetUp]
        public void SetUp()
        {
            _relations = new LinguisticVariableRelations(_variableNumber, _statementNames);
        }

        [Test]
        public void LinguisticVariableNumber_GetterWorksProperly()
        {
            // Assert
            Assert.AreEqual(_variableNumber, _relations.LinguisticVariableNumber);
        }

        [Test]
        public void RelatedUnaryStatementNames_GetterWorksProperly()
        {
            // Assert
            Assert.AreEqual(_statementNames, _relations.RelatedUnaryStatementNames);
        }
    }
}