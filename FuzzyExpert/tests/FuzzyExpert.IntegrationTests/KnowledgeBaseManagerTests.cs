using System.Collections.Generic;
using System.IO;
using FuzzyExpert.Application.Common.Implementations;
using FuzzyExpert.Application.Entities;
using FuzzyExpert.Base.UnitTests;
using FuzzyExpert.Infrastructure.KnowledgeManager.Helpers;
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
        private readonly string _filePathImplicationRules =
            Path.Combine(TestContext.CurrentContext.TestDirectory, @"TestFiles\KnowledgeBaseManager\ImplicationRules.txt");
        private readonly string _filePathLinguisticVariables =
            Path.Combine(TestContext.CurrentContext.TestDirectory, @"TestFiles\KnowledgeBaseManager\LinguisticVariables.txt");

        private KnowledgeBaseManager _knowledgeBaseManager;

        [SetUp]
        public void SetUp()
        {
            PrepareKnowledgeBaseManager();
        }

        private void PrepareKnowledgeBaseManager()
        {
            FileOperations fileOperations = new FileOperations();
            FileValidationOperationResultLogger fileValidationOperationResultLogger = new FileValidationOperationResultLogger(fileOperations);

            // Implication rule manager
            ImplicationRuleFilePathProvider filePathProviderForImplicationRules = new ImplicationRuleFilePathProvider { FilePath = _filePathImplicationRules };
            ImplicationRuleParser ruleParser = new ImplicationRuleParser();
            ImplicationRuleValidator ruleValidator = new ImplicationRuleValidator();
            ImplicationRuleCreator ruleCreator = new ImplicationRuleCreator(ruleParser);
            NameSupervisor nameSupervisor = new NameSupervisor(new UniqueNameProvider());
            FileImplicationRuleProvider ruleProvider = new FileImplicationRuleProvider(
                fileOperations,
                filePathProviderForImplicationRules,
                ruleValidator,
                ruleCreator,
                nameSupervisor,
                fileValidationOperationResultLogger);
            ImplicationRuleManager implicationRuleManager = new ImplicationRuleManager(ruleProvider);

            // Linguistic variable manager
            MembershipFunctionValidator membershipFunctionValidator = new MembershipFunctionValidator();
            LinguisticVariableValidator linguisticVariableValidator = new LinguisticVariableValidator(membershipFunctionValidator);
            MembershipFunctionParser membershipFunctionParser = new MembershipFunctionParser();
            LinguisticVariableParser linguisticVariableParser = new LinguisticVariableParser(membershipFunctionParser);
            MembershipFunctionCreator membershipFunctionCreator = new MembershipFunctionCreator();
            LinguisticVariableCreator linguisticVariableCreator = new LinguisticVariableCreator(membershipFunctionCreator, linguisticVariableParser);
            LinguisticVariableFilePathProvider filePathProviderForLinguisticVariables = new LinguisticVariableFilePathProvider { FilePath = _filePathLinguisticVariables };
            FileLinguisticVariableProvider linguisticVariableProvider = new FileLinguisticVariableProvider(
                linguisticVariableValidator,
                linguisticVariableCreator,
                filePathProviderForLinguisticVariables,
                fileOperations,
                fileValidationOperationResultLogger);
            LinguisticVariableManager linguisticVariableManager = new LinguisticVariableManager(linguisticVariableProvider);

            // Knowledge base manager
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
                new LinguisticVariableRelations(1, new List<string> {"A1"}),
                new LinguisticVariableRelations(2, new List<string> {"A4"}),
                new LinguisticVariableRelations(3, new List<string> {"A2"}),
                new LinguisticVariableRelations(4, new List<string> {"A3"}),
                new LinguisticVariableRelations(5, new List<string> {"A5"}),
                new LinguisticVariableRelations(6, new List<string> {"A6"})
            };

            // Act
            List<LinguisticVariableRelations> actualRelations = _knowledgeBaseManager.GetKnowledgeBase().Value.LinguisticVariablesRelations;

            // Assert
            Assert.AreEqual(expectedRelations.Count, actualRelations.Count);
            for (int i = 0; i < expectedRelations.Count; i++)
            {
                Assert.IsTrue(ObjectComparer.LinguisticVariableRelationsAreEqual(expectedRelations[i], actualRelations[i]));
            }
        }
    }
}