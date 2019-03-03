using System;
using System.Collections.Generic;
using KnowledgeManager.Entities;
using NUnit.Framework;

namespace KnowledgeManager.UnitTests.Entities
{
    [TestFixture]
    public class ImplicationRuleRelationsTests
    {
        private int _implicationRuleNumber = 1;
        private readonly List<ImplicationRulesConnection> _antecedentRuleNumbers =
            new List<ImplicationRulesConnection> {new ImplicationRulesConnection(2), new ImplicationRulesConnection(3)};
        private readonly List<ImplicationRulesConnection> _decendentRuleNumbers =
            new List<ImplicationRulesConnection> {new ImplicationRulesConnection(4), new ImplicationRulesConnection(5)};
        private readonly List<int> _linguisticVariableNumbers = new List<int> {1,2,3};
        private ImplicationRuleRelations _implicationRuleRelations;

        [SetUp]
        public void SetUp()
        {
            _implicationRuleRelations = new ImplicationRuleRelations(
                _implicationRuleNumber, _antecedentRuleNumbers,
                _decendentRuleNumbers, _linguisticVariableNumbers);
        }

        [Test]
        public void Constructor_ThrowsArgumentNullExceptionIfAntecedentRuleNumbersIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ImplicationRuleRelations(
                _implicationRuleNumber, null,
                _decendentRuleNumbers, _linguisticVariableNumbers));
        }

        [Test]
        public void Constructor_ThrowsArgumentNullExceptionIfDecendentRuleNumbersIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ImplicationRuleRelations(
                _implicationRuleNumber, _antecedentRuleNumbers,
                null, _linguisticVariableNumbers));
        }

        [Test]
        public void Constructor_ThrowsArgumentNullExceptionIfLinguisticVariableNumbersIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ImplicationRuleRelations(
                _implicationRuleNumber, _antecedentRuleNumbers,
                _decendentRuleNumbers, null));
        }

        [Test]
        public void Constructor_ThrowsArgumentNullExceptionIfLinguisticVariableNumbersIsEmpty()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ImplicationRuleRelations(
                _implicationRuleNumber, _antecedentRuleNumbers,
                _decendentRuleNumbers, new List<int>()));
        }

        [Test]
        public void ImplicationRuleNumber_GetterReturnsValue()
        {
            // Act
            int actualImplicationRuleNumber = _implicationRuleRelations.ImplicationRuleNumber;

            // Assert
            Assert.AreEqual(_implicationRuleNumber, actualImplicationRuleNumber);
        }

        [Test]
        public void AntecedentRuleNumbers_GetterReturnsValue()
        {
            // Act
            List<ImplicationRulesConnection> actualAntecedentRuleNumbers = _implicationRuleRelations.AntecedentRuleNumbers;

            // Assert
            Assert.AreEqual(_antecedentRuleNumbers, actualAntecedentRuleNumbers);
        }

        [Test]
        public void DecendentRuleNumbers_GetterReturnsValue()
        {
            // Act
            List<ImplicationRulesConnection> actualDecendentRuleNumbers = _implicationRuleRelations.DecendentRuleNumbers;

            // Assert
            Assert.AreEqual(_decendentRuleNumbers, actualDecendentRuleNumbers);
        }

        [Test]
        public void LinguisticVariableNumbers_GetterReturnsValue()
        {
            // Act
            List<int> actualLinguisticVariableNumbers = _implicationRuleRelations.LinguisticVariableNumbers;

            // Assert
            Assert.AreEqual(_linguisticVariableNumbers, actualLinguisticVariableNumbers);
        }
    }
}