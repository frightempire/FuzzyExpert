using System.Collections.Generic;
using System.IO;
using Base.UnitTests;
using CommonLogic.Implementations;
using NUnit.Framework;
using ProductionRuleManager.Implementations;
using ProductionRulesParser.Entities;
using ProductionRulesParser.Enums;
using ProductionRulesParser.Implementations;

namespace IntegrationTests
{
    [TestFixture]
    public class ImplicationRuleManagerTests
    {
        private readonly string _filePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestFiles\\ImplicationRules.txt");

        private ImplicationRuleManager _implicationRuleManager;

        [SetUp]
        public void SetUp()
        {
            _implicationRuleManager = PrepareImplicationRuleManager();
        }

        private ImplicationRuleManager PrepareImplicationRuleManager()
        {
            FileReader fileReader = new FileReader();

            ImplicationRuleParser ruleParser = new ImplicationRuleParser();
            ImplicationRulePreProcessor rulePreProcessor = new ImplicationRulePreProcessor();
            ImplicationRuleCreator ruleCreator = new ImplicationRuleCreator(ruleParser, rulePreProcessor);

            FileImplicationRuleProvider ruleProvider = new FileImplicationRuleProvider(fileReader, ruleCreator)
            {
                FilePath = _filePath
            };

            return new ImplicationRuleManager(ruleProvider);
        }

        [Test]
        public void ImplicationRulesGetterReturnsImplicationRulesList()
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
