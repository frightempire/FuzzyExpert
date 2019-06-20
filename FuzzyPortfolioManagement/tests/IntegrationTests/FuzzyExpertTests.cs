using System.Collections.Generic;
using System.IO;
using CommonLogic.Implementations;
using DataProvider.Implementations;
using FuzzificationEngine.Implementaions;
using InferenceEngine.Implementations;
using InferenceExpert.Entities;
using InferenceExpert.Implementations;
using KnowledgeManager.Helpers;
using KnowledgeManager.Implementations;
using LinguisticVariableParser.Implementations;
using MembershipFunctionParser.Implementations;
using NUnit.Framework;
using ProductionRuleParser.Implementations;

namespace IntegrationTests
{
    [TestFixture]
    public class FuzzyExpertTests
    {
        private readonly string _filePathImplicationRules =
            Path.Combine(TestContext.CurrentContext.TestDirectory, @"TestFiles\FuzzyExpert\ImplicationRules.txt");
        private readonly string _filePathLinguisticVariables =
            Path.Combine(TestContext.CurrentContext.TestDirectory, @"TestFiles\FuzzyExpert\LinguisticVariables.txt");
        private readonly string _initialDataFilePath =
            Path.Combine(TestContext.CurrentContext.TestDirectory, @"TestFiles\FuzzyExpert\InitialData.csv");

        private KnowledgeBaseManager _knowledgeBaseManager;
        private CsvDataProvider _initialDataProvider;
        private FuzzyEngine _fuzzyEngine;
        private InferenceGraph _inferenceGraph;
        private FuzzyExpert _fuzzyExpert;

        [SetUp]
        public void SetUp()
        {
            PrepareKnowledgeBaseManager();
            PrepareInitialDataProvider();
            _fuzzyEngine = new FuzzyEngine();
            _inferenceGraph = new InferenceGraph();
            _fuzzyExpert = new FuzzyExpert(_initialDataProvider, _knowledgeBaseManager, _inferenceGraph, _fuzzyEngine);
        }

        private void PrepareKnowledgeBaseManager()
        {
            FileOperations fileOperations = new FileOperations();
            FileValidationOperationResultLogger fileValidationOperationResultLogger = new FileValidationOperationResultLogger(fileOperations);

            // Implication rule manager
            FilePathProvider filePathProviderForImplicationRules = new FilePathProvider { FilePath = _filePathImplicationRules };
            ImplicationRuleParser ruleParser = new ImplicationRuleParser();
            ImplicationRuleValidator ruleValidator = new ImplicationRuleValidator();
            ImplicationRuleCreator ruleCreator = new ImplicationRuleCreator(ruleParser);
            NameSupervisor nameSupervisor = new NameSupervisor(new UniqueNameProvider());
            FileImplicationRuleProvider ruleProvider = new FileImplicationRuleProvider(
                fileOperations,
                filePathProviderForImplicationRules,
                ruleValidator,
                ruleParser,
                ruleCreator,
                nameSupervisor,
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
            FilePathProvider filePathProviderForLinguisticVariables = new FilePathProvider { FilePath = _filePathLinguisticVariables };
            FileLinguisticVariableProvider linguisticVariableProvider = new FileLinguisticVariableProvider(
                linguisticVariableValidator,
                linguisticVariableParser,
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

        private void PrepareInitialDataProvider()
        {
            CsvFileParser csvParser = new CsvFileParser();
            ParsingResultValidator validator = new ParsingResultValidator();
            FilePathProvider filePathProviderMock = new FilePathProvider { FilePath = _initialDataFilePath };
            FileValidationOperationResultLogger resultLogger = new FileValidationOperationResultLogger(new FileOperations());
            _initialDataProvider = new CsvDataProvider(csvParser, validator, filePathProviderMock, resultLogger);
        }

        [Test]
        public void GetResult_ReturnsCorrectResult()
        {
            // Arrange
            var expectedResult = new List<string> {"A1", "A4", "A2", "A3", "A5", "A6"};
            ExpertOpinion expectedOpinion = new ExpertOpinion();
            expectedOpinion.AddResults(expectedResult);

            // Act
            ExpertOpinion actualOpinion = _fuzzyExpert.GetResult();

            // Assert
            Assert.IsTrue(actualOpinion.IsSuccess);
            Assert.AreEqual(actualOpinion.Result, actualOpinion.Result);
        }
    }
}
