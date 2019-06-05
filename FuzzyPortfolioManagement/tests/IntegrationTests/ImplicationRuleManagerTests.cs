using System;
using System.Collections.Generic;
using System.IO;
using Base.UnitTests;
using CommonLogic.Implementations;
using KnowledgeManager.Helpers;
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
            FileOperations fileOperations = new FileOperations();
            _filePathProvider = new FilePathProvider
            {
                FilePath = _filePath
            };

            ImplicationRuleParser ruleParser = new ImplicationRuleParser();
            ImplicationRuleValidator ruleValidator = new ImplicationRuleValidator();
            ImplicationRuleCreator ruleCreator = new ImplicationRuleCreator(ruleParser);
            NameSupervisor nameSupervisor = new NameSupervisor(new UniqueNameProvider());
            FileValidationOperationResultLogger fileValidationOperationResultLogger = new FileValidationOperationResultLogger(fileOperations);

            FileImplicationRuleProvider ruleProvider = new FileImplicationRuleProvider(
                fileOperations,
                _filePathProvider,
                ruleValidator,
                ruleParser,
                ruleCreator,
                nameSupervisor,
                fileValidationOperationResultLogger);
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
            Dictionary<int, ImplicationRule> expectedImplicationRules = PrepareExpectedImplicationRules();

            // Act
            Dictionary<int, ImplicationRule> actualImplicationRules = _implicationRuleManager.ImplicationRules;

            // Assert
            Assert.AreEqual(expectedImplicationRules.Count, actualImplicationRules.Count);
            for (int i = 1; i <= expectedImplicationRules.Count; i++)
            {
                Assert.IsTrue(ObjectComparer.ImplicationRulesAreEqual(expectedImplicationRules[i], actualImplicationRules[i]));
            }
        }

        private Dictionary<int, ImplicationRule> PrepareExpectedImplicationRules()
        {
            // IF (A > 10) THEN (X = 5)
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

            // IF (B != 1 & C != 2) THEN (X = 10)
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

            // IF ((A = 5 | B = 10) & C = 6) THEN (X = 7)
            ImplicationRule thirdImplicationRule = new ImplicationRule(
                new List<StatementCombination>
                {
                    new StatementCombination(new List<UnaryStatement>
                    {
                        new UnaryStatement("A", ComparisonOperation.Equal, "5"),
                        new UnaryStatement("C", ComparisonOperation.Equal, "6")
                    })
                },
                new StatementCombination(new List<UnaryStatement>
                {
                    new UnaryStatement("X", ComparisonOperation.Equal, "7")
                }));
            ImplicationRule fourthImplicationRule = new ImplicationRule(
                new List<StatementCombination>
                {
                    new StatementCombination(new List<UnaryStatement>
                    {
                        new UnaryStatement("B", ComparisonOperation.Equal, "10"),
                        new UnaryStatement("C", ComparisonOperation.Equal, "6")
                    }),
                },
                new StatementCombination(new List<UnaryStatement>
                {
                    new UnaryStatement("X", ComparisonOperation.Equal, "7")
                }));

            return new Dictionary<int, ImplicationRule>
            {
                { 1, firstImplicationRule },
                { 2, secondImplicationRule },
                { 3, thirdImplicationRule },
                { 4, fourthImplicationRule }
            };
        }
    }
}
