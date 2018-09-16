using System;
using System.Collections.Generic;
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

        [Test, Ignore("Needs adjustment")]
        public void CreateImplicationRuleEntity_ReturnsImplicationRule()
        {
            // Arrange
            ImplicationRuleStrings implicationRuleStrings = new ImplicationRuleStrings(
                "(A=a|(B=b&C=c))", "(D=d)");

            List<UnaryStatement> ifStatements = new List<UnaryStatement>
            {
                new UnaryStatement("A", ComparisonOperation.Equal, "a"),

            };
            string thenStatement = "D=d";
            ImplicationRule expectedImplicationRule = new ImplicationRule(ifStatements, );

            string implicationRuleAfterPreProcessing = "IF(A=a|(B=b&C=c))THEN(D=d)";
            _implicationRulePreProcessor.Expect(irp => irp.PreProcessImplicationRule(implicationRule))
                .Return(implicationRuleAfterPreProcessing);

            _implicationRuleParser.Expect(irp => irp.ExtractStatementParts(implicationRuleAfterPreProcessing))
                .Return(implicationRuleStrings);

            // Act
            ImplicationRuleStrings actualImplicationRuleStrings =
                _implicationRuleCreator.DivideImplicationRule(implicationRule);

            // Assert
            Assert.IsTrue(ImplicationRuleStringsAreEqual(implicationRuleStrings, actualImplicationRuleStrings));
        }

        private bool ImplicationRuleStringsAreEqual(
            ImplicationRuleStrings implicationRuleStringsToCompare,
            ImplicationRuleStrings implicationRuleStringsToCompareWith)
        {
            return implicationRuleStringsToCompare.IfStatement == implicationRuleStringsToCompareWith.IfStatement &&
                   implicationRuleStringsToCompare.ThenStatement == implicationRuleStringsToCompareWith.ThenStatement;
        }
    }
}
