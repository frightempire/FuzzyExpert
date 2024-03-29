﻿using System;
using System.Collections.Generic;
using System.IO;
using FuzzyExpert.Application.Common.Implementations;
using FuzzyExpert.Application.InferenceExpert.Entities;
using FuzzyExpert.Core.Entities;
using FuzzyExpert.Core.FuzzificationEngine.Implementations;
using FuzzyExpert.Core.InferenceEngine.Implementations;
using FuzzyExpert.Infrastructure.DatabaseManagement.Entities;
using FuzzyExpert.Infrastructure.DatabaseManagement.Implementations;
using FuzzyExpert.Infrastructure.InitialDataProviding.Implementations;
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
        private readonly string _profileName = "profile_name";
        private ProfileRepository _profileRepository;
        private readonly string _initialDataFilePath = Path.Combine(TestContext.CurrentContext.TestDirectory, @"TestFiles\InitialData.csv");

        private KnowledgeBaseManager _knowledgeBaseManager;
        private CsvDataProvider _initialDataProvider;
        private FuzzyEngine _fuzzyEngine;
        private InferenceGraph _inferenceGraph;
        private Application.InferenceExpert.Implementations.FuzzyExpert _fuzzyExpert;

        [SetUp]
        public void SetUp()
        {
            PrepareProfileRepository();
            PrepareKnowledgeBaseManager();
            PrepareInitialDataProvider();
            _fuzzyEngine = new FuzzyEngine();
            _inferenceGraph = new InferenceGraph();
            _fuzzyExpert = new Application.InferenceExpert.Implementations.FuzzyExpert(
                _initialDataProvider, _knowledgeBaseManager, _inferenceGraph, _fuzzyEngine);
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
            var expectedResult = new List<DeFuzzifiedInferenceResult>
            {
                new DeFuzzifiedInferenceResult("Temperature = HOT", 0.8, 63.33),
                new DeFuzzifiedInferenceResult("Pressure = HIGH", 0.8, 110),
                new DeFuzzifiedInferenceResult("Volume = BIG", 0.7, 1112.5),
                new DeFuzzifiedInferenceResult("Color = RED", 0.6, 64.05),
                new DeFuzzifiedInferenceResult("Danger = HIGH", 0.6, 63.33),
                new DeFuzzifiedInferenceResult("Evacuate = TRUE", 0.6, 11.67)
            };

            // Act
            var actualOpinion = _fuzzyExpert.GetResult(_profileName);

            // Assert
            Assert.IsTrue(actualOpinion.IsSuccess);
            Assert.AreEqual(expectedResult.Count, actualOpinion.Result.Count);
        }
    }
}