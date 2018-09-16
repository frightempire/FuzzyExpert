using System;
using System.Collections.Generic;
using System.Linq;
using Base.UnitTests;
using NUnit.Framework;
using ProductionRulesParser.Entities;
using ProductionRulesParser.Enums;
using ProductionRulesParser.Implementations;
using ProductionRulesParser.Interfaces;
using Rhino.Mocks;

namespace ProductionRulesParser.UnitTests.Implementations
{
    [TestFixture]
    public class ImplicationRuleCreatorTests
    {
        private IImplicationRuleParser _implicationRuleParser;
        private IImplicationRulePreProcessor _implicationRulePreProcessor;
        private ImplicationRuleCreator _implicationRuleCreator;

        [SetUp]
        public void SetUp()
        {
            _implicationRuleParser = MockRepository.GenerateMock<IImplicationRuleParser>();
            _implicationRulePreProcessor = MockRepository.GenerateMock<IImplicationRulePreProcessor>();
            _implicationRuleCreator = new ImplicationRuleCreator(_implicationRuleParser, _implicationRulePreProcessor);
        }

        [Test]
        public void Constructor_ThrowsArgumentNullExceptionIfImplicationRuleParserIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ImplicationRuleCreator(null, _implicationRulePreProcessor));
        }

        [Test]
        public void Constructor_ThrowsArgumentNullExceptionIfImplicationRulePreProcessorIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new ImplicationRuleCreator(_implicationRuleParser, null));
        }

        [Test]
        public void DivideImplicationRule_ReturnsImplicationRuleStrings()
        {
            // Arrange
            string implicationRule = "IF (A=a | (B=b & C=c)) THEN (D=d)";
            ImplicationRuleStrings expectedImplicationRuleStrings = new ImplicationRuleStrings(
                "(A=a|(B=b&C=c))", "(D=d)");

            string implicationRuleAfterPreProcessing = "IF(A=a|(B=b&C=c))THEN(D=d)";
            _implicationRulePreProcessor.Expect(irp => irp.PreProcessImplicationRule(implicationRule))
                .Return(implicationRuleAfterPreProcessing);

            _implicationRuleParser.Expect(irp => irp.ExtractStatementParts(implicationRuleAfterPreProcessing))
                .Return(expectedImplicationRuleStrings);

            // Act
            ImplicationRuleStrings actualImplicationRuleStrings =
                _implicationRuleCreator.DivideImplicationRule(implicationRule);

            // Assert
            Assert.IsTrue(ImplicationRuleStringsAreEqual(expectedImplicationRuleStrings, actualImplicationRuleStrings));
        }

        [Test]
        public void CreateImplicationRuleEntity_ReturnsImplicationRule()
        {
            // Arrange
            string ifStatementPart = "(A=a|(B=b&C=c))";
            string thenStatementpart = "(D=d)";
            string implicationRule = "IF" + ifStatementPart + "THEN" + thenStatementpart;
            ImplicationRuleStrings implicationRuleStrings = new ImplicationRuleStrings(
                ifStatementPart, thenStatementpart);

            // (A=a|(B=b&C=c))
            List<StatementCombination> ifStatementCombinations = new List<StatementCombination>
            {
                new StatementCombination(new List<UnaryStatement>
                {
                    new UnaryStatement("A", ComparisonOperation.Equal, "a"),
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
            _implicationRuleParser.Expect(irp => irp.ParseStatementCombination(thenStatementpart))
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
            Assert.IsTrue(ImplicationRulesAreEqual(expectedImplicationRule, actualImplicationRule));
        }

        private bool ImplicationRuleStringsAreEqual(
            ImplicationRuleStrings implicationRuleStringsToCompare,
            ImplicationRuleStrings implicationRuleStringsToCompareWith)
        {
            return implicationRuleStringsToCompare.IfStatement == implicationRuleStringsToCompareWith.IfStatement &&
                   implicationRuleStringsToCompare.ThenStatement == implicationRuleStringsToCompareWith.ThenStatement;
        }

        private bool UnaryStatementsAreEqual(
            UnaryStatement unaryStatementToCompare,
            UnaryStatement unaryStatementToCompareWith)
        {
            return unaryStatementToCompare.LeftOperand == unaryStatementToCompareWith.LeftOperand &&
                   unaryStatementToCompare.ComparisonOperation == unaryStatementToCompareWith.ComparisonOperation &&
                   unaryStatementToCompare.RightOperand == unaryStatementToCompareWith.RightOperand;
        }

        private bool ImplicationRulesAreEqual(
            ImplicationRule implicationRuleToCompare,
            ImplicationRule implicationRuleToCompareWith)
        {
            if (implicationRuleToCompare.IfStatement.Count != implicationRuleToCompareWith.IfStatement.Count)
                return false;

            for (int i = 0; i < implicationRuleToCompare.IfStatement.Count; i++)
            {
                List<UnaryStatement> ifUnaryStetementsToCompare = implicationRuleToCompare.IfStatement[i].UnaryStatements;
                List<UnaryStatement> ifUnaryStetementsToCompareWith = implicationRuleToCompareWith.IfStatement[i].UnaryStatements;

                if (ifUnaryStetementsToCompare.Count != ifUnaryStetementsToCompareWith.Count)
                    return false;

                for (var j = 0; j < ifUnaryStetementsToCompare.Count; j++)
                {
                    if (!UnaryStatementsAreEqual(ifUnaryStetementsToCompare[j], ifUnaryStetementsToCompareWith[j]))
                        return false;
                }
            }

            List<UnaryStatement> thenUnaryStetementsToCompare = implicationRuleToCompare.ThenStatement.UnaryStatements;
            List<UnaryStatement> thenUnaryStetementsToCompareWith = implicationRuleToCompareWith.ThenStatement.UnaryStatements;

            if (thenUnaryStetementsToCompare.Count != thenUnaryStetementsToCompareWith.Count)
                return false;

            for (var i = 0; i < thenUnaryStetementsToCompare.Count; i++)
            {
                if (!UnaryStatementsAreEqual(thenUnaryStetementsToCompare[i], thenUnaryStetementsToCompareWith[i]))
                    return false;
            }

            return true;
        }
    }
}
