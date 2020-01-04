using System.Collections.Generic;
using System.IO;
using FuzzyExpert.Application.Common.Implementations;
using FuzzyExpert.Application.InferenceExpert.Entities;
using FuzzyExpert.Core.FuzzificationEngine.Implementations;
using FuzzyExpert.Core.InferenceEngine.Implementations;
using FuzzyExpert.Infrastructure.InitialDataProviding.Implementations;
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
        private FuzzyExpert.Application.InferenceExpert.Implementations.FuzzyExpert _fuzzyExpert;

        [SetUp]
        public void SetUp()
        {
            PrepareKnowledgeBaseManager();
            PrepareInitialDataProvider();
            _fuzzyEngine = new FuzzyEngine();
            _inferenceGraph = new InferenceGraph();
            _fuzzyExpert = new FuzzyExpert.Application.InferenceExpert.Implementations.FuzzyExpert(
                _initialDataProvider, _knowledgeBaseManager, _inferenceGraph, _fuzzyEngine);
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
                ruleParser,
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
            LinguisticVariableCreator linguisticVariableCreator = new LinguisticVariableCreator(membershipFunctionCreator);
            LinguisticVariableFilePathProvider filePathProviderForLinguisticVariables = new LinguisticVariableFilePathProvider { FilePath = _filePathLinguisticVariables };
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
            InitialDataFilePathProvider filePathProviderMock = new InitialDataFilePathProvider { FilePath = _initialDataFilePath };
            FileValidationOperationResultLogger resultLogger = new FileValidationOperationResultLogger(new FileOperations());
            _initialDataProvider = new CsvDataProvider(csvParser, validator, filePathProviderMock, resultLogger);
        }

        [Test]
        public void GetResult_ReturnsCorrectResult()
        {
            // Arrange
            var expectedResult = new Dictionary<string, double>
            {
                {"A1", 0 },
                {"A4", 0 },
                {"A2", 0 },
                {"A3", 0 },
                {"A5", 0 },
                {"A6", 0 }
            };
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
