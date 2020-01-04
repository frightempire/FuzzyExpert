using System;
using System.Collections.Generic;
using FuzzyExpert.Base.UnitTests;
using FuzzyExpert.Core.Entities;
using FuzzyExpert.Core.Enums;
using FuzzyExpert.Infrastructure.ProductionRuleParsing.Entities;
using FuzzyExpert.Infrastructure.ProductionRuleParsing.Implementations;
using FuzzyExpert.Infrastructure.ProductionRuleParsing.Interfaces;
using NUnit.Framework;
using Rhino.Mocks;

namespace FuzzyExpert.Infrastructure.UnitTests.ProductionRuleParsing.Implementations
{
    [TestFixture]
    public class ImplicationRuleCreatorTests
    {
        private IImplicationRuleParser _implicationRuleParser;
        private ImplicationRuleCreator _implicationRuleCreator;

        [SetUp]
        public void SetUp()
        {
            _implicationRuleParser = MockRepository.GenerateMock<IImplicationRuleParser>();
            _implicationRuleCreator = new ImplicationRuleCreator(_implicationRuleParser);
        }

        [Test]
        public void Constructor_ThrowsArgumentNullExceptionIfOneOfInputParametersIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ImplicationRuleCreator(null));
        }

        [Test]
        public void CreateImplicationRuleEntity_ReturnsImplicationRule()
        {
            // Arrange
            string ifStatementPart = "(A=a|(B=b&C=c))";
            string thenStatementPart = "(D=d)";
            ImplicationRuleStrings implicationRuleStrings = new ImplicationRuleStrings(
                ifStatementPart, thenStatementPart);

            // (A=a|(B=b&C=c))
            List<StatementCombination> ifStatementCombinations = new List<StatementCombination>
            {
                new StatementCombination(new List<UnaryStatement>
                {
                    new UnaryStatement("A", ComparisonOperation.Equal, "a")
                }),
                new StatementCombination(new List<UnaryStatement>
                {
                    new UnaryStatement("B", ComparisonOperation.Equal, "b"),
                    new UnaryStatement("C", ComparisonOperation.Equal, "c")
                })
            };
            // (D=d)
            StatementCombination thenStatementCombination = new StatementCombination(new List<UnaryStatement>
            {
                new UnaryStatement("D", ComparisonOperation.Equal, "d")
            });
            ImplicationRule expectedImplicationRule = new ImplicationRule(ifStatementCombinations, thenStatementCombination);

            List<string> ifStatementParts = new List<string> { "A=a", "B=b&C=c" };
            _implicationRuleParser.Expect(irp => irp.ParseImplicationRule(ref ifStatementPart))
                .Return(ifStatementParts);
            List<string> thenStatementParts = new List<string> { "D=d" };
            _implicationRuleParser.Expect(irp => irp.ParseStatementCombination(thenStatementPart))
                .Return(thenStatementParts);

            List<string> aIfUnaryStatementStrings = new List<string> {"A=a"};
            List<string> bcIfUnaryStatementStrings = new List<string> { "B=b", "C=c" };
            _implicationRuleParser.Expect(irp => irp.ParseStatementCombination("A=a")).Return(aIfUnaryStatementStrings);
            _implicationRuleParser.Expect(irp => irp.ParseStatementCombination("B=b&C=c")).Return(bcIfUnaryStatementStrings);

            UnaryStatement aUnaryStatement = new UnaryStatement("A", ComparisonOperation.Equal, "a");
            UnaryStatement bUnaryStatement = new UnaryStatement("B", ComparisonOperation.Equal, "b");
            UnaryStatement cUnaryStatement = new UnaryStatement("C", ComparisonOperation.Equal, "c");
            UnaryStatement dUnaryStatement = new UnaryStatement("D", ComparisonOperation.Equal, "d");
            _implicationRuleParser.Expect(irp => irp.ParseUnaryStatement("A=a")).Return(aUnaryStatement);
            _implicationRuleParser.Expect(irp => irp.ParseUnaryStatement("B=b")).Return(bUnaryStatement);
            _implicationRuleParser.Expect(irp => irp.ParseUnaryStatement("C=c")).Return(cUnaryStatement);
            _implicationRuleParser.Expect(irp => irp.ParseUnaryStatement("D=d")).Return(dUnaryStatement);

            // Act
            ImplicationRule actualImplicationRule =
                _implicationRuleCreator.CreateImplicationRuleEntity(implicationRuleStrings);

            // Assert
            Assert.IsTrue(ObjectComparer.ImplicationRulesAreEqual(expectedImplicationRule, actualImplicationRule));
        }
    }
}