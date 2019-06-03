using System;
using System.Collections.Generic;
using InferenceEngine.Implementations;
using InferenceEngine.Interfaces;
using NUnit.Framework;
using ProductionRuleParser.Enums;
using Rhino.Mocks;

namespace InferenceEngine.UnitTests.Implementations
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
        public void Constructor_CalculatesMinMaxForAndOperation()
        {
            // Arrange
            var node1 = MockRepository.GenerateMock<IInferenceNode>();
            var node2 = MockRepository.GenerateMock<IInferenceNode>();
            var ifNodes = new List<IInferenceNode> { node1, node2 };
            var operation = LogicalOperation.And;
            var thenNodes = new List<IInferenceNode> { node1 };
            Tuple<int, int> expectedMinMax = new Tuple<int, int>(ifNodes.Count, ifNodes.Count);

            // Act
            var graphRule = new GraphRule(ifNodes, operation, thenNodes);

            // Assert
            Assert.AreEqual(expectedMinMax, graphRule.MinMax);
        }

        [Test]
        public void Constructor_CalculatesMinMaxForOrOperation()
        {
            // Arrange
            var node1 = MockRepository.GenerateMock<IInferenceNode>();
            var node2 = MockRepository.GenerateMock<IInferenceNode>();
            var ifNodes = new List<IInferenceNode> { node1, node2 };
            var operation = LogicalOperation.Or;
            var thenNodes = new List<IInferenceNode> { node1 };
            Tuple<int, int> expectedMinMax = new Tuple<int, int>(1, ifNodes.Count);

            // Act
            var graphRule = new GraphRule(ifNodes, operation, thenNodes);

            // Assert
            Assert.AreEqual(expectedMinMax, graphRule.MinMax);
        }

        [Test]
        public void Constructor_CalculatesMinMaxForNoneOperation()
        {
            // Arrange
            var node = MockRepository.GenerateMock<IInferenceNode>();
            var ifNodes = new List<IInferenceNode> { node };
            var operation = LogicalOperation.None;
            var thenNodes = new List<IInferenceNode> { node };
            Tuple<int, int> expectedMinMax = new Tuple<int, int>(1, 1);

            // Act
            var graphRule = new GraphRule(ifNodes, operation, thenNodes);

            // Assert
            Assert.AreEqual(expectedMinMax, graphRule.MinMax);
        }

        [Test]
        public void Constructor_ThrowsArgumentException_IfCalculatesMinMaxForNoneOperationHasInvalidIfPart()
        {
            // Arrange
            var node1 = MockRepository.GenerateMock<IInferenceNode>();
            var node2 = MockRepository.GenerateMock<IInferenceNode>();
            var ifNodes = new List<IInferenceNode> { node1, node2 };
            var operation = LogicalOperation.None;
            var thenNodes = new List<IInferenceNode> { node1 };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => { new GraphRule(ifNodes, operation, thenNodes); });
        }

        [Test]
        public void Properties_DefaultValues()
        {
            Assert.IsNull(_graphRule.Status);
        }
    }
}