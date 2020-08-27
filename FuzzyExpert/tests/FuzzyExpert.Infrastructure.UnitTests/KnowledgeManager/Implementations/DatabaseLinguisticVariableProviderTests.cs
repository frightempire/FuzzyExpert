using System;
using System.Collections.Generic;
using FuzzyExpert.Application.Common.Entities;
using FuzzyExpert.Core.Entities;
using FuzzyExpert.Infrastructure.DatabaseManagement.Entities;
using FuzzyExpert.Infrastructure.DatabaseManagement.Interfaces;
using FuzzyExpert.Infrastructure.KnowledgeManager.Implementations;
using FuzzyExpert.Infrastructure.LinguisticVariableParsing.Interfaces;
using NUnit.Framework;
using Rhino.Mocks;

namespace FuzzyExpert.Infrastructure.UnitTests.KnowledgeManager.Implementations
{
    [TestFixture]
    public class DatabaseLinguisticVariableProviderTests
    {
        private IProfileRepository _profileRepositoryMock;
        private ILinguisticVariableCreator _linguisticVariableCreatorMock;
        private DatabaseLinguisticVariableProvider _databaseLinguisticVariableProvider;

        [SetUp]
        public void SetUp()
        {
            _profileRepositoryMock = MockRepository.GenerateMock<IProfileRepository>();
            _linguisticVariableCreatorMock = MockRepository.GenerateMock<ILinguisticVariableCreator>();
            _databaseLinguisticVariableProvider = new DatabaseLinguisticVariableProvider(
                _profileRepositoryMock,
                _linguisticVariableCreatorMock);
        }

        [Test]
        public void Constructor_ExpectedBehavior()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => { new DatabaseLinguisticVariableProvider(null, _linguisticVariableCreatorMock); });
            Assert.Throws<ArgumentNullException>(() => { new DatabaseLinguisticVariableProvider(_profileRepositoryMock, null); });
        }

        [Test]
        public void GetImplicationRules_ReturnsEmptyOptional_IfThereIsNoProfile()
        {
            // Arrange
            var profileName = "profile_name";
            _profileRepositoryMock.Stub(x => x.GetProfileByName(profileName)).Return(Optional<InferenceProfile>.Empty());

            // Act
            var linguisticVariables = _databaseLinguisticVariableProvider.GetLinguisticVariables(profileName);

            // Assert
            Assert.IsFalse(linguisticVariables.IsPresent);
        }

        [Test]
        public void GetImplicationRules_ReturnsEmptyOptional_IfProfileHasNoVariables()
        {
            // Arrange
            var profileName = "profile_name";
            var profileWithNoRules = new InferenceProfile
            {
                ProfileName = profileName
            };
            _profileRepositoryMock.Stub(x => x.GetProfileByName(profileName)).Return(Optional<InferenceProfile>.For(profileWithNoRules));

            // Act
            var linguisticVariables = _databaseLinguisticVariableProvider.GetLinguisticVariables(profileName);

            // Assert
            Assert.IsFalse(linguisticVariables.IsPresent);
        }

        [Test]
        public void GetLinguisticVariables_ReturnsCorrectListOfVariables()
        {
            // Arrange
            var firstLinguisticVariableStringFromFile = "[Water]:Initial:[Cold:Trapezoidal:(0,20,20,30)|Hot:Trapezoidal:(50,60,60,80)]";
            var secondLinguisticVariableStringFromFile = "[Pressure]:Derivative:[Low:Trapezoidal:(20,50,50,60)|High:Trapezoidal:(80,100,100,150)]";
            var linguisticVariablesInDatabase = new List<string>
            {
                firstLinguisticVariableStringFromFile, secondLinguisticVariableStringFromFile
            };
            var profileName = "profile_name";
            var profile = new InferenceProfile
            {
                ProfileName = profileName,
                Variables = linguisticVariablesInDatabase
            };
            _profileRepositoryMock.Stub(x => x.GetProfileByName(profileName)).Return(Optional<InferenceProfile>.For(profile));

            // Water variable
            var firstMembershipFunctionList = new MembershipFunctionList
            {
                new TrapezoidalMembershipFunction("Cold", 0, 20, 20, 30),
                new TrapezoidalMembershipFunction("Hot", 50, 60, 60, 80)
            };
            var firstLinguisticVariable = new LinguisticVariable("Water", firstMembershipFunctionList, isInitialData:true);
            var firstLinguisticVariables = new List<LinguisticVariable> {firstLinguisticVariable};

            // Pressure variable
            var secondsMembershipFunctionList = new MembershipFunctionList
            {
                new TrapezoidalMembershipFunction("Low", 20, 50, 50, 60),
                new TrapezoidalMembershipFunction("High", 80, 100, 100, 150)
            };
            var secondLinguisticVariable = new LinguisticVariable("Pressure", secondsMembershipFunctionList, isInitialData:false);
            var secondLinguisticVariables = new List<LinguisticVariable> { secondLinguisticVariable };

            var expectedLinguisticVariables = new List<LinguisticVariable>
            {
                firstLinguisticVariable, secondLinguisticVariable
            };
            var expectedOptional = Optional<List<LinguisticVariable>>.For(expectedLinguisticVariables);

            // Stubs
            _linguisticVariableCreatorMock.Stub(x => x.CreateLinguisticVariableEntities(firstLinguisticVariableStringFromFile)).Return(firstLinguisticVariables);
            _linguisticVariableCreatorMock.Stub(x => x.CreateLinguisticVariableEntities(secondLinguisticVariableStringFromFile)).Return(secondLinguisticVariables);

            // Act
            var actualOptional = _databaseLinguisticVariableProvider.GetLinguisticVariables(profileName);

            // Assert
            Assert.IsTrue(expectedOptional.IsPresent);
            Assert.AreEqual(expectedOptional.Value, actualOptional.Value);
        }
    }
}