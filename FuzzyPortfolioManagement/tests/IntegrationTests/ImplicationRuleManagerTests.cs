using System;
using System.Collections.Generic;
using System.IO;
using Base.UnitTests;
using CommonLogic.Implementations;
using KnowledgeManager.Implementations;
using NUnit.Framework;
using ProductionRuleParser.Entities;
using ProductionRuleParser.Enums;
using ProductionRuleParser.Implementations;

namespace IntegrationTests
{
    [TestFixture]
    public class ImplicationRuleManagerTests
    {
        private readonly string _filePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestFiles\\ImplicationRules.txt");

        private ImplicationRuleManager _implicationRuleManager;
        private FilePathProvider _filePathProvider;

        [SetUp]
        public void SetUp()
        {
            PrepareImplicationRuleManager();
        }

        private void PrepareImplicationRuleManager()
        {
            FileReader fileReader = new FileReader();
            _filePathProvider = new FilePathProvider
            {
                FilePath = _filePath
            };

            ImplicationRuleParser ruleParser = new ImplicationRuleParser();
            ImplicationRulePreProcessor rulePreProcessor = new ImplicationRulePreProcessor();
            ImplicationRuleCreator ruleCreator = new ImplicationRuleCreator(ruleParser, rulePreProcessor);

            FileImplicationRuleProvider ruleProvider = new FileImplicationRuleProvider(fileReader, _filePathProvider, ruleCreator);
            _implicationRuleManager = new ImplicationRuleManager(ruleProvider);
        }

        [Test]
        public void ImplicationRulesGetter_ThrowsArgumentNullExceptionIfFilePathIsNull()
        {
            // Arrange
            _filePathProvider.FilePath = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => { var rules = _implicationRuleManager.ImplicationRules; });
        }

        [Test]
        public void ImplicationRulesGetter_ThrowsFileNotFoundExceptionIfFilePathIsEmpty()
        {
            // Arrange
            _filePathProvider.FilePath = string.Empty;

            // Act & Assert
            Assert.Throws<FileNotFoundException>(() => { var rules = _implicationRuleManager.ImplicationRules; });
        }

        [Test]
        public void ImplicationRulesGetter_ReturnsImplicationRulesList()
        {
            // Arrange
            List<ImplicationRule> expectedImplicationRules = PrepareExpectedImplicationRules();

            // Act
            List<ImplicationRule> actualImplicationRules = _implicationRuleManager.ImplicationRules;

            // Assert
            Assert.AreEqual(expectedImplicationRules.Count, actualImplicationRules.Count);
            for (int i = 0; i < expectedImplicationRules.Count; i++)
            {
                Assert.IsTrue(ObjectComparer.ImplicationRulesAreEqual(expectedImplicationRules[i], actualImplicationRules[i]));
            }
        }

        private List<ImplicationRule> PrepareExpectedImplicationRules()
        {
            ImplicationRule firstImplicationRule = new ImplicationRule(
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

            ImplicationRule secondImplicationRule = new ImplicationRule(
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

            return new List<ImplicationRule>
            {
                firstImplicationRule, secondImplicationRule
            };
        }
    }
}
