using System;
using System.Collections.Generic;
using FuzzyExpert.Application.Common.Entities;
using FuzzyExpert.Core.Entities;
using FuzzyExpert.Core.Enums;
using FuzzyExpert.Infrastructure.DatabaseManagement.Entities;
using FuzzyExpert.Infrastructure.DatabaseManagement.Interfaces;
using FuzzyExpert.Infrastructure.KnowledgeManager.Implementations;
using FuzzyExpert.Infrastructure.KnowledgeManager.Interfaces;
using FuzzyExpert.Infrastructure.ProductionRuleParsing.Interfaces;
using NUnit.Framework;
using Rhino.Mocks;

namespace FuzzyExpert.Infrastructure.UnitTests.KnowledgeManager.Implementations
{
    [TestFixture]
    public class DatabaseImplicationRuleProviderTests
    {
        private IProfileRepository _profileRepositoryMock;
        private IImplicationRuleCreator _implicationRuleCreatorMock;
        private INameSupervisor _nameSupervisorMock;

        private DatabaseImplicationRuleProvider _databaseImplicationRuleProvider;

        [SetUp]
        public void SetUp()
        {
            _profileRepositoryMock = MockRepository.GenerateMock<IProfileRepository>();
            _implicationRuleCreatorMock = MockRepository.GenerateMock<IImplicationRuleCreator>();
            _nameSupervisorMock = MockRepository.GenerateMock<INameSupervisor>();

            _databaseImplicationRuleProvider = new DatabaseImplicationRuleProvider(
                _profileRepositoryMock,
                _implicationRuleCreatorMock,
                _nameSupervisorMock);
        }

        [Test]
        public void Constructor_ThrowsArgumentNullExceptionIfOneOfInputParametersIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                new DatabaseImplicationRuleProvider(
                    null,
                    _implicationRuleCreatorMock,
                    _nameSupervisorMock);
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                new DatabaseImplicationRuleProvider(
                    _profileRepositoryMock,
                    null,
                    _nameSupervisorMock);
            });
            Assert.Throws<ArgumentNullException>(() =>
            {
                new DatabaseImplicationRuleProvider(
                    _profileRepositoryMock,
                    _implicationRuleCreatorMock,
                    null);
            });
        }

        [Test]
        public void GetImplicationRules_ReturnsEmptyOptional_IfThereIsNoProfile()
        {
            // Arrange
            var profileName = "profile_name";
            _profileRepositoryMock.Stub(x => x.GetProfileByName(profileName)).Return(Optional<InferenceProfile>.Empty());

            // Act
            var expectedImplicationRules = _databaseImplicationRuleProvider.GetImplicationRules(profileName);

            // Assert
            Assert.IsFalse(expectedImplicationRules.IsPresent);
        }

        [Test]
        public void GetImplicationRules_ReturnsEmptyOptional_IfTProfileHasNoRules()
        {
            // Arrange
            var profileName = "profile_name";
            var profileWithNoRules = new InferenceProfile
            {
                ProfileName = profileName
            };
            _profileRepositoryMock.Stub(x => x.GetProfileByName(profileName)).Return(Optional<InferenceProfile>.For(profileWithNoRules));

            // Act
            Optional<List<ImplicationRule>> expectedImplicationRules = _databaseImplicationRuleProvider.GetImplicationRules(profileName);

            // Assert
            Assert.IsFalse(expectedImplicationRules.IsPresent);
        }

        [Test]
        public void GetImplicationRules_ReturnsCorrectListOfRules()
        {
            // Arrange

            var firstImplicationRuleString = "IF(A>10)THEN(X=5)";
            var secondImplicationRuleString = "IF(B!=1&C!=2)THEN(X=10)";
            var implicationRulesInDatabase = new List<string>
            {
                firstImplicationRuleString, secondImplicationRuleString
            };
            var profileName = "profile_name";
            var profile = new InferenceProfile
            {
                ProfileName = profileName,
                Rules = implicationRulesInDatabase
            };
            _profileRepositoryMock.Stub(x => x.GetProfileByName(profileName)).Return(Optional<InferenceProfile>.For(profile));

            // IF (A > 10) THEN (X = 5)
            var firstImplicationRule = new ImplicationRule(
                new List<StatementCombination>
                {
                    new StatementCombination(new List<UnaryStatement>
                    {
                        new UnaryStatement("A", ComparisonOperation.Greater, "10")
                    })
                },
                new StatementCombination(new List<UnaryStatement>
                {
                    new UnaryStatement("X", ComparisonOperation.Equal, "5")
                }));
            _implicationRuleCreatorMock.Stub(x => x.CreateImplicationRuleEntity(firstImplicationRuleString)).Return(firstImplicationRule);

            // IF (B != 1 & C != 2) THEN (X = 10)
            var secondImplicationRule = new ImplicationRule(
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
            _implicationRuleCreatorMock.Stub(x => x.CreateImplicationRuleEntity(secondImplicationRuleString)).Return(secondImplicationRule);

            var expectedImplicationRules = new List<ImplicationRule>
            {
                firstImplicationRule, secondImplicationRule
            };
            var expectedOptional = Optional<List<ImplicationRule>>.For(expectedImplicationRules);

            // Act
            var actualOptional = _databaseImplicationRuleProvider.GetImplicationRules(profileName);

            // Assert
            Assert.IsTrue(actualOptional.IsPresent);
            Assert.AreEqual(expectedOptional.Value, actualOptional.Value);
        }
    }
}