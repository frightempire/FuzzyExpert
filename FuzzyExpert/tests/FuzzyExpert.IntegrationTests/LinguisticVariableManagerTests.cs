using System.Collections.Generic;
using FuzzyExpert.Application.Common.Entities;
using FuzzyExpert.Base.UnitTests;
using FuzzyExpert.Core.Entities;
using FuzzyExpert.Infrastructure.KnowledgeManager.Implementations;
using FuzzyExpert.Infrastructure.LinguisticVariableParsing.Implementations;
using FuzzyExpert.Infrastructure.MembershipFunctionParsing.Implementations;
using FuzzyExpert.Infrastructure.ProfileManaging.Entities;
using FuzzyExpert.Infrastructure.ProfileManaging.Implementations;
using NUnit.Framework;

namespace FuzzyExpert.IntegrationTests
{
    [TestFixture]
    public class LinguisticVariableManagerTests
    {
        private readonly string _profileName = "profile_name";
        private ProfileRepository _profileRepository;
        private LinguisticVariableManager _linguisticVariableManager;

        [SetUp]
        public void SetUp()
        {
            PrepareProfileRepository();
            PrepareLinguisticVariableManager();
        }

        private void PrepareProfileRepository()
        {
            _profileRepository = new ProfileRepository();
            var profile = new InferenceProfile
            {
                ProfileName = _profileName,
                Variables = new List<string>
                {
                    "Water:Initial:[Cold:Trapezoidal:(0,20,20,30)|Hot:Trapezoidal:(50,60,60,80)]",
                    "Pressure:Derivative:[Low:Trapezoidal:(20,50,50,60)|High:Trapezoidal:(80,100,100,150)]"
                }
            };
            _profileRepository.SaveProfile(profile);
        }

        private void PrepareLinguisticVariableManager()
        {
            MembershipFunctionParser membershipFunctionParser = new MembershipFunctionParser();
            LinguisticVariableParser linguisticVariableParser = new LinguisticVariableParser(membershipFunctionParser);
            MembershipFunctionCreator membershipFunctionCreator = new MembershipFunctionCreator();
            LinguisticVariableCreator linguisticVariableCreator = new LinguisticVariableCreator(membershipFunctionCreator, linguisticVariableParser);

            DatabaseLinguisticVariableProvider linguisticVariableProvider = new DatabaseLinguisticVariableProvider(
                _profileRepository,
                linguisticVariableCreator);
            _linguisticVariableManager = new LinguisticVariableManager(linguisticVariableProvider);
        }

        [Test]
        public void GetLinguisticVariables_ReturnsLinguisticVariablesList()
        {
            // Arrange
            Optional<Dictionary<int, LinguisticVariable>> expectedLinguisticVariables =
                Optional<Dictionary<int, LinguisticVariable>>.For(PrepareExpectedLinguisticVariables());

            // Act
            Optional<Dictionary<int, LinguisticVariable>> actualLinguisticVariables = _linguisticVariableManager.GetLinguisticVariables(_profileName);

            // Assert
            Assert.IsTrue(actualLinguisticVariables.IsPresent);
            Assert.AreEqual(expectedLinguisticVariables.Value.Count, actualLinguisticVariables.Value.Count);
            for (int i = 1; i <= actualLinguisticVariables.Value.Count; i++)
            {
                Assert.IsTrue(ObjectComparer.LinguisticVariablesAreEqual(expectedLinguisticVariables.Value[i], actualLinguisticVariables.Value[i]));
            }
        }

        private Dictionary<int, LinguisticVariable> PrepareExpectedLinguisticVariables()
        {
            // Water variable
            MembershipFunctionList firstMembershipFunctionList = new MembershipFunctionList
            {
                new TrapezoidalMembershipFunction("Cold", 0, 20, 20, 30),
                new TrapezoidalMembershipFunction("Hot", 50, 60, 60, 80)
            };
            LinguisticVariable firstLinguisticVariable = new LinguisticVariable("Water", firstMembershipFunctionList, isInitialData: true);

            // Pressure variable
            MembershipFunctionList secondsMembershipFunctionList = new MembershipFunctionList
            {
                new TrapezoidalMembershipFunction("Low", 20, 50, 50, 60),
                new TrapezoidalMembershipFunction("High", 80, 100, 100, 150)
            };
            LinguisticVariable secondLinguisticVariable = new LinguisticVariable("Pressure", secondsMembershipFunctionList, isInitialData: false);

            return new Dictionary<int, LinguisticVariable>
            {
                {1, firstLinguisticVariable},
                {2, secondLinguisticVariable}
            };
        }
    }
}