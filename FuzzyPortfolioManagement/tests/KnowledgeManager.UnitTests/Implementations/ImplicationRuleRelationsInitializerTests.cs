using System.Collections.Generic;
using Base.UnitTests;
using KnowledgeManager.Entities;
using KnowledgeManager.Implementations;
using LinguisticVariableParser.Entities;
using MembershipFunctionParser.Entities;
using MembershipFunctionParser.Implementations;
using NUnit.Framework;
using ProductionRuleParser.Entities;
using ProductionRuleParser.Enums;

namespace KnowledgeManager.UnitTests.Implementations
{
    [TestFixture]
    public class ImplicationRuleRelationsInitializerTests
    {
        private ImplicationRuleRelationsInitializer _implicationRuleRelationsInitializer;

        [SetUp]
        public void SetUp()
        {
            _implicationRuleRelationsInitializer = new ImplicationRuleRelationsInitializer();
        }

        [Test]
        public void FormImplicationRuleRelations_ReturnsCorrectImplicationRuleRelations()
        {
            // Arrange
            // Implication rules
            ImplicationRule firstImplicationRule = new ImplicationRule(
                new List<StatementCombination>
                {
                    new StatementCombination(new List<UnaryStatement>
                    {
                        new UnaryStatement("Temperature", ComparisonOperation.Greater, "HOT")
                    })
                },
                new StatementCombination(new List<UnaryStatement>
                {
                    new UnaryStatement("Pressure", ComparisonOperation.Equal, "HIGH")
                }));
            ImplicationRule secondImplicationRule = new ImplicationRule(
                new List<StatementCombination>
                {
                    new StatementCombination(new List<UnaryStatement>
                    {
                        new UnaryStatement("Volume", ComparisonOperation.GreaterOrEqual, "BIG"),
                        new UnaryStatement("Color", ComparisonOperation.Equal, "RED")
                    })
                },
                new StatementCombination(new List<UnaryStatement>
                {
                    new UnaryStatement("Danger", ComparisonOperation.Equal, "HIGH")
                }));
            ImplicationRule thirdImplicationRule = new ImplicationRule(
                new List<StatementCombination>
                {
                    new StatementCombination(new List<UnaryStatement>
                    {
                        new UnaryStatement("Pressure", ComparisonOperation.Equal, "HIGH"),
                        new UnaryStatement("Danger", ComparisonOperation.Equal, "HIGH")
                    })
                },
                new StatementCombination(new List<UnaryStatement>
                {
                    new UnaryStatement("Evacuate", ComparisonOperation.Equal, "TRUE")
                }));

            Dictionary<int, ImplicationRule> implicationRules = new Dictionary<int, ImplicationRule>
            {
                {1, firstImplicationRule},
                {2, secondImplicationRule},
                {3, thirdImplicationRule}
            };

            // Linguistic variables
            MembershipFunctionList firstMembershipFunctionList = new MembershipFunctionList
            {
                new TrapezoidalMembershipFunction("Cold", 0, 20, 20, 30),
                new TrapezoidalMembershipFunction("Hot", 50, 60, 60, 80)
            };
            LinguisticVariable firstLinguisticVariable = new LinguisticVariable("Temperature", firstMembershipFunctionList, isInitialData: true);

            MembershipFunctionList secondMembershipFunctionList = new MembershipFunctionList
            {
                new TrapezoidalMembershipFunction("Low", 20, 50, 50, 60),
                new TrapezoidalMembershipFunction("High", 80, 100, 100, 150)
            };
            LinguisticVariable secondLinguisticVariable = new LinguisticVariable("Pressure", secondMembershipFunctionList, isInitialData: false);

            MembershipFunctionList thirdMembershipFunctionList = new MembershipFunctionList
            {
                new TrapezoidalMembershipFunction("Small", 100, 200, 200, 600),
                new TrapezoidalMembershipFunction("Big", 800, 1000, 1000, 1500)
            };
            LinguisticVariable thirdLinguisticVariable = new LinguisticVariable("Volume", thirdMembershipFunctionList, isInitialData: true);

            MembershipFunctionList fourthMembershipFunctionList = new MembershipFunctionList
            {
                new TrapezoidalMembershipFunction("Blue", 5, 10, 10, 20),
                new TrapezoidalMembershipFunction("Red", 50, 60, 60, 80)
            };
            LinguisticVariable fourthLinguisticVariable = new LinguisticVariable("Color", fourthMembershipFunctionList, isInitialData: true);

            MembershipFunctionList fifthMembershipFunctionList = new MembershipFunctionList
            {
                new TrapezoidalMembershipFunction("Low", 5, 10, 10, 20),
                new TrapezoidalMembershipFunction("High", 50, 60, 60, 80)
            };
            LinguisticVariable fifthLinguisticVariable = new LinguisticVariable("Danger", fifthMembershipFunctionList, isInitialData: false);

            MembershipFunctionList sixthMembershipFunctionList = new MembershipFunctionList
            {
                new TrapezoidalMembershipFunction("True", 5, 10, 10, 20),
                new TrapezoidalMembershipFunction("False", 50, 60, 60, 80)
            };
            LinguisticVariable sixthLinguisticVariable = new LinguisticVariable("Evacuate", sixthMembershipFunctionList, isInitialData: false);

            Dictionary<int, LinguisticVariable> linguisticVariables = new Dictionary<int, LinguisticVariable>
            {
                {1, firstLinguisticVariable},
                {2, secondLinguisticVariable},
                {3, thirdLinguisticVariable},
                {4, fourthLinguisticVariable},
                {5, fifthLinguisticVariable},
                {6, sixthLinguisticVariable}
            };

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
            List<ImplicationRuleRelations> actualImplicationRuleRelations = 
                _implicationRuleRelationsInitializer.FormImplicationRuleRelations(implicationRules, linguisticVariables);

            // Assert
            Assert.AreEqual(expectedImplicationRuleRelations.Count, actualImplicationRuleRelations.Count);
            for (int i = 0; i < expectedImplicationRuleRelations.Count; i++)
            {
                Assert.IsTrue(ObjectComparer.ImplicationRuleRelationsAreEqual(
                    expectedImplicationRuleRelations[i], actualImplicationRuleRelations[i]));
            }
        }
    }
}