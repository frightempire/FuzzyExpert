using System;
using System.Collections.Generic;
using FuzzyExpert.Core.Enums;
using FuzzyExpert.Core.InferenceEngine.Implementations;
using FuzzyExpert.Core.InferenceEngine.Interfaces;
using NUnit.Framework;
using Rhino.Mocks;

namespace FuzzyExpert.Core.UnitTests.InferenceEngine.Implementations
{
    [TestFixture]
    public class GraphRuleTests
    {
        private GraphRule _graphRule;

        [SetUp]
        public void SetUp()
        {
            var node1 = MockRepository.GenerateMock<IInferenceNode>();
            var node2 = MockRepository.GenerateMock<IInferenceNode>();
            var ifNodes = new List<IInferenceNode> { node1, node2 };
            var operation = LogicalOperation.And;
            var thenNodes = new List<IInferenceNode> { node1 };
            _graphRule = new GraphRule(ifNodes, operation, thenNodes);
        }

        [Test]
        public void Constructor_ThrowsArgumentNullExceptionIfNodeListsAreNullOrEmpty()
        {
            // Arrange
            var node = MockRepository.GenerateMock<IInferenceNode>();

            // Assert
            Assert.Throws<ArgumentNullException>(
                () => { new GraphRule(null, LogicalOperation.And, new List<IInferenceNode> { node }); });
            Assert.Throws<ArgumentNullException>(
                () => { new GraphRule(new List<IInferenceNode>(), LogicalOperation.And, new List<IInferenceNode> { node }); });
            Assert.Throws<ArgumentNullException>(
                () => { new GraphRule(new List<IInferenceNode> { node }, LogicalOperation.And, null); });
            Assert.Throws<ArgumentNullException>(
                () => { new GraphRule(new List<IInferenceNode> { node }, LogicalOperation.And, new List<IInferenceNode>()); });
        }

        [Test]
        public void Properties_DefaultValues()
        {
            Assert.AreEqual(0, _graphRule.ConfidenceFactor);
        }
    }
}