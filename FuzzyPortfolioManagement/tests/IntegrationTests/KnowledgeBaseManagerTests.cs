using System.Collections.Generic;
using System.IO;
using Base.UnitTests;
using CommonLogic.Implementations;
using KnowledgeManager.Entities;
using KnowledgeManager.Implementations;
using LinguisticVariableParser.Implementations;
using MembershipFunctionParser.Implementations;
using NUnit.Framework;
using ProductionRuleParser.Implementations;

namespace IntegrationTests
{
    [TestFixture]
    public class KnowledgeBaseManagerTests
    {
        private readonly string _filePathImplicationRules =
            Path.Combine(TestContext.CurrentContext.TestDirectory, "TestFiles\\ImplicationRules_KnowledgeBaseManager.txt");
        private FilePathProvider _filePathProviderForImplicationRules;

        private readonly string _filePathLinguisticVariables =
            Path.Combine(TestContext.CurrentContext.TestDirectory, "TestFiles\\LinguisticVariables_KnowledgeBaseManager.txt");
        private FilePathProvider _filePathProviderForLinguisticVariables;

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
            _filePathProviderForImplicationRules = new FilePathProvider { FilePath = _filePathImplicationRules };
            ImplicationRuleParser ruleParser = new ImplicationRuleParser();
            ImplicationRuleValidator ruleValidator = new ImplicationRuleValidator();
            ImplicationRuleCreator ruleCreator = new ImplicationRuleCreator(ruleParser);
            FileImplicationRuleProvider ruleProvider = new FileImplicationRuleProvider(
                fileOperations,
                _filePathProviderForImplicationRules,
                ruleValidator,
                ruleParser,
                ruleCreator,
                fileValidationOperationResultLogger);
            ImplicationRuleManager implicationRuleManager = new ImplicationRuleManager(ruleProvider);

            // Linguistic variable manager
            MembershipFunctionValidator membershipFunctionValidator = new MembershipFunctionValidator();
            LinguisticVariableValidator linguisticVariableValidator = new LinguisticVariableValidator(membershipFunctionValidator);
            MembershipFunctionParser.Implementations.MembershipFunctionParser membershipFunctionParser =
                new MembershipFunctionParser.Implementations.MembershipFunctionParser();
            LinguisticVariableParser.Implementations.LinguisticVariableParser linguisticVariableParser =
                new LinguisticVariableParser.Implementations.LinguisticVariableParser(membershipFunctionParser);
            MembershipFunctionCreator membershipFunctionCreator = new MembershipFunctionCreator();
            LinguisticVariableCreator linguisticVariableCreator = new LinguisticVariableCreator(membershipFunctionCreator);
            _filePathProviderForLinguisticVariables = new FilePathProvider { FilePath = _filePathLinguisticVariables };
            FileLinguisticVariableProvider linguisticVariableProvider = new FileLinguisticVariableProvider(
                linguisticVariableValidator,
                linguisticVariableParser,
                linguisticVariableCreator,
                _filePathProviderForLinguisticVariables,
                fileOperations,
                fileValidationOperationResultLogger);
            LinguisticVariableManager linguisticVariableManager = new LinguisticVariableManager(linguisticVariableProvider);

            // Knowledge base manager
            KnowledgeBaseValidator knowledgeBaseValidator = new KnowledgeBaseValidator();
            ImplicationRuleRelationsInitializer implicationRuleRelationsInitializer = new ImplicationRuleRelationsInitializer();
            _knowledgeBaseManager = new KnowledgeBaseManager(
                implicationRuleManager,
                linguisticVariableManager,
                knowledgeBaseValidator,
                implicationRuleRelationsInitializer,
                fileValidationOperationResultLogger);
        }

        [Test]
        public void GetImplicationRulesMap_ReturnsCorrectListOfImplicationRuleRelations()
        {
            // Arrange
            List<ImplicationRuleRelations> expectedImplicationRuleRelations = new List<ImplicationRuleRelations>
            {
                new ImplicationRuleRelations(
                    1,
                    new List<ImplicationRulesConnection>(),
                    new List<ImplicationRulesConnection> { new ImplicationRulesConnection(3) },
                    new List<int> {1, 2}),
                new ImplicationRuleRelations(
                    2,
                    new List<ImplicationRulesConnection>(),
                    new List<ImplicationRulesConnection> { new ImplicationRulesConnection(3) },
                    new List<int> {3, 4, 5}),
                new ImplicationRuleRelations(
                    3,
                    new List<ImplicationRulesConnection> { new ImplicationRulesConnection(1), new ImplicationRulesConnection(2) },
                    new List<ImplicationRulesConnection>(),
                    new List<int> {2, 5, 6})
            };

            // Act
            List<ImplicationRuleRelations> actualImplicationRuleRelations = _knowledgeBaseManager.GetImplicationRulesMap();

            // Assert
            Assert.AreEqual(expectedImplicationRuleRelations.Count, actualImplicationRuleRelations.Count);
            for (int i = 0; i < expectedImplicationRuleRelations.Count; i++)
            {
                Assert.IsTrue(ObjectComparer.ImplicationRuleRelationsAreEqual(
                    expectedImplicationRuleRelations[i],
                    actualImplicationRuleRelations[i]));
            }
        }
    }
}
