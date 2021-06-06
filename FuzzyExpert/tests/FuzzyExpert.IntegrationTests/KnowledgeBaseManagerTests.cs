using System.Collections.Generic;
using FuzzyExpert.Application.Common.Implementations;
using FuzzyExpert.Application.Entities;
using FuzzyExpert.Base.UnitTests;
using FuzzyExpert.Infrastructure.DatabaseManagement.Entities;
using FuzzyExpert.Infrastructure.DatabaseManagement.Implementations;
using FuzzyExpert.Infrastructure.KnowledgeManager.Implementations;
using FuzzyExpert.Infrastructure.LinguisticVariableParsing.Implementations;
using FuzzyExpert.Infrastructure.MembershipFunctionParsing.Implementations;
using FuzzyExpert.Infrastructure.ProductionRuleParsing.Implementations;
using FuzzyExpert.Infrastructure.ResultLogging.Implementations;
using NUnit.Framework;

namespace FuzzyExpert.IntegrationTests
{
    [TestFixture]
    public class KnowledgeBaseManagerTests
    {
        private readonly string _profileName = "profile_name";
        private ProfileRepository _profileRepository;   
        private KnowledgeBaseManager _knowledgeBaseManager;

        [SetUp]
        public void SetUp()
        {
            PrepareProfileRepository();
            PrepareKnowledgeBaseManager();
        }

        private void PrepareProfileRepository()
        {
            _profileRepository = new ProfileRepository(new ConnectionStringProvider());
            var profile = new InferenceProfile
            {
                ProfileName = _profileName,
                Rules = new List<string>
                {
                    "IF(Temperature=HOT)THEN(Pressure=HIGH)",
                    "IF(Volume=BIG&Color=RED)THEN(Danger=HIGH)",
                    "IF(Pressure=HIGH&Danger=HIGH)THEN(Evacuate=TRUE)"
                },
                Variables = new List<string>
                {
                    "[Temperature]:Initial:[Cold:Trapezoidal:(0,20,20,30)|HOT:Trapezoidal:(50,60,60,80)]",
                    "[Pressure]:Derivative:[Low: Trapezoidal:(20, 50, 50, 60) | HIGH:Trapezoidal: (80, 100, 100, 150)]",
                    "[Volume]:Initial:[Small:Trapezoidal:(100,200,200,600)|BIG:Trapezoidal:(800,1000,1100,1500)]",
                    "[Color]:Initial:[Blue:Trapezoidal:(5,10,10,20)|RED:Trapezoidal:(50,60,65,80)]",
                    "[Danger]:Derivative:[Low:Trapezoidal:(5,10,10,20)|HIGH:Trapezoidal:(50,60,60,80)]",
                    "[Evacuate]:Derivative:[TRUE:Trapezoidal:(5,10,10,20)|False:Trapezoidal:(50,60,60,80)]"
                }
            };
            _profileRepository.SaveProfile(profile);
        }

        private void PrepareKnowledgeBaseManager()
        {


            // Implication rule manager
            ImplicationRuleParser ruleParser = new ImplicationRuleParser();
            ImplicationRuleCreator ruleCreator = new ImplicationRuleCreator(ruleParser);
            DatabaseImplicationRuleProvider ruleProvider = new DatabaseImplicationRuleProvider(
                _profileRepository,
                ruleCreator);
            ImplicationRuleManager implicationRuleManager = new ImplicationRuleManager(ruleProvider);

            // Linguistic variable manager
            MembershipFunctionParser membershipFunctionParser = new MembershipFunctionParser();
            LinguisticVariableParser linguisticVariableParser = new LinguisticVariableParser(membershipFunctionParser);
            MembershipFunctionCreator membershipFunctionCreator = new MembershipFunctionCreator();
            LinguisticVariableCreator linguisticVariableCreator = new LinguisticVariableCreator(membershipFunctionCreator, linguisticVariableParser);
            DatabaseLinguisticVariableProvider linguisticVariableProvider = new DatabaseLinguisticVariableProvider(
                _profileRepository,
                linguisticVariableCreator);
            LinguisticVariableManager linguisticVariableManager = new LinguisticVariableManager(linguisticVariableProvider);

            // Knowledge base manager
            FileOperations fileOperations = new FileOperations();
            FileValidationOperationResultLogger fileValidationOperationResultLogger = new FileValidationOperationResultLogger(fileOperations);
            KnowledgeBaseValidator knowledgeBaseValidator = new KnowledgeBaseValidator();
            LinguisticVariableRelationsInitializer relationsInitializer = new LinguisticVariableRelationsInitializer();
            _knowledgeBaseManager = new KnowledgeBaseManager(
                implicationRuleManager,
                linguisticVariableManager,
                knowledgeBaseValidator,
                relationsInitializer,
                fileValidationOperationResultLogger);
        }

        [Test]
        public void GetKnowledgeBase_ReturnsCorrectLinguisticVariablesRelations()
        {
            // Arrange
            List<LinguisticVariableRelations> expectedRelations = new List<LinguisticVariableRelations>
            {
                new LinguisticVariableRelations(1, new List<string> {"Temperature = HOT"}),
                new LinguisticVariableRelations(2, new List<string> {"Pressure = HIGH"}),
                new LinguisticVariableRelations(3, new List<string> {"Volume = BIG"}),
                new LinguisticVariableRelations(4, new List<string> {"Color = RED"}),
                new LinguisticVariableRelations(5, new List<string> {"Danger = HIGH"}),
                new LinguisticVariableRelations(6, new List<string> {"Evacuate = TRUE"})
            };

            // Act
            List<LinguisticVariableRelations> actualRelations = _knowledgeBaseManager.GetKnowledgeBase(_profileName).Value.LinguisticVariablesRelations;

            // Assert
            Assert.AreEqual(expectedRelations.Count, actualRelations.Count);
            for (var i = 0; i < expectedRelations.Count; i++)
            {
                Assert.IsTrue(ObjectComparer.LinguisticVariableRelationsAreEqual(expectedRelations[i], actualRelations[i]));
            }
        }
    }
}