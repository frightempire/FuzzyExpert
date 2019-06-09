﻿using System.Collections.Generic;
using System.IO;
using Base.UnitTests;
using CommonLogic.Implementations;
using KnowledgeManager.Entities;
using KnowledgeManager.Helpers;
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
            NameSupervisor nameSupervisor = new NameSupervisor(new UniqueNameProvider());
            FileImplicationRuleProvider ruleProvider = new FileImplicationRuleProvider(
                fileOperations,
                _filePathProviderForImplicationRules,
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
            List<LinguisticVariableRelations> actualRelations = _knowledgeBaseManager.GetKnowledgeBase().LinguisticVariablesRelations;

            // Assert
            Assert.AreEqual(expectedRelations.Count, actualRelations.Count);
            for (int i = 0; i < expectedRelations.Count; i++)
            {
                Assert.IsTrue(ObjectComparer.LinguisticVariableRelationsAreEqual(expectedRelations[i], actualRelations[i]));
            }
        }
    }
}