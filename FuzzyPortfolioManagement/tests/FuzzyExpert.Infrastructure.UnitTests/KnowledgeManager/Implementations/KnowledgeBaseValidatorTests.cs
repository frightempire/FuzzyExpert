using System.Collections.Generic;
using FuzzyExpert.Application.Entities;
using FuzzyExpert.Core.Entities;
using FuzzyExpert.Core.Enums;
using FuzzyExpert.Infrastructure.KnowledgeManager.Implementations;
using NUnit.Framework;

namespace FuzzyExpert.Infrastructure.UnitTests.KnowledgeManager.Implementations
{
    [TestFixture]
    public class KnowledgeBaseValidatorTests
    {
        private KnowledgeBaseValidator _knowledgeBaseValidator;

        [SetUp]
        public void SetUp()
        {
            _knowledgeBaseValidator = new KnowledgeBaseValidator();
        }

        [Test]
        public void ValidateLinguisticVariablesNames_ReturnValidationOperationResultWithErrorIfOneOfVariablesInImplicationRulesIsNotKnownToKnowledgeBase()
        {
            // Arrange
            string expectedVariable = "Air";
            Dictionary<int, ImplicationRule> implicationRules = PrepareImplicationRules();
            Dictionary<int, LinguisticVariable> linguisticVariables = PrepareLinguisticVariables();
            string errorMessage = $"Knowledge base: linguistic variable {expectedVariable} is unknown to linguistic variable base";

            // Act
            ValidationOperationResult validationOperationResult = _knowledgeBaseValidator.ValidateLinguisticVariablesNames(
                implicationRules, linguisticVariables);

            // Assert
            Assert.IsTrue(validationOperationResult.Messages.Contains(errorMessage));
        }

        private Dictionary<int, LinguisticVariable> PrepareLinguisticVariables()
        {
            // Water variable
            MembershipFunctionList firstMembershipFunctionList = new MembershipFunctionList
            {
                new TrapezoidalMembershipFunction("Cold", 0, 20, 20, 30),
                new TrapezoidalMembershipFunction("Hot", 50, 60, 60, 80)
            };
            LinguisticVariable firstLinguisticVariable =
                new LinguisticVariable("Water", firstMembershipFunctionList, isInitialData: true);

            // Pressure vatiable
            MembershipFunctionList secondsMembershipFunctionList = new MembershipFunctionList
            {
                new TrapezoidalMembershipFunction("Low", 20, 50, 50, 60),
                new TrapezoidalMembershipFunction("Medium", 60, 65, 65, 80),
                new TrapezoidalMembershipFunction("High", 80, 100, 100, 150)
            };
            LinguisticVariable secondLinguisticVariable =
                new LinguisticVariable("Pressure", secondsMembershipFunctionList, isInitialData: false);

            Dictionary<int, LinguisticVariable> linguisticVariables = new Dictionary<int, LinguisticVariable>
            {
                {1, firstLinguisticVariable},
                {2, secondLinguisticVariable}
            };
            return linguisticVariables;
        }

        private Dictionary<int, ImplicationRule> PrepareImplicationRules()
        {
            // IF(Water IS Cold) THEN (Pressure IS Low)
            ImplicationRule firstImplicationRule = new ImplicationRule(
                new List<StatementCombination>
                {
                    new StatementCombination(new List<UnaryStatement>
                    {
                        new UnaryStatement("Water", ComparisonOperation.Equal, "Cold")
                    })
                },
                new StatementCombination(new List<UnaryStatement>
                {
                    new UnaryStatement("Pressure", ComparisonOperation.Equal, "Low")
                }));

            // IF(Water IS Hot AND Air IS Cold) THEN (Pressure IS Medium)
            ImplicationRule secondImplicationRule = new ImplicationRule(
                new List<StatementCombination>
                {
                    new StatementCombination(new List<UnaryStatement>
                    {
                        new UnaryStatement("Water", ComparisonOperation.Equal, "Hot"),
                        new UnaryStatement("Air", ComparisonOperation.Equal, "Cold")
                    })
                },
                new StatementCombination(new List<UnaryStatement>
                {
                    new UnaryStatement("Pressure", ComparisonOperation.Equal, "Medium")
                }));

            Dictionary<int, ImplicationRule> implicationRules = new Dictionary<int, ImplicationRule>
            {
                {1, firstImplicationRule},
                {2, secondImplicationRule}
            };
            return implicationRules;
        }
    }
}
